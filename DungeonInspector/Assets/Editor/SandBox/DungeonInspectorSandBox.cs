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
            // Main Player
            new DGameEntity("Player", typeof(DRendererComponent), typeof(Player));

            //Camera
            new DGameEntity("Camera", typeof(DCamera), typeof(DCameraFollow));

            // Tile Painter
            new DGameEntity("TilePainter", typeof(DTilesPainter), typeof(DRenderingGroup));

        }
        
        public override void OnQuit()
        {

        }
    }
}
