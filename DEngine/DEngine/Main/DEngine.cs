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
        private DEngineSystemBase[] _currentServices;

        private bool _isPaused = false;
        private DRendering _renderingSystem;

        public DEngine(DSandboxBase playModeSandBox, DSandboxBase editModeSandBox)
        {
            _playModeSandBox = playModeSandBox;
            _editModeSandBox = editModeSandBox;


            _currentServices = ConstructServices(editModeSandBox);

            _editorSystem = DIEngineCoreServices.Get<DEditorSystem>();
          

            _editorSystem.Init();

            _editorSystem.Toolbar.OnPlayBegin += OnPlayBegin; 
            _editorSystem.Toolbar.OnPlayEnd += OnPlayEnd;
            _editorSystem.Toolbar.OnPauseBegin += OnPauseBegin;
            _editorSystem.Toolbar.OnPauseEnd += OnPauseEnd;

            RunSandbox(_editModeSandBox);
        }

        private void OnPauseBegin()
        {
            _isPaused = true;
        }

        private void OnPauseEnd()
        {
            _isPaused = false;
        }

        private void OnPlayEnd()
        {
            _isPaused = false;
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

            _renderingSystem = (DRendering)_currentServices.First(x => x as DRendering != null);

            _currentSandBox.OnInitialize();

            for (int i = 0; i < _currentServices.Length; i++)
            {
                _currentServices[i].Init();
            }

            _editorSystem.Init();
        }

        public void Update()
        {
            if (!_isPaused)
            {
                for (int i = 0; i < _currentServices.Length; i++)
                {
                    _currentServices[i].Update();
                }
            }
            else
            {
                _renderingSystem.Update();
            }
           
            _editorSystem.Update();
        }

        private DSandboxBase GetSandboxCopy(DSandboxBase original)
        {
            return (DSandboxBase)Activator.CreateInstance(original.GetType(), (object)original.Services);
        }

        private DEngineSystemBase[] ConstructServices(DSandboxBase sandbox)
        {
            var services = new DEngineSystemBase[sandbox.Services.Length];

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