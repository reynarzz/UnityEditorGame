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
            if (renderer.TileMap != null && renderer.TileMap.Entity.IsActive && renderer.TileMap.Enabled)
            {
                foreach (var tile in renderer.TileMap.Tiles)
                {
                    material = _mat_DELETE; // Remove this

                    var position = renderer.Entity.Transform.Position + tile.Key; // remove this, instead do it in the "DTilemap" class
                    var scale = renderer.Entity.Transform.Scale;

                    Graphics.DrawTexture(camera.World2RectPos(position, scale), tile.Value.Texture, material);
                }
            }
        }
    }
}
