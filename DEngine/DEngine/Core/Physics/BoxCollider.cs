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
        public DVec2 Center { get; set; }
        public DVec2 Size { get; set; } = new DVec2(1, 1);
        public bool IsTrigger { get; set; } = true;
        [DExpose]private bool _debug = false;
        public bool Debug { get => _debug; set => _debug = value; }
        internal bool IsColliding { get; set; }

        private bool _activatedDebug = false;

        protected override void OnStart()
        {

        }


        protected override void OnUpdate()
        {
            if (!_activatedDebug)
            {
                _activatedDebug = true;
                DIEngineCoreServices.Get<DRendering>().AddDebugGUI(RenderBoundingBox_Test);
            }

            _boundingBox.Max = new DVec2(Transform.Position.x + Transform.Offset.x + Center.x + Size.x * 0.5f,
                                            Transform.Position.y + Transform.Offset.y + Center.y + Size.y * 0.5f);

            _boundingBox.Min = new DVec2(Transform.Position.x + Transform.Offset.x + Center.x - Size.x * 0.5f,
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
            DIEngineCoreServices.Get<DRendering>().RemoveDebugGUI(RenderBoundingBox_Test);
        }
    }
}
