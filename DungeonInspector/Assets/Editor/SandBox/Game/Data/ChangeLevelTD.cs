using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonInspector
{
    [Serializable]
    public class ChangeLevelTD : IntDataTD
    {
        public override int Value { get; set; }
    }
}