using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonInspector
{
    public class DungeonInspectorSandBox : DSandboxBase
    {
        public override void OnInitialize()
        {
            var entity = new DGameEntity("Player");
            var comp = entity.AddComponent<DRendererComponent>();
             entity.AddComponent<Player>();

            var camera = new DGameEntity("Camera");
            camera.AddComponent<DCamera>();
            camera.AddComponent<DCameraFollow>();

            //var camera = new DGameEntity();
            //camera.AddComponent<DCamera>();
        }

        public override void OnClose()
        {

        }
    }
}
