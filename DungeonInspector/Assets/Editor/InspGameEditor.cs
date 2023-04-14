using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using TreeEditor;

namespace DungeonInspector
{
    [CustomEditor(typeof(InspGame))]
    public class InspGameEditor : Editor
    {
        private Rect _gameViewport;
        private const float _pixelPerUnit = 25f;
        private Vector2 _cameraPos;

        private E_SpriteAtlas _playerSprites;
        private System.Diagnostics.Stopwatch _stopWatch;

        private float _dt = 0;
        private float _time = 0;
        private float _prev;
        private Vector2 _playerPos;

        private SpriteAnimator _playerAnimator;
        private const float _moveSpeed = 2;
        private Perlin _perlin;
        //private Vector2 _playerWalkDir;

        private List<(Vector2, Texture2D)> _tiles;

        private Vector2Int _mouseTileGuidePosition;

        private void OnEnable()
        {
            _playerSprites = Resources.Load<E_SpriteAtlas>("PlayerSpriteAtlas");

            _stopWatch = new System.Diagnostics.Stopwatch();
            _stopWatch.Start();
            _prev = _stopWatch.ElapsedMilliseconds / 1000f;

            _time = 0;
            _playerPos = default;
            //_playerAnimator = new SpriteAnimator(GetAnimation("WalkLeft"), GetAnimation("WalkRight"), GetAnimation("WalkUp"), GetAnimation("WalkDown"));

            var name = "Character2/WalkLeft";
            _playerAnimator = new SpriteAnimator(GetAnimation(name), GetAnimation(name), GetAnimation(name), GetAnimation(name));


            _playerAnimator.Speed = 14f;
            _playerAnimator.Play(0);
            _playerAnimator.Stop();

            _perlin = new Perlin();
            _tiles =new List<(Vector2, Texture2D)>();
        }

        private SpriteAnimation GetAnimation(string atlasName)
        {
            return new SpriteAnimation(Resources.Load<E_SpriteAtlas>(atlasName));
        }

        public override void OnInspectorGUI()
        {
            Repaint();

            var secElapsep = _stopWatch.ElapsedMilliseconds / 1000f;

            _dt = secElapsep - _prev;
            _time += _dt;
            _prev = secElapsep;

            var viewportHeight = 300;
            var screenSize = new Vector2(600, 300);

            _gameViewport = new Rect(EditorGUIUtility.currentViewWidth / 2 - screenSize.x / 2, viewportHeight / 2 - screenSize.y / 2, screenSize.x, screenSize.y);

            // Background.
            EditorGUI.DrawRect(_gameViewport, Color.black * 0.7f);

            DrawGrid(new Vector2(screenSize.x, screenSize.y), Color.white * 0.3f);

            GUILayout.Space(viewportHeight);
            //Debug.Log(", t: " + _time);
            //GUI.matrix = Matrix4x4.Rotate(Quaternion.Euler(new Vector3(0, 0, 10)));

            Vector2 scale = new Vector2(1, 1);

            DrawSprite(Vector2.one, scale, 0);

            Input();
            _playerAnimator.Update(_dt);

             
            DrawSprite(_playerPos, new Vector2(1 + _playerAnimator.CurrentTex.width / _playerAnimator.CurrentTex.height , 1 + _playerAnimator.CurrentTex.height / _playerAnimator.CurrentTex.width), 15f, _playerAnimator.CurrentTex);

            _cameraPos = Vector2.Lerp(_cameraPos, new Vector2(_playerPos.x, _playerPos.y) * _pixelPerUnit, 10 * _dt);

             
            var mouse = Event.current;
            //mouse.mousePosition = new Vector2(Mathf.Clamp(mouse.mousePosition.x, _gameViewport.x + _pixelPerUnit / 2, _gameViewport.width - _pixelPerUnit / 2), Mathf.Clamp(mouse.mousePosition.y, _gameViewport.y + _pixelPerUnit / 2, _gameViewport.height - _pixelPerUnit / 2));

            var newMousePos = (new Vector2(mouse.mousePosition.x - _gameViewport.x - _gameViewport.width / 2, -(mouse.mousePosition.y - _gameViewport.height / 2)) + _cameraPos) / _pixelPerUnit;


            newMousePos.x = Mathf.RoundToInt(newMousePos.x);
            newMousePos.y = Mathf.RoundToInt(newMousePos.y);
            //Debug.Log(newMousePos);

            _mouseTileGuidePosition = new Vector2Int((int)newMousePos.x, (int)newMousePos.y);


            // Mouse sprite pointer
            DrawSprite(newMousePos, Vector2.one, 0);

            SetTile();

            for (int i = 0; i < _tiles.Count; i++)
            {
                DrawSprite(_tiles[i].Item1, new Vector2(1,1), 0, _tiles[i].Item2);
            }
        }


