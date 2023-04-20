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
    public class DInput : EngineSystemBase
    {
        private static KeyCode _currentKey;
        private static KeyCode _prevKey;


        private bool _keyDown;
        private bool _mouseDown;

        private static int _mouseButton = -1;
        private static int _prevMouseButton = -1;

        public static string CurrentKeyString() => _currentKey.ToString();

        //TODO:Listen for multiple keys held down
        public override void Update()
        {
            var ev = Event.current;

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

            if (ev.type == EventType.MouseDown)
            {
                if (!_mouseDown)
                {
                    _mouseButton = ev.button;
                }

                _mouseDown = true;
            }
            else if (ev.type == EventType.MouseUp)
            {
                if (_mouseDown)
                {
                    _mouseButton = -1;
                    _prevMouseButton = -1;
                }

                _mouseDown = false;
            }
        }

        public static DVector2 GetMouseWorldPos()
        {
            var mouse = Event.current;

            return Utils.Mouse2WorldPos(mouse.mousePosition, DCamera._viewportRect, DCamera._Position, DCamera.PixelSize);
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
            Debug.Log(_mouseButton);
            if (_prevMouseButton != _mouseButton && _mouseButton == button)
            {
                _prevMouseButton = _mouseButton;

                return true;
            }

            return false;
        }
    }
}
