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
            new GameEntity("GameMaster", typeof(GameMaster));

            // Main Player
            new GameEntity("Player", typeof(DRendererComponent), typeof(DAnimatorComponent), typeof(Player));

            //Camera 
            new GameEntity("MainCamera", typeof(DCamera), typeof(DCameraFollow));

            // Tile Painter
            new GameEntity("TileMaster", typeof(DRenderingGroup), typeof(DTilemap), typeof(DTilemapRenderer));

            // World Editor
            new GameEntity("WorldEditor", typeof(DWorldEditor));
        }

        public override void OnQuit() { }
    }
}