using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonInspector
{
    // _TB: Tile Behavior
    public abstract class TileBehaviorBase
    {
        public virtual void OnEnter(Player player) { }
        public virtual void OnExit(Player player) { }
        public virtual void OnUpdate(Player player) { }
    }
}