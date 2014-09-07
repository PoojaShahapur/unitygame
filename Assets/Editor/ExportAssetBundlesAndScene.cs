using UnityEngine;
using UnityEditor;

/**
 * @brief 资源打包，导出 Bundles 和 Scene
 */
public class ExportAssetBundlesAndScen
{
	// 在Unity编辑器中添加菜单，导出的时候，一定要先选择自己设置的预设，然后点击菜单
    [MenuItem("Assets/Build AssetBundle From Selection - Track dependencies")]
    static void ExportAssetBundles()
	{
		// 打开保存面板，获得用户选择的路径  
		string path = EditorUtility.SaveFilePanel("Save Resource", "", "New Resource", "assetbundle");
		
		if (path.Length != 0)
		{  
			// 选择的要保存的对象  
			Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets); 
			//打包  
            BuildPipeline.BuildAssetBundle(Selection.activeObject, selection, path, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets, BuildTarget.StandaloneWindows);

            Selection.objects = selection;
		}
	}

    [MenuItem("Assets/Build AssetBundle From Selection - No dependency tracking")]
    static void ExportResourceNoTrack()
    {
        // Bring up save panel
        string path = EditorUtility.SaveFilePanel("Save Resource", "", "New Resource", "unity3d");
        if (path.Length != 0)
        {
            // Build the resource file from the active selection.
            BuildPipeline.BuildAssetBundle(Selection.activeObject, Selection.objects, path, 0, BuildTarget.StandaloneWindows);
        }
    }

    // 首先保存场景，然后再导出加载的场景资源文件
	[MenuItem("Assets/Save Scene")]
	static void ExportScene()
	{  
		// 打开保存面板，获得用户选择的路径  
		string path = EditorUtility.SaveFilePanel("Save Resource", "", "New Resource", "unity3d");
		if (path.Length != 0)
		{
			// 选择的要保存的对象
			string[] levels = new string[1];
			levels[0] = "Assets/Level.unity";	// 这个是要保存在本地的 unity 文件，如果没有保存场景，一定要先保存，然后才能导出 assetbundle
			//打包
            BuildPipeline.BuildStreamedSceneAssetBundle(levels, "Level.unity3d", BuildTarget.StandaloneWindows);
		}
	}
}