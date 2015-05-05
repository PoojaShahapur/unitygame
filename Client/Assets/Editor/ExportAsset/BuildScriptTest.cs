using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using EditorTool;

public class BuildScriptTest
{
    // 打包 Editor 的 Asset Labels 中指定的 AssetBundle
	public static void BuildAssetLabelsAssetBundles()
	{
		// Choose the output path according to the build target.
        string outputPath = Path.Combine(ExportUtil.ASSET_BUNDLES_OUTPUT_PATH, ExportUtil.GetPlatformFolderForAssetBundles(EditorUserBuildSettings.activeBuildTarget));
		if (!Directory.Exists(outputPath) )
			Directory.CreateDirectory (outputPath);

        AssetBundleParam param = new AssetBundleParam();
        param.m_pathName = outputPath;
        param.m_assetBundleOptions = 0;
        param.m_targetPlatform = EditorUserBuildSettings.activeBuildTarget;

        ExportUtil.BuildAssetBundle(param);
	}

    // 打包自己指定资源
    public static void BuildCustomAssetBundles()
	{
		// Choose the output path according to the build target.
        string outputPath = ExportUtil.getStreamingDataPath("");
		if (!Directory.Exists(outputPath) )
			Directory.CreateDirectory (outputPath);

        AssetBundleParam param = new AssetBundleParam();
        param.m_buildList = new AssetBundleBuild[1];
        param.m_buildList[0].assetBundleName = "TestExportPrefab";
        param.m_buildList[0].assetBundleVariant = ExportUtil.UNITY3D;
        param.m_buildList[0].assetNames = new string[1];
        param.m_buildList[0].assetNames[0] = "Assets/TestAssets/TestPrefab.prefab";     // 这个目录一定要是 Assets 下面写，并且加上扩展名字
        param.m_pathName = outputPath;
        param.m_assetBundleOptions = 0;
        param.m_targetPlatform = EditorUserBuildSettings.activeBuildTarget;
        ExportUtil.BuildAssetBundle(param);

        // 打包成 unity3d 后文件名字会变成小写，这里修改一下
        ExportUtil.modifyFileName(outputPath, "TestExportPrefab");
	}

    public static void BuildStreamedSceneAssetBundles()
    {
        // Choose the output path according to the build target.
        string outputPath = ExportUtil.getStreamingDataPath("");
        if (!Directory.Exists(outputPath))
            Directory.CreateDirectory(outputPath);

        string[] levels = ExportUtil.GetLevelsFromBuildSettings();
        AssetBundleParam param = new AssetBundleParam();
        param.m_buildList = new AssetBundleBuild[1];
        param.m_buildList[0].assetBundleName = "TestExportScene";
        param.m_buildList[0].assetBundleVariant = ExportUtil.UNITY3D;
        param.m_buildList[0].assetNames = new string[1];
        param.m_buildList[0].assetNames[0] = levels[3];     // 这个目录一定要是 Assets 下面写，并且加上扩展名字
        param.m_pathName = outputPath;
        param.m_assetBundleOptions = 0;
        param.m_targetPlatform = EditorUserBuildSettings.activeBuildTarget;
        ExportUtil.BuildAssetBundle(param);
    }

    // 打包场景
	public static void BuildPlayer()
	{
		var outputPath = EditorUtility.SaveFolderPanel("Choose Location of the Built Game", "", "");
		if (outputPath.Length == 0)
			return;

        string[] levels = ExportUtil.GetLevelsFromBuildSettings();
		if (levels.Length == 0)
		{
			Debug.Log("Nothing to build.");
			return;
		}

        string targetName = ExportUtil.GetBuildTargetName(EditorUserBuildSettings.activeBuildTarget);
		if (targetName == null)
			return;

		// Build and copy AssetBundles.
        BuildScriptTest.BuildAssetLabelsAssetBundles();
        BuildScriptTest.CopyAssetBundlesTo(Path.Combine(Application.streamingAssetsPath, ExportUtil.ASSET_BUNDLES_OUTPUT_PATH));
		BuildOptions option = EditorUserBuildSettings.development ? BuildOptions.Development : BuildOptions.None;

        PlayerParam param = new PlayerParam();
        param.m_levels = levels;
        param.m_locationPath = outputPath + targetName;
        param.m_target = EditorUserBuildSettings.activeBuildTarget;
        param.m_options = option;

        ExportUtil.BuildPlayer(param);
	}

    // 拷贝资源到 StreamingAssets 目录下
	static void CopyAssetBundlesTo(string outputPath)
	{
		// Clear streaming assets folder.
		FileUtil.DeleteFileOrDirectory(Application.streamingAssetsPath);
		Directory.CreateDirectory(outputPath);

        string outputFolder = ExportUtil.GetPlatformFolderForAssetBundles(EditorUserBuildSettings.activeBuildTarget);

		// Setup the source folder for assetbundles.
        var source = Path.Combine(Path.Combine(System.Environment.CurrentDirectory, ExportUtil.ASSET_BUNDLES_OUTPUT_PATH), outputFolder);
		if (!System.IO.Directory.Exists(source) )
			Debug.Log("No assetBundle output folder, try to build the assetBundles first.");

		// Setup the destination folder for assetbundles.
		var destination = System.IO.Path.Combine(outputPath, outputFolder);
		if (System.IO.Directory.Exists(destination) )
			FileUtil.DeleteFileOrDirectory(destination);
		
		FileUtil.CopyFileOrDirectory(source, destination);
	}
}
