using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;

namespace DungeonInspector
{
    public class DRendererComponent : DTransformableComponent
    {
        public int ZSorting { get; set; } = 0;
        private DTransformComponent _transform;
        public bool TransformWithCamera { get; set; } = true;

        public DRendererComponent()
        {
            _transform = new DTransformComponent();
        }

        public bool FlipX { get; set; }
        public bool FlipY { get; set; }

        public Texture2D Texture { get; set; }

        new public DTransformComponent Transform
        {
            get
            {
                _transform.Position = base.Transform.Position;
                _transform.Offset = base.Transform.Offset;

                if (Texture != null)
                {
                    _transform.Scale = new DVector2(base.Transform.Scale.x + Texture.width / Texture.height, base.Transform.Scale.y + Texture.height / Texture.width) / 1.2f;
                    //Debug.Log(Texture.width + ", " + Texture.height);
                    

                }
                return _transform;
            }
        }
    }
}