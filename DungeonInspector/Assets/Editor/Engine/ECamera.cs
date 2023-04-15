using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonInspector
{
    public class DCamera
    {
        public Vector2 position { get; set; }
        public int PixelsPerUnit { get; set; } = 32;
        public Rect ViewportRect { get; set; }


        public DCamera()
        {
            position = new Vector2(0, 0);
        }


        public Rect World2RectPos(Vector2 pos, Vector2 scale)
        {
            return Utils.World2RectPos(pos, scale, ViewportRect, position, PixelsPerUnit);
        }
    }
}
