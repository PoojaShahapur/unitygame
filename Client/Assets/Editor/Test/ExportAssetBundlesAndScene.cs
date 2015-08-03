using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System.Xml;
using System.Collections.Generic;

/**
 * @brief 资源打包，导出 Bundles 和 Scene
 */
public class ExportAssetBundlesAndScene
{
	// 在Unity编辑器中添加菜单，导出的时候，一定要先选择自己设置的预设，然后点击菜单，支持选中文件夹，导出选中的资源到一个文件
    //[MenuItem("Assets/Build AssetBundle From Selection All - Track dependencies")]
    static void ExportAssetBundlesAll()
	{
		// 打开保存面板，获得用户选择的路径  
        string path = EditorUtility.SaveFilePanel("Save Resource", "", "New Resource", "unity3d");
        //EditorUtility.GetAssetPath
        //EditorUtility.InstanceIDToObject
        //EditorUtility.CreateGameObjectWithHideFlags
		
		if (path.Length != 0)
		{  
			// 选择的要保存的对象  
			Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets); 
			//打包  
            //BuildPipeline.BuildAssetBundle(Selection.activeObject, selection, path, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets, BuildTarget.StandaloneWindows);

#if UNITY_5
            List<string> assetNameList = new List<string>();
            string nameStr;
            foreach (Object item in selection)
            {
                nameStr = AssetDatabase.GetAssetPath(item);
                nameStr = nameStr.Substring(nameStr.IndexOf("Assets"), nameStr.Length - nameStr.IndexOf("Assets"));
                assetNameList.Add(nameStr);
            }

            AssetBundleBuild[] buildList = new AssetBundleBuild[1];
            buildList[0].assetBundleName = "SavePrefab";
            buildList[0].assetBundleVariant = "unity3d";
            buildList[0].assetNames = assetNameList.ToArray();
            BuildPipeline.BuildAssetBundles(path, buildList, 0, BuildTarget.StandaloneWindows);
#elif UNITY_4_6 || UNITY_4_5
            BuildPipeline.BuildAssetBundle(null, selection, path, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.UncompressedAssetBundle, BuildTarget.StandaloneWindows);
#endif
            Selection.objects = selection;
            //FileStream fs = File.Open(path + ".log", FileMode.OpenOrCreate);
            //StreamWriter sw = new StreamWriter(fs);
            //sw.WriteLine("文件 " + path + " 中的内容如下：");
            foreach (Object obj in Selection.objects)
            {
                //sw.WriteLine("Name: " + obj.name + " Type:" + obj.GetType());
                if (obj.GetType() == typeof(Object))
                {
                    Debug.LogWarning("Name: " + obj.name + ", Type: " + obj.GetType() + ". 可能是unity3d不能识别的文件，可能未被打包成功");
                }
            }
            //sw.Flush();
            //fs.Flush();
            //sw.Close();
            //fs.Close();
		}

