using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonInspector
{
    public class ChangeLevelTB : TileBehaviorBase<ChangeLevelTD>
    {
        protected override void OnEnter(Actor actor, ChangeLevelTD data)
        {
            var gameMaster = DGameEntity.FindGameEntity("GameMaster").GetComp<GameMaster>();

            gameMaster.ChangeToLevel(data.Value, data.SpawnPosition);
        }
    }
}
