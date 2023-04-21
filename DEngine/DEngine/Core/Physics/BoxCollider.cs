using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace DungeonInspector
{
    public class DBoxCollider : DBehavior
    {
        private DAABB _boundingBox;
        public DAABB AABB => _boundingBox;
        public DVector2 Center { get; set; }
        public DVector2 Size { get; set; } = new DVector2(1, 1);
        public bool IsColliding { get; set; }
        public bool IsTrigger { get; set; } = true;
        public bool Debug { get; set; } = false;
        protected override void OnStart()
        {
            DIEngineCoreServices.Get<DRenderingController>().AddDebugGUI(RenderBoundingBox_Test);
        }


        protected override void OnUpdate()
        {
            _boundingBox.Max = new DVector2(Transform.Position.x + Transform.Offset.x + Center.x + Size.x * 0.5f,
                                            Transform.Position.y + Transform.Offset.y + Center.y + Size.y * 0.5f);

            _boundingBox.Min = new DVector2(Transform.Position.x + Transform.Offset.x + Center.x - Size.x * 0.5f,
                                            Transform.Position.y + Transform.Offset.y + Center.y - Size.y * 0.5f);
        }

       
        private void RenderBoundingBox_Test()
        {
            if (IsTrigger && Debug)
            {
                var color = IsColliding ? Color.green : Color.white;

                Utils.DrawBounds(_boundingBox, color);
            }
        }

        public override void OnDestroy()
        {
            DIEngineCoreServices.Get<DRenderingController>().RemoveDebugGUI(RenderBoundingBox_Test);
        }
    }
}
