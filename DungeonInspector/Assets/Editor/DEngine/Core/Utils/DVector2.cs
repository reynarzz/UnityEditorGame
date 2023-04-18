using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonInspector
{
    public struct DVector2
    {
        public float x { get; set; }
        public float y { get; set; }
        [JsonIgnore] public float Magnitude => (float)Math.Sqrt(x * x + y * y);
        [JsonIgnore] public float SqrMagnitude => x * x + y * y;
        [JsonIgnore] public DVector2 Normalize => this / Magnitude;

        public DVector2 Floor()
        {
            return new DVector2(MathF.Floor(x), MathF.Floor(y));
        }

        public DVector2 Ceil()
        {
            return new DVector2(MathF.Ceiling(x), MathF.Ceiling(y));
        }

        public DVector2 Round()
        {
            return new DVector2(MathF.Round(x), MathF.Round(y));
        }

        public Vector2Int RoundToInt()
        {
            return new Vector2Int((int)MathF.Round(x), (int)MathF.Round(y));
        }

        public DVector2 Abs()
        {
            return new DVector2(MathF.Abs(x), MathF.Abs(y));
        }


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

        // Json deserializer needs this
        public static implicit operator DVector2(string vector)
        {
            var start = vector.IndexOf('(');
            var comma = vector.IndexOf(',');
            var end = vector.IndexOf(')');

            var xStr = vector.Substring(start+1, comma-1);
            var yStr = vector.Substring(comma+1, end - comma -1);

            return new DVector2(float.Parse(xStr), float.Parse(yStr));
        }

        public static DVector2 operator /(DVector2 a, float n)
        {
            return new DVector2(a.x / n, a.y / n);
        }

        public static DVector2 operator *(DVector2 a, float n)
        {
            return new DVector2(a.x * n, a.y * n);
        }

        public static DVector2 operator -(DVector2 a, DVector2 b)
        {
            return new DVector2(a.x - b.x, a.y - b.y);
        }


        public static DVector2 operator -(DVector2 a)
        {
            return new DVector2(-a.x, -a.y);
        }
        public static DVector2 operator +(DVector2 a, DVector2 b)
        {
            return new DVector2(a.x + b.x, a.y + b.y);
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
