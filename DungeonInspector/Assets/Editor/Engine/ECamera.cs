using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace DungeonInspector
{
    public class DCamera
    {
        public Vector2 ScreenSize { get; set; }
        public Vector2 position { get; set; }
        public int PixelsPerUnit { get; set; } = 32;
        public Rect BoundsRect { get; set; }


        public DCamera()
        {
            ScreenSize = new Vector2(0, 360);

            position = default;
        }

        public Rect World2RectPos(Vector2 pos, Vector2 scale)
        {
            return Utils.World2RectPos(pos, scale, BoundsRect, position, PixelsPerUnit);
        }

        public Vector2 Mouse2WorldPos(Vector2 mousePosition)
        {
            var xPos = mousePosition.x - BoundsRect.x - BoundsRect.width / 2;
            var yPos = -(mousePosition.y - BoundsRect.y - BoundsRect.height / 2);

            return new Vector2(xPos + position.x, yPos + position.y) / PixelsPerUnit;
        }

        public void Update()
        {
            ScreenSize = new Vector2(EditorGUIUtility.currentViewWidth, ScreenSize.y);
            BoundsRect = new Rect(EditorGUIUtility.currentViewWidth / 2 - ScreenSize.x / 2, 0, ScreenSize.x, ScreenSize.y);
        }
    }
}
