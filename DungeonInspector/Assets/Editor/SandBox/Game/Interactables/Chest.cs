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
        [DExpose] private bool _isOpen;
        [DExpose] private float _time;
        [DExpose] private bool _canOpenChest;
        [DExpose] private float _showSpeed = 5f;

        private DAtlasRendererComponent _interactableButton;

        protected override string TagTarget => "Player";

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
            if (_canOpenChest && !_isOpen)
            {
                Debug.Log("Open chest");
                DAudio.PlayAudio("ChestOpen");
                GetComp<DAnimatorComponent>().Play(0);
                _isOpen = true;
                _canOpenChest = false;
            }
        }

        protected override void OnTriggerEnter(DCollider collider)
        {
            base.OnTriggerEnter(collider);

            Debug.Log(collider.Entity.Tag);

            if (!_isOpen && collider.Entity.Tag == "Player")
            {
                _canOpenChest = true;
            }
        }

        protected override void OnTriggerExit(DCollider collider)
        {
            base.OnTriggerExit(collider);

            if (!_isOpen && collider.Entity.Tag == "Player")
            {
                _canOpenChest = false;
            }
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            if (!_isOpen)
            {
                _interactableButton.Entity.Transform.Position = Transform.Position + new DVec2(0, 1f + (float)Math.Sin(DTime.Time * 7) * 0.1f);
            }

            if (_canOpenChest)
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
