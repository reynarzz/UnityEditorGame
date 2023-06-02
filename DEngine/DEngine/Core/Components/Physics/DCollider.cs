using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonInspector
{
    public abstract class DCollider : DBehavior
    {
        [DExpose] private bool _debug = false;
        public bool Debug { get => _debug; set => _debug = value; }
        public bool IsTrigger { get; set; } = true;
        internal bool IsColliding { get; set; }

        protected override void OnAwake()
        {
            DIEngineCoreServices.Get<DRendering>().AddDebugGUI(RenderBoundingBox);
        }
        
        protected virtual void RenderBoundingBox() { }

        public override void OnDestroy()
        {
            DIEngineCoreServices.Get<DRendering>().RemoveDebugGUI(RenderBoundingBox);
        }
    }
}
