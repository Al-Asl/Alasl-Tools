using System.Collections.Generic;
using UnityEngine;

namespace AlaslTools
{
    public static class MeshCombiner
    {
        public struct RendererInfo
        {
            public Matrix4x4 matrix;
            public Mesh mesh;
            public List<Material> materials;

            public static RendererInfo Create()
            {
                return new RendererInfo(new Mesh(), new List<Material>(), Matrix4x4.identity);
            }

            public RendererInfo(Mesh mesh, List<Material> materials, Matrix4x4 matrix)
            {
                this.mesh = mesh;
                this.materials = materials;
                this.matrix = matrix;
            }

            public void ToGameObject(GameObject gameObject)
            {
                gameObject.GetOrAddComponent<MeshFilter>().sharedMesh = mesh;
                gameObject.GetOrAddComponent<MeshRenderer>().sharedMaterials = materials.ToArray();
            }
        }

        public static List<RendererInfo> GetAndRemoveRenderers(Transform target)
        {
            var result = new List<RendererInfo>();
            GetRenderers(target, result, Matrix4x4.TRS(target.localPosition, target.localRotation, target.localScale).inverse);
            RemoveRenderers(target);
            return result;
        }

        public static List<RendererInfo> GetRenderers(Transform target)
        {
            var result = new List<RendererInfo>();
            GetRenderers(target, result, Matrix4x4.TRS(target.localPosition, target.localRotation, target.localScale).inverse);
            return result;
        }

        private static void GetRenderers(Transform target, List<RendererInfo> infos, Matrix4x4 matrix)
        {
            matrix *= Matrix4x4.TRS(target.localPosition, target.localRotation, target.localScale);
            GetRenderInfo(target, infos, matrix);
            for (int i = 0; i < target.childCount; i++)
                GetRenderers(target.GetChild(i), infos, matrix);
        }

        private static void GetRenderInfo(Transform target, List<RendererInfo> infos, Matrix4x4 matrix)
        {
            if (target.GetComponent<MeshCombineIgnore>() != null)
                return;

            var meshf = target.GetComponent<MeshFilter>();
            if (meshf != null)
            {
                var renderer = target.GetComponent<MeshRenderer>();
                if (renderer != null)
                    infos.Add(new RendererInfo(meshf.sharedMesh, new List<Material>(renderer.sharedMaterials), matrix));
            }
        }

        public static void RemoveRenderers(Transform target)
        {
            RemoveRenderer(target);
            for (int i = 0; i < target.childCount; i++)
                RemoveRenderers(target.GetChild(i));
        }

        static void RemoveRenderer(Transform target)
        {
            if (target.GetComponent<MeshCombineIgnore>() != null)
                return;

            var meshf = target.GetComponent<MeshFilter>();
            if (meshf != null)
                Object.DestroyImmediate(meshf, false);

            var renderer = target.GetComponent<MeshRenderer>();
            if (renderer != null)
                Object.DestroyImmediate(renderer, false);
        }

        public static RendererInfo Combine(List<RendererInfo> infos)
        {
            var result = RendererInfo.Create();
            Combine(infos, result);
            return result;
        }

        public static void Combine(List<RendererInfo> infos, RendererInfo result)
        {
            HashSet<Material> materialsSet = new HashSet<Material>();
            foreach (var e in infos)
                foreach (var m in e.materials)
                    materialsSet.Add(m);

            result.materials.Clear();
            result.materials.AddRange(materialsSet);

            var materials = new BiDirectionalList<Material>(result.materials);

            CombineInstance[][] stage01 = new CombineInstance[materials.Count][];
            int[] meshCount = new int[materials.Count];

            foreach (var e in infos)
                foreach (var mat in e.materials)
                    meshCount[materials.GetIndex(mat)]++;

            for (int i = 0; i < materials.Count; i++)
                stage01[i] = new CombineInstance[meshCount[i]];

            meshCount.Fill(() => 0);

            foreach (var e in infos)
            {
                for (int i = 0; i < e.materials.Count; i++)
                {
                    var mat = e.materials[i];
                    var index = materials.GetIndex(mat);
                    stage01[index][meshCount[index]++] = new CombineInstance()
                    {
                        mesh = e.mesh,
                        subMeshIndex = i,
                        transform = e.matrix
                    };
                }
            }

            CombineInstance[] stage02 = new CombineInstance[stage01.Length];

            for (int i = 0; i < stage01.Length; i++)
            {
                var m = new Mesh();
                m.CombineMeshes(stage01[i]);
                stage02[i] = new CombineInstance()
                {
                    mesh = m,
                    subMeshIndex = 0,
                    transform = Matrix4x4.identity
                };
            }

            result.mesh.CombineMeshes(stage02, false);

            for (int i = 0; i < stage02.Length; i++)
                GameObjectUtil.SafeDestroy(stage02[i].mesh);
        }
    }
}
