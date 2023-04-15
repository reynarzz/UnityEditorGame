using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;

namespace DungeonInspector
{
    [CustomEditor(typeof(DungeonInspector))]
    public class DungeonInspector_Editor : Editor
    {
      
        private Vector2 _playerPos;

        private SpriteAnimator _playerAnimator;
        private const float _moveSpeed = 0.7f;
        //private Vector2 _playerWalkDir;

        private List<(Vector2, Texture2D)> _tiles;

        private Vector2Int _mouseTileGuidePosition;

        private ComponentsContainer _componentsContainer; 
        private RenderingSystem _renderer;
        private DCamera _camera;
        private DTime _time;

        private DSandboxBase _sandbox;

        private void OnEnable()
        {
            _time = new DTime();
            _camera = new DCamera();
            _renderer = new RenderingSystem();
            _sandbox = new DungeonInspectorMain();
            _componentsContainer = new ComponentsContainer();
            
            _playerPos = default;

            var name = "Character2/WalkLeft";
            var walkLeft = GetAnimation(name);
            walkLeft.Speed = 14;

            var idle = GetAnimation("Character2/Idle");
            idle.Speed = 10;
            _playerAnimator = new SpriteAnimator(walkLeft, walkLeft, walkLeft, walkLeft, idle);


            _playerAnimator.Play(4);
            _playerAnimator.Stop();

            if (_tiles == null)
                _tiles = new List<(Vector2, Texture2D)>();


            _sandbox.Time = _time;
            _sandbox.OnInitialize();
        }

        private SpriteAnimation GetAnimation(string atlasName)
        {
            return new SpriteAnimation(Resources.Load<E_SpriteAtlas>(atlasName));
        }

        public override void OnInspectorGUI()
        {

            Repaint();

            _time.Update();
            _camera.Update();
            _renderer.Update();

        
            

            var screen = _camera.BoundsRect;
            screen.height += 24;
            // Background.
            EditorGUI.DrawRect(screen, Color.black * 0.7f);

            GUILayout.Space(_camera.ScreenSize.y);

            

            _camera.position = Vector2.Lerp(_camera.position, new Vector2((int)_playerPos.x, (int)_playerPos.y) * (int)_camera.PixelsPerUnit, 7 * _time.DeltaTime);


            var mouse = Event.current;
            
            var newMousePos = _camera.Mouse2WorldPos(mouse.mousePosition);


            newMousePos.x = Mathf.RoundToInt(newMousePos.x);
            newMousePos.y = Mathf.RoundToInt(newMousePos.y);

            _mouseTileGuidePosition = new Vector2Int((int)newMousePos.x, (int)newMousePos.y);


            SetTile();

            for (int i = 0; i < _tiles.Count; i++)
            {
                DrawSprite(_tiles[i].Item1, new Vector2(1, 1), _camera, _tiles[i].Item2);
            }

            // Mouse sprite pointer
            DrawSprite(newMousePos, Vector2.one, _camera, WorldEditorEditor.SelectedTex);

            PlayerMovement();
            _playerAnimator.Update(_time.DeltaTime);


            DrawSprite(_playerPos, new Vector2(1 + _playerAnimator.CurrentTex.width / _playerAnimator.CurrentTex.height, 1 + _playerAnimator.CurrentTex.height / _playerAnimator.CurrentTex.width), _camera, _playerAnimator.CurrentTex);
        }


        private void SetTile()
        {
            if (Event.current.isMouse && Event.current.button == 0)
            {
                _tiles.Add((_mouseTileGuidePosition, WorldEditorEditor.SelectedTex));
            }

        }

       

        Vector2Int _playerWalkDir = new Vector2Int();

        private void PlayerMovement()
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

            _playerPos += dir * _moveSpeed * _camera.PixelsPerUnit * _time.DeltaTime;
        }

        private Rect DrawSprite(Vector2 worldPos, Vector2 scale, DCamera camera, Texture2D sprite = null)
        {
            if (sprite == null)
            {
                sprite = Texture2D.whiteTexture;
            }

            var matrix = GUI.matrix;
            //GUI.matrix = Matrix4x4.TRS(pos, Quaternion.Euler(0,0, zRot), Vector3.one);
            var finalRect = _camera.World2RectPos(worldPos, scale);

            bool snap = true;

            if (snap)
            {
                finalRect = new Rect((int)finalRect.x, (int)finalRect.y, (int)finalRect.width, (int)finalRect.height);
            }

            if (finalRect.y < camera.BoundsRect.height && finalRect.y > camera.BoundsRect.y)
            {
                Graphics.DrawTexture(finalRect, sprite);
                GUI.matrix = matrix;
            }


            return finalRect;
        }


        
    }
}
