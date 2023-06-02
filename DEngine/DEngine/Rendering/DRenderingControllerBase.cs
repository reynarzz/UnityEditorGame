using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonInspector
{
    public interface IDRenderingControllerBase
    {
        void Draw(DRendererComponent renderer, DCamera camera, Material defaultMat, Texture2D defaultTexture);
    }

    public abstract class DRenderingControllerBase<T> : IDRenderingControllerBase where T : DRendererComponent
    {
        void IDRenderingControllerBase.Draw(DRendererComponent renderer, DCamera camera, Material material, Texture2D defaultTexture)
        {
            Draw(renderer as T, camera, material, defaultTexture);
        }

        protected abstract void Draw(T renderer, DCamera camera, Material material, Texture2D defaultTexture);


        public void SetState(string varName, KeyValuePair<ShaderStateDataType, object> data, Material mat)
        {
            switch (data.Key)
            {
                case ShaderStateDataType.Vector:
                    mat.SetVector(varName, (Vector4)data.Value);
                    break;
                case ShaderStateDataType.Int:
                    mat.SetInt(varName, (int)data.Value);
                    break;
                case ShaderStateDataType.Float:
                    mat.SetFloat(varName, (float)data.Value);
                    break;
                case ShaderStateDataType.Matrix:
                    mat.SetMatrix(varName, (Matrix4x4)data.Value);
                    break;
                case ShaderStateDataType.Color:
                    mat.SetColor(varName, (Color)data.Value);
                    break;

            }
        }

        public void ClearState(string varName, ShaderStateDataType dataType, Material mat)
        {
            switch (dataType)
            {
                case ShaderStateDataType.Vector:
                    mat.SetVector(varName, default);
                    break;
                case ShaderStateDataType.Int:
                    mat.SetInt(varName, default);
                    break;
                case ShaderStateDataType.Float:
                    mat.SetFloat(varName, default);
                    break;
                case ShaderStateDataType.Matrix:
                    mat.SetMatrix(varName, default);
                    break;

            }
        }
    }
}
