using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonInspector
{
    public class ScreenUI : DBehavior
    {
        private DRendererUIComponent _renderer;

        [DExpose] private bool _fadeIn;
        [DExpose] private float _secs = 0.8f;

        private float _time;
        private Action _callback;
        private bool _called;

        protected override void OnAwake()
        {
            _renderer = GetComp<DRendererUIComponent>();
            _renderer.TransformWithCamera = false;

            _renderer.Transform.Scale = new DVec2(300, 300);
            _renderer.ZSorting = 100;
            _renderer.Color = Color.black;
        }

        protected override void OnLateUpdate()
        {
            if (_fadeIn)
            {
                _time += DTime.DeltaTime / _secs;

                if(_time > 1 && !_called)
                {
                    _called = true;
                    _callback?.Invoke();
                }
            }
            else
            {
                _time -= DTime.DeltaTime / _secs;

                if (_time < 0 && !_called)
                {
                    _called = true;
                    _callback?.Invoke();
                }
            }

            _time = UnityEngine.Mathf.Clamp(_time, 0.0f, 1.0f);

            var apha = UnityEngine.Mathf.Lerp(0, 1, _time);

            var c = _renderer.Color;

            _renderer.Color = new UnityEngine.Color(c.r / 255f, c.g / 255f, c.b / 255f, apha);


        }

        public void FadeIn(Action callback)
        {
            _fadeIn = true;
            _called = false;

            _callback=callback;
        }

        public void FadeOut(Action callback)
        {
            _fadeIn = false;
            _called = false;

            _callback = callback;
        }
    }
}
