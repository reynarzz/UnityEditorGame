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
       // private readonly EngineSystemBase _components;
        private readonly DEditorSystem _editorSystem;
        //private readonly EngineSystemBase _renderer;
        //private readonly EngineSystemBase _physics;
        //private readonly EngineSystemBase _input;
        //private readonly EngineSystemBase _audio;
        //private readonly EngineSystemBase _time;

        private readonly DSandboxBase _playModeSandBox;
        private readonly DSandboxBase _editModeSandBox;

        private DSandboxBase _currentSandBox;
        private List<EngineSystemBase> _currentServices;

        public DEngine(DSandboxBase playModeSandBox, DSandboxBase editModeSandBox)
        {
            _playModeSandBox = playModeSandBox;
            _editModeSandBox = editModeSandBox;

            new DIEngineCoreServices();
            new Utils();

            _currentServices = new List<EngineSystemBase>();

            ConstructServices(_currentServices, editModeSandBox);

            Debug.Log("ok1");

            //_time = DIEngineCoreServices.Get<DTime>();
            //_input = DIEngineCoreServices.Get<DInput>();
            //_audio = DIEngineCoreServices.Get<DAudioSystem>();
            _editorSystem = DIEngineCoreServices.Get<DEditorSystem>();
            //_physics = DIEngineCoreServices.Get<DPhysicsController>();
            //_renderer = DIEngineCoreServices.Get<DRenderingController>();
            //_components = DIEngineCoreServices.Get<DEntitiesController>();

            _editorSystem.Init();

            _editorSystem.Toolbar.OnPlayBegin += OnPlayBegin; 
            _editorSystem.Toolbar.OnPlayEnd += OnPlayEnd;

            OnPlayEnd();
        }


        private void OnPlayEnd()
        {
            RunSandbox(_editModeSandBox);
        }

        private void OnPlayBegin()
        {
            RunSandbox(_playModeSandBox);
        }

        private void RunSandbox(DSandboxBase target)
        {
            _currentSandBox?.OnQuit();
            _currentSandBox = GetSandboxCopy(target);
            _currentSandBox.OnInitialize();

            for (int i = 0; i < _currentServices.Count; i++)
            {
                _currentServices[i].Init();
            }
        }

        public void Update()
        {
            for (int i = 0; i < _currentServices.Count; i++)
            {
                _currentServices[i].Update();
            }

            _editorSystem.Update();
            //_time.Update();
            //_input.Update();
            //_components.Update();
            //_physics.Update();
            //_renderer.Update();
            //_editorSystem.Update();
        }

        private DSandboxBase GetSandboxCopy(DSandboxBase original)
        {
            var copy = (DSandboxBase)Activator.CreateInstance(original.GetType(), (object)original.Services);

            ConstructServices(_currentServices, original);

            return copy;
        }

        private void ConstructServices(List<EngineSystemBase> services, DSandboxBase sandbox)
        {
            services.Clear();
            new DIEngineCoreServices();

            for (int i = 0; i < sandbox.Services.Length; i++)
            {
                services.Add(DIEngineCoreServices.Get(sandbox.Services[i]));
            }
        }

        public void Destroy()
        {
            _currentSandBox?.OnQuit();

            for (int i = 0; i < _currentServices.Count; i++)
            {
                _currentServices[i].Cleanup();
            }

            //_time.Cleanup();
            //_audio.Cleanup();
            //_input.Cleanup();
            //_components.Cleanup();
            //_physics.Cleanup();
            //_renderer.Cleanup();
            //_editorSystem.Cleanup();
        }
    }
}