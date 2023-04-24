using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;

namespace DungeonInspector
{
    [CustomEditor(typeof(DEngineView))]
    public class DungeonInspector_Editor : Editor
    {
        private /*static*/ DEngine _engine;

        private void OnEnable()
        {
            if (_engine == null)
            {
                var gameSandBox = new DungeonPlaymodeSandBox(
                                  typeof(DTime),
                                  typeof(DInput),
                                  typeof(DAudioSystem),
                                  typeof(DPhysicsController),
                                  typeof(DRenderingController),
                                  typeof(DEntitiesController));

                var editorSandbox = new DungeonEditModeSandbox(
                                    //typeof(DEditorSystem),
                                    typeof(DTime),
                                    typeof(DInput),
                                    typeof(DRenderingController),
                                    typeof(DEntitiesController));

                _engine = new DEngine(gameSandBox, editorSandbox);
            }
        }

        public override void OnInspectorGUI()
        {
            Repaint();

            _engine?.Update();
        }

        private void OnDisable()
        {
            _engine?.Destroy();
        }

        //private void OnDestroy()
        //{
        //    _engine?.Destroy();
        //}
    }
}
