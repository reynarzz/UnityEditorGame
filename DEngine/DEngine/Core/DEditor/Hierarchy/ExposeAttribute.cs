using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonInspector
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ExposeAttribute : Attribute
    {

    }
}
