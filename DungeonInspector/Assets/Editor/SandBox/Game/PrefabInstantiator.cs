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
        private Texture2D _greenFlask;

        public PrefabInstantiator()
        {
            _doorAtlas = Resources.Load<DSpriteAtlas>("Interactables/DoorAtlas");
            _greenFlask = Resources.Load<Texture2D>("GameAssets/Dungeon/flask_big_green");
        }

        public DGameEntity InstancePlayer(string name)
        {
            return null;
        }

        public DGameEntity InstanceOrcEnemy(string name)
        {
            return null;
        }

        public DGameEntity InstanceCollectible<T>(string name) where T : CollectibleBase, new()
        {
            var entity = GetEntity(name, typeof(T), typeof(DPhysicsComponent));

            var collider = entity.AddComp<DBoxCollider>();
            collider.Size = new DVector2(0.7f, 0.71f);
            collider.Center = new DVector2(0, -0.36f);

            var render = entity.AddComp<DRendererComponent>();
            render.ZSorting = -1;
            render.Sprite = _greenFlask;
            entity.Transform.Offset = new DVector2(0, 0.3f);

            return entity;
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
