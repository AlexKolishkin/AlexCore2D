using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.Utils
{
    public static class Utils
    {
        public static readonly DateTime startDate = new DateTime(1970, 1, 1);

        public static double Clamp01(double value)
        {
            if (value < 0)
                value = 0;
            else if (value > 1) value = 1;

            return value;
        }

        public static Vector2 Orthogonal(this Vector2 input)
        {
            if (!Mathf.Approximately(input.y, 0)) return new Vector2(1, -input.x / input.y).normalized;

            return new Vector2(0, 1);
        }

        public static Vector2 ControlPoint(Vector2 first, Vector2 second, float linearFactor, float orthoFactor, int side)
        {
            var direction = second - first;
            var length = direction.magnitude;
            var directionNormalized = direction.normalized;
            var orthogonal = direction.Orthogonal();
            if (side < 0) orthogonal = -orthogonal;

            return first + directionNormalized * length * linearFactor + orthogonal * length * orthoFactor;
        }
        
        public static void Reset(this Transform transform)
        {
            if (transform != null)
            {
                transform.localScale = Vector3.one;
                transform.localPosition = Vector3.zero;
            }
        }

        public static bool IsWholeNumber(float value)
        {
            return Mathf.Approximately(value - Mathf.FloorToInt(value), 0f);
        }

        public static void CopyDictionary<K, V>(Dictionary<K, V> source, Dictionary<K, V> destination)
        {
            destination.Clear();
            foreach (var kvp in source) destination.Add(kvp.Key, kvp.Value);
        }

        public static List<T> MakeList<T>(params T[] objs)
        {
            return objs.ToList();
        }

        public static double GetUnixTimeFor(DateTime dt)
        {
            return (dt - startDate).TotalSeconds;
        }

        public static string FormatSplash(this DateTime date)
        {
            return date.ToString("MM/dd/yyyy");
        }

        public static int MonthDifference(this DateTime lValue, DateTime rValue)
        {
            return Math.Abs(lValue.Month - rValue.Month + 12 * (lValue.Year - rValue.Year));
        }
    }
}