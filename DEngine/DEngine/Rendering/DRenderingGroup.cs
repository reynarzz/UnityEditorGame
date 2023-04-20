using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonInspector
{
    public class RenderingData
    {
        public UnityEngine.Texture2D MyProperty { get; set; }
        public DVector2 WorldPosition { get; set; }
    }

    // to render things that are not necessaryly related to an entity (maybe?)
    public class DRenderingGroup : DComponent
    {
        /// <summary>Base Z sorting, the elements's zSorting will be added to this value</summary>
        public int ZGroupSorting { get; set; }
        public List<RenderingData> RenderingData { get; }

        public DRenderingGroup()
        {
            RenderingData = new List<RenderingData>();
        }
    }
}
