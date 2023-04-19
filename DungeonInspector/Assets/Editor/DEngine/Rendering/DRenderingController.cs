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
        private Dictionary<int, List<DRendererComponent>> _renderers;
        private IOrderedEnumerable<KeyValuePair<int, List<DRendererComponent>>> _renderersOrdered;
        private bool _pendingToReorder;

        private Material _mat;
        private Material _maskMat;
        private Texture2D _viewportRect;
        public DCamera CurrentCamera => _cameras.LastOrDefault();

        private List<DCamera> _cameras;
        public DRenderingController()
        {
            _renderers = new Dictionary<int, List<DRendererComponent>>();
            _cameras = new List<DCamera>();
            _mat = Resources.Load<Material>("Materials/DStandard");
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
                _renderersOrdered = _renderers.OrderByDescending(x => x.Key);
            }

            if (_renderers.Count > 0)
            {
                foreach (var item in _renderers)
                {
                    for (int i = 0; i < item.Value.Count; i++)
                    {
                        Draw(item.Value[i], _cameras[_cameras.Count - 1]);
                    }
                }
            }
        }

        public override void Add(DRendererComponent renderer)
        {
            if (_renderers.TryGetValue(renderer.ZSorting, out var rendererList))
            {
                rendererList.Add(renderer);
            }
            else
            {
                _renderers.Add(renderer.ZSorting, new List<DRendererComponent>() { renderer });
            }

            _pendingToReorder = true;
        }

        public override void Remove(DRendererComponent renderer)
        {
            // Todo
            _pendingToReorder = true;

            if (_renderers.TryGetValue(renderer.ZSorting, out var rendererList))
            {
                rendererList.Remove(renderer);

                if (rendererList.Count == 0)
                {
                    _renderers.Remove(renderer.ZSorting);
                }

                //return true;
            }
            else
            {
                Debug.LogError("Could not remove render");
            }
            //return false;
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
                    rect = Utils.World2RectNCamPos(renderer.Transform.Position, renderer.Transform.Scale, camera.ViewportRect, DCamera.PixelsPerUnit);
                }

                // snaping.
                rect = new Rect((int)rect.x + renderer.Transform.Offset.x * DCamera.PixelsPerUnit, (int)rect.y - renderer.Transform.Offset.y * DCamera.PixelsPerUnit, (int)rect.width, (int)rect.height);

                var mat = default(Material);

                if (renderer.Material)
                {
                    mat = renderer.Material;
                }
                else
                {
                    mat = _mat;
                }

                mat.SetVector("_cutOffColor", renderer.CutOffColor);
                mat.SetFloat("_xCutOff", renderer.CutOffValue);
                mat.SetVector("_color", (Color)renderer.Color);
                mat.SetVector("_flip", new Vector4(renderer.FlipX ? 1 : 0, renderer.FlipY ? 1 : 0, renderer.ZRotate));
                Graphics.DrawTexture(rect, renderingTex, mat);
                mat.SetVector("_flip", default);
                mat.SetVector("_cutOffColor", Color.white);
                mat.SetFloat("_xCutOff", 0);
                mat.SetVector("_color", Color.white);

                _debugCallback?.Invoke();
            }
        }
    }
}
