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
        private DAtlasRendererComponent _interactableButton;
        [DExpose] private bool _isOpen;
        [DExpose] private float _time;
        [DExpose] private bool _showInteractableButton;
        [DExpose] private float _showSpeed = 5f;

        protected override void OnAwake()
        {
            base.OnAwake();

            var ent = new DGameEntity("KeyboardButton");
            var atlas = Resources.Load<DSpriteAtlasInfo>("UI/KeyboardAtlas");

            _interactableButton = ent.AddComp<DAtlasRendererComponent>();
            _interactableButton.AtlasInfo = atlas;
            _interactableButton.SpriteCoord = new DVec2(4, 11);
        }

        protected override void OnInteracted()
        {
            Debug.Log("Open chest");
            GetComp<DAnimatorComponent>().Play(0);
            _isOpen = true;
            _showInteractableButton = false;
        }

        protected override void OnTriggerStay(DBoxCollider collider)
        {
            base.OnTriggerStay(collider);


            if (!_isOpen && collider.Entity.Tag == "Player")
            {
                Debug.Log("Enter");

                _showInteractableButton = true;
            }
        }

        protected override void OnTriggerExit(DBoxCollider collider)
        {
            base.OnTriggerExit(collider);

            if (!_isOpen && collider.Entity.Tag == "Player")
            {
                Debug.Log("Exit");
                _showInteractableButton = false;
            }
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            if (!_isOpen)
            {
                _interactableButton.Entity.Transform.Position = Transform.Position + new DVec2(0, 1f + (float)Math.Sin(DTime.Time * 7) * 0.1f);
            }

            if (_showInteractableButton)
            {
                _time += DTime.DeltaTime * _showSpeed;
            }
            else
            {
                _time -= DTime.DeltaTime * _showSpeed;
            }

            _time = Mathf.Clamp01(_time);

            var c = _interactableButton.Color;
            _interactableButton.Color = new Color32(c.r, c.g, c.b, (byte)Mathf.Lerp(0, 255, _time));
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            _interactableButton.Entity.Destroy();
        }
    }
}
