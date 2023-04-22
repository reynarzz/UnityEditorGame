using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Experimental.Rendering;

namespace DungeonInspector
{
    public class EnemyDatabase
    {
        public List<DRendererComponent> _enemyRenderers;
        public int Count => _enemyRenderers.Count;

        public EnemyDatabase()
        {
            _enemyRenderers = new List<DRendererComponent>()
            {
                GetEnemy<OrcEnemy>("Orc", "GameAssets/Dungeon/ogre_idle_anim_f0"),
                GetEnemy<OrcEnemy>("Monk", "GameAssets/Dungeon/necromancer_idle_anim_f0"),
            };

            var test = GetEnemy<OrcEnemy>("ShortMaskedORc", "GameAssets/Dungeon/masked_orc_idle_anim_f0");

            _enemyRenderers[1].Entity.Transform.Position = new DVector2(1, -3); // remove
            test.Entity.Transform.Position = new DVector2(6, -1); // remove
        }

        private DRendererComponent GetEnemy<T>(string name, string texturePath) where T : EnemyBase, new()
        {
            var entity = new DGameEntity(name, typeof(ActorHealth), typeof(HealthBarUI), typeof(DPhysicsComponent), typeof(DBoxCollider));
            entity.Tag = "Enemy";
            entity.AddComp<T>().Tag = "Player";

            var anim = entity.AddComp<DAnimatorComponent>();
            //anim.AddAnimation();

            var renderer = entity.AddComp<DRendererComponent>();
            renderer.ZSorting = 2;
            renderer.Sprite = Utils.Load<UnityEngine.Texture2D>(texturePath);

            return renderer;
        }

        public List<DVector2> GetFreePath()
        {
            // returns a non taken path, ordering by enemy distance to the target.
            return null;
        }

        public DRendererComponent GetEnemy(int index)
        {
            return _enemyRenderers[index];
        }
    }
}