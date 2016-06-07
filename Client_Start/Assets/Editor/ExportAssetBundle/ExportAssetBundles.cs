using UnityEngine;
using UnityEditor;
using System.IO;
using SDK.Lib;

namespace EditorTool
{
    public class BuildScript
    {
        public static void BuildAssetBundles(BuildTarget target)
        {
            string targetName = UtilEditor.GetPlatformFolderForAssetBundles(target);
            string outputPath = UtilPath.combine(UtilPath.getCurrentDirectory(),
                                                 UtilApi.ASSETBUNDLES,
                                                 targetName
                                                );
            if (UtilPath.existDirectory(outputPath))
            {
                UtilPath.deleteDirectory(outputPath);
            }
            if (!UtilPath.existDirectory(outputPath))
            {
                UtilPath.createDirectory(outputPath);
            }

            BuildPipeline.BuildAssetBundles(outputPath, BuildAssetBundleOptions.UncompressedAssetBundle, target);

            string manifestSrcName = UtilPath.combine(UtilPath.getCurrentDirectory(),
                                                 UtilApi.ASSETBUNDLES,
                                                 targetName,
                                                 targetName
                                                );

            string manifestDestName = UtilPath.combine(UtilPath.getCurrentDirectory(),
                                                 UtilApi.ASSETBUNDLES,
                                                 targetName,
                                                 targetName + UtilApi.DOTUNITY3D
                                                );
            UtilPath.renameFile(manifestSrcName, manifestDestName);
        }

        public static void BuildPlayer(BuildTarget target, bool isRelease)
        {
            string[] levels = UtilEditor.GetLevelsFromBuildSettings();
            if (levels.Length == 0)
            {
                Debug.Log("Nothing to build.");
                return;
            }

            string targetFolder = UtilEditor.GetPlatformFolderForAssetBundles(target);
            if (targetFolder == null)
                return;

            BuildScript.BuildAssetBundles(target);

            string sourcePath = UtilPath.combine(UtilPath.getCurrentDirectory(), UtilApi.ASSETBUNDLES, targetFolder);
            string outputPath = MFileSys.msStreamingAssetsPath;

            MList<string> extList = new MList<string>();
            extList.Add("manifest");

            UtilPath.copyDirectory(sourcePath, outputPath, true);
            UtilPath.deleteFiles(outputPath, null, extList, true);

            BuildOptions option = BuildOptions.None;
            if (!isRelease)
            {
                option = BuildOptions.Development;
                option = BuildOptions.AllowDebugging;
                option = BuildOptions.ConnectWithProfiler;
            }

            string pakPath = UtilPath.combine(UtilPath.getCurrentDirectory(), "BuildOut", targetFolder);
            UtilPath.createDirectory(pakPath, true);
            string targetName = UtilEditor.GetBuildTargetName(target);
            if (targetName == null)
                return;
            //BuildPipeline.BuildPlayer(levels, pakPath + "/" + targetName, target, option);
        }

        static void CopyAssetBundlesTo(BuildTarget target, string sourcePath, string outputPath)
        {
            FileUtil.DeleteFileOrDirectory(outputPath);
            Directory.CreateDirectory(outputPath);

            string outputFolder = UtilEditor.GetPlatformFolderForAssetBundles(target);
            string source = Path.Combine(sourcePath, outputFolder);

            if (!UtilPath.existDirectory(source))
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