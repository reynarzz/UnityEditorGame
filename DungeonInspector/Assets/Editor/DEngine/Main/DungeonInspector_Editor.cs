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
        private DInput _input;

        private void OnEnable()
        {
            new DIEngineCoreServices();

            _time = new DTime();
            _input = new DInput();
            _sandbox = new DungeonInspectorSandBox();
            _renderer = DIEngineCoreServices.Get<DRenderingController>();
            _componentsContainer = DIEngineCoreServices.Get<DEntitiesController>();


            _sandbox.OnInitialize();

            _renderer.CameraTest = DCamera.MainCamera;

            _componentsContainer.OnAwake();
        }
        
        public override void OnInspectorGUI()
        {
            Repaint();

            _time.Update();
            _input.Update();
            _componentsContainer.Update();
            _renderer.Update();
        }

        private void OnDestroy()
        {
            _sandbox.OnQuit();
        }
    }
}
