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
        private static DCamera _mainCamera;
        public static DCamera MainCamera
        {
            get
            {
                return _mainCamera;
            }
            set
            {
               // if (_mainCamera == null)
                {
                    _mainCamera = value;
                }
               // else
                {
                    Debug.Log("Main camera already set");
                }
            }
        }

        public DVector2 ScreenSize { get; set; }
        public static int PixelsPerUnit { get; set; } = 32;
        public Rect BoundsRect { get; set; }

        public DCamera()
        {
            ScreenSize = new DVector2(0, 360);

        }

        public Rect World2RectPos(DVector2 pos, DVector2 scale)
        {
            return Utils.World2RectPos(pos, scale, BoundsRect, Transform.Position, PixelsPerUnit);
        }

        public DVector2 Mouse2WorldPos(DVector2 mousePosition)
        {
            var xPos = mousePosition.x - BoundsRect.x - BoundsRect.width / 2;
            var yPos = -(mousePosition.y - BoundsRect.y - BoundsRect.height / 2);

            return new DVector2(xPos + Transform.Position.x * PixelsPerUnit, yPos + Transform.Position.y * PixelsPerUnit) / PixelsPerUnit;
        }

        public override void Loop()
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
