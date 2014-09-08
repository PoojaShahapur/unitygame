﻿using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

/**
 * @brief 资源打包，导出 Bundles 和 Scene
 */
public class ExportAssetBundlesAndScen
{
	// 在Unity编辑器中添加菜单，导出的时候，一定要先选择自己设置的预设，然后点击菜单，支持选中文件夹，导出选中的资源到一个文件
    [MenuItem("Assets/Build AssetBundle From Selection All - Track dependencies")]
    static void ExportAssetBundlesAll()
	{
		// 打开保存面板，获得用户选择的路径  
        string path = EditorUtility.SaveFilePanel("Save Resource", "", "New Resource", "unity3d");
		
		if (path.Length != 0)
		{  
			// 选择的要保存的对象  
			Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets); 
			//打包  
            //BuildPipeline.BuildAssetBundle(Selection.activeObject, selection, path, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets, BuildTarget.StandaloneWindows);

            BuildPipeline.BuildAssetBundle(null, selection, path, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets, BuildTarget.StandaloneWindows);

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

    [MenuItem("Assets/Build AssetBundle From Selection All - No dependency tracking")]
    static void ExportResourceNoTrack()
    {
        // Bring up save panel
        string path = EditorUtility.SaveFilePanel("Save Resource", "", "New Resource", "unity3d");
        if (path.Length != 0)
        {
            // Build the resource file from the active selection.
            BuildPipeline.BuildAssetBundle(Selection.activeObject, Selection.objects, path, 0, BuildTarget.StandaloneWindows);
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
    [MenuItem("Assets/Save Scene One")]
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
                    BuildPipeline.BuildStreamedSceneAssetBundle(levels, path, BuildTarget.StandaloneWindows);
                }
            }
        }
    }

    // 首先保存场景，然后再导出加载的场景资源文件，保存所有的场景到各自的资源文件
	[MenuItem("Assets/Save Scene All")]
	static void ExportSceneAll()
	{
		// 选择的要保存的对象
        string[] levels = new string[1];
        string srcpath;
        string destpath;
        string destfilename;
        string destext = ".unity3d";
        string destFileNoExt = "";

        srcpath = Application.dataPath + "/Scenes/";                            // 查找目录
        destpath = Application.dataPath + "/StreamingAssets/Scene/";            // 存放目录
        string[] fileEntries = Directory.GetFiles(srcpath, "*.unity", SearchOption.TopDirectoryOnly);     // 仅仅查找当前目录

        int idx = 0;
        int dotidx = 0;
        int splashidx = 0;
        while (idx < fileEntries.Length)
        {
            dotidx = fileEntries[idx].IndexOf('.');
            splashidx = fileEntries[idx].LastIndexOf('/');
            destFileNoExt = fileEntries[idx].Substring(splashidx + 1, dotidx - splashidx - 1);

            destfilename = destpath + destFileNoExt + destext;
            levels[0] = fileEntries[idx];
            //打包
            BuildPipeline.BuildStreamedSceneAssetBundle(levels, destfilename, BuildTarget.StandaloneWindows);
            ++idx;
        }
	}
}