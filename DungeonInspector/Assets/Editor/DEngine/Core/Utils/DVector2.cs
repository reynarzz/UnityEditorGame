using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;

namespace DungeonInspector
{
    public struct DVector2
    {
        public float x { get; set; }
        public float y { get; set; }
        [JsonIgnore] public float Magnitude => (float)Math.Sqrt(x * x + y * y);
        [JsonIgnore] public float SqrMagnitude => x * x + y * y;
        [JsonIgnore] public DVector2 Normalize => this / Magnitude;

        public DVector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public static implicit operator UnityEngine.Vector2(DVector2 vector)
        {
            return new UnityEngine.Vector2(vector.x, vector.y);
        }

        public static implicit operator DVector2(UnityEngine.Vector2 vector)
        {
            return new DVector2(vector.x, vector.y);
        }

        public static implicit operator UnityEngine.Vector2Int(DVector2 vector)
        {
            return new UnityEngine.Vector2Int((int)vector.x, (int)vector.y);
        }

        public static implicit operator DVector2(UnityEngine.Vector2Int vector)
        {
            return new DVector2(vector.x, vector.y);
        }


        public static DVector2 operator /(DVector2 a, float n)
        {
            return new DVector2(a.x / n, a.y / n);
        }

        public static DVector2 operator *(DVector2 a, float n)
        {
            return new DVector2(a.x * n, a.y * n);
        }

       
        public static float Dot(DVector2 a, DVector2 b)
        {
            return a.x * b.x + a.y * b.y;
        }

        public static float Distance(DVector2 a, DVector2 b)
        {
            var x = a.x - b.x;
            var y = a.y - b.y;

            return (float)Math.Sqrt(x * x + y * y);
        }

        public override string ToString()
        {
            return $"({x}, {y})";
        }
    }
}
