using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonInspector
{
    public class DungeonEditModeSandbox : DSandboxBase
    {
        public DungeonEditModeSandbox(params Type[] services) : base(services)
        {

        }

        public override void OnInitialize()
        {
            new DGameEntity("MainCamera", typeof(DCamera)/*, typeof(DCameraFollow)*/);

            new DGameEntity("TileMaster", typeof(DRenderingGroup), typeof(DTilemap), typeof(DTilemapRenderer));

            // World Editor
            new DGameEntity("WorldEditor", typeof(DWorldEditor))/*.IsActive = false*/;
        }

        public override void OnQuit()
        {

        }
    }
}
