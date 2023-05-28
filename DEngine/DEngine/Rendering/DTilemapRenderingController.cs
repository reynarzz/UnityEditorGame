using System;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DungeonInspector
{
    public class DTilemapRenderingController : DRenderingControllerBase<DTilemapRendererComponent>
    {
        private Material _mat_DELETE;// Remove this
        public DTilemapRenderingController()
        {
            _mat_DELETE = Resources.Load<Material>("Materials/DStandard");// Remove this, put to "DRendering" instead.
        }

        protected override void Draw(DTilemapRendererComponent renderer, DCamera camera, Material material, Texture2D defaultTex)
        {
            foreach (var item in renderer.TileMap.Tiles)
            {
                foreach (var pair in item.Value)
                {
                    material = _mat_DELETE; // Remove this

                    Graphics.DrawTexture(camera.World2RectPos(item.Key, Vector2.one), pair.Value.Texture, material);
                }
            }
        }
    }
}
