using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace DungeonInspector
{
    public class DBoxCollider : DBehavior
    {
        private DAABB _boundingBox;
        public DAABB AABB => _boundingBox;
        public DVector2 Center { get; set; }
        public DVector2 Size { get; set; } = new DVector2(1, 1);
        public bool IsColliding { get; set; }
        public bool IsTrigger { get; set; } = true;
        public bool Debug { get; set; } = false;
        protected override void OnStart()
        {
            DIEngineCoreServices.Get<DRenderingController>().AddDebugGUI(RenderBoundingBox_Test);
        }


        protected override void OnUpdate()
        {
            _boundingBox.Max = new DVector2(Transform.Position.x + Transform.Offset.x + Center.x + Size.x * 0.5f,
                                            Transform.Position.y + Transform.Offset.y + Center.y + Size.y * 0.5f);

            _boundingBox.Min = new DVector2(Transform.Position.x + Transform.Offset.x + Center.x - Size.x * 0.5f,
                                            Transform.Position.y + Transform.Offset.y + Center.y - Size.y * 0.5f);
        }

       
        private void RenderBoundingBox_Test()
        {
            if (IsTrigger && Debug)
            {
                var scale = 0.1f;
                var color = IsColliding ? Color.green : Color.white;

                var pos1 = Utils.World2RectPos(new UnityEngine.Vector2(_boundingBox.Min.x, _boundingBox.Min.y),
                                              new Vector2(scale, scale), DCamera._viewportRect, DCamera._Position, DCamera.PixelSize);

                var pos2 = Utils.World2RectPos(new UnityEngine.Vector2(_boundingBox.Min.x, _boundingBox.Max.y),
                                               new Vector2(scale, scale), DCamera._viewportRect, DCamera._Position, DCamera.PixelSize);

                var pos3 = Utils.World2RectPos(new UnityEngine.Vector2(_boundingBox.Max.x, _boundingBox.Max.y),
                                               new Vector2(scale, scale), DCamera._viewportRect, DCamera._Position, DCamera.PixelSize);

                var pos4 = Utils.World2RectPos(new UnityEngine.Vector2(_boundingBox.Max.x, _boundingBox.Min.y),
                                               new Vector2(scale, scale), DCamera._viewportRect, DCamera._Position, DCamera.PixelSize);


                var lineScale = 1;

                //EditorGUI.DrawRect(new UnityEngine.Rect(pos1.x, pos1.y, pos1.width, pos1.height), color);
                //EditorGUI.DrawRect(new UnityEngine.Rect(pos2.x, pos2.y, pos2.width, pos2.height), color);
                //EditorGUI.DrawRect(new UnityEngine.Rect(pos3.x, pos3.y, pos3.width, pos3.height), color);
                //EditorGUI.DrawRect(new UnityEngine.Rect(pos4.x, pos4.y, pos4.width, pos4.height), color);


                EditorGUI.DrawRect(new UnityEngine.Rect(pos1.x, pos1.y, lineScale, lineScale + pos2.y - pos1.y), color);
                EditorGUI.DrawRect(new UnityEngine.Rect(pos2.x, pos2.y, lineScale + pos3.x - pos2.x, lineScale), color);
                EditorGUI.DrawRect(new UnityEngine.Rect(pos3.x, pos3.y, lineScale, lineScale + pos4.y - pos3.y), color);
                EditorGUI.DrawRect(new UnityEngine.Rect(pos4.x, pos4.y, lineScale + pos1.x - pos4.x, lineScale), color);

            }
        }

        public override void OnDestroy()
        {
            DIEngineCoreServices.Get<DRenderingController>().RemoveDebugGUI(RenderBoundingBox_Test);
        }
    }
}
