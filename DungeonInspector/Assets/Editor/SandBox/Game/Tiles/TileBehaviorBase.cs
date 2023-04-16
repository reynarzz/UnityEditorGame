using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonInspector
{
    public interface ITileBehaviorBase
    {
        public void OnEnter(Actor actor);
        public void OnExit(Actor actor);
        public void OnUpdate(Actor actor);
    }

    // _TB: Tile Behavior
    public abstract class TileBehaviorBase : ITileBehaviorBase
    {
        private Actor _actor;
        protected Actor Actor => _actor;

        void ITileBehaviorBase.OnEnter(Actor actor) { _actor = actor; OnEnter(); }
        void ITileBehaviorBase.OnExit(Actor actor) { _actor = actor; OnExit(); }
        void ITileBehaviorBase.OnUpdate(Actor actor) { _actor = actor; OnUpdate(); }

        protected virtual void OnEnter() { }
        protected virtual void OnExit() { }
        protected virtual void OnUpdate() { }


        public bool TryGetActorBehavior<T>(out T behavior) where T : DBehavior, new()
        {
            behavior = _actor.GetComp<T>();

            return behavior != null;
        }
    }
}