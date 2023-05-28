using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace DungeonInspector
{
    public enum ShaderStateDataType
    {
        Vector,
        Int,
        Float,
        Matrix
    }

    public abstract class DRendererComponent : DTransformableComponent
    {
        public int ZSorting { get; set; } = 0;
        protected DTransformComponent _transform { get; private set; }
        public bool TransformWithCamera { get; set; } = true;
        public Color32 Color { get; set; } = UnityEngine.Color.white;
        public Material Material { get; set; }

        public DRendererComponent()
        {
            _transform = new DTransformComponent();
            _shaderState = new Dictionary<string, KeyValuePair<ShaderStateDataType, object>>();
        }

        public bool FlipX { get; set; }
        public bool FlipY { get; set; }
        private float _cutOffValue = -1;
        public float CutOffValue { get => _cutOffValue; set => _cutOffValue = 1 - value; } 
        public Color CutOffColor { get; set; } = UnityEngine.Color.white;

        //public Texture2D Sprite { get; set; }

        public Dictionary<string, KeyValuePair<ShaderStateDataType, object>> _shaderState;
        public Dictionary<string, KeyValuePair<ShaderStateDataType, object>> ShaderState => _shaderState;

        
        public void SetMatVector(string varName, Vector4 value)
        {
            if (!_shaderState.ContainsKey(varName))
            {
                _shaderState.Add(varName, new KeyValuePair<ShaderStateDataType, object>(ShaderStateDataType.Vector, value));
            }
            else
            {
                _shaderState[varName] = new KeyValuePair<ShaderStateDataType, object>(ShaderStateDataType.Vector, value);
            }
        }

        public void RemoveMatValue(string varName)
        {
            _shaderState.Remove(varName);
        }

        public void SetMatFloat(string varName, float value)
        {
            if (!_shaderState.ContainsKey(varName))
            {
                _shaderState.Add(varName, new KeyValuePair<ShaderStateDataType, object>(ShaderStateDataType.Float, value));
            }
            else
            {
                _shaderState[varName] = new KeyValuePair<ShaderStateDataType, object>(ShaderStateDataType.Float, value);
            }
        }


        public void SetMatInt(string varName, int value)
        {
            if (!_shaderState.ContainsKey(varName))
            {
                _shaderState.Add(varName, new KeyValuePair<ShaderStateDataType, object>(ShaderStateDataType.Int, value));
            }
            else
            {
                _shaderState[varName] = new KeyValuePair<ShaderStateDataType, object>(ShaderStateDataType.Int, value);
            }
        }

        public void SetMatMatrix(string varName, Matrix4x4 value)
        {
            if (!_shaderState.ContainsKey(varName))
            {
                _shaderState.Add(varName, new KeyValuePair<ShaderStateDataType, object>(ShaderStateDataType.Matrix, value));
            }
            else
            {
                _shaderState[varName] = new KeyValuePair<ShaderStateDataType, object>(ShaderStateDataType.Matrix, value);
            }
        }

    }
}