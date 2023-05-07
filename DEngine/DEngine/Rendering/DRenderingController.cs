using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace DungeonInspector
{
    public class DRenderingController : EngineSystemBase<DRendererComponent>
    {
        private List<DRendererComponent> _renderers;
        private List<DRendererUIComponent> _uirenderers;
        private IOrderedEnumerable<DRendererComponent> _renderersOrdered;
        private IOrderedEnumerable<DRendererUIComponent> _uiRenderersOrdered;

        private bool _pendingToReorder;

        private Material _mat;
        private readonly Material _uiMat;
        private Material _maskMat;
        private Material _screenSpaceEffects;
        private Texture2D _viewportRectTex;
        public DCamera CurrentCamera => _cameras.LastOrDefault();
        private List<DCamera> _cameras;

        private Texture2D _whiteTex;
        private RenderTexture _renderTarget;

        public bool V2Rendering { get; set; }

        public DRenderingController()
        {
            _renderers = new List<DRendererComponent>();
            _uirenderers = new List<DRendererUIComponent>();

            _cameras = new List<DCamera>();
            //_mat = Resources.Load<Material>("Materials/DStandard");
            _mat = Resources.Load<Material>("Materials/DStandardShadow");

            _maskMat = Resources.Load<Material>("Materials/Mask");
            _screenSpaceEffects = Resources.Load<Material>("Materials/ScreenSpace");
            _uiMat = UnityEngine.Resources.Load<UnityEngine.Material>("Materials/DUI");


            _viewportRectTex = new Texture2D(1, 1);
            _whiteTex = Texture2D.whiteTexture;
        }

        private Action _debugCallback;
        private Action _renderControl;

        public void AddDebugGUI(Action debugCallback)
        {
            _debugCallback += debugCallback;
        }

        public void RemoveDebugGUI(Action debugCallback)
        {
            _debugCallback -= debugCallback;
        }

        public void AddCamera(DCamera camera)
        {
            _cameras.Add(camera);
        }

        public void RemoveCamera(DCamera camera)
        {
            _cameras.Remove(camera);
        }

        public void AddCustomRenderControl(Action renderer)
        {
            _renderControl = renderer;
        }

        private void PreRender()
        {
            if (_renderTarget == null)
            {
                var pix =  EditorGUIUtility.PointsToPixels(CurrentCamera.ViewportRect);

                _renderTarget = new RenderTexture((int)pix.width, (int)pix.height, 1);
                _renderTarget.filterMode = FilterMode.Point;
                _renderTarget.dimension = UnityEngine.Rendering.TextureDimension.Tex2D;
                _renderTarget.wrapMode = TextureWrapMode.Clamp;

                _renderTarget.Create();
            }

            RenderTexture.active = _renderTarget;
        }

        private void PostRender()
        {
            //Graphics.Blit(null, tex, _screenSpaceEffects);
            RenderTexture.active = null;
            //EditorGUI.DrawRect(CurrentCamera.ViewportRect, Color.red);



            //pix.width = pix.height;
            var rec = CurrentCamera.ViewportRect;
            rec.x = 0;
            rec.y = -rec.height / 2f;
            rec.height *= 2;
            rec.height += 1;

            rec.y -= 12;
            DrawMask();

            _screenSpaceEffects.SetVector("_dtime", new Vector4(DTime.Time, DTime.DeltaTime, Mathf.Sin(DTime.Time), Mathf.Cos(DTime.Time)));
            Graphics.DrawTexture(rec, _renderTarget, _screenSpaceEffects);
        }


        public override void Update()
        {

            if (V2Rendering)
            {
                PreRender();
            }

            DrawMask();
            _renderControl?.Invoke();

            _debugCallback?.Invoke();

            //if (_pendingToReorder)
            {
                _pendingToReorder = false;
                //_ordered.Clear();


                // TODO: please, make this not be every frame, (bad perfomance)
                _renderersOrdered = _renderers.OrderByDescending(x => x.Transform.Position.y + x.Transform.Offset.y - x.ZSorting);
                _uiRenderersOrdered = _uirenderers.OrderBy(x => x.ZSorting);
                // _renderersOrdered = _renderers.OrderByDescending(x => x.Key);
            }

            if (_renderers.Count > 0)
            {
                // var tex = GetNextTexture();

                foreach (var item in _renderersOrdered)
                {
                    Draw(item, _cameras[_cameras.Count - 1], _mat);
                }


                foreach (var item in _uiRenderersOrdered)
                {
                    Draw(item, _cameras[_cameras.Count - 1], _uiMat);
                }
            }

            if (V2Rendering)
            {
                PostRender();
            }
        }

        public override void Add(DRendererComponent renderer)
        {
            if (renderer is DRendererUIComponent)
            {
                renderer.TransformWithCamera = false;
                _uirenderers.Add(renderer as DRendererUIComponent);
            }
            else
            {
                _renderers.Add(renderer);
            }

            _pendingToReorder = true;
        }

        public override void Remove(DRendererComponent renderer)
        {
            _pendingToReorder = true;
            var wasRemove = false;

            if (renderer is DRendererUIComponent)
            {
                wasRemove = _uirenderers.Remove(renderer as DRendererUIComponent);
            }
            else
            {
                wasRemove = _renderers.Remove(renderer);
            }


            if (!wasRemove)
            {
                Debug.LogError("Could not remove render");
            }
        }

        private void DrawMask()
        {
            var rect = CurrentCamera?.ViewportRect ?? new Rect(0, 0, EditorGUIUtility.currentViewWidth, 360);

            rect.height += 18;

            GUILayoutUtility.GetRect(rect.width, rect.height);

            // if a component is set bellow it will take space

            //rect.height += 12;
            Graphics.DrawTexture(rect, _viewportRectTex, _maskMat);
            //GUILayout.Space(CameraTest.ScreenSize.y);
        }

        private void Draw(DRendererComponent renderer, DCamera camera, Material defaultMat)
        {

            if (camera != null && renderer.Entity.IsActive && renderer.Enabled)
            {
                var renderingTex = renderer.Sprite;

                if (renderingTex == null)
                {
                    renderingTex = _whiteTex;
                }

                var rect = default(Rect);

                if (renderer.TransformWithCamera)
                {
                    rect = camera.World2RectPos(renderer.Transform.Position, renderer.Transform.Scale);
                }
                else
                {
                    // rect will not be moved by the camera
                    rect = Utils.World2RectNCamPos(renderer.Transform.Position, renderer.Transform.Scale, camera.ViewportRect, 25);
                }

                // snaping.
                rect = new Rect((int)rect.x + renderer.Transform.Offset.x * DCamera.PixelSize, (int)rect.y - renderer.Transform.Offset.y * DCamera.PixelSize, (int)rect.width, (int)rect.height);

                var mat = default(Material);

                if (renderer.Material)
                {
                    mat = renderer.Material;
                }
                else
                {
                    mat = defaultMat;
                }

                if(renderer is DRendererAtlasComponent)
                {
                    var atlasRenderer = renderer as DRendererAtlasComponent;

                    var atlasTex = atlasRenderer.AtlasInfo.Texture;
                    var blockSize = atlasRenderer.AtlasInfo.BlockSIze;


                    // mat.SetVector("_AtlasSpriteIndex", atlasRenderer.SpriteIndex);
                    mat.SetInt("_IsAtlas", 1);
                    mat.SetInt("_AtlasBlockSize", blockSize);
                    mat.SetTexture("_AtlasTex", atlasTex);

                    var width = (float)atlasTex.width / (float)blockSize;
                    var height = (float)atlasTex.height / (float)blockSize;

                    mat.SetVector("_AtlasRect", new Vector4(atlasRenderer.SpriteCoord.x / width, atlasRenderer.SpriteCoord.y / height, width, height));
                }

                foreach (var states in renderer.ShaderState)
                {
                    SetState(states.Key, states.Value, mat);
                }

                mat.SetVector("_dtime", new Vector4(DTime.Time, DTime.DeltaTime, Mathf.Sin(DTime.Time), Mathf.Cos(DTime.Time)));
                mat.SetVector("_cutOffColor", renderer.CutOffColor);
                mat.SetFloat("_xCutOff", renderer.CutOffValue);
                mat.SetVector("_color", (Color)renderer.Color);
                mat.SetVector("_flip", new Vector4(renderer.FlipX ? 1 : 0, renderer.FlipY ? 1 : 0, renderer.Transform.Rotation));


                Graphics.DrawTexture(rect, renderingTex, mat);

                mat.SetVector("_flip", default);
                mat.SetVector("_cutOffColor", Color.white);
                mat.SetFloat("_xCutOff", 0);
                mat.SetVector("_color", Color.white);
                mat.SetInt("_IsAtlas", 0);

                foreach (var states in renderer.ShaderState)
                {
                    ClearState(states.Key, states.Value.Key, mat);
                }
            }


        }

        private void SetState(string varName, KeyValuePair<ShaderStateDataType, object> data, Material mat)
        {
            switch (data.Key)
            {
                case ShaderStateDataType.Vector:
                    mat.SetVector(varName, (Vector4)data.Value);
                    break;
                case ShaderStateDataType.Int:
                    mat.SetInt(varName, (int)data.Value);
                    break;
                case ShaderStateDataType.Float:
                    mat.SetFloat(varName, (float)data.Value);
                    break;
                case ShaderStateDataType.Matrix:
                    mat.SetMatrix(varName, (Matrix4x4)data.Value);
                    break;

            }
        }

        private void ClearState(string varName, ShaderStateDataType dataType, Material mat)
        {
            switch (dataType)
            {
                case ShaderStateDataType.Vector:
                    mat.SetVector(varName, default);
                    break;
                case ShaderStateDataType.Int:
                    mat.SetInt(varName, default);
                    break;
                case ShaderStateDataType.Float:
                    mat.SetFloat(varName, default);
                    break;
                case ShaderStateDataType.Matrix:
                    mat.SetMatrix(varName, default);
                    break;

            }
        }
    }
}
