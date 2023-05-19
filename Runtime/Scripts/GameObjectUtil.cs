using System.Collections.Generic;
using UnityEngine;

namespace AlaslTools
{
    public static class GameObjectUtil
    {
        private static List<Component> internal_components = new List<Component>();
        public static bool HaveComponents(this GameObject gameObject)
        {
            gameObject.GetComponents(internal_components);
            return internal_components.Count > 1;
        }

        public static void RemoveComponent<T>(this GameObject gameObject) where T : Component
        {
            var com = gameObject.GetComponent<T>();
            if (com != null)
                SafeDestroy(com);
        }

        public static void RemoveComponents<T>(this GameObject gameObject) where T : Component
        {
            var com = gameObject.GetComponents<T>();
            for (int i = 0; i < com.Length; i++)
                SafeDestroy(com[i]);
        }

        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            var comp = gameObject.GetComponent<T>();
            return comp != null ? comp : gameObject.AddComponent<T>();
        }

        public static void Reset(this Transform transform)
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale    = Vector3.one;
        }

        public static Mesh GetPrimitiveMesh(PrimitiveType meshType)
        {
            var go = GameObject.CreatePrimitive(meshType);
            var mesh = go.GetComponent<MeshFilter>().sharedMesh;
            SafeDestroy(go);
            return mesh;
        }

        public static void SafeDestroy(Object obj)
        {
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlaying)
                Object.DestroyImmediate(obj, false);
            else
#endif
                Object.Destroy(obj);

        }
    }
}
