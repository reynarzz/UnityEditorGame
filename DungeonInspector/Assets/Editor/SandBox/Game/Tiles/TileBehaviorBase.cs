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
        void OnEnter(Actor actor, BaseTD data, DTile tile);
        void OnExit(Actor actor, BaseTD data, DTile tile);
        void OnUpdate(Actor actor, BaseTD data, DTile tile);
    }

    public class TileBehaviorBase<T> : ITileBehaviorBase where T : BaseTD
    {
        void ITileBehaviorBase.OnEnter(Actor actor, BaseTD data, DTile tile) { OnEnter(actor, GetDataSafe(data), tile); }
        void ITileBehaviorBase.OnExit(Actor actor, BaseTD data, DTile tile) { OnExit(actor, GetDataSafe(data), tile); }
        void ITileBehaviorBase.OnUpdate(Actor actor, BaseTD data, DTile tile) { OnUpdate(actor, GetDataSafe(data), tile); }

        protected virtual void OnEnter(Actor actor, T data, DTile tile) { }
        protected virtual void OnExit(Actor actor, T data, DTile tile) { }
        protected virtual void OnUpdate(Actor actor, T data, DTile tile) { }

        private T GetDataSafe(BaseTD data)
        {
            if (data != null)
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