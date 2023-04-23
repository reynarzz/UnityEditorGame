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
        private DSpriteAtlas _coinCollectibleAtlas;
        private Texture2D _greenFlask;
        private Texture2D _bulletCircle;

        public PrefabInstantiator()
        {
            _doorAtlas = Resources.Load<DSpriteAtlas>("Interactables/DoorAtlas");
            _greenFlask = Resources.Load<Texture2D>("GameAssets/Dungeon/flask_big_green");
            _bulletCircle = Resources.Load<Texture2D>("GameAssets/bullet");
            
            _coinCollectibleAtlas = Resources.Load<DSpriteAtlas>("Interactables/CoinAtlas");
            
        }

        public DGameEntity InstancePlayer(string name)
        {
            return null;
        }

        public DGameEntity InstanceOrcEnemy(string name)
        {
            return null;
        }

        public DGameEntity InstanceCoin(string name)
        {
            var animation = new DSpriteAnimation(_coinCollectibleAtlas);
            var coin = InstanceCollectible<Coin>(name, _coinCollectibleAtlas.GetTexture(0));
            var animator = coin.AddComp<DAnimatorComponent>();
            animator.SetRenderer(coin.GetComp<DRendererComponent>());
            animator.AddAnimation(animation);
            animator.Speed = 5;
            animator.Play(0);

            return coin;
        }

        public DGameEntity InstanceBullet1()
        {
            var entity = new DGameEntity("Bullet", typeof(Projectile), typeof(DPhysicsComponent));

            var box = entity.AddComp<DBoxCollider>();

            box.Center = new DVector2();
            box.Size = new DVector2(0.46f, 0.46f);

            entity.AddComp<DRendererComponent>().Sprite = _bulletCircle;
            
            return entity;
        }

        public DGameEntity GetHealthPotion(string name)
        {
            var potion = InstanceCollectible<HealthPotion>(name, _greenFlask);

            return potion;
        }

        private DGameEntity InstanceCollectible<T>(string name, Texture2D sprite) where T : CollectibleBase, new()
        {
            var entity = GetEntity(name, typeof(T), typeof(DPhysicsComponent));

            var collider = entity.AddComp<DBoxCollider>();
            collider.Size = new DVector2(0.5f, 0.71f);
            collider.Center = new DVector2(0, -0.36f);

            var render = entity.AddComp<DRendererComponent>();
            render.ZSorting = -1;
            render.Sprite = sprite;
            entity.Layer = 2;

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
