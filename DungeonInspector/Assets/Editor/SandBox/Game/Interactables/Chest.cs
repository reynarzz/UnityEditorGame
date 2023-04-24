using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonInspector
{
    public class Chest : ButtonInteractableBase
    {

        protected override void OnInteracted()
        {
            Debug.Log("Open chest");
            GetComp<DAnimatorComponent>().Play(0);
        }
    }
}
