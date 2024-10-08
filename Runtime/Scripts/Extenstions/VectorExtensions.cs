﻿using UnityEngine;

namespace AlaslTools
{
    public static class VectorExtensions
    {
        public static Vector4 ToVector4(this Vector3 vec, float w)
            => new Vector4(vec.x, vec.y, vec.z, w);
        public static Vector3 SetX(this Vector3 v, float x)
        {
            v.x = x;
            return v;
        }
        public static Vector3 SetY(this Vector3 v, float y)
        {
            v.y = y;
            return v;
        }
        public static Vector3 SetZ(this Vector3 v, float z)
        {
            v.z = z;
            return v;
        }
        public static Vector3Int SetX(this Vector3Int v, int x)
        {
            v.x = x;
            return v;
        }
        public static Vector3Int SetY(this Vector3Int v, int y)
        {
            v.y = y;
            return v;
        }
        public static Vector3Int SetZ(this Vector3Int v, int z)
        {
            v.z = z;
            return v;
        }
        public static Vector3 xyn(this Vector2 v) => new Vector3(v.x, v.y, 0);
        public static Vector3Int xyn(this Vector2Int v) => new Vector3Int(v.x, v.y, 0);
        public static Vector3 nxy(this Vector2 v) => new Vector3(0, v.x, v.y);
        public static Vector3Int nxy(this Vector2Int v) => new Vector3Int(0, v.x, v.y);
        public static Vector3 xny(this Vector2 v) => new Vector3(v.x, 0, v.y);
        public static Vector3Int xny(this Vector2Int v) => new Vector3Int(v.x, 0, v.y);
        public static Vector2 SetX(this Vector2 v, float x)
        {
            v.x = x;
            return v;
        }
        public static Vector2 SetY(this Vector2 v, float y)
        {
            v.y = y;
            return v;
        }
        public static Vector2Int SetX(this Vector2Int v, int x)
        {
            v.x = x;
            return v;
        }
        public static Vector2Int SetY(this Vector2Int v, int y)
        {
            v.y = y;
            return v;
        }
        public static Vector2 xy(this Vector3 v) => new Vector2(v.x, v.y);
        public static Vector2Int xy(this Vector3Int v) => new Vector2Int(v.x, v.y);
        public static Vector2 yz(this Vector3 v) => new Vector2(v.y, v.z);
        public static Vector2Int yz(this Vector3Int v) => new Vector2Int(v.y, v.z);
        public static Vector2 xz(this Vector3 v) => new Vector2(v.x, v.z);
        public static Vector2Int xz(this Vector3Int v) => new Vector2Int(v.x, v.z);
        public static Vector2Int Swap(this Vector2Int v)
        {
            var temp = v.x;
            v.x = v.y;
            v.y = temp;
            return v;
        }
    }
}