using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonInspector
{
    public class EnemyBase : Actor
    {
        private DRendererComponent _renderer;
        private HealthBarUI _healthBar;
        private DBoxCollider _collider;
        protected ActorHealth _health;
        private NavWorld _navWorld;

        private const float _healthBarYOffset = 0.3f;
        private const float _isHitMaxTime = 0.25f;

        private float _isHitTime;
        private bool _isHit;
        public float WalkSpeed { get; set; } = 2;
        public string Tag { get; set; } = "Player";
        public Actor Target { get; set; }
        private Player _playerTest;
        private int _pathIndex;
        private List<DVector2> _pathToTarget;
        private DVector2 _movePos;
        private DVector2 _prevPos;

        private bool _canMove = true;
        private DTilemap _tilemap;
        private DTile _prevTile;
        private static bool _recalcPath;

        protected override void OnAwake()
        {
            Transform.Offset = new DVector2(0, 0.7f);
            _healthBar = GetComp<HealthBarUI>();
            _collider = GetComp<DBoxCollider>();
            _health = GetComp<ActorHealth>();
            _renderer = GetComp<DRendererComponent>();
            _health.OnHealthChanged += OnHealthChanged;
            _health.OnHealthDepleted += OnHealthDepleted;
        }

        protected override void OnStart()
        {
            var gameMaster = DGameEntity.FindGameEntity("GameMaster").GetComp<GameMaster>();

            _navWorld = gameMaster.NavWorld;
            _tilemap = gameMaster.Tilemap;

            _playerTest = DGameEntity.FindGameEntity("Player").GetComp<Player>();
            _pathToTarget = _navWorld.GetPathToTarget(this, _playerTest);
            _pathIndex = 0;

            _movePos = Transform.Position;
        }

        private void OnHealthChanged(float amount, float max, bool increased)
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

        protected override void OnUpdate()
        {
            _healthBar.Transform.Position = new DVector2(Transform.Position.x, _collider.AABB.Max.y + _healthBarYOffset);

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

            Walk();
        }

        private void Walk()
        {
            if(DInput.IsKeyDown(KeyCode.F))
            {
                _recalcPath = true;
                
            }

            if (_recalcPath)
            {
                _recalcPath = false;
                _pathIndex = 0;

                _pathToTarget = _navWorld.GetPathToTarget(this, _playerTest);
                _pathToTarget.RemoveAt(_pathToTarget.Count - 1);
                _prevPos = _movePos = Transform.Position.RoundToInt();
                _prevTile.IsOccupied = false;
            }

            if (_pathToTarget != null && _pathToTarget.Count > 0 && _pathToTarget.Count > _pathIndex)
            {
                var nextPos = GetNextPos();

                var tile = _tilemap.GetTile(nextPos, 0);
                
                if ((_movePos - Transform.Position).SqrMagnitude <= 0.001f && !tile.IsOccupied && _playerTest.Transform.RoundPosition != nextPos)
                {
                    _pathIndex++;
                    tile.IsOccupied = true;
                    _prevTile = tile;

                    _prevPos = _movePos;

                    _tilemap.GetTile(_prevPos, 0).IsOccupied = false;
                    _movePos = nextPos;
                }
            }

            Transform.Position = Vector2.MoveTowards(Transform.Position, _movePos, DTime.DeltaTime * WalkSpeed);
        }


        private DVector2 GetNextPos()
        {
            if(_pathToTarget.Count > _pathIndex + 1)
            {
                return _pathToTarget[_pathIndex + 1];
            }

            return _movePos;
        }
        protected virtual void OnHealthDepleted()
        {
            Entity.Destroy();
        }


        public override void OnDestroy()
        {
            _health.OnHealthChanged -= OnHealthChanged;
        }
    }
}