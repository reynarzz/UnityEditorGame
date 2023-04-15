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
            // Game Master
            new DGameEntity("GameMaster", typeof(DGameMaster));

            // Main Player
            new DGameEntity("Player", typeof(DRendererComponent), typeof(DAnimatorComponent), typeof(Player));

            //Camera
            new DGameEntity("Camera", typeof(DCamera), typeof(DCameraFollow));

            // Tile Painter
            new DGameEntity("TilePainter", typeof(DRenderingGroup), typeof(DTilesPainter));

        }

        public override void OnQuit()
        {

        }
    }
}
