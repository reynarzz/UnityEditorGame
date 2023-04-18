using System;
using UnityEngine;

namespace DungeonInspector
{
    // dungeon with pokemon-like battles.
    public class Player : Actor
    {
        private DAnimatorComponent _playerAnimator;
        private GameMaster _gameMaster;

        private const float _moveSpeed = 15f;

        private bool _canMove = true;
        private bool _tileEnter = false;

        public Action<Player, DTile> OnTileReached;

        private ActorHealth _health;
        private DRendererComponent _renderer;
        DVector2 _gridPos = default;
        private Vector2Int _moveDir = default;
        private bool _tryingToWalk;

        protected override void OnAwake()
        {
            var name = "Character2/WalkLeft";
            var walk = GetAnimation(name);
            walk.Speed = 14;

            var idle = GetAnimation("Character2/Idle");
            idle.Speed = 5;

            _playerAnimator = GetComp<DAnimatorComponent>();

            _playerAnimator.AddAnimation(idle, walk);

            _gameMaster = DGameEntity.FindGameEntity("GameMaster").GetComp<GameMaster>();
            _renderer = GetComp<DRendererComponent>();
            _renderer.Sprite = idle.CurrentTexture;

            AddComp<DBoxCollider>();
            AddComp<DPhysicsComponent>();
            _health = AddComp<ActorHealth>();

        }

        protected override void OnTriggerEnter(DBoxCollider collider)
        {
            Debug.Log("Enter: " + collider.Name);

            collider.Entity.Destroy();
        }

        protected override void OnStart()
        {
            Transform.Offset = new DVector2(0, 0.7f);

            _gridPos = new DVector2(0, -3);
            Transform.Position = new DVector2(0, -3);
        }

        private DSpriteAnimation GetAnimation(string atlasName)
        {
            return new DSpriteAnimation(UnityEngine.Resources.Load<E_SpriteAtlas>(atlasName));
        }


        protected override void OnUpdate()
        {
            PlayerMovement();
        }

        private void PlayerMovement()
        {
            if (_canMove)
            {
                if (DInput.IsKey(UnityEngine.KeyCode.A))
                {
                    _moveDir.x = -1;
                    _moveDir.y = 0;
                    _renderer.FlipX = true;
                    _tryingToWalk = true;
                }
                else if (DInput.IsKey(UnityEngine.KeyCode.D))
                {
                    _moveDir.x = 1;
                    _moveDir.y = 0;

                    _renderer.FlipX = false;
                    _tryingToWalk = true;
                }
                else if (DInput.IsKey(UnityEngine.KeyCode.W))
                {
                    _moveDir.x = 0;
                    _moveDir.y = 1;
                    _tryingToWalk = true;
                }
                else if (DInput.IsKey(UnityEngine.KeyCode.S))
                {
                    _moveDir.x = 0;
                    _moveDir.y = -1;
                    _tryingToWalk = true;
                }
                else
                {
                    _tryingToWalk = false;

                    _moveDir.x = 0;
                    _moveDir.y = 0;
                }

                if (_moveDir.x != 0 || _moveDir.y != 0)
                {
                    _canMove = false;
                    _gridPos = GetMoveDir(_gridPos, _moveDir.x, _moveDir.y);
                }
            }

            Transform.Position = UnityEngine.Vector2.MoveTowards(Transform.Position, _gridPos, DTime.DeltaTime * 3);

            if (Transform.Position.RoundToInt() == _gridPos.RoundToInt() && !_canMove && !_tileEnter)
            {
                _tileEnter = true;
                var currentTile = _gameMaster.Tilemap.GetTile(Transform.Position.Round(), 0);

                _gameMaster.OnActorEnterTile(this, currentTile);
            }

            if ((Transform.Position - _gridPos).SqrMagnitude >= 0.0001f)
            {
                _playerAnimator.Play(1);
            }
            else
            {
                _canMove = true;
                _tileEnter = false;

                _playerAnimator.Play(0);

                //if (!_tryingToWalk)
                //{
                //    _playerAnimator.Play(0);
                //}
                //else
                //{
                //    _playerAnimator.Play(1);

                //}
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