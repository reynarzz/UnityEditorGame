using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Windows;

namespace DungeonInspector
{
    public class DEngine
    {
        private readonly IDService _components;
        private readonly IDService _physics;
        private readonly IDService _input;
        private readonly IDService _time;

        private readonly DRenderingController _renderer;
        private readonly DSandboxBase _sandbox;

        public DEngine(DSandboxBase sandbox)
        {
            _sandbox = sandbox;

            new DIEngineCoreServices();

            _time = DIEngineCoreServices.Get<DTime>();
            _input = DIEngineCoreServices.Get<DInput>();
            _components = DIEngineCoreServices.Get<DEntitiesController>();
            _physics = DIEngineCoreServices.Get<DPhysicsController>();
            _renderer = DIEngineCoreServices.Get<DRenderingController>();

            _sandbox.OnInitialize();
            _time.Init();
            _input.Init();
            _physics.Init();
            _renderer.Init();
            _components.Init();
        }

        public void Update()
        {
            _time.Update();
            _input.Update();
            _components.Update();
            _physics.Update();
            _renderer.Update();
        }

        public void Destroy()
        {
            _sandbox.OnQuit();
        }
    }
}