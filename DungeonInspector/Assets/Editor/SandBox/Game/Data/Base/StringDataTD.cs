using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonInspector
{
    [Serializable]
    public class StringDataTD : BaseTD
    {
        public virtual string Value { get; set; }
    }
}
