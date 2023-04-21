using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonInspector
{
    public class PrefabInstantiator
    {
        private DSpriteAtlas _doorAtlas;

        public PrefabInstantiator()
        {
            _doorAtlas = Resources.Load<DSpriteAtlas>("Interactables/DoorAtlas");
        }

        public DGameEntity InstancePlayer(string name)
        {
            return null;
        }

        public DGameEntity InstanceOrcEnemy(string name)
        {
            return null;
        }

        public DGameEntity InstancePotion1(string name)
        {
            return null;
        }

        public DGameEntity InstancePotion2(string name)
        {
            return null;
        }

        public DGameEntity InstancePotion3(string name)
        {
            return null;
        }

        public DGameEntity InstanceChest(string name)
        {
            return null;
        }

        public DGameEntity InstanceDoor(string name)
        {
            var entity = GetEntity(name, typeof(DRendererComponent), typeof(DBoxCollider));

            entity.AddComp<Door>().SetAtlas(_doorAtlas);

            return entity;


        }

        private DGameEntity GetEntity(string name, params Type[] components)
        {
            return new DGameEntity(name, components);
        }
    }
}
