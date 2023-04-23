using System;
using System.Reflection;
using System.Windows.Documents;
using UnityEditor;
using UnityEngine;

namespace DungeonInspector
{
    public class Utils
    {
        private static Material _mat;
        private static Texture2D _whiteTex;

        public Utils()
        {
            _mat = Resources.Load<Material>("Materials/DStandard");
            _whiteTex = Texture2D.whiteTexture;
        }

        private static void OnDebugGUI()
        {
        }

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

        //public static Rect World2Rect(Vector2 pos, Vector2 scale)
        //{
        //    Matrix3x2
        //}

        public static DVector2 Mouse2WorldPos(DVector2 mousePosition, Rect ViewportRect, DVector2 cameraPos, float pixelsPerUnit)
        {
            var xPos = mousePosition.x - ViewportRect.x - ViewportRect.width / 2;
            var yPos = -(mousePosition.y - ViewportRect.y - ViewportRect.height / 2);

            return new DVector2(xPos + cameraPos.x * pixelsPerUnit, yPos + cameraPos.y * pixelsPerUnit) / pixelsPerUnit;
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
            var xCount = 20f; //(float)Math.RoundToInt(screenSize.x / _pixelPerUnit) -1;
            var yCount = 20f;// (float)Math.RoundToInt(screenSize.y / _pixelPerUnit) - 1;

            var pixelPerUnit = DCamera.PixelSize;
            var viewportRect = camera.ViewportRect;

            var totalSpaceX = (screenSize.x - (pixelPerUnit * (xCount))) / 2f;
            var totalSpaceY = (screenSize.y - (pixelPerUnit * yCount)) / 2f;

            var cameraPos = camera.Transform.Position;

            Debug.Log(pixelPerUnit * xCount + "w: " + screenSize.x + ". s: " + totalSpaceX);

            for (int i = 0; i < (int)Math.Round(xCount); i++)
            {
                EditorGUI.DrawRect(new Rect(viewportRect.x + totalSpaceX + i * pixelPerUnit - cameraPos.x + pixelPerUnit / 2, viewportRect.y + cameraPos.y, 1f, pixelPerUnit * yCount), color);
            }

            for (int i = 0; i < (int)Math.Round(yCount); i++)
            {
                EditorGUI.DrawRect(new Rect(viewportRect.x - cameraPos.x, viewportRect.y - totalSpaceY + i * pixelPerUnit + cameraPos.y, totalSpaceX * xCount, 1), color);
            }
        }

        public static T Load<T>(string pathFromRes) where T : UnityEngine.Object
        {
            return Resources.Load<T>(pathFromRes);
        }

        public static void DrawBounds(DAABB bounds, Color color, float offset = 0)
        {
            var scale = 0.1f;

            var pos1 = World2RectPos(new Vector2(bounds.Min.x - offset, bounds.Min.y - offset),
                                     new Vector2(scale, scale), DCamera._viewportRect, DCamera._Position, DCamera.PixelSize);

            var pos2 = World2RectPos(new Vector2(bounds.Min.x - offset, bounds.Max.y + offset),
                                     new Vector2(scale, scale), DCamera._viewportRect, DCamera._Position, DCamera.PixelSize);

            var pos3 = World2RectPos(new Vector2(bounds.Max.x + offset, bounds.Max.y + offset),
                                     new Vector2(scale, scale), DCamera._viewportRect, DCamera._Position, DCamera.PixelSize);

            var pos4 = World2RectPos(new Vector2(bounds.Max.x + offset, bounds.Min.y - offset),
                                     new Vector2(scale, scale), DCamera._viewportRect, DCamera._Position, DCamera.PixelSize);

            var lineScale = 1;

            EditorGUI.DrawRect(new Rect(pos1.x, pos1.y, lineScale, lineScale + pos2.y - pos1.y), color);
            EditorGUI.DrawRect(new Rect(pos2.x, pos2.y, lineScale + pos3.x - pos2.x, lineScale), color);
            EditorGUI.DrawRect(new Rect(pos3.x, pos3.y, lineScale, lineScale + pos4.y - pos3.y), color);
            EditorGUI.DrawRect(new Rect(pos4.x, pos4.y, lineScale + pos1.x - pos4.x, lineScale), color);
            //_mat.SetVector("_color", color);
            //Graphics.DrawTexture(new Rect(pos1.x, pos1.y, lineScale, lineScale + pos2.y - pos1.y), _whiteTex, _mat);
            //Graphics.DrawTexture(new Rect(pos2.x, pos2.y, lineScale + pos3.x - pos2.x, lineScale), _whiteTex, _mat);
            //Graphics.DrawTexture(new Rect(pos3.x, pos3.y, lineScale, lineScale + pos4.y - pos3.y), _whiteTex, _mat);
            //Graphics.DrawTexture(new Rect(pos4.x, pos4.y, lineScale + pos1.x - pos4.x, lineScale), _whiteTex, _mat);
            //_mat.SetVector("_color", Color.white);
        }

