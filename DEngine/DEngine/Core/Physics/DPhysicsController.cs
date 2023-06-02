using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonInspector
{
    public class BodyCollisionState
    {
        public bool CollisionEnter { get; set; }
        public bool TriggerEnter { get; set; }
    }

    public enum CollisionType
    {
        Trigger,
        Collision,
    }

    public class DPhysicsController : DEngineSystemBase<DPhysicsComponent>
    {
        private List<DPhysicsComponent> _components;
        private Dictionary<DPhysicsComponent, Dictionary<DPhysicsComponent, BodyCollisionState>> _bodiesState;

        internal DPhysicsController()
        {
            _components = new List<DPhysicsComponent>();
            _bodiesState = new Dictionary<DPhysicsComponent, Dictionary<DPhysicsComponent, BodyCollisionState>>();
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
                    component.Remove(element);
                }
            }

            _components.Remove(element);
        }

        // TODO: This is a proof of concept to test the AABB collision
        public override void Update()
        {
            //for (int i = 0; i < _components.Count; i++)
            //{
            //    _components[i].OnPhysicsUpdate();
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

                    if (body1 != body2)
                    {
                        var areClose = (body1.Transform.Position - body2.Transform.Position).SqrMagnitude <= 2f;
                        bool isColliding = false;

                        if (areClose)
                        {
                            isColliding = DetectCollision(body1, body2);

                            if (isColliding)
                            {
                                if (!body1.Contains(body2))
                                {
                                    body1.Add(body2);
                                }

                                if (!body2.Contains(body1))
                                {
                                    body2.Add(body1);
                                }
                            }
                            else
                            {
                                body1.Remove(body2);
                                body2.Remove(body1);
                            }
                        }
                        else
                        {
                            body1.Remove(body2);
                            body2.Remove(body1);
                        }

                        RaiseOnTriggerEvent(body1, body2, isColliding);
                        RaiseOnTriggerEvent(body2, body1, isColliding);
                    }
                }
            }
        }

        private bool DetectCollision(DPhysicsComponent current, DPhysicsComponent target)
        {
            if (current.Collider is DBoxCollider && target.Collider is DBoxCollider)
            {
                return DetectAABBCollision((DBoxCollider)current.Collider, (DBoxCollider)target.Collider, current, target);
            }

            return false;
        }

        private BodyCollisionState GetCollisionState(DPhysicsComponent current, DPhysicsComponent target)
        {
            if (_bodiesState.TryGetValue(current, out var collision))
            {
                if (collision.TryGetValue(target, out var state))
                {
                    return state;
                }
                else
                {
                    var collisionState = new BodyCollisionState();

                    collision.Add(target, collisionState);

                    return collisionState;
                }
            }
            else
            {
                var collisionState = new BodyCollisionState();

                _bodiesState.Add(current, new Dictionary<DPhysicsComponent, BodyCollisionState>() { { target, collisionState } });

                return collisionState;
            }

            return null;
        }

        private void RemoveCollisionState(DPhysicsComponent current, DPhysicsComponent target)
        {
            _bodiesState[current].Remove(target);

            if (_bodiesState[current].Count == 0)
            {
                _bodiesState.Remove(current);
            }
        }

        private void RaiseOnTriggerEvent(DPhysicsComponent current, DPhysicsComponent target, bool isColliding)
        {
            if (current.Collider != null && current.Collider.IsTrigger)
            {
                var state = GetCollisionState(current, target);

                var allcomponents = current.Entity.GetAllComponents();

                if (isColliding)
                {
                    for (int i = 0; i < allcomponents.Count; i++)
                    {
                        var behavior = allcomponents.ElementAt(i) as IDBehavior;

                        if (!state.TriggerEnter)
                        {
                            if (behavior != null)
                            {
                                behavior.OnTriggerEnter(target.Collider);
                            }
                        }
                        else
                        {
                            if (behavior != null)
                            {
                                behavior.OnTriggerStay(target.Collider);
                            }
                        }
                    }

                    state.TriggerEnter = true;
                }
                else
                {
                    if (state.TriggerEnter)
                    {
                        foreach (var item in allcomponents)
                        {
                            var behavior = item as IDBehavior;
                            if (behavior != null)
                            {
                                behavior.OnTriggerExit(target.Collider);
                            }
                        }

                        RemoveCollisionState(current, target);
                    }

                    state.TriggerEnter = false;
                }
            }
        }

        public bool DetectAABBCollision(DBoxCollider a, DBoxCollider b, DPhysicsComponent compA, DPhysicsComponent compB)
        {
            return (((a.AABB.Min.x >= b.AABB.Min.x && a.AABB.Min.x <= b.AABB.Max.x ||
                   a.AABB.Max.x <= b.AABB.Max.x && a.AABB.Max.x >= b.AABB.Min.x) &&
                   (a.AABB.Min.y >= b.AABB.Min.y && a.AABB.Min.y <= b.AABB.Max.y ||
                   a.AABB.Max.y <= b.AABB.Max.y && a.AABB.Max.y >= b.AABB.Min.y))
                   ||
                   ((b.AABB.Min.x >= a.AABB.Min.x && b.AABB.Min.x <= a.AABB.Max.x ||
                   b.AABB.Max.x <= a.AABB.Max.x && b.AABB.Max.x >= a.AABB.Min.x) &&
                   (b.AABB.Min.y >= a.AABB.Min.y && b.AABB.Min.y <= a.AABB.Max.y ||
                   b.AABB.Max.y <= a.AABB.Max.y && b.AABB.Max.y >= a.AABB.Min.y)))
                   && compA.Collider.IsTrigger && compB.Collider.IsTrigger && a.Enabled && b.Enabled && compA.Collider.Enabled && compB.Collider.Enabled;

        }

        public List<DPhysicsComponent> GetAllBodies()
        {
            return _components;
        }
    }
}
