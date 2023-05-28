using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonInspector
{
    public class DEditorSystem : DEngineSystemBase
    {
        private DHierarchyEditor _hierarchy;
        private DEditorToolBar _toolbar;

        public DEditorToolBar Toolbar => _toolbar;

        public DEditorSystem()
        {
            _toolbar = new DEditorToolBar(this);
        }

        public override void Init()
        {
            _hierarchy = new DHierarchyEditor();
        }

        public override void Update()
        {
            _toolbar.Update();
            _hierarchy.Update();
        }
    }
}
