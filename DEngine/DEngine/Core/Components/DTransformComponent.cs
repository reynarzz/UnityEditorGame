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
        public DVec2 Position { get; set; }
        public DVec2 RoundPosition => Position.Round();
        public DVec2 TruncatedPosition => new DVec2((int)Math.Truncate(Position.x), (int)Math.Truncate(Position.y));
        public DVec2 Scale { get; set; } = new DVec2(1, 1);
        public DVec2 Offset { get; set; }
        public float Rotation { get; set; }
    }
}