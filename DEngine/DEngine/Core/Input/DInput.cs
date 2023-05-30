using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions.Must;

namespace DungeonInspector
{
    public class DInput : DEngineSystemBase
    {
        private static KeyCode _currentKey;
        private static KeyCode _prevKey;


        private bool _keyDown;
        private bool _mouseDown;

        private static int _mouseButton = -1;
        private static int _prevMouseButton = -1;

        public static string CurrentKeyString() => _currentKey.ToString();
        private static DVec2 _mouseDelta;
        private static DVec2 _mouseWheelDelta;
        private DVec2 _mousePrev;
        private static bool _isMouseValid;

        public static DVec2 MouseDelta => _mouseDelta;
        public static DVec2 MouseWheelDelta => _mouseWheelDelta;
        private static DVec2 _prevWorldMousePos;

        //TODO:Listen for multiple keys held down
        public override void Update()
        {
            var ev = Event.current;
            
            //if (ev.isMouse)
            //{
            //    _isMouseValid = DCamera._viewportRect.Contains(ev.mousePosition);
            //}
            _isMouseValid = true;
            if (ev.type == EventType.KeyDown)
            {
                if (!_keyDown)
                {
                    _currentKey = ev.keyCode;
                }

                _keyDown = true;
            }
            else if (ev.type == EventType.KeyUp)
            {
                if (_keyDown)
                {
                    _currentKey = KeyCode.None;
                    _prevKey = KeyCode.None;
                }

                _keyDown = false;
            }

            if (_isMouseValid)
            {
                if (ev.type == EventType.MouseDown)
                {
                    if (!_mouseDown)
                    {
                        _mouseButton = ev.button;
                        _mousePrev = ev.mousePosition;

                    }
                    _mouseDown = true;
                }
                else if (ev.type == EventType.MouseUp)
                {
                    if (_mouseDown)
                    {
                        _mouseButton = -1;
                        _prevMouseButton = -1;
                        _mouseDelta = default;
                    }

                    _mouseDown = false;
                }

                if (_mouseDown)
                {
                    _mouseDelta = ev.delta;
                }

                if (ev.type == EventType.ScrollWheel)
                {
                    _mouseWheelDelta = ev.delta;
                }
                else
                {
                    _mouseWheelDelta = 0;
                }
            }
        }

        public static DVec2 GetMouseWorldPos()
        {
            if (_isMouseValid)
            {
                var mouse = Event.current;

                _prevWorldMousePos = Utils.Mouse2WorldPos(mouse.mousePosition, DCamera._viewportRect, DCamera._Position, DCamera.PixelSize);
            }
         
            return _prevWorldMousePos;
        }

        public static bool IsKey(KeyCode key)
        {
            return _currentKey == key;
        }

        // TODO: fix, if same key is called in the same frame will be false, and cannot read more that one key at a time.
        public static bool IsKeyDown(KeyCode key)
        {
            if(_prevKey != _currentKey && _currentKey == key)
            {
                _prevKey = _currentKey;

                return true;
            }

            return false;
        }

        public static bool IsMouseDown(int button)
        {
            if (_prevMouseButton != _mouseButton && _mouseButton == button)
            {
                _prevMouseButton = _mouseButton;

                return true;
            }

            return false;
        }

        public static bool IsMouse(int button)
        {
            return _mouseButton == button && _isMouseValid;
        }

    }
}
