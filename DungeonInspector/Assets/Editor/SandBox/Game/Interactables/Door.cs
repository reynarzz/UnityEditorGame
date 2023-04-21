using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace DungeonInspector
{
    public class Door : InteractableBase
    {
        private DRendererComponent _renderer;
        private DSpriteAtlas _atlas;
        private GameMaster _gameMaster;

        private DTile[] _blockedTiles = new DTile[2];
        protected override void OnAwake()
        {
            _renderer = GetComp<DRendererComponent>();
            _renderer.Sprite = _atlas.GetTexture(0);
            _gameMaster = DGameEntity.FindGameEntity("GameMaster").GetComp<GameMaster>();
        }

        protected override void OnStart()
        {
            var basePos = Transform.Position + (DVector2)Vector2.down;

            _blockedTiles[0] = _gameMaster.Tilemap.GetTile(basePos.RoundToInt(), 0);
            _blockedTiles[1] = _gameMaster.Tilemap.GetTile((basePos + DVector2.Right).RoundToInt(), 0);
            Transform.Offset = new DVector2(0.28f, 0);
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

            for (int i = 0; i < _blockedTiles.Length; i++)
            {
                _blockedTiles[i].IsWalkable = value == 1;
            }
        }

        public override void OnInteracted(InteractionType interaction, Actor actor)
        {
        }
    }
}
