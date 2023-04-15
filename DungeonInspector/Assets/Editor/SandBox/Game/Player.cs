using System;
using UnityEngine;

namespace DungeonInspector
{
    public class Player : DBehavior
    {
        private DAnimatorComponent _playerAnimator;
        private DGameMaster _gameMaster;

        private const float _moveSpeed = 15f;
        UnityEngine.Vector2Int _playerWalkDir = new UnityEngine.Vector2Int();

        private DVector2 _gridPos;
        private bool _canMove = true;
        private float _moveTime = 0;
        private const float _maxTime = 0.1f;

        public override void OnStart()
        {
            var name = "Character2/WalkLeft";
            var walkLeft = GetAnimation(name);
            walkLeft.Speed = 14;

            var idle = GetAnimation("Character2/Idle");
            idle.Speed = 7;

            _playerAnimator = GetComponent<DAnimatorComponent>();

            _playerAnimator.AddAnimation(walkLeft, walkLeft, walkLeft, walkLeft, idle);

            _playerAnimator.Play(4);
            _playerAnimator.Stop();

            _gameMaster = FindGameEntity("GameMaster").GetComponent<DGameMaster>();
        }


        private SpriteAnimation GetAnimation(string atlasName)
        {
            return new SpriteAnimation(UnityEngine.Resources.Load<E_SpriteAtlas>(atlasName));
        }


        public override void OnUpdate()
        {
            var e = UnityEngine.Event.current;
            if (e.keyCode == UnityEngine.KeyCode.F)
            {
                GameEntity.Destroy();
            }

            PlayerMovement();
        }


        private void PlayerMovement()
        {
            var e = UnityEngine.Event.current;
            // var _playerWalkDir = new Vector2Int();


            if (e.type == UnityEngine.EventType.KeyDown && _canMove)
            {
                if (e.keyCode == UnityEngine.KeyCode.A)
                {
                    _playerAnimator.Play(0);

                    _gridPos = GetMoveDir(_gridPos, -1, 0);

                    _canMove = false;
                    _moveTime = 0;

                    //_playerWalkDir = new UnityEngine.Vector2Int(-1, 0);
                }
                else if (e.keyCode == UnityEngine.KeyCode.D)
                {
                    //_playerWalkDir = new UnityEngine.Vector2Int(1, 0);
                    _gridPos = GetMoveDir(_gridPos, 1, 0);

                    _canMove = false;

                    _playerAnimator.Play(1);
                    _moveTime = 0;


                }
                else if (e.keyCode == UnityEngine.KeyCode.W)
                {
                    _playerAnimator.Play(2);
                    _gridPos = GetMoveDir(_gridPos, 0, 1);

                    _canMove = false;
                    _moveTime = 0;

                    //_playerWalkDir = new UnityEngine.Vector2Int(0, 1);


                }
                else if (e.keyCode == UnityEngine.KeyCode.S)
                {
                    _gridPos = GetMoveDir(_gridPos, 0, -1);

                    _canMove = false;
                    _moveTime = 0;

                    _playerAnimator.Play(3);
                    //_playerWalkDir = new UnityEngine.Vector2Int(0, -1);

                }
                else //if (_playerWalkDir.x == 0 && _playerWalkDir.y == 0)
                {
                    //_playerAnimator.Play(4);
                    _playerWalkDir = default;
                }
                //if (e.type == EventType.KeyUp || !e.isKey)
                //{
                //    _playerWalkDir = default;
                //   // _playerAnimator.Stop();
                //}
            }
            else if (_canMove)
            {
                _playerAnimator.Play(4);
            }

            // Debug.Log(_playerWalkDir);

            var dir = (UnityEngine.Vector2)_playerWalkDir;


            _moveTime += DTime.DeltaTime;

            Transform.Position = UnityEngine.Vector2.MoveTowards(Transform.Position, _gridPos, DTime.DeltaTime * 3);

            if ((UnityEngine.Vector2Int)Transform.Position == (UnityEngine.Vector2Int)_gridPos)
            {
                _canMove = true;
            }

            //Transform.Position += dir * _moveSpeed * DTime.DeltaTime;
        }

        private DVector2 GetMoveDir(DVector2 currentPos, int x, int y)
        {
            var destine = new Vector2Int((int)currentPos.x + x, (int)currentPos.y + y);

            if (_gameMaster.CurrentWorldData.IsWalkable(destine.x, destine.y))
            {
                return destine;
            }

            return destine/*currentPos*/;
        }
    }
}