using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonInspector
{
    public class DTilemapAnimatorComponent : DBehavior
    {
        private DTilemap _tilemap;
        private Dictionary<DVec2, DTile> _animatedTiles;
        private List<DVec2> _tilesPlaying;
        public bool PlayOnAwake { get; set; }
        protected override void OnAwake()
        {
            _tilemap = GetComp<DTilemap>();

            if (_tilemap == null)
            {
                Debug.Log($"Object: {Name} doesn't contain A DTIlemap component.");
            }
            else
            {
                _tilesPlaying = new List<DVec2>();
                _animatedTiles = new Dictionary<DVec2, DTile>();

                Init();

                if (PlayOnAwake)
                {
                    PlayAllTilesAnim();
                }
            }
        }

        private void Init()
        {
            _animatedTiles.Clear();

            var tiles = _tilemap.GetAllTiles();

            for (int i = 0; i < tiles.Count; i++)
            {
                var tile = tiles[i];

                if (tile.AnimationAtlas != null)
                {
                   tile.Animation = new DSpriteAnimation(tile.AnimationAtlas);

                    if (!_animatedTiles.ContainsKey(tile.Position))
                    {
                        _animatedTiles.Add(tile.Position, tile);
                    }
                }
            }

            if(_animatedTiles.Count > 0)
            {
                Debug.Log("Animated : " + _animatedTiles.Count);
            }
        }

        protected override void OnUpdate()
        {
            for (int i = 0; i < _tilesPlaying.Count; i++)
            {
                if(_animatedTiles.TryGetValue(_tilesPlaying[i], out var tile))
                {
                    var animation = tile.Animation;

                    animation.Update(DTime.DeltaTime);

                    tile.Texture = animation.CurrentTexture;
                }
            }
        }

        public void PlayTileAnim(int x, int y)
        {
            var pos = new DVec2(x, y);
            if (_animatedTiles.TryGetValue(pos, out DTile tile))
            {
                tile.Animation.Play();
                _tilesPlaying.Add(pos);
            }
            else
            {
                Debug.LogError($"No animated tiles exist at ({x}, {y}).");
            }
        }

        public void StopTileAnim(int x, int y)
        {
            var pos = new DVec2(x, y);

            if (_animatedTiles.TryGetValue(new DVec2(x, y), out DTile tile))
            {
                tile.Animation.Stop();
                _tilesPlaying.Remove(pos);
            }
            else
            {
                Debug.LogError($"No animated tiles exist at ({x}, {y}).");
            }
        }

        public void PlayAllTilesAnim()
        {

            Debug.Log("Play all tiles");
            for (int i = 0; i < _animatedTiles.Count; i++)
            {
                var tile = _animatedTiles.ElementAt(i);
                var pos = tile.Key;
                tile.Value.Animation.Play();
                _tilesPlaying.Add(pos);
            }
        }

        public void StopAllTileAnim()
        {
            for (int i = 0; i < _animatedTiles.Count; i++)
            {
                var pos = _tilesPlaying[i];

                _animatedTiles[pos].Animation.Stop();
            }

            _tilesPlaying.Clear();
        }
    }
}