using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonInspector
{
    public struct AABB
    {
        public DVector2 Max { get; set; }
        public DVector2 Min { get; set; }

        public static AABB Default => new AABB { Max = new DVector2(1, 1), Min = new DVector2(0, 0) };
    }

    public class DPhysicsComponent : DBehavior
    {

        private DBoxCollider _collider;
        public DBoxCollider Collider => _collider;

        public bool TriggerEnter { get; set; }
        protected override void OnAwake()
        {
            _collider = GetComp<DBoxCollider>();
        }

        public void OnPhysicsUpdate()
        {
            if(_collider == null)
            {
                _collider = GetComp<DBoxCollider>();
            }

        }
    }
}