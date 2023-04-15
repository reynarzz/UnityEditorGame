using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;

namespace DungeonInspector
{
    [CustomEditor(typeof(DungeonInspector))]
    public class DungeonInspector_Editor : Editor
    {
        private DEntitiesController _componentsContainer; 
        private DRenderingController _renderer;
        private DTime _time;

        private DSandboxBase _sandbox;

        private void OnEnable()
        {
            _time = new DTime();
            _sandbox = new DungeonInspectorSandBox();
            _renderer = DIEngineCoreServices.Get<DRenderingController>();
            _componentsContainer = DIEngineCoreServices.Get<DEntitiesController>();
         

            _sandbox.Time = _time;
            _sandbox.OnInitialize();

            _componentsContainer.OnStart();

            _renderer.CameraTest = DCamera.MainCamera;
        }

        public override void OnInspectorGUI()
        {
            _time.Update();
            _componentsContainer.Update();
            _renderer.Update();

            Repaint();
        }
    }
}
