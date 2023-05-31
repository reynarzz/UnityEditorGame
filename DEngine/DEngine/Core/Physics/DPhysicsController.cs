using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonInspector
{
    public class DPhysicsController : DEngineSystemBase<DPhysicsComponent>
    {
        private List<DPhysicsComponent> _components;

        public DPhysicsController()
        {
            _components = new List<DPhysicsComponent>();
        }

        internal override void Add(DPhysicsComponent element)
        {
            _components.Add(element);
        }

        internal override void Remove(DPhysicsComponent element)
        {
            foreach (DPhysicsComponent component in _components)
            {
                if (component != element)
                {
                    component.Collisions.Remove(element);
                }
            }

            _components.Remove(element);
        }

        // TODO: This is a proof of concept to test the AABB collision
        public override void Update()
        {
            for (int i = 0; i < _components.Count; i++)
            {
                _components[i].OnPhysicsUpdate();
            }

            CollisionChecks();
        }

        private void CollisionChecks()
        {
            //int _closeCount = 0;

            for (int i = 0; i < _components.Count; i++)
            {
                var body1 = _components[i];

                for (int j = 0; j < _components.Count; j++)
                {
                    var body2 = _components[j];

                    if (body1 != body2)
                    {
                        var areClose = (body1.Transform.Position - body2.Transform.Position).SqrMagnitude <= 2f;
                        bool isColliding = false;

                        if (areClose)
                        {
                            //_closeCount++;
                            isColliding = DetectCollision(body1, body2);

                            if (isColliding)
                            {
                                if (!body1.Collisions.Contains(body2))
                                {
                                    body1.Collisions.Add(body2);
                                }

                                if (!body2.Collisions.Contains(body1))
                                {
                                    body2.Collisions.Add(body1);
                                }
                            }
                            else
                            {
                                body1.Collisions.Remove(body2);
                                body2.Collisions.Remove(body1);
                            }
                        }
                        else
                        {
                            body1.Collisions.Remove(body2);
                            body2.Collisions.Remove(body1);
                        }

                        body2.Collider.IsColliding = body2.Collisions.Count > 0;

                        RaiseOnTriggerEvent(body1, body2, isColliding);
                        RaiseOnTriggerEvent(body2, body1, isColliding);
                    }
                }

                body1.Collider.IsColliding = body1.Collisions.Count > 0;

            }
        }


        private void RaiseOnTriggerEvent(DPhysicsComponent current, DPhysicsComponent target, bool isColliding)
        {
            if (current.Collider != null && current.Collider.IsTrigger)
            {
                var allcomponents = current.Entity.GetAllComponents();

                if (isColliding)
                {
                    if (!current.TriggerEnter)
                    {
                        for (int i = 0; i < allcomponents.Count; i++)
                        {
                            var behavior = allcomponents.ElementAt(i) as IDBehavior;
                            // }
                            //foreach (var item in allcomponents)
                            //{
                            if (behavior != null)
                            {
                                behavior.OnTriggerStay(target.Collider);
                            }
                        }

                        current.TriggerEnter = true;
                    }
                }
                else
                {
                    if (current.TriggerEnter)
                    {
                        foreach (var item in allcomponents)
                        {
                            var behavior = item as IDBehavior;
                            if (behavior != null)
                            {
                                behavior.OnTriggerExit(target.Collider);
                            }
                        }
                    }

                    current.TriggerEnter = false;
                }
            }
        }

        public bool DetectCollision(DPhysicsComponent a, DPhysicsComponent b)
        {
            return (((a.Collider.AABB.Min.x >= b.Collider.AABB.Min.x && a.Collider.AABB.Min.x <= b.Collider.AABB.Max.x ||
                   a.Collider.AABB.Max.x <= b.Collider.AABB.Max.x && a.Collider.AABB.Max.x >= b.Collider.AABB.Min.x) &&
                   (a.Collider.AABB.Min.y >= b.Collider.AABB.Min.y && a.Collider.AABB.Min.y <= b.Collider.AABB.Max.y ||
                   a.Collider.AABB.Max.y <= b.Collider.AABB.Max.y && a.Collider.AABB.Max.y >= b.Collider.AABB.Min.y))
                   ||
                   ((b.Collider.AABB.Min.x >= a.Collider.AABB.Min.x && b.Collider.AABB.Min.x <= a.Collider.AABB.Max.x ||
                   b.Collider.AABB.Max.x <= a.Collider.AABB.Max.x && b.Collider.AABB.Max.x >= a.Collider.AABB.Min.x) &&
                   (b.Collider.AABB.Min.y >= a.Collider.AABB.Min.y && b.Collider.AABB.Min.y <= a.Collider.AABB.Max.y ||
                   b.Collider.AABB.Max.y <= a.Collider.AABB.Max.y && b.Collider.AABB.Max.y >= a.Collider.AABB.Min.y)))
                   && a.Collider.IsTrigger && b.Collider.IsTrigger;

        }

        public List<DPhysicsComponent> GetAllBodies()
        {
            return _components;
        }
    }
}
