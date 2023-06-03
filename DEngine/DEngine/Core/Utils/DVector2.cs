using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UnityEngine;

namespace DungeonInspector
{
    public struct DVec2
    {
        public float x;
        public float y;
        [JsonIgnore] public float Magnitude => (float)Math.Sqrt(x * x + y * y);
        [JsonIgnore] public float SqrMagnitude => x * x + y * y;
        [JsonIgnore] public DVec2 Normalize => this / Magnitude;
        [JsonIgnore] public static DVec2 One => new DVec2(1, 1);

        public static DVec2 Right => new DVec2(1, 0);

        public DVec2 Floor()
        {
            return new DVec2((float)Math.Floor(x), (float)Math.Floor(y));
        }

        public DVec2 Ceil()
        {
            return new DVec2((float)Math.Ceiling(x), (float)Math.Ceiling(y));
        }

        public DVec2 Round()
        {
            return new DVec2((float)Math.Round(x), (float)Math.Round(y));
        }

        public Vector2Int RoundToInt()
        {
            return new Vector2Int((int)(float)Math.Round(x), (int)(float)Math.Round(y));
        }

        public DVec2 Abs()
        {
            return new DVec2((float)Math.Abs(x), (float)Math.Abs(y));
        }


        public DVec2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public static implicit operator UnityEngine.Vector2(DVec2 vector)
        {
            return new UnityEngine.Vector2(vector.x, vector.y);
        }

        public static implicit operator DVec2(float xyValue)
        {
            return new DVec2(xyValue, xyValue);
        }

        public static implicit operator DVec2(int xyValue)
        {
            return new DVec2(xyValue, xyValue);
        }

        public static implicit operator DVec2(UnityEngine.Vector2 vector)
        {
            return new DVec2(vector.x, vector.y);
        }

        public static bool operator >(DVec2 a, DVec2 b)
        {
            return a.x > b.x && a.y > b.y;
        }

        public static bool operator !=(DVec2 a, DVec2 b)
        {
            var aInt = a.RoundToInt();
            var bInt = b.RoundToInt();

            return aInt.x != bInt.x || aInt.y != bInt.y;
        }

        public static bool operator ==(DVec2 a, DVec2 b)
        {
            var aInt = a.RoundToInt();
            var bInt = b.RoundToInt();

            return aInt.x == bInt.x && aInt.y == bInt.y;
        }

        public static bool operator <(DVec2 a, DVec2 b)
        {
            return a.x < b.x && a.y < b.y;
        }

        public static implicit operator UnityEngine.Vector2Int(DVec2 vector)
        {
            return new UnityEngine.Vector2Int((int)vector.x, (int)vector.y);
        }

        public static implicit operator DVec2(UnityEngine.Vector2Int vector)
        {
            return new DVec2(vector.x, vector.y);
        }

        // Json deserializer needs this
        public static implicit operator DVec2(string vector)
        {
            if(string.IsNullOrEmpty(vector))
            {
                Debug.Log("Null pos!");
            }
            var start = vector.IndexOf('(');
            var comma = vector.IndexOf(',');
            var end = vector.IndexOf(')');

            var xStr = vector.Substring(start + 1, comma - 1);
            var yStr = vector.Substring(comma + 1, end - comma - 1);

            return new DVec2(float.Parse(xStr), float.Parse(yStr));
        }

        public static DVec2 operator /(DVec2 a, float n)
        {
            return new DVec2(a.x / n, a.y / n);
        }

        public static DVec2 operator *(DVec2 a, float n)
        {
            return new DVec2(a.x * n, a.y * n);
        }

        public static DVec2 operator -(DVec2 a, DVec2 b)
        {
            return new DVec2(a.x - b.x, a.y - b.y);
        }


        public static DVec2 operator -(DVec2 a)
        {
            return new DVec2(-a.x, -a.y);
        }
        public static DVec2 operator +(DVec2 a, DVec2 b)
        {
            return new DVec2(a.x + b.x, a.y + b.y);
        }

        public static float Dot(DVec2 a, DVec2 b)
        {
            return a.x * b.x + a.y * b.y;
        }

        public static float Distance(DVec2 a, DVec2 b)
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
