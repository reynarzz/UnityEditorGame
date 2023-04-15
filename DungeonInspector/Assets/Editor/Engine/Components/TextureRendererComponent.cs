using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonInspector
{
    public class TextureRendererComponent : Component
    {
        public Texture2D Texture { get; set; }
        public Rect Bounds { get; set; }
    }
}
