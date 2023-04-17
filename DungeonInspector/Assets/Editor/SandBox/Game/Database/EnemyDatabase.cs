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

        public EnemyDatabase()
        {
            var enemy1 = GetEnemy<EnemyBase>("Orc");
            enemy1.Texture = Utils.Load<UnityEngine.Texture2D>("");
            _enemyRenderers = new List<DRendererComponent>()
            {
                enemy1
            };
        }

        private DRendererComponent GetEnemy<T>(string name) where T : DBehavior
        {
            var entity = new DGameEntity(name, typeof(T));


            return entity.AddComponent<DRendererComponent>();
        }
    }
}
