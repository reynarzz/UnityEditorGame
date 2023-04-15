using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonInspector
{
    public class DungeonInspectorMain : DSandboxBase
    {
        public override void OnInitialize()
        {
            var entity = new DGameEntity();
            var comp = entity.AddComponent<DRendererComponent>();
            
        }

        public override void OnClose()
        {

        }
    }
}
