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
        private readonly EngineSystemBase _editorSystem;
        private readonly EngineSystemBase _components;
        private readonly EngineSystemBase _renderer;
        private readonly EngineSystemBase _physics;
        private readonly EngineSystemBase _input;
        private readonly EngineSystemBase _audio;
        private readonly EngineSystemBase _time;

        private readonly DSandboxBase _sandbox;

        public DEngine(DSandboxBase sandbox)
        {
            _sandbox = sandbox;

            new DIEngineCoreServices();
            new Utils();

            _time = DIEngineCoreServices.Get<DTime>();
            _audio = DIEngineCoreServices.Get<DAudioSystem>();
            _input = DIEngineCoreServices.Get<DInput>();
            _physics = DIEngineCoreServices.Get<DPhysicsController>();
            _renderer = DIEngineCoreServices.Get<DRenderingController>();
            _components = DIEngineCoreServices.Get<DEntitiesController>();
            _editorSystem = DIEngineCoreServices.Get<DEditorSystem>();

            _sandbox.OnInitialize();
            _time.Init();
            _input.Init();
            _physics.Init();
            _renderer.Init();
            _components.Init();

            _editorSystem.Init();
        }
         
        public void Update()
        {
            _time.Update();
            _input.Update();
            _components.Update();
            _physics.Update();
            _renderer.Update();
            _editorSystem.Update();
        }

        public void Destroy()
        {
            _sandbox.OnQuit();

            _time.Cleanup();
            _audio.Cleanup();
            _input.Cleanup();
            _components.Cleanup();
            _physics.Cleanup();
            _renderer.Cleanup();
            _editorSystem.Cleanup();
        }
    }
}