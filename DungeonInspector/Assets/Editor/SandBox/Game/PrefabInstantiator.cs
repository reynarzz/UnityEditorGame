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
        private DSpriteAtlas _coinCollectibleAtlas;
        private DSpriteAtlas _heartAtlas;
        private DSpriteAtlas _doorAtlas;
        private DSpriteAtlas _emptyChestAtlas;
        private DSpriteAtlas _priceChestAtlas;

        private Texture2D _bulletCircle;
        private Texture2D _greenFlask;
        private List<DGameEntity> _instancedEntities;
        private EnemyDatabase _enemyDatabase;

        public PrefabInstantiator()
        {
            _doorAtlas = Resources.Load<DSpriteAtlas>("Interactables/DoorAtlas");
            _heartAtlas = Resources.Load<DSpriteAtlas>("UI/HeartAtlas");
            _coinCollectibleAtlas = Resources.Load<DSpriteAtlas>("Interactables/CoinAtlas");
            _emptyChestAtlas = Resources.Load<DSpriteAtlas>("Interactables/Chest1Atlas");
            _priceChestAtlas = Resources.Load<DSpriteAtlas>("Interactables/Chest2Atlas");


            _greenFlask = Resources.Load<Texture2D>("GameAssets/Dungeon/flask_big_green");
            _bulletCircle = Resources.Load<Texture2D>("GameAssets/bullet");

            _instancedEntities = new List<DGameEntity>();
            _enemyDatabase = new EnemyDatabase();

        }

        public DGameEntity InstancePlayer(string name)
        {
            return null;
        }

        public DGameEntity InstanceOrcEnemy(string name)
        {
            var enty = _enemyDatabase.InstanceOrc().Entity;
            _instancedEntities.Add(enty);

            return enty;
        }

        public DGameEntity InstanceMaskedOrcEnemy(string name)
        {
            var enty = _enemyDatabase.InstanceMaskedOrc().Entity;
            _instancedEntities.Add(enty);

            return enty;
        }

        public DGameEntity InstanceCoin(string name)
        {
            var animation = new DSpriteAnimation(_coinCollectibleAtlas);
            var coin = InstanceCollectible<Coin>(name, _coinCollectibleAtlas.GetTexture(0));
            var animator = coin.AddComp<DAnimatorComponent>();
            animator.SetRenderer(coin.GetComp<DSpriteRendererComponent>());
            animator.AddAnimation(animation);
            animator.Speed = 5;
            animator.Play(0);

            return coin;
        }

        public DGameEntity InstanceBullet1()
        {
            var entity = GetEntity("Bullet", typeof(Projectile), typeof(DPhysicsComponent));

            entity.Tag = "PlayerBullet";

            var box = entity.AddComp<DBoxCollider>();

            box.Center = new DVec2();
            box.Size = new DVec2(0.46f, 0.46f);

            entity.AddComp<DSpriteRendererComponent>().Sprite = _bulletCircle;

            return entity;
        }

        public DGameEntity InstanceHealthPotion(string name)
        {
            var potion = InstanceCollectible<HealthPotion>(name, _greenFlask);

            return potion;
        }

        public HeartUI InstanceHeartUI(string name)
        {
            var heartUI = new DGameEntity(name);


            var heart = heartUI.AddComp<HeartUI>();
            heart.Init(_heartAtlas);

            return heart;
        }

        private DGameEntity InstanceCollectible<T>(string name, Texture2D sprite) where T : CollectibleBase, new()
        {
            var entity = GetEntity(name, typeof(T), typeof(DPhysicsComponent));

            var collider = entity.AddComp<DBoxCollider>();
            collider.Size = new DVec2(0.5f, 0.71f);
            collider.Center = new DVec2(0, -0.36f);

            var render = entity.AddComp<DSpriteRendererComponent>();
            render.ZSorting = -1;
            render.Sprite = sprite;
            entity.Layer = 2;

            entity.Transform.Offset = new DVec2(0, 0.3f);

            return entity;
        }

        public Chest InstanceChest(string name, int priceIndex = -1)
        {
            var entity = GetEntity(name, typeof(DPhysicsComponent));

            var collider = entity.AddComp<DBoxCollider>();
            
            collider.Center = new DVec2(0, -0.33f);
            collider.Size = new DVec2(0.5f, 1.17f);

            var chest = entity.AddComp<Chest>();

            var renderer = entity.AddComp<DSpriteRendererComponent>();
            
            renderer.Sprite = _emptyChestAtlas.GetTexture(0);

            var spriteAtlas = priceIndex != -1 ? _priceChestAtlas : _emptyChestAtlas;

            var animation = new DSpriteAnimation(spriteAtlas);
            animation.Loop = false;

            var animator = entity.AddComp<DAnimatorComponent>();
            animator.SetRenderer(renderer);
            animator.AddAnimation(animation);
            animator.Speed = 5;
            animator.PlayOnStart = false;

            return chest;
        }

        public DGameEntity InstanceDoor(string name)
        {
            var entity = GetEntity(name, typeof(DSpriteRendererComponent), typeof(DBoxCollider));
            
            entity.AddComp<Door>().SetAtlas(_doorAtlas);

            return entity;
        }


        private DGameEntity GetEntity(string name, params Type[] components)
        {
            var entity = new DGameEntity(name, components);

            _instancedEntities.Add(entity);

            return entity;
        }

        public DGameEntity InstanceEntityByID(EntityID id)
        {
            switch (id)
            {
                case EntityID.Player:
                    break;
                case EntityID.Ogre:
                    return InstanceOrcEnemy("Orc");
                case EntityID.MaskedOrc:
                    return InstanceMaskedOrcEnemy("Masked Orc");
                case EntityID.ChestEmpty:
                    return InstanceChest("Chest").Entity;
                case EntityID.ChestPrice:
                    break;
                case EntityID.Coin:
                    return InstanceCoin("Coin");
                case EntityID.Door:
                    return InstanceDoor("Door");
                case EntityID.GreenPotion:
                    return InstanceHealthPotion("GreenPotion/Health");
                case EntityID.Crate:
                    break;
            }

            return null;
        }

        // we don't like object pooling ;) (joking, this needs refactoring asap)
        public void DestroyAllInstances()
        {
            for (int i = 0; i < _instancedEntities.Count; i++)
            {
                _instancedEntities[i].Destroy();
            }

            _instancedEntities.Clear();
        }
    }
}
