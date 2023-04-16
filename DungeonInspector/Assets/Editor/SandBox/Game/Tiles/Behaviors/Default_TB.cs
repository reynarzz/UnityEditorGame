using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonInspector
{
    public class Default_TB : TileBehaviorBase 
    {
        public override void OnEnter(Player player)
        {
            $"Actor {player.GameEntity.Name}, Enter".LOG();
        }

        public override void OnExit(Player player)
        {
            $"Actor {player.GameEntity.Name}, Exit".LOG();
        }
    }
}