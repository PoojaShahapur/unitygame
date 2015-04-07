using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEditor;
using UnityEngine;

namespace EditorTool
{
    class ExportUtil
    {
        public const string BUNDLE = "bundle";
        public const string LEVEL = "level";

        public const string DOTPREFAB = ".prefab";
        public const string PREFAB = "prefab";
        public const string TEXTASSET = "textasset";

        public const string DOTUNITY3D = ".unity3d";
        public const string UNITY3D = "unity3d";

        public const string ASSET_BUNDLES_OUTPUT_PATH = "AssetBundles";
        public const string ASSETS = "Assets";
        public const string METAEXT = "meta";

        static public void BuildAssetBundle(AssetBundleParam param)
        {
#if UNITY_5
            if (param.m_buildList != null)
            {
                BuildPipeline.BuildAssetBundles(param.m_pathName, param.m_buildList, param.m_assetBundleOptions, param.m_targetPlatform);
            }
            else
            {
                BuildPipeline.BuildAssetBundles(param.m_pathName, param.m_assetBundleOptions, param.m_targetPlatform);
            }
#elif UNITY_4_6
            BuildPipeline.BuildAssetBundle(param.m_mainAsset, param.m_assets, param.m_pathName, param.m_assetBundleOptions, param.m_targetPlatform);
#endif
        }

        static public void BuildStreamedSceneAssetBundle(StreamedSceneAssetBundleParam param)
        {
#if UNITY_5
            BuildPipeline.BuildPlayer(param.m_levels, param.m_locationPath, param.m_target, param.m_options);
#elif UNITY_4_6
            BuildPipeline.BuildStreamedSceneAssetBundle(param.m_levels, param.m_locationPath, param.m_target, param.m_options);
#endif
        }

        static public void BuildPlayer(PlayerParam param)
        {
#if UNITY_5
            BuildPipeline.BuildPlayer(param.m_levels, param.m_locationPath, param.m_target, param.m_options);
#endif
        }

        static public string getDataPath(string path)
        {
            return Application.dataPath + "/" + path;
        }

        const BuildTarget defaultValue = (BuildTarget)Int32.MaxValue;
        static public string getStreamingDataPath(string path, BuildTarget buildTarget = defaultValue)
        {
            if (defaultValue == buildTarget)
            {
                buildTarget = EditorUserBuildSettings.activeBuildTarget;
            }
            //return Application.streamingAssetsPath + "/" + path;
            //string outputPath = Application.dataPath.Substring(0, Application.dataPath.IndexOf("Assets"));
            string outputPath;
            outputPath = Path.Combine(System.Environment.CurrentDirectory, ExportUtil.ASSET_BUNDLES_OUTPUT_PATH);
            outputPath = Path.Combine(outputPath, ExportUtil.GetPlatformFolderForAssetBundles(buildTarget));
            //outputPath = Path.Combine(outputPath, "StreamingAssets");
            //outputPath = Path.Combine(outputPath, Application.streamingAssetsPath);
            if (string.IsNullOrEmpty(path))
            {
                outputPath = Path.Combine(outputPath, path);
            }
            return outputPath;
        }

        static public string getRelDataPath(string path)
        {
            return "Assets/" + path;
        }

        static public void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        static public void RecurCreateDirectory(string pathAndName)
        {
            string curpath = "";
            string leftpath = "";
            string fullpath = "";
            leftpath = pathAndName.Substring(Application.streamingAssetsPath.Length + 1);
            int slashIdx = 0;
            slashIdx = leftpath.IndexOf("/");

            while (slashIdx != -1)
            {
                if (curpath.Length > 0)
                {
                    curpath += "/";
                }
                curpath += leftpath.Substring(0, slashIdx);
                leftpath = leftpath.Substring(slashIdx + 1, leftpath.Length - slashIdx - 1);

                fullpath = getStreamingDataPath(curpath);
                CreateDirectory(fullpath);

                slashIdx = leftpath.IndexOf("/");
            }
        }

        static public string combine(params string[] pathList)
        {
            int idx = 0;
            string ret = "";
            while(idx < pathList.Length)
            {
                if (ret.Length > 0)
                {
                    if (pathList[idx].Length > 0)
                    {
                        if (ret[ret.Length - 1] != '/' || pathList[idx][pathList[idx].Length - 1] != '/')
                        {
                            ret += "/";
                        }
                        ret += pathList[idx];
                    }
                }
                else
                {
                    if (pathList[idx].Length > 0)
                    {
                        ret += pathList[idx];
                    }
                }
                ++idx;
            }
            ret.Replace("//", "/");
            return ret;
        }

        static public Type convResStr2Type(string resStr)
        {
            Type retType = null;
            if(PREFAB == resStr)
            {
                retType = typeof(GameObject);
            }
            else if (TEXTASSET == resStr)
            {
                retType = typeof(TextAsset);
            }

            return retType;
        }

        static public string convExt2ResStr(string ext)
        {
            string ret = "";
            if("unity" == ext)
            {
                ret = "";
            }
            else if("prefab" == ext)
            {
                ret = PREFAB;
            }
            else if ("txt" == ext)
            {
                ret = TEXTASSET;
            }
            else if ("xml" == ext)
            {
                ret = TEXTASSET;
            }

            return ret;
        }

        static public List<string> GetAll(string path, bool recursion = false)//搜索文件夹中的文件
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            List<string> FileList = new List<string>();

            FileInfo[] allFile = dir.GetFiles();
            foreach (FileInfo fi in allFile)
            {
                //FileList.Add(fi.Name);
                FileList.Add(normalPath(fi.FullName));
            }

