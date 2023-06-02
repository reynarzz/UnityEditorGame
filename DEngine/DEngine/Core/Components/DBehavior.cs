using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonInspector
{
    public interface IDBehavior
    {
        bool IsStarted { get; }
        bool IsAwaken { get; }

        void Awake();
        void Start();
        void Update();
        void FixedUpdate();

        void OnTriggerEnter(DCollider collider);
        void OnTriggerExit(DCollider collider);
        void LateUpdate();
        void OnGUI();
        void OnTriggerStay(DCollider collider);
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
        protected virtual void OnGUI() { }
        protected virtual void OnUpdate() { }
        protected virtual void OnFixedUpdate() { }
        protected virtual void OnLateUpdate() { }
        protected virtual void OnTriggerEnter(DCollider collider) { }
        protected virtual void OnTriggerStay(DCollider collider) { }
        protected virtual void OnTriggerExit(DCollider collider) { }

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

        void IDBehavior.FixedUpdate()
        {
            OnFixedUpdate();
        }

        void IDBehavior.LateUpdate()
        {
            OnLateUpdate();
        }

        void IDBehavior.OnTriggerEnter(DCollider collider)
        {
            OnTriggerEnter(collider);
        }

        void IDBehavior.OnTriggerStay(DCollider collider)
        {
            OnTriggerStay(collider);
        }

        void IDBehavior.OnTriggerExit(DCollider collider)
        {
            OnTriggerExit(collider);
        }

        void IDBehavior.OnGUI()
        {
            OnGUI();
        }
    }
}