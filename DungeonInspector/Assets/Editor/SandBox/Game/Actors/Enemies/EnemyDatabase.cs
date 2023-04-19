using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                GetEnemy<OrcEnemy>("Orc", "GameAssets/Dungeon/ogre_idle_anim_f0")
            };
        }

        private DRendererComponent GetEnemy<T>(string name, string texturePath) where T : DBehavior
        {
            var entity = new DGameEntity(name, typeof(T), typeof(HealthBarUI), typeof(DPhysicsComponent), typeof(DBoxCollider));
            entity.Tag = "Enemy";

            entity.AddComp<ActorHealth>().EnemyTag = "Player";

            var renderer = entity.AddComp<DRendererComponent>();

            renderer.Sprite = Utils.Load<UnityEngine.Texture2D>(texturePath);

            return renderer;
        }

        public DRendererComponent GetEnemy(int index)
        {
            return _enemyRenderers[index];
        }
    }
}
