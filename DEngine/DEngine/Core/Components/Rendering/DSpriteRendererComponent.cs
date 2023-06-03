using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonInspector
{
    public class DSpriteRendererComponent : DRendererComponent
    {
        public Texture2D Sprite { get; set; }
        new public DTransformComponent Transform
        {
            get
            {
                _transform.Position = base.Transform.Position;
                _transform.Offset = base.Transform.Offset;
                _transform.Rotation = base.Transform.Rotation;

                if (Sprite != null)
                {
                    //    var rect = EditorGUIUtility.(DCamera._viewportRect);


                    //0.05 

                    var factor = 0.064f;// base.Transform.Scale.x / Sprite.height;// (EditorGUIUtility.pixelsPerPoint );
                    //Debug.Log(factor);
                    _transform.Scale = new DVec2((base.Transform.Scale.x * Sprite.width) * factor, (base.Transform.Scale.y * (float)Sprite.height) * factor);
                    //Debug.Log(Texture.width + ", " + Texture.height);
                }
                return _transform;
            }
            set
            {
                base.Transform.Position = value.Position;
                base.Transform.Offset = value.Offset;
                base.Transform.Scale = value.Scale;
                base.Transform.Rotation = value.Rotation;
            }
        }
    }
}
