using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DungeonInspector
{
    public class DRendering : DEngineSystemBase<DRendererComponent>
    {
        private Dictionary<Type, IDRenderingControllerBase> _renderingControllers;

        private List<DRendererComponent> _renderers;
        private IOrderedEnumerable<DRendererComponent> _renderersOrdered;

        private List<DRendererUIComponent> _uirenderers;
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

        public DRendering()
        {
            _renderers = new List<DRendererComponent>();
            _uirenderers = new List<DRendererUIComponent>();

            _cameras = new List<DCamera>();
            //_mat = Resources.Load<Material>("Materials/DStandard");
            _mat = Resources.Load<Material>("Materials/DStandardShadow");

            _maskMat = Resources.Load<Material>("Materials/Mask");
            _screenSpaceEffects = Resources.Load<Material>("Materials/ScreenSpace");
            _uiMat = Resources.Load<Material>("Materials/DUI");


            _viewportRectTex = new Texture2D(1, 1);
            _whiteTex = Texture2D.whiteTexture;

            _renderingControllers = new Dictionary<Type, IDRenderingControllerBase>()
            {
                { typeof(DSpriteRendererComponent), new DSpriteRenderingController() },
                { typeof(DAtlasRendererComponent), new DAtlasRenderingController() },
                { typeof(DTilemapRendererComponent), new DTilemapRenderingController() },
                { typeof(DRendererUIComponent), new DSpriteRenderingController() },
            };
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

        //public void AddCustomRenderControl(Action renderer)
        //{
        //    _renderControl = renderer;
        //}

        private void PreRender()
        {
            if (_renderTarget == null)
            {
                var pix = EditorGUIUtility.PointsToPixels(CurrentCamera.ViewportRect);

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


        public override void OnGUI()
        {
            if (V2Rendering)
            {
                PreRender();
            }

            DrawMask();

            //if (_pendingToReorder)
            {
                _pendingToReorder = false;
                //_ordered.Clear();

                // TODO: please, make this not be every frame, (bad perfomance)
                //_renderersOrdered = _renderers.OrderByDescending(x => x.Transform.Position.y + x.Transform.Offset.y - x.ZSorting);
                _renderersOrdered = _renderers.OrderBy(x => /*x.Transform.Position.y + x.Transform.Offset.y -*/ x.ZSorting);

                _uiRenderersOrdered = _uirenderers.OrderBy(x => x.ZSorting);
                // _renderersOrdered = _renderers.OrderByDescending(x => x.Key);
            }

            if(Event.current.type != EventType.Repaint)
            {
                if (_renderers.Count > 0)
                {
                    //Render World
                    DrawGroup(_renderersOrdered);
                }

                if (_uirenderers.Count > 0)
                {
                    // Render UI
                    DrawGroup(_uiRenderersOrdered);
                }
            }
          

            _debugCallback?.Invoke();

            if (V2Rendering)
            {
                PostRender();
            }
        }

        //public override void OnGUI()
        //{
        //    //_debugCallback?.Invoke();
        //}

        private void DrawGroup<T>(IOrderedEnumerable<T> collection) where T : DRendererComponent
        {
            foreach (var item in collection)
            {
                if (_renderingControllers.TryGetValue(item.GetType(), out var controller))
                {
                    controller.Draw(item, _cameras[_cameras.Count - 1], _mat, _whiteTex);
                }
                else
                {
                    throw new Exception($"There is not rendering controller for: {item.GetType()}");
                }
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

            // Sets viewport/client area
            //GUILayoutUtility.GetRect(rect.width, rect.height);
            //Debug.Log("ViewHeight: " + rect.height);
            GUILayout.Space(rect.height);

            // if a component is set bellow it will take space

            //rect.height += 12;
            Graphics.DrawTexture(rect, _viewportRectTex, _maskMat);
        }


    }
}
