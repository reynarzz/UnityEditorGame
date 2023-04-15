using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonInspector
{
    public class ComponentsContainer
    {

        public List<GameEntity> _entities;

        public ComponentsContainer()
        {
            _entities = new List<GameEntity>();
        }

        public void Update()
        {

        }
    }
}