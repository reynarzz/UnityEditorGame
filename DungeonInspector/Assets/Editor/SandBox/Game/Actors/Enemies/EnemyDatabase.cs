using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonInspector
{
    public class EnemyDatabase
    {
        public Dictionary<Type, EnemyBase> _enemies;

        public int Count => _enemies.Count;

        public EnemyDatabase()
        {
            //_enemies = new Dictionary<Type, EnemyBase>()
            //{
            //    { typeof(OrcEnemy), GetEnemy<OrcEnemy>(LoadAnims("Enemies/Orc/OrcIdle", "Enemies/Orc/OrcWalk")) },
            //    { typeof(MaskedOrcEnemy), GetEnemy<MaskedOrcEnemy>(LoadAnims("Enemies/Masked/MaskedIdle", "Enemies/Masked/MaskedWalk")) },
            //    //GetEnemy<OrcEnemy>("Monk", LoadAnims("GameAssets/Dungeon/necromancer_idle_anim_f0")),
            //};

            //  var test = GetEnemy<OrcEnemy>("ShortMaskedORc", "GameAssets/Dungeon/masked_orc_idle_anim_f0");


            //_enemyRenderers[1].Entity.Transform.Position = new DVec2(1, -3); // remove
            //_enemyRenderers[1].Entity.Transform.Offset = new DVec2(0, -1); // remove
            // test.Entity.Transform.Position = new DVec2(6, -1); // remove
        }

        // this should be public
        private T GetEnemy<T>(params DSpriteAnimation[] animations) where T : EnemyBase, new()
        {
            var entity = new DGameEntity(typeof(T).Name, typeof(ActorHealth), typeof(HealthBarUI), typeof(DPhysicsComponent), typeof(DBoxCollider));
            entity.Tag = "Enemy";
            var enemy = entity.AddComp<T>();
            enemy.Tag = "Player";


            var renderer = entity.AddComp<DRendererComponent>();
            renderer.ZSorting = 2;

            var anim = entity.AddComp<DAnimatorComponent>();

            for (int i = 0; i < animations.Length; i++)
            {
                anim.AddAnimation(animations[i]);
            }

            anim.Speed = 10;

            anim.SetRenderer(renderer);

            return enemy;
        }

        private DSpriteAnimation[] LoadAnims(params string[] animsPath)
        {
            var anims = new DSpriteAnimation[animsPath.Length];

            for (int i = 0; i < anims.Length; i++)
            {
                anims[i] = new DSpriteAnimation(Resources.Load<DSpriteAtlas>(animsPath[i]));
            }

            return anims;
        }
    }
}