using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonInspector
{
    public class TileBehaviorsContainer
    {
        private Dictionary<TileBehavior, TileBehaviorBase> _behaviors;

        public TileBehaviorsContainer()
        {
            _behaviors = new Dictionary<TileBehavior, TileBehaviorBase>()
            {
                { TileBehavior.None , new Default_TB() },
                { TileBehavior.ChangeLevel, new ChangeLevel_TB() }
            };
        }

        public ITileBehaviorBase GetBehavior(TileBehavior behavior)
        {
            if(_behaviors.TryGetValue(behavior, out TileBehaviorBase behav))
            {
                return behav;
            }

            UnityEngine.Debug.LogError($"Behavior of type '{behavior}' could not be found, maybe hasn't been added/created.");
            return null;
        }
    }
}
