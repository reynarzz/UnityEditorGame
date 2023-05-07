using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonInspector
{
    public class DRendererAtlasComponent : DRendererComponent
    {
        public DVec2 SpriteCoord { get; set; }
        public DSpriteAtlasInfo AtlasInfo { get; set; }

        public DRendererAtlasComponent()
        {
            AtlasInfo = new DSpriteAtlasInfo();
        }
    }
}
