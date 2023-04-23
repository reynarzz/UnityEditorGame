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
        DVec2 _gridPos = default;

        private Vector2Int _moveDir = default;

        private DGameEntity _weaponTest;
        private DRendererComponent _weaponRendererTest;
        private DRendererComponent _rayHitGuideTest;
        private DRendererComponent _rayDraw;

        private float _lookDirAngle;
        private float _shootTime;
        private const float _shootCooldown = 0.1f;
        public bool IsPlayerDead { get; private set; }

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

            var collider = AddComp<DBoxCollider>();
            collider.IsTrigger = true;

            collider.Center = new DVec2(0, -0.75f);
            collider.Size = new DVec2(0.78f, 0.79f);

            AddComp<DPhysicsComponent>();
            _health = AddComp<ActorHealth>();
            _health.SetInitialHealth(9);
            _health.OnHealthDepleted += OnHealthDepleted;
            _weaponTest = new DGameEntity("WeaponTest");
            _weaponRendererTest = _weaponTest.AddComp<DRendererComponent>();

            _weaponRendererTest.Sprite = Resources.Load<Texture2D>("GameAssets/Dungeon/weapon_knife");
            _weaponRendererTest.ZSorting = 1;
            Entity.Tag = "Player";
            Entity.Layer = 2;

            //_health = AddComp<ActorHealth>();
            //_health.EnemyTag = "Enemy";

            _rayDraw = new DGameEntity("RayGuide", typeof(DRendererComponent)).GetComp<DRendererComponent>();
            _rayDraw.ZSorting = 3;
            _rayHitGuideTest = new DGameEntity("RayGuide", typeof(DRendererComponent)).GetComp<DRendererComponent>();
        }

        private void OnHealthDepleted()
        {
            IsPlayerDead = true;
            Entity.Destroy();
        }

        protected override void OnStart()
        {
            Transform.Offset = new DVec2(0, 0.7f);

            Transform.Position = _gridPos = new DVec2(3, 0);

            _rayHitGuideTest.Transform.Scale = new DVec2(0.2f, 0.2f);
            _rayHitGuideTest.ZSorting = 3;
            _rayDraw.ZSorting = 3;
            _rayDraw.Entity.IsActive = false;
        }

        private float _timeToGetHit;
        private const float _timeToGetHitCooldown = 1;

        protected override void OnTriggerEnter(DBoxCollider collider)
        {
            //Debug.Log("Enter: " + collider.Name);

            //collider.Entity.Destroy();

            _timeToGetHit -= DTime.DeltaTime;

            if (_timeToGetHit <= 0)
            {
                _timeToGetHit = _timeToGetHitCooldown;
                _health.AddAmount(-1);
            }
        }


        private DSpriteAnimation GetAnimation(string atlasName)
        {
            var atlas = UnityEngine.Resources.Load<DSpriteAtlas>(atlasName);
            return new DSpriteAnimation(atlas);
        }


        protected override void OnUpdate()
        {
            PlayerMovement();
        }
        private bool _testHitDamageRay;

        private void PlayerMovement()
        {
            if (_canMove)
            {
                if (DInput.IsKey(UnityEngine.KeyCode.A))
                {
                    _moveDir.x = -1;
                    _moveDir.y = 0;
                }
                else if (DInput.IsKey(UnityEngine.KeyCode.D))
                {
                    _moveDir.x = 1;
                    _moveDir.y = 0;
                }
                else if (DInput.IsKey(UnityEngine.KeyCode.W))
                {
                    _moveDir.x = 0;
                    _moveDir.y = 1;
                }
                else if (DInput.IsKey(UnityEngine.KeyCode.S))
                {
                    _moveDir.x = 0;
                    _moveDir.y = -1;
                }
                else
                {
                    _moveDir.x = 0;
                    _moveDir.y = 0;
                }

                if (_moveDir.x != 0 || _moveDir.y != 0)
                {
                    _canMove = false;
                    _gridPos = GetMoveDir(_gridPos, _moveDir.x, _moveDir.y);
                }
            }

            var mouseDiff = DInput.GetMouseWorldPos() - Transform.Position;

            _lookDirAngle = Mathf.Atan2(mouseDiff.y, mouseDiff.x);

            var dist = Transform.Position - DCamera._Position;

            var dir = new DVec2(Mathf.Cos(_lookDirAngle), Mathf.Sin(_lookDirAngle));

            _weaponTest.Transform.Position = (Transform.Position + Transform.Offset - dist);/*+ */;
            _weaponTest.Transform.Rotation = _lookDirAngle + Mathf.Deg2Rad * -90;

            Transform.Position = UnityEngine.Vector2.MoveTowards(Transform.Position, _gridPos, DTime.DeltaTime * 3);


            // if (DVec2.Dot(DVec2.Right, DInput.GetMouseWorldPos() - Transform.Position) < 0)
            _renderer.FlipX = DInput.GetMouseWorldPos().x - Transform.Position.x < 0;

            _weaponTest.Transform.Scale = new DVec2(1 * Mathf.Sign(DInput.GetMouseWorldPos().x - Transform.Position.x), 1);
            //if (Utils.Raycast(Transform.Position, dir, 0, out var info))
            //{
            //    //_rayHitGuideTest.Entity.IsActive = true;
            //    //_rayHitGuideTest.Entity.Transform.Position = info.Point;


            //    //var magnitude = (_rayHitGuideTest.Entity.Transform.Position + Transform.Position).Magnitude;

            //    //_rayDraw.Entity.Transform.Position = Transform.Position;// + new DVec2(magnitude / 2, 0);
            //    //_rayDraw.Entity.Transform.Rotation = angle;
            //    //_rayDraw.Transform.Scale = new DVec2(magnitude, 0.06f);
            //}
            //else
            //{
            //    _testHitDamageRay = false;
            //    _rayHitGuideTest.Entity.IsActive = false;
            //}

            if (Transform.Position.RoundToInt() == _gridPos.RoundToInt() && !_canMove && !_tileEnter)
            {
                _tileEnter = true;
                var currentTile = _gameMaster.Tilemap.GetTile(Transform.Position.Round(), 0);

                if (currentTile != null)
                {
                    _gameMaster.OnActorEnterTile(this, currentTile);
                }
            }

            if ((Transform.Position - _gridPos).SqrMagnitude >= 0.01f)
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

            if (_shootTime <= 0)
            {
                Shoot();
            }
            else
            {
                _shootTime -= DTime.DeltaTime;
            }
            //_playerAnimator.Stop();
        }


        private void Shoot()
        {
            if (DInput.IsMouseDown(0))
            {
                _shootTime = _shootCooldown;

                var dir = new DVec2(Mathf.Cos(_lookDirAngle), Mathf.Sin(_lookDirAngle));

                var projectile = _gameMaster.PrefabInstantiator.InstanceBullet1().GetComp<Projectile>();

                projectile.Transform.Position = (dir / 2) + _weaponTest.Transform.Position - Transform.Offset / 2;// Transform.Position;
                projectile.Shoot(dir);

                //DAudio.PlayAudio("Audio/ForgottenPlains/Fx/16_Hit_on_brick_1.wav");
            }
        }

        private DVec2 GetMoveDir(DVec2 currentPos, int x, int y)
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