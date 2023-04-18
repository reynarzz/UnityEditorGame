using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonInspector
{
    public class DEditorSystem : EngineSystemBase
    {
        private DHierarchyEditor _hierarchy;
        public override void Init()
        {
            _hierarchy = new DHierarchyEditor();
        }

        public override void Update()
        {
            _hierarchy.Update();
        }
    }
}
