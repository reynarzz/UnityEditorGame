using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonInspector
{
    public class DEntitiesController : EngineSystemBase<DGameEntity>
    {
        private List<DGameEntity> _toUpdate;
        private List<DGameEntity> _notAwaken;
        private List<IDBehavior> _notStarted;

        public int Count => _toUpdate.Count;
        public DEntitiesController()
        {
            _toUpdate = new List<DGameEntity>();
            _notAwaken = new List<DGameEntity>();
            _notStarted = new List<IDBehavior>();
        }

        public override void Add(DGameEntity entity)
        {
            _toUpdate.Add(entity);
            _notAwaken.Add(entity);
        }

        public override void Remove(DGameEntity entity)
        {
            _toUpdate.Remove(entity);
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
        }
    }
}
