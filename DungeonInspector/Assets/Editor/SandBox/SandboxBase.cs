using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonInspector
{
    public abstract class SandboxBase
    {
        public DTime Time { get; set; }
        public virtual void OnInitialize() { }
        public virtual void OnClose() { }
    }
}
