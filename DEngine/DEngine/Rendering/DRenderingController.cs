using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace DungeonInspector
{
    public class DRenderingController : EngineSystemBase<DRendererComponent>
    {
        private List<DRendererComponent> _renderers;
        private List<DRendererComponent> _ordered;

        private bool _pendingToReorder;

        private Material _mat;
        private Material _maskMat;
        private Texture2D _viewportRect;
        public DCamera CurrentCamera => _cameras.LastOrDefault();

        private List<DCamera> _cameras;
        public DRenderingController()
        {
            _ordered = new List<DRendererComponent>();
            _renderers = new List<DRendererComponent>();
            _cameras = new List<DCamera>();
            //_mat = Resources.Load<Material>("Materials/DStandard");
            _mat = Resources.Load<Material>("Materials/DStandardShadow");

            _maskMat = Resources.Load<Material>("Materials/Mask");
            _viewportRect = new Texture2D(1, 1);
        }

        private Action _debugCallback;

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

        public override void Update()
        {
            DrawMask();

            if (_pendingToReorder)
            {
                _pendingToReorder = false;
                _ordered.Clear();

                for (int i = 0; i < _renderers.Count; i++)
                {
                    var element = _renderers.ElementAt(i);

                    var zValue = element.ZSorting;

                    if (_ordered.Count > zValue)
                    {
                        _ordered.Insert(zValue, element);

                    }
                    else
                    {
                        _ordered.Add(element);
                    }
                }

                //_ordered = _ordered.OrderBy(x => x.Transform.Position.y).ToList();

                // _renderersOrdered = _renderers.OrderByDescending(x => x.Key);
            }

            if (_renderers.Count > 0)
            {
                foreach (var item in _ordered)
                {
                    Draw(item, _cameras[_cameras.Count - 1]);
                }
            }
        }

        public override void Add(DRendererComponent renderer)
        {
            _renderers.Add(renderer);

            _pendingToReorder = true;
        }

        public override void Remove(DRendererComponent renderer)
        {
            _pendingToReorder = true;

            var wasRemove = _renderers.Remove(renderer);
            if (!wasRemove)
            {
                Debug.LogError("Could not remove render");
            }
        }

        private void DrawMask()
        {
            var rect = CurrentCamera.ViewportRect;

            //rect.height += 12;
            Graphics.DrawTexture(rect, _viewportRect, _maskMat);
            //GUILayout.Space(CameraTest.ScreenSize.y);
        }

        private void Draw(DRendererComponent renderer, DCamera camera)
        {
            if (camera != null && renderer.Entity.IsActive && renderer.Enabled)
            {
                var renderingTex = renderer.Sprite;

                if (renderingTex == null)
                {
                    renderingTex = Texture2D.whiteTexture;
                }

                var rect = default(Rect);

                if (renderer.TransformWithCamera)
                {
                    rect = camera.World2RectPos(renderer.Transform.Position, renderer.Transform.Scale);
                }
                else
                {
                    // rect will not be moved by the camera
                    rect = Utils.World2RectNCamPos(renderer.Transform.Position, renderer.Transform.Scale, camera.ViewportRect, DCamera.PixelSize);
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
                    mat = _mat;
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

                foreach (var states in renderer.ShaderState)
                {
                    ClearState(states.Key, states.Value.Key, mat);
                }

                _debugCallback?.Invoke();
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
                    mat.SetInt(varName,default);
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
