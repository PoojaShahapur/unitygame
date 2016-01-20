using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace EditorTool
{
    public class EditorConfig
    {
        public static string PREFAB_PATH = "";
        public static string ASSETBUNDLE = "";
        public static string OUTPUT_PATH = "";
    }

    public class TestSetAssetBundleName : Editor
    {
        [MenuItem("Tool/SetFileBundleName")]
        static public void SetBundleName()
        {
            // 设置资源的AssetBundle的名称和文件扩展名
            UnityEngine.Object[] selects = Selection.objects;
            foreach (UnityEngine.Object selected in selects)
            {
                string path = AssetDatabase.GetAssetPath(selected);
                AssetImporter asset = AssetImporter.GetAtPath(path);
                // 这个设置后都是小写的字符串，即使自己设置的是大写的字符串，也会被转换成小写的
                asset.assetBundleName = "aaa/" + selected.name + ".unity3d"; //设置Bundle文件的名称
                //asset.assetBundleVariant = "unity3d";//设置Bundle文件的扩展名
                asset.SaveAndReimport();

            }
            AssetDatabase.Refresh();
        }

        // 设置assetbundle的名字(修改meta文件)
        [MenuItem("Tools/SetAssetBundleName")]
        static void OnSetAssetBundleName()
        {
            UnityEngine.Object obj = Selection.activeObject;
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            string[] extList = new string[] { ".prefab.meta", ".png.meta", ".jpg.meta", ".tga.meta" };
            //EditorUtil.Walk(path, extList, DoSetAssetBundleName);

            //刷新编辑器
            AssetDatabase.Refresh();
            Debug.Log("AssetBundleName修改完毕");
        }
        
        static void DoSetAssetBundleName(string path)
        {
            path = path.Replace("\\", "/");
            int index = path.IndexOf(EditorConfig.PREFAB_PATH);
            string relativePath = path.Substring(path.IndexOf(EditorConfig.PREFAB_PATH) + EditorConfig.PREFAB_PATH.Length);
            string prefabName = relativePath.Substring(0, relativePath.IndexOf('.')) + EditorConfig.ASSETBUNDLE;
            StreamReader fs = new StreamReader(path);
            List<string> ret = new List<string>();
        
            string line;
            while((line = fs.ReadLine()) != null) {
                line = line.Replace("\n", "");
                if (line.IndexOf("assetBundleName:") != -1) {
                    line = "  assetBundleName: " + prefabName.ToLower();
                }
        
                ret.Add(line);
            }
        
            fs.Close();
            File.Delete(path);
        
            StreamWriter writer = new StreamWriter(path + ".tmp");
            foreach (var each in ret) {
                writer.WriteLine(each);
            }
            writer.Close();

            File.Copy(path + ".tmp", path);
            File.Delete(path + ".tmp");
        }
        
        [MenuItem("Tools/CreateAssetBundle")]
        static void OnCreateAssetBundle()
        {
            BuildPipeline.BuildAssetBundles(EditorConfig.OUTPUT_PATH);
            //刷新编辑器
            AssetDatabase.Refresh();
            Debug.Log("AssetBundle打包完毕");
        }
    }
}