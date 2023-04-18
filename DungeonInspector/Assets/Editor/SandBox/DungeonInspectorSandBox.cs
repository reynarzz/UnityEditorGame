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
            new GameEntity("MainCamera", typeof(DCamera), typeof(DCameraFollow));

            // Main Player
            new GameEntity("Player", typeof(DRendererComponent), typeof(DAnimatorComponent), typeof(Player));

            // Tile Painter
            new GameEntity("TileMaster", typeof(DRenderingGroup), typeof(DTilemap), typeof(DTilemapRenderer));

            // World Editor
            new GameEntity("WorldEditor", typeof(DWorldEditor)).IsActive = false;

            // Game Master
            new GameEntity("GameMaster", typeof(GameMaster));

            
            
        }

        public override void OnQuit() { }
    }
}