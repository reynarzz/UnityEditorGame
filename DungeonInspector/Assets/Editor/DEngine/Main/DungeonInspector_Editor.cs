using UnityEditor;

namespace DungeonInspector
{
    [CustomEditor(typeof(DungeonInspector))]
    public class DungeonInspector_Editor : Editor
    {
        private DEntitiesController _componentsContainer;
        private DRenderingController _renderer;
        private DSandboxBase _sandbox;
        private DTime _time;


        private void OnEnable()
        {
            new DIEngineCoreServices();

            _time = new DTime();
            _sandbox = new DungeonInspectorSandBox();
            _renderer = DIEngineCoreServices.Get<DRenderingController>();
            _componentsContainer = DIEngineCoreServices.Get<DEntitiesController>();


            _sandbox.OnInitialize();

            _renderer.CameraTest = DCamera.MainCamera;

            _componentsContainer.OnAwake();
        }
        
        public override void OnInspectorGUI()
        {
            _time.Update();
            _componentsContainer.Update();
            _renderer.Update();

            Repaint();
        }

        private void OnDestroy()
        {
            _sandbox.OnQuit();
        }
    }
}
