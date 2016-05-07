using UnityEngine;
using UnityEditor;
using System.IO;
using SDK.Lib;

namespace EditorTool
{
    public class BuildScript
    {
        [MenuItem("My/ExportPlayer/Window")]
        public static void BuildWindowDebug()
        {
            BuildPlayer(BuildTarget.StandaloneWindows, false);
        }

        public static void BuildAssetBundles(BuildTarget target)
        {
            string outputPath = Path.Combine(UtilApi.kAssetBundlesOutputPath, ExportUtil.GetPlatformFolderForAssetBundles(target));
            if (UtilApi.ExistDirectory(outputPath))
            {
                UtilApi.DeleteDirectory(outputPath);
            }
            if (!UtilApi.ExistDirectory(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }

            BuildPipeline.BuildAssetBundles(outputPath, BuildAssetBundleOptions.UncompressedAssetBundle, target);
        }

        public static void BuildPlayer(BuildTarget target, bool isRelease)
        {
            string[] levels = ExportUtil.GetLevelsFromBuildSettings();
            if (levels.Length == 0)
            {
                Debug.Log("Nothing to build.");
                return;
            }

            string targetName = ExportUtil.GetBuildTargetName(target);
            if (targetName == null)
                return;

            BuildScript.BuildAssetBundles(target);

            string sourcePath = Path.Combine(System.Environment.CurrentDirectory, UtilApi.kAssetBundlesOutputPath);
            string outputPath = Path.Combine(Application.streamingAssetsPath, UtilApi.kAssetBundlesOutputPath);
            BuildScript.CopyAssetBundlesTo(target, sourcePath, outputPath);

            BuildOptions option = BuildOptions.None;
            if (!isRelease)
            {
                option = BuildOptions.Development;
            }
            BuildPipeline.BuildPlayer(levels, outputPath + targetName, target, option);
        }

        static void CopyAssetBundlesTo(BuildTarget target, string sourcePath, string outputPath)
        {
            FileUtil.DeleteFileOrDirectory(outputPath);
            Directory.CreateDirectory(outputPath);

            string outputFolder = ExportUtil.GetPlatformFolderForAssetBundles(target);
            string source = Path.Combine(sourcePath, outputFolder);

            if (!System.IO.Directory.Exists(source))
            {
                Debug.Log("No assetBundle output folder, try to build the assetBundles first.");
            }

            string destination = System.IO.Path.Combine(outputPath, outputFolder);
            if (System.IO.Directory.Exists(destination))
            {
                FileUtil.DeleteFileOrDirectory(destination);
            }

            FileUtil.CopyFileOrDirectory(source, destination);
        }
    }
}