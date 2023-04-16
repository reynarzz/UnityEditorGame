using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonInspector
{
    public enum TileBehavior
    {
        None,
        Damage,
        IncreaseHealth,
        ChangeLevel
    }

    public enum TileType
    {
        Static,
        Interactable,
        InteractableWhenTouch
    }

    [Serializable]
    public class DTile
    {
        //private string _textureName;
        //private int _zSorting = 0;
        //private bool _isWalkable;
        //private TileType _type;

        //private SpriteAnimation _idleAnimation; // use array with the names of the textures
        //private SpriteAnimation _interactableAnimation;

        private string[] _idleTexAnim;
        private string[] _interactableTexAnim;

        //public string Texture => _textureName;
        //public bool IsWalkable { get; set; }
        //public int ZSorting => _zSorting;
        //public TileType Type => _type;
        public int Index { get; set; }
        public string TextureName { get; set; }
        public bool IsWalkable;
        public int ZSorting;
        public TileType Type;
        public TileBehavior TileBehavior;
    }
}
