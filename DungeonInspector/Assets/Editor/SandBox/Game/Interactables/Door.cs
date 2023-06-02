using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace DungeonInspector
{
    public class Door : ButtonInteractableBase
    {
        private DSpriteRendererComponent _renderer;
        private DSpriteAtlas _atlas;
        private GameMaster _gameMaster;

        private DTile[] _tiles = new DTile[4];

        protected override string TagTarget => "Player";

        protected override void OnAwake()
        {
            _renderer = GetComp<DSpriteRendererComponent>();
            _renderer.Sprite = _atlas.GetTexture(0);
            _gameMaster = DGameEntity.FindGameEntity("GameMaster").GetComp<GameMaster>();
        }

        protected override void OnStart()
        {
            var basePos = Transform.Position + (DVec2)Vector2.down;

            _tiles[0] = _gameMaster.Tilemap.GetTile(basePos.RoundToInt());
            _tiles[1] = _gameMaster.Tilemap.GetTile((basePos + DVec2.Right).RoundToInt());



            Transform.Offset = new DVec2(0.4f, -0.5f);

            SetDoorStatus(false);
        }

        public void SetAtlas(DSpriteAtlas atlas)
        {
            _atlas = atlas;
        }

        protected override void OnUpdate()
        {
        }

        public void SetDoorStatus(bool isOpen)
        {
            _renderer.Sprite = _atlas.GetTexture(isOpen ? 1 : 0);

            if (isOpen)
            {
                DAudio.PlayAudio("DoorOpen");
            }

            // Check if the actor is above or below to lock/unlock the proper tiles
            for (int i = 0; i < 2; i++)
            {
                _tiles[i].IsWalkable = isOpen;
            }
        }
    }
}
