using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonInspector
{
    public class DEntitiesController : DEngineSystemBase<DGameEntity>
    {
        private List<DGameEntity> _toUpdate;
        private List<DGameEntity> _toDestroy;
        private List<DGameEntity> _toAdd;
        private List<DGameEntity> _notAwaken;
        private List<IDBehavior> _notStarted;

        public int Count => _toUpdate.Count;

        public DEntitiesController()
        {
            _toUpdate = new List<DGameEntity>();
            _toDestroy = new List<DGameEntity>();
            _toAdd = new List<DGameEntity>();
            _notAwaken = new List<DGameEntity>();
            _notStarted = new List<IDBehavior>();
        }

        public override void Add(DGameEntity entity)
        {
            _toUpdate.Add(entity);
            _notAwaken.Add(entity);

            //_toAdd.Add(entity);
        }

        public override void Remove(DGameEntity entity)
        {
            _toUpdate.Remove(entity);

            //if (!_toDestroy.Contains(entity))
            //{
            //    _toDestroy.Add(entity);
            //}
            //else
            //{
            //    UnityEngine.Debug.Log($"Tried to destroy '{entity.Name}' more than once!");
            //}
        }

        public List<DGameEntity> GetAllGameEntities()
        {
            return _toUpdate;
        }

        public DGameEntity FindGameEntity(string name)
        {
            for (int i = 0; i < _toUpdate.Count; i++)
            {
                if (_toUpdate[i].Name.Equals(name))
                {
                    return _toUpdate[i];
                }
            }

            return null;
        }

        private void OnAwakeBehaviors()
        {
            if (_notAwaken.Count > 0)
            {

                for (int i = 0; i < _notAwaken.Count; i++)
                {
                    var behaviors = _notAwaken[i].GetAllUpdatableComponents();

                    for (int j = 0; j < behaviors.Count; j++)
                    {
                        behaviors[j].Awake();
                        _notStarted.Add(behaviors[j]);
                    }
                }

                _notAwaken.Clear();
            }
        }

        private void OnStartBehaviors()
        {
            if (_notStarted.Count > 0)
            {
                for (int i = 0; i < _notStarted.Count; i++)
                {
                    _notStarted[i].Start();
                }

                _notStarted.Clear();
            }
        }
        
        public override void Update()
        {
            OnAwakeBehaviors();
            OnStartBehaviors();

            for (int i = 0; i < _toUpdate.Count; i++)
            {
                var entity = _toUpdate[i];

                if (entity.IsActive)
                {
                    var updatables = entity.GetAllUpdatableComponents();

                    for (int j = 0; j < updatables.Count; j++)
                    {
                        updatables[j].Update();
                    }
                }
            }

            // Late Update
            for (int i = 0; i < _toUpdate.Count; i++)
            {
                var entity = _toUpdate[i];

                if (entity.IsActive)
                {
                    var updatables = entity.GetAllUpdatableComponents();

                    for (int j = 0; j < updatables.Count; j++)
                    {
                        updatables[j].LateUpdate();
                    }
                }
            }

            EndFrameWork();
        }

        private void EndFrameWork()
        {
            //if (Event.current.type == EventType.Repaint)
            //{
            //    for (int i = 0; i < _toDestroy.Count; i++)
            //    {
            //        _toUpdate.Remove(_toDestroy[i]);
            //    }

            //    _toDestroy.Clear();
            //}

            //for (int i = 0; i < _toAdd.Count; i++)
            //{
            //    _toUpdate.Add(_toAdd[i]);
            //    _notAwaken.Add(_toAdd[i]);
            //}

            //_toAdd.Clear();
        }
    }
}
