using System;
using UnityEngine;

namespace DungeonInspector
{
    public class Player : Actor
    {
        private DAnimatorComponent _playerAnimator;
        private DGameMaster _gameMaster;

        private const float _moveSpeed = 15f;

        private bool _canMove = true;

        public Action<Player, DTile> OnTileReached;

        public DVector2 PrevPosition { get; private set; }
        private ActorHealth _health;
        private DRendererComponent _renderer;
        DVector2 _gridPos = default;

        protected override void OnAwake()
        {
            var name = "Character2/WalkLeft";
            var walkLeft = GetAnimation(name);
            walkLeft.Speed = 14;

            var idle = GetAnimation("Character2/Idle");
            idle.Speed = 5;

            _playerAnimator = GetComp<DAnimatorComponent>();

            _playerAnimator.AddAnimation(idle, walkLeft);

            _gameMaster = FindGameEntity("GameMaster").GetComp<DGameMaster>();
            _renderer = GetComp<DRendererComponent>();
            _renderer.Sprite = idle.CurrentTexture;

            _health = AddComp<ActorHealth>();
            Transform.Offset = new DVector2(0, 0.7f);
        }

        protected override void OnStart()
        {
        }

        private SpriteAnimation GetAnimation(string atlasName)
        {
            return new SpriteAnimation(UnityEngine.Resources.Load<E_SpriteAtlas>(atlasName));
        }


        protected override void OnUpdate()
        {
            PlayerMovement();
        }

        private void PlayerMovement()
        {
            var e = Event.current;

            if (e.type == UnityEngine.EventType.KeyDown)
            {
                if (_canMove)
                {
                    if (e.keyCode == UnityEngine.KeyCode.A)
                    {
                        //e.Use();

                        _gridPos = GetMoveDir(_gridPos, -1, 0);

                        _canMove = false;
                        _renderer.FlipX = true;
                    }
                    else if (e.keyCode == UnityEngine.KeyCode.D)
                    {
                        //e.Use();

                        _gridPos = GetMoveDir(_gridPos, 1, 0);

                        _canMove = false;

                        _renderer.FlipX = false;
                    }
                    else if (e.keyCode == UnityEngine.KeyCode.W)
                    {
                        //e.Use();

                        _gridPos = GetMoveDir(_gridPos, 0, 1);

                        _canMove = false;
                    }
                    else if (e.keyCode == UnityEngine.KeyCode.S)
                    {
                        //e.Use();

                        _gridPos = GetMoveDir(_gridPos, 0, -1);

                        _canMove = false;
                    }
                }
            }


            Transform.Position = UnityEngine.Vector2.MoveTowards(Transform.Position, _gridPos, DTime.DeltaTime * 3);

            if ((UnityEngine.Vector2Int)Transform.Position.Round() == (UnityEngine.Vector2Int)_gridPos.Round())
            {
                if (!_canMove)
                {
                    _gameMaster.OnActorEnterTile(this, _gameMaster.Tilemap.GetTile(Transform.Position, 0));

                    _canMove = true;
                }

                if (!e.isKey)
                {
                    _playerAnimator.Play(0);
                }
            }
            else
            {
                _playerAnimator.Play(1);
            }
        }

        private DVector2 GetMoveDir(DVector2 currentPos, int x, int y)
        {
            var destine = new Vector2Int((int)currentPos.x + x, (int)currentPos.y + y);

            if (_gameMaster.Tilemap.IsTileWalkable(destine.x, destine.y))
            {
                return destine;
            }

            return currentPos;
        }
    }
}