using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonInspector
{
    public struct DAABB
    {
        public DVector2 Max { get; set; }
        public DVector2 Min { get; set; }



        public override string ToString()
        {
            return $"({Min.x}, {Min.y}) < ({Max.x}, {Max.y})";
        }
    }

    public struct DRay
    {
        public DVector2 Origin { get; set; }
        public DVector2 Direction { get; set; }

        public DRay(DVector2 origin, DVector2 direction)
        {
            Origin = origin;
            Direction = direction;
        }
    }

    public struct DRayHitInfo
    {
        public DVector2 Point { get; set; }
        public DVector2 BehindPoint { get; set; }
        public DVector2 Normal { get; set; }
        public DGameEntity Target { get; set; }
        public DRayHitInfo(DVector2 point, DVector2 behindPoint, DVector2 normal)
        {
            Point = point;
            BehindPoint = behindPoint;
            Normal = normal;
            Target = null;
        }
    }

    public class DPhysicsComponent : DBehavior
    {
        private DBoxCollider _collider;
        public DBoxCollider Collider => _collider;
        public bool IsColliding => Collisions.Count > 0;
        internal List<DPhysicsComponent> Collisions { get; set; }

        public bool TriggerEnter { get; set; }

        public DPhysicsComponent()
        {
            Collisions = new List<DPhysicsComponent>();
        }

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