        System.GC.Collect();
	}

    //[MenuItem("Assets/Build AssetBundle From Selection All - No dependency tracking")]
    static void ExportResourceNoTrack()
    {
        // Bring up save panel
        string path = EditorUtility.SaveFilePanel("Save Resource", "", "New Resource", "unity3d");
        if (path.Length != 0)
        {
            // Build the resource file from the active selection.
#if UNITY_5
            List<string> assetNameList = new List<string>();
            string nameStr;
            foreach (Object item in Selection.objects)
            {
                nameStr = AssetDatabase.GetAssetPath(item);
                nameStr = nameStr.Substring(nameStr.IndexOf("Assets"), nameStr.Length - nameStr.IndexOf("Assets"));
                assetNameList.Add(nameStr);
            }

            AssetBundleBuild[] buildList = new AssetBundleBuild[1];
            buildList[0].assetBundleName = "save";
            buildList[0].assetBundleVariant = "unity3d";
            buildList[0].assetNames = assetNameList.ToArray();
            BuildPipeline.BuildAssetBundles(path, buildList, 0, BuildTarget.StandaloneWindows);
#elif UNITY_4_6 || UNITY_4_5
            BuildPipeline.BuildAssetBundle(Selection.activeObject, Selection.objects, path, 0, BuildTarget.StandaloneWindows);
#endif
            //BuildPipeline.BuildAssetBundle(null, Selection.objects, path, 0, BuildTarget.StandaloneWindows);

            //FileStream fs = File.Open(path + ".log", FileMode.OpenOrCreate);
            //StreamWriter sw = new StreamWriter(fs);
            //sw.WriteLine("文件 " + path + " 中的内容如下：");
            foreach (Object obj in Selection.objects)
            {
                //sw.WriteLine("Name: " + obj.name + " Type: " + obj.GetType());
                if (obj.GetType() == typeof(Object))
                {
                    Debug.LogWarning("Name: " + obj.name + ", Type: " + obj.GetType() + ". 可能是unity3d不能识别的文件，可能未被打包成功");
                }
            }
            //sw.Flush();
            //fs.Flush();
            //sw.Close();
            //fs.Close();
        }

        System.GC.Collect();
    }

    /*
    // 首先保存场景，然后再导出加载的场景资源文件
	[MenuItem("Assets/Save Scene")]
	static void ExportScene()
	{
		// 打开保存面板，获得用户选择的路径  
		string path = EditorUtility.SaveFilePanel("Save Resource", "", "New Resource", "unity3d");
		if (path.Length != 0)
		{
			// 选择的要保存的对象
            ArrayList levelssrc = new ArrayList();
            ArrayList levelsdst = new ArrayList();
            string[] levels = new string[1];
            string destpath;
            //string outpath = "Assets/game/scene";
            // 这个是要保存在本地的 unity 文件，如果没有保存场景，一定要先保存，然后才能导出 assetbundle
            levelssrc.Add("Assets/Scenes/App.unity");
            levelsdst.Add("Assets/Game/Scene/App.unity3d");

            levelssrc.Add("Assets/Scenes/Game.unity");
            levelsdst.Add("Assets/game/scene/Game.unity3d");

            levelssrc.Add("Assets/Scenes/cave.unity");
            levelsdst.Add("Assets/Game/Scene/cave.unity3d");

            int idx = 0;
            while (idx < levelssrc.Count)
            {
                levels[0] = levelssrc[idx] as string;
                destpath = levelsdst[idx] as string;
                //打包
                BuildPipeline.BuildStreamedSceneAssetBundle(levels, destpath, BuildTarget.StandaloneWindows);
                ++idx;
            }
		}
	}
    */

    // 首先保存场景，然后再导出加载的场景资源文件，导出选择的场景文件
    //[MenuItem("Assets/Save Scene One")]
    static void ExportSceneOne()
    {
        // 打开保存面板，获得用户选择的路径  
        string path = EditorUtility.SaveFilePanel("Save Resource", "", "New Resource", "unity3d");

        if (path.Length != 0)
        {
            // 选择的要保存的对象  
            Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
            if (selection.Length > 0)
            {
                //得到物体的路径
                string sourcePath = Application.dataPath + "/" + AssetDatabase.GetAssetPath(selection[0]);
                if (sourcePath.IndexOf(".unity") != -1)      // 如果是场景文件
                {
                    //string destext = ".unity3d";
                    //string destFileNoExt = "";

                    // 选择的要保存的对象
                    string[] levels = new string[1];
                    //string destfilename = Application.dataPath + "/StreamingAssets/Scene/";

                    //int dotidx = 0;
                    //int splashidx = 0;
                    //string outpath = "Assets/game/scene";
                    // 这个是要保存在本地的 unity 文件，如果没有保存场景，一定要先保存，然后才能导出 assetbundle
                    levels[0] = sourcePath;

                    //dotidx = sourcePath.IndexOf('.');
                    //splashidx = sourcePath.LastIndexOf('/');
                    //destFileNoExt = sourcePath.Substring(splashidx + 1, dotidx - splashidx - 1);
                    //destfilename = destfilename + destFileNoExt + destext;
                    //打包
#if UNITY_5
                    AssetBundleBuild[] buildList = new AssetBundleBuild[1];
                    buildList[0].assetBundleName = "Save";
                    buildList[0].assetBundleVariant = "unity3d";
                    buildList[0].assetNames = levels;
                    BuildPipeline.BuildAssetBundles(path, buildList, 0, BuildTarget.StandaloneWindows);
#elif UNITY_4_6 || UNITY_4_5
                    BuildPipeline.BuildStreamedSceneAssetBundle(levels, path, BuildTarget.StandaloneWindows);
#endif
                }
            }
        }
    }

    // 首先保存场景，然后再导出加载的场景资源文件，保存所有的场景到各自的资源文件
	//[MenuItem("Assets/Save Scene All")]
	static void ExportSceneAll()
	{
		// 选择的要保存的对象
        string[] levels = new string[1];
        string srcpath;
        string destpath;
        string destfilename;
        string destext = ".unity3d";
        string destFileNoExt = "";
        Dictionary<string, bool> exclude = new Dictionary<string, bool>();
        exclude["App"] = true;
        exclude["Game"] = true;
        exclude["ScrollView"] = true;
        exclude["Start"] = true;
        exclude["TestNguiLocalScale"] = true;
        exclude["TestScene"] = true;

        srcpath = Application.dataPath + "/Scenes/";                            // 查找目录
        destpath = Application.dataPath + "/StreamingAssets/Scene/";            // 存放目录
        string[] fileEntries = Directory.GetFiles(srcpath, "*.unity", SearchOption.TopDirectoryOnly);     // 仅仅查找当前目录

        int idx = 0;
        int dotidx = 0;
        int splashidx = 0;
        bool ret = false;
        while (idx < fileEntries.Length)
        {
            dotidx = fileEntries[idx].IndexOf('.');
            splashidx = fileEntries[idx].LastIndexOf('/');
            destFileNoExt = fileEntries[idx].Substring(splashidx + 1, dotidx - splashidx - 1);

            if (!exclude.TryGetValue(destFileNoExt, out ret))
            {
                destfilename = destpath + destFileNoExt + destext;
                levels[0] = fileEntries[idx];
                //打包
#if UNITY_5
                AssetBundleBuild[] buildList = new AssetBundleBuild[1];
                buildList[0].assetBundleName = destFileNoExt;
                buildList[0].assetBundleVariant = "unity3d";
                buildList[0].assetNames = new string[1];
                buildList[0].assetNames[0] = levels[0];
                BuildPipeline.BuildAssetBundles(destpath, buildList, 0, BuildTarget.StandaloneWindows);
#elif UNITY_4_6 || UNITY_4_5
                BuildPipeline.BuildStreamedSceneAssetBundle(levels, destfilename, BuildTarget.StandaloneWindows);
#endif
            }
            ++idx;
        }
	}

    // 场景文件导出，导出自己的地图文件格式，然后自己裁剪逐渐加载可见的模型资源
    //[MenuItem("Assets/Save Scene XML")]
    static void ExportSceneXmlOneAndTerrainPrefab()
    {
        // 打开保存面板，获得用户选择的路径  
        string path = EditorUtility.SaveFilePanel("Save Resource", "", "New Resource", "xml");
        {
            // 导出地形规则，地形还是打包成 Level 的 unity3d，地图中摆放的地物，需要制作成 Prefab ，然后导出 Xml ，Xml 中记录这些预设的资源包的 unity3d 的名字，然后是资源的 id ，然后是位置，旋转，缩放。凡是静态的地物，不加物理，物理通过导航格子模拟。游戏中动态生成的内容才可以加物理
            // 所有需要导出的静态模型的根节点的 name 是 StaticRoot 
            GameObject[] gos = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
            // 开始导出所有静态节点
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode sceneNode = xmlDoc.CreateElement("config");
            xmlDoc.AppendChild(sceneNode);
            XmlElement terrainNode = xmlDoc.CreateElement("Terrain");
            sceneNode.AppendChild(terrainNode);
            XmlElement sceneEntity;
            string elemvalue;
            foreach (GameObject go in gos)
            {
                if (go.name != "Terrain" && go.name != "Main Camera")       //  将地形排除在外
                {
                    if (go.transform.parent == null)     // 只有顶层 GameObject 才需要导出
                    {
                        sceneEntity = xmlDoc.CreateElement("go");
                        sceneNode.AppendChild(sceneEntity);
                        sceneEntity.SetAttribute("prefab", go.name);
                        elemvalue = go.transform.localPosition.ToString();
                        sceneEntity.SetAttribute("pos", elemvalue);
                        elemvalue = go.transform.localRotation.ToString();
                        sceneEntity.SetAttribute("rotate", elemvalue);
                        elemvalue = go.transform.localScale.ToString();
                        sceneEntity.SetAttribute("scale", elemvalue);
                    }
                }
                else if (go.name == "Terrain")
                {
                    Terrain terrain = go.GetComponent<Terrain>();
                    terrainNode.SetAttribute("size", terrain.terrainData.size.ToString());
                }
            }

            xmlDoc.Save(path);
        }
    }

    // 清除缓存
    [MenuItem("Assets/CleanCache")]
    static void CleanCache()
    {
        Caching.CleanCache();
    }
}