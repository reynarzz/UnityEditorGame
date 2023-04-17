using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace DungeonInspector
{
    public class DInput
    {
        private static Event _event;

        private bool _endOfFrame;

        private KeyCode _currentKey;
        public void Update()
        {
            _event = Event.current;

            _currentKey = _event.keyCode;
            //EditorGUIUtility.
        }

        public static bool IsKey(KeyCode key)
        {
            return _event.keyCode == key;
        }

    }
}
