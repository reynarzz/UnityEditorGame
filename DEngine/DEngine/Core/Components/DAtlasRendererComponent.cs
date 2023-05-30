using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonInspector
{
    public class DAtlasRendererComponent : DSpriteRendererComponent
    {
        public DVec2 SpriteCoord { get; set; }
        public DSpriteAtlasInfo AtlasInfo { get; set; }
     
        public DAtlasRendererComponent()
        {
            AtlasInfo = ScriptableObject.CreateInstance<DSpriteAtlasInfo>();
        }
    }
}
