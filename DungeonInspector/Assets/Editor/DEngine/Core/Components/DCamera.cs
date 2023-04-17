using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace DungeonInspector
{
    public class DCamera : DBehavior
    {
        public static int PixelsPerUnit { get; set; } = 32;
        public static DCamera MainCamera { get; set; }
        public DVector2 ScreenSize { get; set; }
        public Rect ViewportRect { get; set; }

        public DCamera()
        {
            ScreenSize = new DVector2(640, 360);
        }

        public Rect World2RectPos(DVector2 pos, DVector2 scale)
        {
            return Utils.World2RectPos(pos, scale, ViewportRect, Transform.Position, PixelsPerUnit);
        }

        public DVector2 Mouse2WorldPos(DVector2 mousePosition)
        {
            var xPos = mousePosition.x - ViewportRect.x - ViewportRect.width / 2;
            var yPos = -(mousePosition.y - ViewportRect.y - ViewportRect.height / 2);

            return new DVector2(xPos + Transform.Position.x * PixelsPerUnit, yPos + Transform.Position.y * PixelsPerUnit) / PixelsPerUnit;
        }

        public bool IsInside(DVector2 worldpos, Vector2 scale)
        {
            var rect = World2RectPos(worldpos, scale);

            return false;
        }

        protected override void OnUpdate()
        {
            ScreenSize = new DVector2(EditorGUIUtility.currentViewWidth, ScreenSize.y);
            ViewportRect = new Rect(EditorGUIUtility.currentViewWidth / 2 - ScreenSize.x / 2, 0, ScreenSize.x, ScreenSize.y);

            // should not be here
            GUILayoutUtility.GetRect(ViewportRect.width, ViewportRect.height);

        }
    }
}
