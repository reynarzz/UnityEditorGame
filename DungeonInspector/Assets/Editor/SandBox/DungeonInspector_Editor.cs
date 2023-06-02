using UnityEditor;
using UnityEngine;

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
                                  typeof(DEntitiesController),
                                  typeof(DPhysicsController),
                                  typeof(DRendering));

                var editorSandbox = new DungeonEditModeSandbox(
                                    //typeof(DEditorSystem),
                                    typeof(DTime),
                                    typeof(DInput),
                                    typeof(DRendering),
                                    typeof(DEntitiesController));

                _engine = new DEngine(gameSandBox, editorSandbox);
            }
        }

        public override void OnInspectorGUI()
        {
            _engine?.Update();
            Repaint();

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
