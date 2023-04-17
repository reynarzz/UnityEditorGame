using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonInspector
{
    public interface ITileBehaviorBase
    {
        public void OnEnter(Actor actor, BaseTD data);
        public void OnExit(Actor actor, BaseTD data);
        public void OnUpdate(Actor actor, BaseTD data);
    }

    public abstract class TileBehaviorBase<T> : ITileBehaviorBase where T : BaseTD
    {
        void ITileBehaviorBase.OnEnter(Actor actor, BaseTD data) { OnEnter(actor, data as T); }
        void ITileBehaviorBase.OnExit(Actor actor, BaseTD data) { OnExit(actor, data as T); }
        void ITileBehaviorBase.OnUpdate(Actor actor, BaseTD data) { OnUpdate(actor, data as T); }

        protected virtual void OnEnter(Actor actor, T data) { }
        protected virtual void OnExit(Actor actor, T data) { }
        protected virtual void OnUpdate(Actor actor, T data) { }


    }
}