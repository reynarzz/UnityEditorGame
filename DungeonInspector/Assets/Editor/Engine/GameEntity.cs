using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonInspector
{
    public class GameEntity
    {
        public string Name { get; set; }

        public List<Component> Components { get; }

        public GameEntity()
        {
            Components = new List<Component>();
        }
    }

   
}