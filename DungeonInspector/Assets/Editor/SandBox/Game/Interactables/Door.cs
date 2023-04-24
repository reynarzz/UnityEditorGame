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
        private DRendererComponent _renderer;
        private DSpriteAtlas _atlas;
        private GameMaster _gameMaster;

        private DTile[] _tiles = new DTile[4];
        protected override void OnAwake()
        {
            _renderer = GetComp<DRendererComponent>();
            _renderer.Sprite = _atlas.GetTexture(0);
            _gameMaster = DGameEntity.FindGameEntity("GameMaster").GetComp<GameMaster>();
        }

        protected override void OnStart()
        {
            var basePos = Transform.Position + (DVec2)Vector2.down;

            _tiles[0] = _gameMaster.Tilemap.GetTile(basePos.RoundToInt(), 0);
            _tiles[1] = _gameMaster.Tilemap.GetTile((basePos + DVec2.Right).RoundToInt(), 0);



            Transform.Offset = new DVec2(0.28f, 0);
        }
        public void SetAtlas(DSpriteAtlas atlas)
        {
            _atlas = atlas;
        }

        protected override void OnUpdate()
        {
            //GUI.DrawTexture(new Rect(200, 200, 100, 100), _renderer.Sprite);
            var value = (int)Math.Round(((float)Math.Sin(DTime.Time) + 1) * 0.5f);
            _renderer.Sprite = _atlas.GetTexture(value);

            // Check if the actor is above or below to lock/unlock the proper tiles
            for (int i = 0; i < 2; i++)
            {
                
                _tiles[i].IsWalkable = value == 1;
            }
        }
    }
}
