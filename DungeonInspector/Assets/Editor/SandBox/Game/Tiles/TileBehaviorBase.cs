using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonInspector
{
    public interface ITileBehaviorBase
    {
        void OnEnter(Actor actor, BaseTD data);
        void OnExit(Actor actor, BaseTD data);
        void OnUpdate(Actor actor, BaseTD data);
    }

    public abstract class TileBehaviorBase<T> : ITileBehaviorBase where T : BaseTD
    {
        void ITileBehaviorBase.OnEnter(Actor actor, BaseTD data) { OnEnter(actor, GetDataSafe(data)); }
        void ITileBehaviorBase.OnExit(Actor actor, BaseTD data) { OnExit(actor, GetDataSafe(data)); }
        void ITileBehaviorBase.OnUpdate(Actor actor, BaseTD data) { OnUpdate(actor, GetDataSafe(data)); }

        protected virtual void OnEnter(Actor actor, T data) { }
        protected virtual void OnExit(Actor actor, T data) { }
        protected virtual void OnUpdate(Actor actor, T data) { }
        public Sprite Sprite { get; protected set; }

        private T GetDataSafe(BaseTD data)
        {
            if(data != null)
            {
                var d = data as T;

                if (d == null)
                {
                    UnityEngine.Debug.LogError($"Cannot cast '{data.GetType().Name}' to {typeof(T)}");
                }

                return d;
            }

            return null;
        }
    }
}