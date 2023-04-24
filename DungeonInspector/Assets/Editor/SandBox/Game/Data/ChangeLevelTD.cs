using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonInspector
{
    [Serializable]
    public class ChangeLevelTD : StringDataTD
    {
        public override string Value { get; set; }
    }
}