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
            _engine = new DEngine(new DungeonInspectorSandBox());
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
