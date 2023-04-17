using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonInspector
{
    public class TileBehaviorsContainer
    {
        private Dictionary<TileBehavior, ITileBehaviorBase> _behaviors;

        public TileBehaviorsContainer()
        {
            _behaviors = new Dictionary<TileBehavior, ITileBehaviorBase>()
            {
                { TileBehavior.None , new DefaultTB() },
                { TileBehavior.ChangeLevel, new ChangeLevelTB() }
            };
        }

        public ITileBehaviorBase GetBehavior(TileBehavior behavior)
        {
            if(_behaviors.TryGetValue(behavior, out ITileBehaviorBase behav))
            {
                return behav;
            }

            UnityEngine.Debug.LogError($"Behavior of type '{behavior}' could not be found, maybe hasn't been added/created.");
            return null;
        }
    }
}
