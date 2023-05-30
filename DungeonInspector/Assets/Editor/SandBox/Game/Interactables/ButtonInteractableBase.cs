using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonInspector
{
    public class ButtonInteractableBase : InteractableBase 
    {
        private bool _interacted = false;
        public bool BlocksPath { get; set; }

        protected override void OnTriggerStay(DBoxCollider collider)
        {
            //if (!_interacted)
            {
                if(DInput.IsKeyDown(UnityEngine.KeyCode.E))
                {
                    OnInteracted();
                }
            }
        }

        protected virtual void OnInteracted() {  }
    }
}
