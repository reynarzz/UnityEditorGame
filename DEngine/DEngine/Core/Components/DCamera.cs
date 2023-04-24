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
        public static int PixelSize { get; set; } = 32;
        public static DCamera MainCamera { get; set; }
        public DVec2 ScreenSize { get; set; }
        public Rect ViewportRect { get; set; }

        public static Rect _viewportRect { get; set; }
        public static DVec2 _Position { get; set; }


        public DCamera()
        {
            ScreenSize = new DVec2(640, 360);
            DIEngineCoreServices.Get<DRenderingController>().AddCamera(this);
        }

        public Rect World2RectPos(DVec2 pos, DVec2 scale)
        {
            return Utils.World2RectPos(pos, scale, ViewportRect, Transform.Position, PixelSize);
        }

        public DVec2 Mouse2WorldPos(DVec2 mousePosition)
        {
            var xPos = mousePosition.x - ViewportRect.x - ViewportRect.width / 2;
            var yPos = -(mousePosition.y - ViewportRect.y - ViewportRect.height / 2);

            return new DVec2(xPos + Transform.Position.x * PixelSize, yPos + Transform.Position.y * PixelSize) / PixelSize;
        }

        public bool IsInside(DVec2 worldpos, DVec2 scale)
        {
            var rect = World2RectPos(worldpos, scale);

            return rect.x >= ViewportRect.x && rect.x <= ViewportRect.x + ViewportRect.width &&
                   rect.y >= ViewportRect.y && rect.y <= ViewportRect.y + ViewportRect.height;
        }

        protected override void OnUpdate()
        {
            ScreenSize = new DVec2(EditorGUIUtility.currentViewWidth, ScreenSize.y);
            _viewportRect = ViewportRect = new Rect(EditorGUIUtility.currentViewWidth / 2 - ScreenSize.x / 2, 30, ScreenSize.x, ScreenSize.y);

            _Position = Transform.Position;
            // should not be here
            GUILayoutUtility.GetRect(ViewportRect.width, ViewportRect.height);

        }

        public override void OnDestroy()
        {
            DIEngineCoreServices.Get<DRenderingController>().RemoveCamera(this);
        }
    }
}
