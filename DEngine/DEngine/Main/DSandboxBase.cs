using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonInspector
{
    public abstract class DSandboxBase
    {
        public Type[] Services { get; }

        public DSandboxBase(params Type[] engineServices)
        {
            Services = engineServices;
        }

        public virtual void OnInitialize() { }
        public virtual void OnQuit() { }
    }
}
