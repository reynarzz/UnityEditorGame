using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonInspector
{
    public abstract class ButtonInteractableBase : InteractableBase
    {
        [DExpose] private bool _interactable = false;
        protected abstract string TagTarget { get; }

        protected override void OnTriggerEnter(DBoxCollider collider)
        {
            base.OnTriggerEnter(collider);

            if (collider.Entity.Tag.Equals(TagTarget))
            {
                _interactable = true;
            }
        }

        protected override void OnTriggerExit(DBoxCollider collider)
        {
            base.OnTriggerExit(collider);

            if (collider.Entity.Tag.Equals(TagTarget))
            {
                _interactable = false;
            }
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
