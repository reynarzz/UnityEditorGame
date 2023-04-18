using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DungeonInspector
{
    public interface IDBehavior
    {
        public bool IsStarted { get; }
        public bool IsAwaken { get; }

        public void Awake();
        public void Start();
        public void Update();
    }

    public abstract class DBehavior : DTransformableComponent, IDBehavior
    {
        public string Name => Entity.Name;

        private bool _isStarted = false;
        private bool _isAwaken = false;

        bool IDBehavior.IsStarted => _isStarted;
        bool IDBehavior.IsAwaken => _isAwaken;

        protected virtual void OnAwake() { }
        protected virtual void OnStart() { }
        protected virtual void OnUpdate() { }

        public T GetComp<T>() where T : DComponent, new()
        {
            return Entity.GetComp<T>();
        }

        public T AddComp<T>() where T : DComponent, new()
        {
            return Entity.AddComp<T>();
        }

        public bool TryGetComp<T>(out T behavior) where T : DBehavior, new()
        {
            behavior = Entity.GetComp<T>();

            return behavior != null;
        }

        
        void IDBehavior.Awake()
        {
            _isAwaken = true;
            OnAwake();
        }

        void IDBehavior.Start()
        {
            _isStarted = true;
            OnStart();
        }

        void IDBehavior.Update()
        {
            OnUpdate();
        }

    }
}
