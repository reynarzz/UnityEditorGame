using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonInspector
{
    public class PrefabInstantiator
    {
        private E_SpriteAtlas _doorAtlas;

        public PrefabInstantiator()
        {

        }

        public static DGameEntity InstancePlayer(string name)
        {
            return null;
        }

        public static DGameEntity InstanceOrcEnemy(string name)
        {
            return null;
        }

        public static DGameEntity InstancePotion1(string name)
        {
            return null;
        }

        public static DGameEntity InstancePotion2(string name)
        {
            return null;
        }

        public static DGameEntity InstancePotion3(string name)
        {
            return null;
        }

        public static DGameEntity InstanceChest(string name)
        {
            return null;
        }

        public static DGameEntity InstanceDoor(string name)
        {
            return GetEntity(name, typeof(DRenderingController), typeof(DBoxCollider), typeof(Door));

        }

        private static DGameEntity GetEntity(string name, params Type[] components)
        {
            return new DGameEntity(name, components);
        }
    }
}
