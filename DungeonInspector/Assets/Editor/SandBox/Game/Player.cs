using System;

namespace DungeonInspector
{
    public class Player : DBehavior
    {
        private DAnimatorComponent _playerAnimator;
        private const float _moveSpeed = 10f;
        UnityEngine.Vector2Int _playerWalkDir = new UnityEngine.Vector2Int();

        public override void OnStart()
        {
            var name = "Character2/WalkLeft";
            var walkLeft = GetAnimation(name);
            walkLeft.Speed = 14;

            var idle = GetAnimation("Character2/Idle");
            idle.Speed = 10;

            _playerAnimator = GetComponent<DAnimatorComponent>();

            _playerAnimator.AddAnimation(walkLeft, walkLeft, walkLeft, walkLeft, idle);

            _playerAnimator.Play(4);
            _playerAnimator.Stop();
        }


        private SpriteAnimation GetAnimation(string atlasName)
        {
            return new SpriteAnimation(UnityEngine.Resources.Load<E_SpriteAtlas>(atlasName));
        }


        public override void UpdateFrame()
        {
            var e = UnityEngine.Event.current;
            if (e.keyCode == UnityEngine.KeyCode.F)
            {
                GameEntity.Destroy();
            }
            //Transform.Position = new DVector2(MathF.Cos(DTime.TimeSinceStarted), MathF.Sin(DTime.TimeSinceStarted));

            PlayerMovement();
        }


        private void PlayerMovement()
        {
            var e = UnityEngine.Event.current;
            // var _playerWalkDir = new Vector2Int();


            //if (e.type == UnityEngine.EventType.KeyDown)
            {
                if (e.keyCode == UnityEngine.KeyCode.A)
                {
                    _playerAnimator.Play(0);

                    _playerWalkDir = new UnityEngine.Vector2Int(-1, 0);
                }
                else if (e.keyCode == UnityEngine.KeyCode.D)
                {
                    //_playerPos.x += _moveSpeed * _pixelPerUnit * _dt;
                    _playerWalkDir = new UnityEngine.Vector2Int(1, 0);

                    _playerAnimator.Play(1);

                }
                else if (e.keyCode == UnityEngine.KeyCode.W)
                {
                    _playerAnimator.Play(2);

                    //_playerPos.y += _moveSpeed * _pixelPerUnit * _dt;
                    _playerWalkDir = new UnityEngine.Vector2Int(0, 1);


                }
                else if (e.keyCode == UnityEngine.KeyCode.S)
                {
                    _playerAnimator.Play(3);
                    //_playerPos.y -= _moveSpeed * _pixelPerUnit * _dt;
                    _playerWalkDir = new UnityEngine.Vector2Int(0, -1);

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
            // Debug.Log(_playerWalkDir);


            var dir = (UnityEngine.Vector2)_playerWalkDir;

            Transform.Position += dir * _moveSpeed * DTime.DeltaTime;

            //if(e.type == UnityEngine.EventType.KeyDown && e.keyCode == UnityEngine.KeyCode.A)
            //{
            //    Transform.Position = new DVector2(Transform.Position.x - 1, 0);
            //}
        }
    }
}