        public static bool Raycast(DVector2 origin, DVector2 direction, float length, out DRayHitInfo hitInfo, int layer = 0)
        {
            var colliders = DIEngineCoreServices.Get<DPhysicsController>().GetAllBodies();

            hitInfo = default;

            var closestPoint = new DVector2(float.MaxValue, float.MaxValue);

            for (int i = 0; i < colliders.Count; i++)
            {
                var collider = colliders[i].Collider;

                if (collider.IsTrigger && (collider.Entity.Layer == layer))
                {
                    var aaBB = collider.AABB;

                    var hit = default(DRayHitInfo);

                    var success = Raycast(new DRay(origin, direction), aaBB, length, out hit);

                    if (success)
                    {
                        if ((hit.Point - origin).SqrMagnitude < (closestPoint - origin).SqrMagnitude)
                        {
                            hit.Target = collider.Entity;
                            closestPoint = hit.Point;
                            hitInfo = hit;
                        }
                    }
                }
            }

            return hitInfo.Target != null;
        }

        private static bool Raycast(DRay ray, DAABB aabb, float length, out DRayHitInfo hitInfo)
        {
            float t1 = (aabb.Min.x - ray.Origin.x) / ray.Direction.x;
            float t2 = (aabb.Max.x - ray.Origin.x) / ray.Direction.x;
            float t3 = (aabb.Min.y - ray.Origin.y) / ray.Direction.y;
            float t4 = (aabb.Max.y - ray.Origin.y) / ray.Direction.y;

            float tmin = (float)Math.Max((float)Math.Min(t1, t2), (float)Math.Min(t3, t4));
            float tmax = (float)Math.Min((float)Math.Max(t1, t2), (float)Math.Max(t3, t4));


            // if tmax < 0, ray (line) is intersecting AABB, but whole AABB is behing us
            if (tmax < 0)
            {
                hitInfo = new DRayHitInfo(default, default, default);

                return false;
            }

            // if tmin > tmax, ray doesn't intersect AABB
            if (tmin > tmax)
            {
                hitInfo = new DRayHitInfo(default, default, default);

                return false;
            }

            if (tmin < 0f)
            {
                hitInfo = new DRayHitInfo(ray.Origin + ray.Direction * tmax, ray.Origin + ray.Direction * tmin, default);

                return true;
            }

            hitInfo = new DRayHitInfo(ray.Origin + ray.Direction * tmin, ray.Origin + ray.Direction * tmax, default);

            return true;
        }

        public static void DrawSquare(DVector2 pos, DVector2 scale = default)
        {
            EditorGUI.DrawRect(World2RectPos(pos, scale, DCamera._viewportRect, DCamera._Position, DCamera.PixelSize), Color.white);
        }

        public static void DrawRay()
        {

        }

        //public static float Raycast(DRay ray, DAABB aabb)
        //{
        //    float t1 = (aabb.Min.x - ray.Origin.x) / ray.Direction.x;
        //    float t2 = (aabb.Max.x - ray.Origin.x) / ray.Direction.x;
        //    float t3 = (aabb.Min.y - ray.Origin.y) / ray.Direction.y;
        //    float t4 = (aabb.Max.y - ray.Origin.y) / ray.Direction.y;
        //    float t5 = 0; //(aabb.Min.Z - ray.Origin.Z) / ray.Direction.Z;
        //    float t6 = 0;// (aabb.Max.Z - ray.Origin.Z) / ray.Direction.Z;

        //    float tmin = (float)Math.Max((float)Math.Max((float)Math.Min(t1, t2), (float)Math.Min(t3, t4)), (float)Math.Min(t5, t6));
        //    float tmax = (float)Math.Min((float)Math.Min((float)Math.Max(t1, t2), (float)Math.Max(t3, t4)), (float)Math.Max(t5, t6));

        //    // if tmax < 0, ray (line) is intersecting AABB, but whole AABB is behing us
        //    if (tmax < 0)
        //    {
        //        return -1;
        //    }

        //    // if tmin > tmax, ray doesn't intersect AABB
        //    if (tmin > tmax)
        //    {
        //        return -1;
        //    }

        //    if (tmin < 0f)
        //    {
        //        return tmax;
        //    }
        //    return tmin;
        //}
    }
}
