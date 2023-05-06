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
        public EntityID EntityID { get; set; }
        public int MaxEntitiesOfThisType { get; set; } = 1;
    }

    public class EditModePrefabInstantiator
    {
        private Dictionary<EntityID, EntityInfo> _entities;

        public int Count => _entities.Count;

        public EditModePrefabInstantiator()
        {

            _entities = new Dictionary<EntityID, EntityInfo>()
            {
               { EntityID.Player,       new EntityInfo() { EntityID = EntityID.Player, Tex = Load("GameAssets/Dungeon/knight_f_idle_anim_f0")} },
               { EntityID.Ogre ,        new EntityInfo() { EntityID = EntityID.Ogre, Tex = Load("GameAssets/Dungeon/ogre_idle_anim_f0") } },
               { EntityID.MaskedOrc,    new EntityInfo() { EntityID = EntityID.MaskedOrc, Tex = Load("GameAssets/Dungeon/masked_orc_idle_anim_f0") }},
               { EntityID.ChestEmpty,   new EntityInfo() { EntityID = EntityID.ChestEmpty, Tex = Load("GameAssets/Dungeon/chest_empty_open_anim_f2")  }},
               { EntityID.ChestPrice ,  new EntityInfo() { EntityID = EntityID.ChestPrice, Tex = Load("GameAssets/Dungeon/chest_full_open_anim_f1") } },

               { EntityID.Coin ,        new EntityInfo() { EntityID = EntityID.Coin, Tex = Load("GameAssets/Dungeon/coin_anim_f0") } },
               { EntityID.Door,         new EntityInfo() { EntityID = EntityID.Door, Tex = Load("GameAssets/Dungeon/doors_semi_all 1") } },
               { EntityID.GreenPotion,  new EntityInfo() { EntityID = EntityID.GreenPotion, Tex = Load("GameAssets/Dungeon/flask_big_green") } },
               { EntityID.Crate ,       new EntityInfo() { EntityID = EntityID.Crate, Tex = Load("GameAssets/Dungeon/crate") } },
            };
        }


        private Texture2D Load(string path)
        {
            return Resources.Load<Texture2D>(path);
        }

        public EntityInfo GetEntityInfo(EntityID id)
        {
            return _entities[id];
        }
    }
}
