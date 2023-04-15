using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonInspector
{
    public class DRenderingController : IDService
    {
        private Dictionary<int, List<DRendererComponent>> _renderers;
        private IOrderedEnumerable<KeyValuePair<int, List<DRendererComponent>>> _renderersOrdered;
        private bool _pendingToReorder;
        public DCamera CameraTest { get; set; }

        public DRenderingController()
        {
            _renderers = new Dictionary<int, List<DRendererComponent>>();
        }

        public void Update()
        {
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
                        Render(item.Value[i], CameraTest);
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

        public void Render(DRendererComponent renderer, DCamera camera)
        {
            var renderingTex = renderer.Texture;

            if (renderingTex == null)
            {
                renderingTex = Texture2D.whiteTexture;
            }

            var rect = camera.World2RectPos(renderer.Transform.Position, renderer.Transform.Scale);

            // snaping.
            rect = new Rect((int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height);

            if (rect.y < camera.BoundsRect.height && rect.y > camera.BoundsRect.y)
            {
                Graphics.DrawTexture(rect, renderingTex);
            }
        }
    }
}
