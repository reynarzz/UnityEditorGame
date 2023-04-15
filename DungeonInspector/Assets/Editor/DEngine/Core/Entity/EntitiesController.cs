using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonInspector
{
    public class DEntitiesController : IDService
    {
        private List<DGameEntity> _entities;

        public int Count => _entities.Count;
        public DEntitiesController()
        {
            _entities = new List<DGameEntity>();
        }

        public void AddEntity(DGameEntity entity)
        {
            _entities.Add(entity);
        }

        public void RemoveEntity(DGameEntity entity)
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

        public void OnStart()
        {
            for (int i = 0; i < _entities.Count; i++)
            {
                OnStartBehaviors(_entities[i]);
            }
        }

        private void OnStartBehaviors(DGameEntity entity)
        {
            var updatables = entity.GetAllUpdatableComponents();

            for (int j = 0; j < updatables.Count; j++)
            {
                updatables[j].Start();
            }
        }

        public void Update()
        {
            for (int i = 0; i < _entities.Count; i++)
            {
                var updatables = _entities[i].GetAllUpdatableComponents();

                for (int j = 0; j < updatables.Count; j++)
                {
                    updatables[j].Update();
                }
            }
        }

        
    }
}
