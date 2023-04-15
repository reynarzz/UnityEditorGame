using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonInspector
{
    public class DTransformComponent : DComponent
    {
        public Vector2 Position { get; set; }
        public Vector2 Scale { get; set; }
    }
}