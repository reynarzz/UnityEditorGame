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
        public DVector2 Position { get; set; }
        public DVector2 RoundPosition => Position.Round();
        public DVector2 Scale { get; set; } = new DVector2(1, 1);
        public DVector2 Offset { get; set; }
    }
}