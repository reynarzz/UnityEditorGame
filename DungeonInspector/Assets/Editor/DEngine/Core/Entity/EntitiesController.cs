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

        public void OnStart()
        {
            for (int i = 0; i < _entities.Count; i++)
            {
                var updatables = _entities[i].GetAllUpdatableComponents();

                for (int j = 0; j < updatables.Count; j++)
                {
                    updatables[j].Start();
                }
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
