using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonInspector
{
    public class DBoxCollider : DCollider
    {
        private DAABB _boundingBox;
        public DAABB AABB => _boundingBox;
        public DVec2 Center { get; set; }
        public DVec2 Size { get; set; } = new DVec2(1, 1);
       
        protected override void OnUpdate()
        {
            _boundingBox.Max = new DVec2(Transform.Position.x + Transform.Offset.x + Center.x + Size.x * 0.5f,
                                         Transform.Position.y + Transform.Offset.y + Center.y + Size.y * 0.5f);

            _boundingBox.Min = new DVec2(Transform.Position.x + Transform.Offset.x + Center.x - Size.x * 0.5f,
                                         Transform.Position.y + Transform.Offset.y + Center.y - Size.y * 0.5f);
        }

        protected override void RenderBoundingBox()
        {
            if (IsTrigger && Debug)
            {
                var color = IsColliding ? Color.green : Color.white;

                Utils.DrawBounds(_boundingBox, color);
            }
        }
    }
}