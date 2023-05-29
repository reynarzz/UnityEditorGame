using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonInspector
{
    public class DungeonEditModeSandbox : DSandboxBase
    {
        public DungeonEditModeSandbox(params Type[] services) : base(services) { }

        public override void OnInitialize()
        {
            // Camera
            new DGameEntity("MainCamera", typeof(DCamera));

            // World Editor
            new DGameEntity("WorldEditor", typeof(DWorldEditor));
        }

        public override void OnQuit()
        {

        }
    }
}
