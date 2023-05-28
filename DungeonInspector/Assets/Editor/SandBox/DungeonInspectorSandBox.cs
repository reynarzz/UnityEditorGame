using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

namespace DungeonInspector
{
    public class DungeonPlaymodeSandBox : DSandboxBase
    {
        public DungeonPlaymodeSandBox(params Type[] engineServices) : base(engineServices) {  }

        public override void OnInitialize()
        {
            //Camera 
            new DGameEntity("MainCamera", typeof(DCamera), typeof(DCameraFollow), typeof(CameraShake));

            //// Main Player
            //new DGameEntity("Player", typeof(DRendererComponent), typeof(DAnimatorComponent), typeof(Player));

            // Tile Painter
            var tile = new DGameEntity("TileMaster", typeof(DRenderingGroup));
            var renderer = tile.AddComp<DTilemapRendererComponent>();
            renderer.TileMap = tile.AddComp<DTilemap>();
            renderer.ZSorting = -3;
            // Game Master
            new DGameEntity("GameMaster", typeof(GameMaster));

        }

        public override void OnQuit() { }
    }
}