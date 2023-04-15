using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace DungeonInspector
{
    public class DCamera : DUpdatableComponent
    {
        public DVector2 ScreenSize { get; set; }
        public DVector2 position { get; set; }
        public int PixelsPerUnit { get; set; } = 32;
        public Rect BoundsRect { get; set; }


        public DCamera()
        {
            ScreenSize = new DVector2(0, 360);

            position = default;
        }

        public Rect World2RectPos(DVector2 pos, DVector2 scale)
        {
            return Utils.World2RectPos(pos, scale, BoundsRect, position, PixelsPerUnit);
        }

        public DVector2 Mouse2WorldPos(DVector2 mousePosition)
        {
            var xPos = mousePosition.x - BoundsRect.x - BoundsRect.width / 2;
            var yPos = -(mousePosition.y - BoundsRect.y - BoundsRect.height / 2);

            return new DVector2(xPos + position.x, yPos + position.y) / PixelsPerUnit;
        }

        public override void Update()
        {
            ScreenSize = new DVector2(EditorGUIUtility.currentViewWidth, ScreenSize.y);
            BoundsRect = new Rect(EditorGUIUtility.currentViewWidth / 2 - ScreenSize.x / 2, 0, ScreenSize.x, ScreenSize.y);

            var screen = BoundsRect;
            screen.height += 24;
            // Background.
            EditorGUI.DrawRect(screen, Color.black * 0.7f);

            GUILayout.Space(ScreenSize.y);

        }
    }
}
