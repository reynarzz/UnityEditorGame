using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonInspector
{
    public class DRendererComponent : DTransformableComponent
    {
        public int ZSorting { get; set; } = 0;
        private DTransformComponent _transform;
        public bool TransformWithCamera { get; set; } = true;
        public Color32 Color { get; set; } = UnityEngine.Color.white;
        public Material Material { get; set; }

        public DRendererComponent()
        {
            _transform = new DTransformComponent();
        }

        public bool FlipX { get; set; }
        public bool FlipY { get; set; }
        private float _cutOffValue = -1;
        public float CutOffValue { get => _cutOffValue; set => _cutOffValue = 1 - value; } 
        public Color CutOffColor { get; set; } = UnityEngine.Color.white;

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
                    _transform.Scale = new DVector2(base.Transform.Scale.x + Sprite.width / Sprite.height, base.Transform.Scale.y + Sprite.height / Sprite.width) / 1.2f;
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