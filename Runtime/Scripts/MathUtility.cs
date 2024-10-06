using System;
using System.Collections.Generic;
using UnityEngine;

namespace AlaslTools
{

    public static class MathUtility
    {
        public static Vector3 Floor(Vector3 vec)
        {
            return new Vector3(
                Mathf.Floor(vec.x),
                Mathf.Floor(vec.y),
                Mathf.Floor(vec.z));
        }

        public static Vector3 Round(Vector3 vec)
        {
            return new Vector3(
                Mathf.Round(vec.x),
                Mathf.Round(vec.y),
                Mathf.Round(vec.z));
        }

        public static Vector3 Abs(Vector3 vec)
        {
            return new Vector3(
                Mathf.Abs(vec.x),
                Mathf.Abs(vec.y),
                Mathf.Abs(vec.z));
        }

        public static float CycleAngle(float angle) => (angle % 360f + 360f) % 360f;

        public static float FlipAngle(float angle) => 360f - angle;

        public static Vector2 RotateBy(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x * b.x - a.y * b.y, a.x * b.y + a.y * b.x);
        }

        public static float ForwardToAngle(Vector2 dir)
        {
            float a = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            a = a < 0 ? a + 360f : a;
            return FlipAngle(CycleAngle(a - 90f));
        }

        public static Vector2 AngleToForward(float angle)
        {
            angle = (FlipAngle(CycleAngle(angle)) + 90f) * Mathf.Deg2Rad;
            return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        }
    }

}