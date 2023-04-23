using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonInspector
{
    public class DEntitiesController : EngineSystemBase<DGameEntity>
    {
        private List<DGameEntity> _entities;
        private bool _started = false;

        public int Count => _entities.Count;
        public DEntitiesController()
        {
            _entities = new List<DGameEntity>();
        }

        public override void Add(DGameEntity entity)
        {
            _entities.Add(entity);
        }

        public override void Remove(DGameEntity entity)
        {
            _entities.Remove(entity);
        }

        public List<DGameEntity> GetAllGameEntities()
        {
            return _entities;
        }

        public DGameEntity FindGameEntity(string name)
        {
            for (int i = 0; i < _entities.Count; i++)
            {
                if (_entities[i].Name.Equals(name))
                {
                    return _entities[i];
                }
            }

            return null;
        }

        // TODO: call awake right after object creation
        public override void Init()
        {
            for (int i = 0; i < _entities.Count; i++)
            {
                var entity = _entities[i];

                if (entity.IsActive)
                {
                    var updatables = entity.GetAllUpdatableComponents();

                    for (int j = 0; j < updatables.Count; j++)
                    {
                        updatables[j].Awake();
                    }
                }
            }
        }

        private void OnStartBehaviors(DGameEntity entity)
        {
            var updatables = entity.GetAllUpdatableComponents();

            for (int i = 0; i < updatables.Count; i++)
            {
                updatables[i].Start();
            }
        }

        public override void Update()
        {
            // Start
            if (!_started)
            {
                _started = true;
                for (int i = 0; i < _entities.Count; i++)
                {
                    OnStartBehaviors(_entities[i]);
                }
            }

            // Update

            for (int i = 0; i < _entities.Count; i++)
            {
                var entity = _entities[i];

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

            for (int i = 0; i < _entities.Count; i++)
            {
                var entity = _entities[i];

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
