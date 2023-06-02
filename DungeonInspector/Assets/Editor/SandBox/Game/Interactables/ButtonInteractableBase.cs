using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonInspector
{
    public class ButtonInteractableBase : InteractableBase
    {
        [DExpose] private bool _interactable = false;

        protected override void OnTriggerEnter(DBoxCollider collider)
        {
            base.OnTriggerEnter(collider);

            _interactable = true;
        }

        protected override void OnTriggerExit(DBoxCollider collider)
        {
            base.OnTriggerExit(collider);

            _interactable = false;
        }
        protected override void OnUpdate()
        {
            base.OnUpdate();

            if (_interactable && DInput.IsKeyDown(UnityEngine.KeyCode.E))
            {
                UnityEngine.Debug.Log("Interac");
                OnInteracted();
            }
        }

        protected virtual void OnInteracted() { }
    }
}
