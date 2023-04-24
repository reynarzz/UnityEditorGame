using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

namespace DungeonInspector
{
    public class DEditorToolBar
    {
        private Color _toolBarColor = new Color(0.1f, 0.1f, 0.1f, 1f);

        private readonly GUIContent _playButton;
        private readonly GUIContent _pauseButton;

        private bool _isPlaying;
        private bool _isPaused;

        private readonly DEditorSystem _system;

        public event Action OnPlayBegin;
        public event Action OnPlayEnd;
        public event Action OnPauseBegin;
        public event Action OnPauseEnd;

        public DEditorToolBar(DEditorSystem dEditorSystem)
        {
            _system = dEditorSystem;

            _playButton = EditorGUIUtility.IconContent("d_PlayButton");
            _pauseButton = EditorGUIUtility.IconContent("d_PauseButton");
        }

        public void Update()
        {
            var rect = new Rect(0, 0, EditorGUIUtility.currentViewWidth, 30);

            GUILayout.BeginArea(rect);
            EditorGUI.DrawRect(rect, _toolBarColor);
            GUILayout.BeginHorizontal(EditorStyles.helpBox);

            var c = GUI.backgroundColor;


            if (_isPlaying)
            {
                GUI.backgroundColor = new Color(0.2f, 0.7f, 1f, 1);
            }
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(_playButton))
            {
                _isPlaying = !_isPlaying;

                if (!_isPlaying)
                {
                    _isPaused = false;
                    OnPlayEnd?.Invoke();
                }
                else
                {
                    OnPlayBegin?.Invoke();
                }
            }
            GUI.backgroundColor = c;

            if (_isPaused && _isPlaying)
            {
                GUI.backgroundColor = new Color(1, 0.3f, 0.3f, 1);
            }

            if (GUILayout.Button(_pauseButton))
            {
                if (_isPlaying)
                {
                    _isPaused = !_isPaused;
                }

                if (_isPaused)
                {
                    OnPauseBegin?.Invoke();
                }
                else
                {
                    OnPauseEnd?.Invoke();
                }
            }
            GUILayout.FlexibleSpace();
            GUI.backgroundColor = c;

            GUILayout.EndHorizontal();

            GUILayout.EndArea();
        }
    }
}
