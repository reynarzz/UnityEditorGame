using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UIElements;

namespace DungeonInspector
{
    public class DPhysicsController : EngineSystemBase<DPhysicsComponent>
    {
        private List<DPhysicsComponent> _components;

        public DPhysicsController()
        {
            _components = new List<DPhysicsComponent>();
        }

        public override void Add(DPhysicsComponent element)
        {
            _components.Add(element);
        }

        public override void Remove(DPhysicsComponent element)
        {
            _components.Remove(element);
        }

        // TODO: This is a proof of concept to test the AABB collision
        public override void Update()
        {
            for (int i = 0; i < _components.Count; i++)
            {
                _components[i].OnPhysicsUpdate();
            }

            //if(_components.Count > 1)
            //{
            //    var colliding = DetectCollision(_components[0], _components[1]);
            //    _components[0].Collider.IsColliding = colliding;
            //    _components[1].Collider.IsColliding = colliding;


            //    RaiseOnTriggerEvent(_components[0], _components[1]);
            //    if (_components.Count > 1)
            //    {
            //        RaiseOnTriggerEvent(_components[1], _components[0]);
            //    }
            //}

            CollisionChecks();
        }

        private void CollisionChecks()
        {
            for (int i = 0; i < _components.Count; i++)
            {
                var body1 = _components[i];

                for (int j = 0; j < _components.Count; j++)
                {
                    var body2 = _components[j];

                    if(body1 != body2)
                    {
                        var colliding = DetectCollision(body1, body2);

                        if (colliding)
                        {
                            body1.Collisions.Add(body2);
                            body2.Collisions.Add(body1);
                        }
                        else
                        {
                            body1.Collisions.Remove(body2);
                            body2.Collisions.Remove(body1);
                        }

                        body1.Collider.IsColliding = colliding;
                        body2.Collider.IsColliding = colliding;

                        RaiseOnTriggerEvent(body1, body2);
                        RaiseOnTriggerEvent(body2, body1);
                    }
                }
            }
        }


        private void RaiseOnTriggerEvent(DPhysicsComponent physicObj, DPhysicsComponent target)
        {
            if (physicObj.Collider != null && physicObj.Collider.IsTrigger)
            {
                var allcomponents = physicObj.Entity.GetAllComponents();

                if (physicObj.Collider.IsColliding)
                {
                    if (!physicObj.TriggerEnter)
                    {
                        for (int i = 0; i < allcomponents.Count; i++)
                        {
                            var behavior = allcomponents.ElementAt(i) as IDBehavior;
                            // }
                            //foreach (var item in allcomponents)
                            //{
                            if (behavior != null)
                            {
                                behavior.OnTriggerEnter(target.Collider);
                            }
                        }

                        physicObj.TriggerEnter = true;
                    }
                }
                else
                {
                    if (physicObj.TriggerEnter)
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

                    physicObj.TriggerEnter = false;
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
