using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonInspector
{
    public class RenderingData
    {
        public Texture2D MyProperty { get; set; }
    }

    // to render things that are not necessaryly related to an entity (maybe?)
    public class DRenderingGroup
    {
        private List<RenderingData> _renderingData;
    }
}
