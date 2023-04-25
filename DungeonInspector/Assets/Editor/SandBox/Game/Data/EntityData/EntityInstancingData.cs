using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonInspector
{
    [Serializable]
    public class EntityInstancingData
    {
        // this can represent if an item can be taken only one time, or something spawned one time only
        public bool OneTimeUse { get; set; }
    }
}
