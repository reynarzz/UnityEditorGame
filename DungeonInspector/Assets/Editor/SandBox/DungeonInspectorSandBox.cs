using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonInspector
{
    public class DungeonPlaymodeSandBox : DSandboxBase
    {
        public DungeonPlaymodeSandBox(params Type[] engineServices) : base(engineServices) {  }

        public override void OnInitialize()
        {
            //Camera 
            new DGameEntity("MainCamera", typeof(DCamera), typeof(DCameraFollow));

            //// Main Player
            //new DGameEntity("Player", typeof(DRendererComponent), typeof(DAnimatorComponent), typeof(Player));

            // Tile Painter
            new DGameEntity("TileMaster", typeof(DRenderingGroup), typeof(DTilemap), typeof(DTilemapRenderer));

            // Game Master
            new DGameEntity("GameMaster", typeof(GameMaster));

        }

        public override void OnQuit() { }
    }
}