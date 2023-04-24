using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

namespace DungeonInspector
{ 
    public class DungeonWindows : EditorWindow
    {
        private DEngine _engine;

        [MenuItem("DEngine/Dungeon Window")]
        static void Open()
        {
             GetWindow<DungeonWindows>("Dungeon Window").Show();
        }
         
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
        
        private void OnGUI()
        {
            Repaint();

            _engine.Update();
        }

        private void OnDestroy()
        {
            _engine.Destroy();
        }
    }
}
