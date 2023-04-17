using UnityEditor;
using UnityEngine;

namespace DungeonInspector
{
    [CustomEditor(typeof(DungeonInspector))]
    public class DungeonInspector_Editor : Editor
    {
        private /*static*/ DEngine _engine;

        private void OnEnable()
        {
            if (_engine == null)
            {
                _engine = new DEngine(new DungeonInspectorSandBox());
            }
        }

        public override void OnInspectorGUI()
        {
            Repaint();

            _engine?.UpdateGUI();
        }

        private void OnDestroy()
        {
            _engine?.Destroy();
        }
    }
}
