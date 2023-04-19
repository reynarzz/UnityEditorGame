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
            //Camera 
            new DGameEntity("MainCamera", typeof(DCamera), typeof(DCameraFollow));

            // Main Player
            new DGameEntity("Player", typeof(DRendererComponent), typeof(DAnimatorComponent), typeof(Player));

            // Tile Painter
            new DGameEntity("TileMaster", typeof(DRenderingGroup), typeof(DTilemap), typeof(DTilemapRenderer));

            // Game Master
            new DGameEntity("GameMaster", typeof(GameMaster));

            // World Editor
            new DGameEntity("WorldEditor", typeof(DWorldEditor)).IsActive = false;
        }

        public override void OnQuit() { }
    }
}