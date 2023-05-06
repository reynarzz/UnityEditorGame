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

            var value = data.Value.ToString();

            screenUI.FadeIn(() => 
            {
                gameMaster.ChangeToLevel(value);
                Debug.Log("Change level: " + value);

                screenUI.FadeOut(() =>
                {

                });
            });
        }


    }
}