        private void SetTile()
        {
            if (Event.current.isMouse && Event.current.button == 0)
            {
                _tiles.Add((_mouseTileGuidePosition, Texture2D.whiteTexture));
            }
        }

        private void BoundingBoxes()
        {

        }

        private void Input()
        {
            var e = Event.current;
            var _playerWalkDir = new Vector2();


            // if (e.type == EventType.KeyDown)
            {
                if (e.keyCode == KeyCode.A)
                {
                    _playerAnimator.Play(0);

                    _playerWalkDir = new Vector2(-1, 0);
                }
                else if (e.keyCode == KeyCode.D)
                {
                    //_playerPos.x += _moveSpeed * _pixelPerUnit * _dt;
                    _playerWalkDir = new Vector2(1, 0);

                    _playerAnimator.Play(1);
                }
                else if (e.keyCode == KeyCode.W)
                {
                    _playerAnimator.Play(2);

                    //_playerPos.y += _moveSpeed * _pixelPerUnit * _dt;
                    _playerWalkDir = new Vector2(0, 1);

                }
                else if (e.keyCode == KeyCode.S)
                {
                    _playerAnimator.Play(3);
                    //_playerPos.y -= _moveSpeed * _pixelPerUnit * _dt;
                    _playerWalkDir = new Vector2(0, -1);
                }
                //if (e.type == EventType.KeyUp || !e.isKey)
                //{
                //    _playerWalkDir = default;
                //   // _playerAnimator.Stop();
                //}
            }

            _playerPos += _playerWalkDir * _moveSpeed * _pixelPerUnit * _dt;
        }

        private void DrawGrid(Vector2 screenSize, Color color)
        {
            for (int i = 0; i < Mathf.RoundToInt(screenSize.x / _pixelPerUnit); i++)
            {
                EditorGUI.DrawRect(new Rect(_gameViewport.x + i * _pixelPerUnit - _cameraPos.x + _pixelPerUnit / 2f, _gameViewport.y + _cameraPos.y, 1, _gameViewport.height), color);
            }

            for (int i = 0; i < Mathf.RoundToInt(screenSize.y / _pixelPerUnit); i++)
            {
                EditorGUI.DrawRect(new Rect(_gameViewport.x - _cameraPos.x, _gameViewport.y + i * _pixelPerUnit + _cameraPos.y + _pixelPerUnit / 2f, _gameViewport.width, 1), color);
            }
        }


        private Rect DrawSprite(Vector2 pos, Vector2 scale, float zRot, Texture2D sprite = null)
        {
            if (sprite == null)
            {
                sprite = Texture2D.whiteTexture;
            }

            var matrix = GUI.matrix;
            //GUI.matrix = Matrix4x4.TRS(pos, Quaternion.Euler(0,0, zRot), Vector3.one);
            var finalRect = ToViewport(pos, scale);

            bool snap = true;

            if (snap)
            {
                finalRect = new Rect((int)finalRect.x, (int)finalRect.y, (int)finalRect.width, (int)finalRect.height);
            }

            if (finalRect.y < _gameViewport.height && finalRect.y > _gameViewport.y )
            {
                Graphics.DrawTexture(finalRect, sprite);
                GUI.matrix = matrix;
            }
           

            return finalRect;
        }


        private Rect ToViewport(Vector2 pos, Vector2 scale)
        {
            var gameScale = scale * _pixelPerUnit;
            pos.y = -pos.y;
            var gamePos = pos * _pixelPerUnit;

            return new Rect(_gameViewport.x + _gameViewport.width * 0.5f + gamePos.x - gameScale.x * 0.5f - _cameraPos.x,
                            _gameViewport.y + _gameViewport.height * 0.5f + gamePos.y - gameScale.y * 0.5f + _cameraPos.y,
                            gameScale.x,
                            gameScale.y);
        }
    }
}
