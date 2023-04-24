using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonInspector
{
    public class EntityInfo 
    {
        public Texture2D Tex { get; set; }
        public int EntityID { get; set; }
    }

    public class EditModePrefabInstantiator
    {
        private List<EntityInfo> _entityTextures;

        public int Count => _entityTextures.Count;

        public EditModePrefabInstantiator()
        {
            _entityTextures = new List<EntityInfo>()
            {
                  new EntityInfo() { EntityID = EntityIDs.Player, Tex = Load("GameAssets/Dungeon/knight_f_idle_anim_f0") },
                  new EntityInfo() { EntityID = EntityIDs.Ogre, Tex = Load("GameAssets/Dungeon/ogre_idle_anim_f0") },
                  new EntityInfo() { EntityID = EntityIDs.MaskedOrc, Tex = Load("GameAssets/Dungeon/masked_orc_idle_anim_f0") },
                  new EntityInfo() { EntityID = EntityIDs.ChestEmpty, Tex = Load("GameAssets/Dungeon/chest_empty_open_anim_f2") },
                  new EntityInfo() { EntityID = EntityIDs.ChestPrice, Tex = Load("GameAssets/Dungeon/chest_full_open_anim_f1") },

                  new EntityInfo() { EntityID = EntityIDs.Coin, Tex = Load("GameAssets/Dungeon/coin_anim_f0") },
                  new EntityInfo() { EntityID = EntityIDs.Door, Tex = Load("GameAssets/Dungeon/doors_semi_all 1") },
                  new EntityInfo() { EntityID = EntityIDs.GreenPotion, Tex = Load("GameAssets/Dungeon/flask_big_green") },
                  new EntityInfo() { EntityID = EntityIDs.Crate, Tex = Load("GameAssets/Dungeon/crate") },



            };
        }


        private Texture2D Load(string path)
        {
            return Resources.Load<Texture2D>(path);
        }

        public EntityInfo GetEntityInfo(int index)
        {
            return _entityTextures[index];
        }
    }
}
