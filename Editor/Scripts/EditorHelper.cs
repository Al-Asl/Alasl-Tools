using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

namespace AlaslTools
{

    public static class EditorHelper
    {
        /// <summary>
        /// get a path relative to the asset folder
        /// </summary>
        public static string GetLocalpath(string fullPath)
        {
            return Path.GetRelativePath(Application.dataPath, fullPath);
        }

        public static List<string> GetAssetsPath(string path, System.Type targetType)
        {
            var assets = LoadAsstesAtPath(path);
            var results = new List<string>();
            foreach (var asset in assets)
            {
                var so = asset as ScriptableObject;
                if (so != null && so.GetType() == targetType)
                    results.Add(AssetDatabase.GetAssetPath(asset));
            }
            return results;
        }

        //AssetDatabase.LoadAllAssetsAtPath this doesn't work for some reason!?! 
        public static List<Object> LoadAsstesAtPath(string path)
        {
            if (!Directory.Exists(path))
                throw new System.Exception("directory doesn't exists!");

            var result = new List<Object>();

            foreach (var file in Directory.EnumerateFiles(path))
            {
                if (!file.EndsWith(".meta"))
                    result.Add(AssetDatabase.LoadAssetAtPath<Object>(file));
            }

            return result;
        }

        /// <summary>
        /// if the name was 'Cube (1)' it will return 'Cube'
        /// </summary>
        public static string ReadName(string name)
        {
            var match = System.Text.RegularExpressions.Regex.Match(name, "\\w*(?=\\s\\(\\d+\\))\\b");
            if (match.Success)
                return match.Value;
            else
                return name;
        }

        /// <summary>
        /// if the name was 'Cube (1)' it will return 1
        /// </summary>
        public static uint ReadNumber(string name)
        {
            var match = System.Text.RegularExpressions.Regex.Match(name, "\\s\\((\\d+)\\)$");
            if (match.Success)
            {
                if (uint.TryParse(match.Groups[1].Value, out var num))
                    return num;
                else
                    return 0;
            }
            else
                return 0;
        }

        public static T CreateOrLoadSO<T>(string assetPath) where T : ScriptableObject
        {
            T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance<T>();
                AssetDatabase.CreateAsset(asset, assetPath);
            }
            return asset;
        }

        public static Object CreateOrLoadSO(System.Type type, string assetPath)
        {
            Object asset = AssetDatabase.LoadAssetAtPath<Object>(assetPath);
            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance(type);
                AssetDatabase.CreateAsset(asset, assetPath);
                AssetDatabase.Refresh();
            }
            return asset;
        }

        public static string GetAssetName(Object asset)
        {
            var path = AssetDatabase.GetAssetPath(asset);
            return Path.GetFileNameWithoutExtension(path);
        }

        public static void SetAssetName(Object asset, string name)
        {
            AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(asset), name);
        }

        public static List<string> GetActiveScenes()
        {
            var scenes = EditorBuildSettings.scenes;
            var result = new List<string>();

            for (int i = 0; i < scenes.Length; i++)
                if (scenes[i].enabled) result.Add(scenes[i].path);

            return result;
        }

        /// <summary>
        /// get the path for any given type assembly, the asmdef file name need to be 
        /// the same as the assembly name
        /// </summary>
        public static string GetAssemblyDirectory<T>()
        {
            var asseblyName = typeof(T).Assembly.GetName().Name;
            asseblyName += ".asmdef";
            var assemblies = AssetDatabase.FindAssets("t:AssemblyDefinitionAsset");
            for (int i = 0; i < assemblies.Length; i++)
            {
                var path = AssetDatabase.GUIDToAssetPath(assemblies[i]);
                if (path.EndsWith(asseblyName))
                    return path.Remove(path.Length - asseblyName.Length);
            }
            throw new System.Exception("path not found!");
        }

        /// <summary>
        /// get the path for any given class, the class file name need to be 
        /// the same as the class name
        /// </summary>
        public static string GetScriptDirectory<T>() where T : Object
        {
            var scriptName = typeof(T).Name;
            scriptName += ".cs";
            var scripts = AssetDatabase.FindAssets("t:script");
            for (int i = 0; i < scripts.Length; i++)
            {
                var path = AssetDatabase.GUIDToAssetPath(scripts[i]);
                if (path.EndsWith(scriptName))
                    return path.Remove(path.Length - scriptName.Length);
            }
            throw new System.Exception("path not found!");
            
        }
    }

}