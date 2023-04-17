using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro.EditorUtilities;
using UnityEditor;
using UnityEngine;

namespace DungeonInspector
{
    public class DRenderingController : IDService
    {
        private Dictionary<int, List<DRendererComponent>> _renderers;
        private IOrderedEnumerable<KeyValuePair<int, List<DRendererComponent>>> _renderersOrdered;
        private bool _pendingToReorder;
        public DCamera CameraTest { get; set; }

        private Material _mat;
        private Material _maskMat;
        private Texture2D _viewportRect;

        public DRenderingController()
        {
            _renderers = new Dictionary<int, List<DRendererComponent>>();

            _mat = Resources.Load<Material>("Materials/DStandard");
            _maskMat = Resources.Load<Material>("Materials/Mask");
            _viewportRect = new Texture2D(1, 1);
        }

        public void Update()
        {
            DrawMask();

            if (_pendingToReorder)
            {
                _pendingToReorder = false;
                _renderersOrdered = _renderers.OrderByDescending(x => x.Key);
            }

            if (_renderers.Count > 0)
            {
                foreach (var item in _renderersOrdered)
                {
                    for (int i = 0; i < item.Value.Count; i++)
                    {
                        Draw(item.Value[i], CameraTest);
                    }
                }
            }
        }

        public void AddRenderer(DRendererComponent renderer)
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

        public bool RemoveRenderer(DRendererComponent renderer)
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

                return true;
            }

            Debug.LogError("Could not remove render");
            return false;
        }

        private void DrawMask()
        {
            Graphics.DrawTexture(CameraTest.ViewportRect, _viewportRect, _maskMat);
            //GUILayout.Space(CameraTest.ScreenSize.y - 18);
        }

        private void Draw(DRendererComponent renderer, DCamera camera)
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

            mat.SetVector("_flip", new Vector4(renderer.FlipX ? 1 : 0, renderer.FlipY ? 1 : 0));
            Graphics.DrawTexture(rect, renderingTex, mat);
            mat.SetVector("_flip", default);
        }
    }
}
