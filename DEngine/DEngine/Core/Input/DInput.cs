using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace DungeonInspector
{
    public class DInput : EngineSystemBase
    {
        private static KeyCode _currentKey;
        private static KeyCode _prevKey;
        private bool _keyDown;

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


        
    }
}
