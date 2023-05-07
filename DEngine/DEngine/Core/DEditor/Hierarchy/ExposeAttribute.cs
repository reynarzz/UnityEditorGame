using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonInspector
{
    /// <summary>Expose this field to the hierarchy.</summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class DExposeAttribute : Attribute
    {

    }
}
