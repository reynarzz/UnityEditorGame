using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonInspector
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Struct)]
    public class DExposeSlider : Attribute
    {
        public float Min { get; private set; }
        public float Max { get; private set; }

        public DExposeSlider(float min, float max)
        {
            Min = min;
            Max = max;
        }
    }
}
