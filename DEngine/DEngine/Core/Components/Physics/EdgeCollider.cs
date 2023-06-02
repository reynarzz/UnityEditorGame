using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonInspector
{
    public class EdgeCollider : DCollider
    {
        // Ordered points forming edges
        public List<DVec2> Edges { get; internal set; }

        protected override void RenderBoundingBox()
        {

        }
    }
}
