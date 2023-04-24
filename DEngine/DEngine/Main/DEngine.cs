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
        private readonly DEditorSystem _editorSystem;
   

        private readonly DSandboxBase _playModeSandBox;
        private readonly DSandboxBase _editModeSandBox;

        private DSandboxBase _currentSandBox;
        private EngineSystemBase[] _currentServices;

        public DEngine(DSandboxBase playModeSandBox, DSandboxBase editModeSandBox)
        {
            _playModeSandBox = playModeSandBox;
            _editModeSandBox = editModeSandBox;


            _currentServices = ConstructServices(editModeSandBox);

            _editorSystem = DIEngineCoreServices.Get<DEditorSystem>();
          

            _editorSystem.Init();

            _editorSystem.Toolbar.OnPlayBegin += OnPlayBegin; 
            _editorSystem.Toolbar.OnPlayEnd += OnPlayEnd;

            RunSandbox(_editModeSandBox);
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
            _currentServices = ConstructServices(target);

            _currentSandBox.OnInitialize();

            for (int i = 0; i < _currentServices.Length; i++)
            {
                _currentServices[i].Init();
            }
        }

        public void Update()
        {
            for (int i = 0; i < _currentServices.Length; i++)
            {
                _currentServices[i].Update();
            }

            _editorSystem.Update();
        }

        private DSandboxBase GetSandboxCopy(DSandboxBase original)
        {
            return (DSandboxBase)Activator.CreateInstance(original.GetType(), (object)original.Services);
        }

        private EngineSystemBase[] ConstructServices(DSandboxBase sandbox)
        {
            var services = new EngineSystemBase[sandbox.Services.Length];

            new DIEngineCoreServices();
            new Utils();

            for (int i = 0; i < sandbox.Services.Length; i++)
            {
                services[i] = DIEngineCoreServices.Get(sandbox.Services[i]);
            }

            return services;
        }

        public void Destroy()
        {
            _currentSandBox?.OnQuit();

            for (int i = 0; i < _currentServices.Length; i++)
            {
                _currentServices[i].Cleanup();
            }
        }
    }
}