using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonInspector
{
    public enum Prize
    {
        Key,
        Money,

    }

    [Serializable]
    public class ChestInstancingData : EntityInstancingData
    {
        public Prize Prize { get; set; }
    }
}
