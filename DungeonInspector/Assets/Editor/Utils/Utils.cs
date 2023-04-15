using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonInspector
{
    public static class Utils
    {
        public static T GetValue<T>(this object target, string fieldName)
        {
            return (T)target.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance).GetValue(target);
        }

        public static void SetValue<T>(this object target, string fieldName, T value)
        {
            target.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance).SetValue(target, value);
        }

        public static Rect World2RectPos(Vector2 pos, Vector2 scale, Rect viewportRect, Vector2 cameraPos, int pixelsPerUnit)
        {
            var gameScale = scale * (float)pixelsPerUnit;
            pos.y = -pos.y;
            var gamePos = pos * (float)pixelsPerUnit;

            return new Rect(viewportRect.x + viewportRect.width * 0.5f + gamePos.x - gameScale.x * 0.5f - cameraPos.x,
                            viewportRect.y + viewportRect.height * 0.5f + gamePos.y - gameScale.y * 0.5f + cameraPos.y,
                            gameScale.x,
                            gameScale.y);
        }
    }
}
