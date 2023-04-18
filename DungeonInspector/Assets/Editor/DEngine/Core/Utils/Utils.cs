using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace DungeonInspector
{
    public static class Utils
    {
        public static T GetValue<T>(object target, string fieldName)
        {
            return (T)target.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance).GetValue(target);
        }

        public static void SetValue<T>(object target, string fieldName, T value)
        {
            target.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance).SetValue(target, value);
        }

        public static Rect World2RectPos(Vector2 pos, Vector2 scale, Rect viewportRect, Vector2 cameraPos, int pixelsPerUnit)
        {
            var gameScale = scale * pixelsPerUnit;
            pos.y = -pos.y;
            var gamePos = pos * pixelsPerUnit;

            return new Rect(viewportRect.x + viewportRect.width * 0.5f + gamePos.x - gameScale.x * 0.5f - cameraPos.x * pixelsPerUnit,
                            viewportRect.y + viewportRect.height * 0.5f + gamePos.y - gameScale.y * 0.5f + cameraPos.y * pixelsPerUnit,
                            gameScale.x,
                            gameScale.y);
        }

        public static Rect World2RectPosKeepPivot(Vector2 pos, Vector2 scale, Rect viewportRect, Vector2 cameraPos, int pixelsPerUnit)
        {
            var gameScale = scale * pixelsPerUnit;
            pos.y = -pos.y;
            var gamePos = pos * pixelsPerUnit;

            return new Rect(viewportRect.x + viewportRect.width * 0.5f + gamePos.x - cameraPos.x * pixelsPerUnit,
                            viewportRect.y + viewportRect.height * 0.5f + gamePos.y + cameraPos.y * pixelsPerUnit,
                            gameScale.x,
                            gameScale.y);
        }

        /// <summary>World2Rect without take the camera movement into account</summary>
        public static Rect World2RectNCamPos(Vector2 pos, Vector2 scale, Rect viewportRect, int pixelsPerUnit)
        {
            return World2RectPos(pos, scale, viewportRect, default, pixelsPerUnit);
        }

        public static void DrawGrid(Vector2 screenSize, Color color, DCamera camera)
        {
            var xCount = 20f; //Mathf.RoundToInt(screenSize.x / _pixelPerUnit) -1;
            var yCount = 20f;// Mathf.RoundToInt(screenSize.y / _pixelPerUnit) - 1;

            var pixelPerUnit = DCamera.PixelsPerUnit;
            var viewportRect = camera.ViewportRect;

            var totalSpaceX = (screenSize.x - (pixelPerUnit * (xCount))) / 2f;
            var totalSpaceY = (screenSize.y - (pixelPerUnit * yCount)) / 2f;

            var cameraPos = camera.Transform.Position;

            Debug.Log(pixelPerUnit * xCount + "w: " + screenSize.x + ". s: " + totalSpaceX);

            for (int i = 0; i < Mathf.RoundToInt(xCount); i++)
            {
                EditorGUI.DrawRect(new Rect(viewportRect.x + totalSpaceX + i * pixelPerUnit - cameraPos.x + pixelPerUnit / 2, viewportRect.y + cameraPos.y, 1f, pixelPerUnit * yCount), color);
            }

            for (int i = 0; i < Mathf.RoundToInt(yCount); i++)
            {
                EditorGUI.DrawRect(new Rect(viewportRect.x - cameraPos.x, viewportRect.y - totalSpaceY + i * pixelPerUnit + cameraPos.y, totalSpaceX * xCount, 1), color);
            }
        }

        public static T Load<T>(string pathFromRes) where T : UnityEngine.Object
        {
            return Resources.Load<T>(pathFromRes);
        }
    }
}