            if (recursion)
            {
                DirectoryInfo[] allDir = dir.GetDirectories();
                foreach (DirectoryInfo d in allDir)
                {
                    GetAll(d.FullName, recursion);
                }
            }
            return FileList;
        }

        static public string normalPath(string path)
        {
            return path.Replace("\\", "/");
        }

        static public string getFileExt(string path)
        {
            int dotIdx = path.LastIndexOf('.');
            if(-1 != dotIdx)
            {
                return path.Substring(dotIdx + 1);
            }

            return "";
        }

        static public string getFileNameNoExt(string path)
        {
            int dotIdx = path.LastIndexOf('.');
            int slashIdx = path.LastIndexOf('/');
            if (-1 != dotIdx)
            {
                if (-1 != slashIdx)
                {
                    return path.Substring(slashIdx + 1, dotIdx - slashIdx - 1);
                }
                else
                {
                    return path.Substring(0, dotIdx);
                }
            }

            return "";
        }

        static public string convFullPath2AssetsPath(string fullpath)
        {
            return fullpath.Substring(fullpath.IndexOf("Assets/"));
        }

        static public bool isArrContainElem(string elem, params string[] arr)
        {
            foreach(string item in arr)
            {
                if(item == elem)
                {
                    return true;
                }
            }

            return false;
        }

        static public string getSubMeshName(string skelName, string submeshName)
        {
            string skelNoExt = "";
            int dotIdx = skelName.LastIndexOf('.');
            if(-1 != dotIdx)
            {
                skelNoExt = skelName.Substring(0, dotIdx);
            }
            
            string submeshNameNoExt = submeshName;
            int slashIdx = submeshName.LastIndexOf('/');
            if(-1 != slashIdx)
            {
                submeshNameNoExt = submeshName.Substring(slashIdx + 1, submeshName.Length - 1 - slashIdx);
            }
            
            string ret = string.Format("{0}_{1}", skelNoExt, submeshNameNoExt);
            return ret;
        }

        public static string GetBuildTargetName(BuildTarget target)
        {
            switch (target)
            {
                case BuildTarget.Android:
                    return "/test.apk";
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    return "/test.exe";
                case BuildTarget.StandaloneOSXIntel:
                case BuildTarget.StandaloneOSXIntel64:
                case BuildTarget.StandaloneOSXUniversal:
                    return "/test.app";
                case BuildTarget.WebPlayer:
                case BuildTarget.WebPlayerStreamed:
                    return "";
                // Add more build targets for your own.
                default:
                    Debug.Log("Target not implemented.");
                    return null;
            }
        }

        public static string[] GetLevelsFromBuildSettings()
        {
            List<string> levels = new List<string>();
            for (int i = 0; i < EditorBuildSettings.scenes.Length; ++i)
            {
                if (EditorBuildSettings.scenes[i].enabled)
                    levels.Add(EditorBuildSettings.scenes[i].path);
            }

            return levels.ToArray();
        }

#if UNITY_EDITOR
        public static string GetPlatformFolderForAssetBundles(BuildTarget target)
        {
            switch (target)
            {
                case BuildTarget.Android:
                    return "Android";
                case BuildTarget.iOS:
                    return "iOS";
                case BuildTarget.WebPlayer:
                    return "WebPlayer";
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    return "Windows";
                case BuildTarget.StandaloneOSXIntel:
                case BuildTarget.StandaloneOSXIntel64:
                case BuildTarget.StandaloneOSXUniversal:
                    return "OSX";
                // Add more build targets for your own.
                // If you add more targets, don't forget to add the same platforms to GetPlatformFolderForAssetBundles(RuntimePlatform) function.
                default:
                    return null;
            }
        }
#endif

        static string GetPlatformFolderForAssetBundles(RuntimePlatform platform)
        {
            switch (platform)
            {
                case RuntimePlatform.Android:
                    return "Android";
                case RuntimePlatform.IPhonePlayer:
                    return "iOS";
                case RuntimePlatform.WindowsWebPlayer:
                case RuntimePlatform.OSXWebPlayer:
                    return "WebPlayer";
                case RuntimePlatform.WindowsPlayer:
                    return "Windows";
                case RuntimePlatform.OSXPlayer:
                    return "OSX";
                // Add more build platform for your own.
                // If you add more platforms, don't forget to add the same targets to GetPlatformFolderForAssetBundles(BuildTarget) function.
                default:
                    return null;
            }
        }

        // 递归深度优先遍历目录
        public static void recrueDirs(string rootPath, Action<string> disp)
        {
            // 遍历所有文件
            traverseFilesInOneDir(rootPath, disp);
            // 遍历当前目录下的所有的文件夹
            DirectoryInfo theFolder = new DirectoryInfo(rootPath);
            DirectoryInfo[] dirInfo = theFolder.GetDirectories();
            foreach (DirectoryInfo NextFolder in dirInfo)
            {
                recrueDirs(NextFolder.FullName, disp);
            }
        }

        // 处理当前目录下的所有文件
        public static void traverseFilesInOneDir(string dirPath, Action<string> disp)
        {
            DirectoryInfo theFolder = new DirectoryInfo(dirPath);

            //遍历文件
            FileInfo[] fileInfo = theFolder.GetFiles();
            foreach (FileInfo NextFile in fileInfo)  //遍历文件
            {
                if (disp != null)
                {
                    disp(NextFile.FullName);
                }
            }
        }
    }
}