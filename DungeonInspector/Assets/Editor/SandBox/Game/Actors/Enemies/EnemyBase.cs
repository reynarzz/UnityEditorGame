using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonInspector
{
    public  class EnemyBase : Actor
    {
        private DSpriteRendererComponent _renderer;
        private HealthBarUI _healthBar;
        private DBoxCollider _collider;
        protected ActorHealth _health;
        private NavWorld _navWorld;

        private const float _healthBarYOffset = 0.3f;
        private const float _isHitMaxTime = 0.25f;
        protected virtual int StartingHealth { get; }

        private float _isHitTime;
        private bool _isHit;
        public float WalkSpeed { get; set; } = 2;
        public string Tag { get; set; } = "Player";
        public Actor Target { get; set; }
        private Player _playerTest;
        private int _pathIndex;
        private List<DVec2> _pathToTarget;
        private DVec2 _movePos;
        private DVec2 _prevPos;

        //private bool _canMove = true;
        private DTilemap _tilemap;
        private DTile _prevTile;

        private DAnimatorComponent _animator;
        public event Action<EnemyBase> OnEnemyBeaten;

        protected override void OnAwake()
        {
            Transform.Offset = new DVec2(0, 0.7f);
            _healthBar = GetComp<HealthBarUI>();
            _collider = GetComp<DBoxCollider>();
            _health = GetComp<ActorHealth>();
            _renderer = GetComp<DSpriteRendererComponent>();
            _animator = GetComp<DAnimatorComponent>();

            _health.OnHealthChanged += OnHealthChanged;
            _health.OnHealthDepleted += OnHealthDepleted;

        }

        protected override void OnStart()
        {
            var gameMaster = DGameEntity.FindGameEntity("GameMaster").GetComp<GameMaster>();

            _navWorld = gameMaster.NavWorld;
            _tilemap = gameMaster.Tilemap;

            _playerTest = DGameEntity.FindGameEntity("Player").GetComp<Player>();
         
            _pathIndex = 0;

            _movePos = Transform.Position;

            _health.SetInitialHealth(StartingHealth);
        }

        protected virtual void OnHealthChanged(float amount, float max, bool increased)
        {
            if (!increased)
            {
                _renderer.SetMatInt("_isHit", 1);
                _isHitTime = 0;
                _isHit = true;
            }

            _healthBar.OnChancePercentage(amount / max);
        }

        //protected override void OnTriggerEnter(DBoxCollider collider)
        //{
        //    if (collider.Entity.Tag == Tag)
        //    {
        //        _health.AddAmount(-1);
        //    }
        //}

        public void OnNewPath(List<DVec2> pathToTarget)
        {
            _pathToTarget = pathToTarget;

            _pathIndex = 0;

            if (_pathToTarget != null && _pathToTarget.Count > 0)
            {
                //--_pathToTarget.RemoveAt(_pathToTarget.Count - 1);

                _movePos = Transform.Position.RoundToInt();

                _prevPos = _movePos.RoundToInt();

                if (_prevTile != null)
                {
                    _prevTile.Ocupe = null;
                }
            }

        }

        protected override void OnUpdate()
        {
            _healthBar.Transform.Position = new DVec2(Transform.Position.x, _collider.AABB.Max.y + _healthBarYOffset);

            if (_isHit)
            {
                _isHitTime += DTime.DeltaTime;

                if (_isHitTime >= _isHitMaxTime)
                {
                    _renderer.RemoveMatValue("_isHit");
                    _isHit = false;
                    _isHitTime = 0;
                }
            }

            if (_pathToTarget == null || (_pathToTarget != null && _pathToTarget.Count > 0 &&
               (_playerTest.Transform.Position - _pathToTarget[_pathToTarget.Count - 1]).SqrMagnitude > 2))
            {
                _navWorld.RequestPath(this, _playerTest);
            }

            Walk();
        }

        private void Walk()
        {
            if (_pathToTarget != null && _pathToTarget.Count > 0 && _pathToTarget.Count > _pathIndex)
            {
                var nextPos = GetNextPos();

                var tile = _tilemap.GetTile(nextPos);

                if ((_movePos - Transform.Position).SqrMagnitude <= 0.001f && !tile.IsOccupied /*&& _playerTest.Transform.RoundPosition != nextPos*/)
                {
                    _pathIndex++;
                    tile.Ocupe = Entity;
                    _prevTile = tile;

                    _prevPos = _movePos.RoundToInt();

                    _tilemap.GetTile(_prevPos).Ocupe = null;
                    _movePos = nextPos;


                    _animator.Play(1);
                }
            }

            Transform.Position = Vector2.MoveTowards(Transform.Position, _movePos, DTime.DeltaTime * WalkSpeed);
        }


        private DVec2 GetNextPos()
        {
            if (_pathToTarget.Count > _pathIndex + 1)
            {
                return _pathToTarget[_pathIndex + 1];
            }

            return _movePos;
        }

        protected virtual void OnHealthDepleted()
        {
            OnEnemyBeaten?.Invoke(this);
            Entity.Destroy();
        }


        public override void OnDestroy()
        {
            _health.OnHealthChanged -= OnHealthChanged;
        }
    }
}