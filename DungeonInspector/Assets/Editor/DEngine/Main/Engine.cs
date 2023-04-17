using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonInspector
{
    public class DEngine
    {
        private readonly DEntitiesController _componentsContainer;
        private readonly DRenderingController _renderer;
        private readonly DSandboxBase _sandbox;
        private readonly DTime _time;
        private readonly DInput _input;

        public DEngine(DSandboxBase sandbox)
        {
            new DIEngineCoreServices();

            _time = new DTime();
            _input = new DInput();
            _sandbox = sandbox;
            _renderer = DIEngineCoreServices.Get<DRenderingController>();
            _componentsContainer = DIEngineCoreServices.Get<DEntitiesController>();


            _sandbox.OnInitialize();

            _renderer.CameraTest = DCamera.MainCamera;

            _componentsContainer.OnAwake();
        }

        public void UpdateGUI()
        {
            _time.Update();
            _input.Update();
            _componentsContainer.Update();
            _renderer.Update();
        }

        public void Destroy()
        {
            _sandbox.OnQuit();
        }

    }
}