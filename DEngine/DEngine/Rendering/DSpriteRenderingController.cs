using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonInspector
{
    public class DSpriteRenderingController : DRenderingControllerBase<DSpriteRendererComponent>
    {
        protected override void Draw(DSpriteRendererComponent renderer, DCamera camera, Material defaultMat, Texture2D defaultTex)
        {
            if (camera != null && renderer.Entity.IsActive && renderer.Enabled)
            {
                var renderingTex = renderer.Sprite;

                if (renderingTex == null)
                {
                    renderingTex = defaultTex;

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
                rect = new Rect(rect.x + renderer.Transform.Offset.x * DCamera.PixelSize, rect.y - renderer.Transform.Offset.y * DCamera.PixelSize, (int)rect.width, (int)rect.height);
                var mat = default(Material);

                if (renderer.Material)
                {
                    mat = renderer.Material;
                }
                else
                {
                    mat = defaultMat;
                }

                if (renderer is DAtlasRendererComponent)
                {
                    var atlasRenderer = renderer as DAtlasRendererComponent;

                    var atlasTex = atlasRenderer.AtlasInfo.Texture;
                    var blockSize = atlasRenderer.AtlasInfo.BlockSIze;


                    // mat.SetVector("_AtlasSpriteIndex", atlasRenderer.SpriteIndex);
                    mat.SetInt("_IsAtlas", 1);
                    mat.SetTexture("_AtlasTex", atlasTex);

                    var width = (float)atlasTex.width / (float)blockSize.x;
                    var height = (float)atlasTex.height / (float)blockSize.y;

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
    }
}
