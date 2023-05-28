using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DungeonInspector
{
    public class SpikeFloorTD : BaseTD
    {
        public int Damage { get; set; } = 1;
        public int DelayToPull { get; set; } = 0;
        public int TimeOnTop { get; set; } = 1;

        public Sprite AlmostUp { get; set; }
    }
}