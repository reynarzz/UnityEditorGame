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
        public DVec2 Max { get; set; }
        public DVec2 Min { get; set; }



        public override string ToString()
        {
            return $"({Min.x}, {Min.y}) < ({Max.x}, {Max.y})";
        }
    }

    public struct DRay
    {
        public DVec2 Origin { get; set; }
        public DVec2 Direction { get; set; }

        public DRay(DVec2 origin, DVec2 direction)
        {
            Origin = origin;
            Direction = direction;
        }
    }

    public struct DRayHitInfo
    {
        public DVec2 Point { get; set; }
        public DVec2 BehindPoint { get; set; }
        public DVec2 Normal { get; set; }
        public DGameEntity Target { get; set; }
        public DRayHitInfo(DVec2 point, DVec2 behindPoint, DVec2 normal)
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

        public DBoxCollider Collider
        {
            get
            {
                if (_collider == null)
                {
                    _collider = GetComp<DBoxCollider>();
                }

                return _collider;
            }
            set
            {
                _collider = value;
            }
        }
        public bool HasAnyCollision => Collisions.Count > 0;
        private List<DPhysicsComponent> Collisions { get; set; }

        public DPhysicsComponent()
        {
            Collisions = new List<DPhysicsComponent>();
        }

        internal void Add(DPhysicsComponent collision)
        {
            Collider.IsColliding = true;
            Collisions.Add(collision);
        }

        internal bool Contains(DPhysicsComponent collision)
        {
            return Collisions.Contains(collision);
        }

        internal void Remove(DPhysicsComponent collision)
        {
            Collisions.Remove(collision);

            Collider.IsColliding = Collisions.Count > 0;
        }
    }
}