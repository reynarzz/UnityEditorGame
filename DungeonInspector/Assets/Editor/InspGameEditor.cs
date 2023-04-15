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
    [CustomEditor(typeof(DungeonInspector))]
    public class InspGameEditor : Editor
    {
        private E_SpriteAtlas _playerSprites;
        private System.Diagnostics.Stopwatch _stopWatch;

        private float _dt = 0;
        private float _time = 0;
        private float _prev;
        private Vector2 _playerPos;

        private SpriteAnimator _playerAnimator;
        private const float _moveSpeed = 0.7f;
        private Perlin _perlin;
        //private Vector2 _playerWalkDir;

        private List<(Vector2, Texture2D)> _tiles;

        private Vector2Int _mouseTileGuidePosition;

        private DCamera _camera;

        private void OnEnable()
        {
            _camera = new DCamera();

            _playerSprites = Resources.Load<E_SpriteAtlas>("PlayerSpriteAtlas");

            _stopWatch = new System.Diagnostics.Stopwatch();
            _stopWatch.Start();
            _prev = _stopWatch.ElapsedMilliseconds / 1000f;

            _time = 0;
            _playerPos = default;
            //_playerAnimator = new SpriteAnimator(GetAnimation("WalkLeft"), GetAnimation("WalkRight"), GetAnimation("WalkUp"), GetAnimation("WalkDown"));

            var name = "Character2/WalkLeft";
            var walkLeft = GetAnimation(name);
            walkLeft.Speed = 14;

            var idle = GetAnimation("Character2/Idle");
            idle.Speed = 10;
            _playerAnimator = new SpriteAnimator(walkLeft, walkLeft, walkLeft, walkLeft, idle);


            _playerAnimator.Play(4);
            _playerAnimator.Stop();

            _perlin = new Perlin();

            if (_tiles == null)
                _tiles = new List<(Vector2, Texture2D)>();
        }

        private SpriteAnimation GetAnimation(string atlasName)
        {
            return new SpriteAnimation(Resources.Load<E_SpriteAtlas>(atlasName));
        }

        public override void OnInspectorGUI()
        {

            Repaint();

            _camera.PixelsPerUnit = (int)EditorGUILayout.Slider("Pixel per unit", _camera.PixelsPerUnit, 1, 64);
            var secElapsep = _stopWatch.ElapsedMilliseconds / 1000f;

            _dt = secElapsep - _prev;
            _time += _dt;
            _prev = secElapsep;

            var viewportHeight = 360;
            var screenSize = new Vector2(EditorGUIUtility.currentViewWidth, 360);

            _camera.ViewportRect = new Rect(EditorGUIUtility.currentViewWidth / 2 - screenSize.x / 2, 0, screenSize.x, screenSize.y);

            

            var screen = _camera.ViewportRect;
            screen.height += 24;
            // Background.
            EditorGUI.DrawRect(screen, Color.black * 0.7f);

            //DrawGrid(new Vector2(_gameViewport.width, _gameViewport.height), Color.white * 0.3f);

            GUILayout.Space(screenSize.y);

            //--DrawSprite(Vector2.zero, new Vector2(1, 1), 0);


            _camera.position = Vector2.Lerp(_camera.position, new Vector2((int)_playerPos.x, (int)_playerPos.y) * (int)_camera.PixelsPerUnit, 7 * _dt);


            var mouse = Event.current;
            //mouse.mousePosition = new Vector2(Mathf.Clamp(mouse.mousePosition.x, _gameViewport.x + _pixelPerUnit / 2, _gameViewport.width - _pixelPerUnit / 2), Mathf.Clamp(mouse.mousePosition.y, _gameViewport.y + _pixelPerUnit / 2, _gameViewport.height - _pixelPerUnit / 2));

            var newMousePos = (new Vector2(mouse.mousePosition.x - _camera.ViewportRect.x - _camera.ViewportRect.width / 2, -(mouse.mousePosition.y - _camera.ViewportRect.y - _camera.ViewportRect.height / 2)) + _camera.position) / _camera.PixelsPerUnit;


            newMousePos.x = Mathf.RoundToInt(newMousePos.x);
            newMousePos.y = Mathf.RoundToInt(newMousePos.y);
            //Debug.Log(newMousePos);

            _mouseTileGuidePosition = new Vector2Int((int)newMousePos.x, (int)newMousePos.y);



            SetTile();

            for (int i = 0; i < _tiles.Count; i++)
            {
                DrawSprite(_tiles[i].Item1, new Vector2(1, 1), _camera, _tiles[i].Item2);
            }

            // Mouse sprite pointer
            DrawSprite(newMousePos, Vector2.one, _camera, WorldEditorEditor.SelectedTex);

            Input();
            _playerAnimator.Update(_dt);

            // Player

            DrawSprite(_playerPos, new Vector2(1 + _playerAnimator.CurrentTex.width / _playerAnimator.CurrentTex.height, 1 + _playerAnimator.CurrentTex.height / _playerAnimator.CurrentTex.width), _camera, _playerAnimator.CurrentTex);


            //delete, or fix
            //--_gameViewport.height += 12;
            //--EditorGUI.DrawRect(_gameViewport, Color.black * (Mathf.Sin(_time * 1f) + 0.3f) * 0.5f);


        }


        private void SetTile()
        {
            if (Event.current.isMouse && Event.current.button == 0)
            {
                _tiles.Add((_mouseTileGuidePosition, WorldEditorEditor.SelectedTex));
            }
        }


        private void BoundingBoxes()
        {

        }
        Vector2Int _playerWalkDir = new Vector2Int();

        private void Input()
        {
            var e = Event.current;
           // var _playerWalkDir = new Vector2Int();


            // if (e.type == EventType.KeyDown)
            {
                if (e.keyCode == KeyCode.A)
                {
                    _playerAnimator.Play(0);

                    _playerWalkDir = new Vector2Int(-1, 0);
                }
                else if (e.keyCode == KeyCode.D)
                {
                    //_playerPos.x += _moveSpeed * _pixelPerUnit * _dt;
                    _playerWalkDir = new Vector2Int(1, 0);

                    _playerAnimator.Play(1);

                }
                else if (e.keyCode == KeyCode.W)
                {
                    _playerAnimator.Play(2);

                    //_playerPos.y += _moveSpeed * _pixelPerUnit * _dt;
                    _playerWalkDir = new Vector2Int(0, 1);


                }
                else if (e.keyCode == KeyCode.S)
                {
                    _playerAnimator.Play(3);
                    //_playerPos.y -= _moveSpeed * _pixelPerUnit * _dt;
                    _playerWalkDir = new Vector2Int(0, -1);

                }
                else //if (_playerWalkDir.x == 0 && _playerWalkDir.y == 0)
                {
                    //_playerAnimator.Play(4);
                    _playerWalkDir = default;
                }
                //if (e.type == EventType.KeyUp || !e.isKey)
                //{
                //    _playerWalkDir = default;
                //   // _playerAnimator.Stop();
                //}
            }
           // Debug.Log(_playerWalkDir);



            var dir = (Vector2)_playerWalkDir;

            _playerPos += dir * _moveSpeed * _camera.PixelsPerUnit * _dt;
        }

        private void DrawGrid(Vector2 screenSize, Color color)
        {
            var xCount = 20f; //Mathf.RoundToInt(screenSize.x / _pixelPerUnit) -1;
            var yCount = 20f;// Mathf.RoundToInt(screenSize.y / _pixelPerUnit) - 1;

            var pixelPerUnit = _camera.PixelsPerUnit;
            var viewportRect = _camera.ViewportRect;

            var totalSpaceX = (screenSize.x - (pixelPerUnit * (xCount))) / 2f;
            var totalSpaceY = (screenSize.y - (pixelPerUnit * yCount)) / 2f;

            Debug.Log(pixelPerUnit * xCount + "w: " + screenSize.x + ". s: " + totalSpaceX);

            for (int i = 0; i < Mathf.RoundToInt(xCount); i++)
            {
                EditorGUI.DrawRect(new Rect(viewportRect.x + totalSpaceX + i * pixelPerUnit - _camera.position.x + pixelPerUnit / 2, viewportRect.y + _camera.position.y, 1f, pixelPerUnit * yCount), color);
            }

            for (int i = 0; i < Mathf.RoundToInt(yCount); i++)
            {
                EditorGUI.DrawRect(new Rect(viewportRect.x - _camera.position.x, viewportRect.y - totalSpaceY + i * pixelPerUnit + _camera.position.y, totalSpaceX * xCount, 1), color);
            }
        }


        private Rect DrawSprite(Vector2 pos, Vector2 scale, DCamera camera, Texture2D sprite = null)
        {
            if (sprite == null)
            {
                sprite = Texture2D.whiteTexture;
            }

            var matrix = GUI.matrix;
            //GUI.matrix = Matrix4x4.TRS(pos, Quaternion.Euler(0,0, zRot), Vector3.one);
            var finalRect = _camera.World2RectPos(pos, scale);

            bool snap = true;

            if (snap)
            {
                finalRect = new Rect((int)finalRect.x, (int)finalRect.y, (int)finalRect.width, (int)finalRect.height);
            }

            if (finalRect.y < camera.ViewportRect.height && finalRect.y > camera.ViewportRect.y)
            {
                Graphics.DrawTexture(finalRect, sprite);
                GUI.matrix = matrix;
            }


            return finalRect;
        }


        
    }
}
