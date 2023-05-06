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
            var screenUI = DGameEntity.FindGameEntity("ScreenUI").GetComp<ScreenUI>();
            var gameMaster = DGameEntity.FindGameEntity("GameMaster").GetComp<GameMaster>();

            screenUI.FadeIn(() =>
            {
                gameMaster.ChangeToLevel(data.Value);
                //Debug.Log("Change level: " + data.Value);

                screenUI.FadeOut(() =>
                {

                });
            });
        }


    }
}
