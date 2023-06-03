using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonInspector
{
    public enum TileAnimation
    {
        None,
        Animated
    }

    [Serializable]
    public abstract class BaseTD
    {
        public TileAnimation AnimationType { get; set; }
        public string SpriteAtlasPath { get; set; }
    }
}
