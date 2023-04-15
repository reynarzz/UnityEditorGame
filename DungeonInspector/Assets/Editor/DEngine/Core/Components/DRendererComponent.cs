using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonInspector
{
    public class DRendererComponent : DComponent
    {
        public int ZSorting { get; set; } = 0;
        public Texture2D Texture { get; set; }
        public DTransformComponent Transform { get; set; }
    }
}