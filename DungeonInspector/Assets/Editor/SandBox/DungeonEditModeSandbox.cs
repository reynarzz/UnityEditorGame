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

            var tile = new DGameEntity("TileMaster", typeof(DRenderingGroup));
            var renderer = tile.AddComp<DTilemapRendererComponent>();
            renderer.TileMap = tile.AddComp<DTilemap>();
            renderer.ZSorting = -1;

            // World Editor
            new DGameEntity("WorldEditor", typeof(DWorldEditor))/*.IsActive = false*/;
        }

        public override void OnQuit()
        {

        }
    }
}
