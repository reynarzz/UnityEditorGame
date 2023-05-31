using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonInspector
{
    [Serializable]
    public class ChangeLevelTD : BaseTD
    {
        public World World { get; set; }
        public DVec2 SpawnPosition { get; set; }
    }
}