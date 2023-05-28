using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonInspector
{
    public abstract class DSandboxBase
    {
        public Type[] Services { get; protected set; }
        public virtual void OnInitialize() { }
        public virtual void OnQuit() { }
        public DSandboxBase(params Type[] services)
        {
            Services = services;
        }
    }

    public abstract class DSandboxBase<T1> : DSandboxBase where T1 : DEngineSystemBase
    {
        public DSandboxBase()
        {
            Services = new Type[] { typeof(T1) };
        }
    }

    public abstract class DSandboxBase<T1, T2> : DSandboxBase where T1 : DEngineSystemBase
                                                              where T2 : DEngineSystemBase
    {
        public DSandboxBase()
        {
            Services = new Type[] { typeof(T1), typeof(T2) };
        }
    }

    public abstract class DSandboxBase<T1, T2, T3> : DSandboxBase where T1 : DEngineSystemBase
                                                                  where T2 : DEngineSystemBase
                                                                  where T3 : DEngineSystemBase
    {
        public DSandboxBase()
        {
            Services = new Type[] { typeof(T1), typeof(T2), typeof(T3) };
        }
    }

    public abstract class DSandboxBase<T1, T2, T3, T4> : DSandboxBase where T1 : DEngineSystemBase
                                                                      where T2 : DEngineSystemBase
                                                                      where T3 : DEngineSystemBase
                                                                      where T4 : DEngineSystemBase
    {
        public DSandboxBase()
        {
            Services = new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4) };
        }
    }

    public abstract class DSandboxBase<T1, T2, T3, T4, T5> : DSandboxBase where T1 : DEngineSystemBase
                                                                          where T2 : DEngineSystemBase
                                                                          where T3 : DEngineSystemBase
                                                                          where T4 : DEngineSystemBase
                                                                          where T5 : DEngineSystemBase
    {
        public DSandboxBase()
        {
            Services = new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5) };
        }
    }

    public abstract class DSandboxBase<T1, T2, T3, T4, T5, T6> : DSandboxBase where T1 : DEngineSystemBase
                                                                              where T2 : DEngineSystemBase
                                                                              where T3 : DEngineSystemBase
                                                                              where T4 : DEngineSystemBase
                                                                              where T5 : DEngineSystemBase
                                                                              where T6 : DEngineSystemBase
    {
        public DSandboxBase()
        {
            Services = new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6) };
        }
    }
}