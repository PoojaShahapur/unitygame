using SDK.Common;
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
        public const string IMAGE_PATH = "Image";
        public const string ASSETS = "Assets";
        public const string METAEXT = "meta";
        public const string PKG_OUTPATH = "PkgOutput";

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
#elif UNITY_4_6 || UNITY_4_5
            BuildPipeline.BuildAssetBundle(param.m_mainAsset, param.m_assets, param.m_pathName, param.m_assetBundleOptions, param.m_targetPlatform);
#endif
        }

        static public void BuildStreamedSceneAssetBundle(StreamedSceneAssetBundleParam param)
        {
#if UNITY_5
            BuildPipeline.BuildPlayer(param.m_levels, param.m_locationPath, param.m_target, param.m_options);
#elif UNITY_4_6 || UNITY_4_5
            BuildPipeline.BuildStreamedSceneAssetBundle(param.m_levels, param.m_locationPath, param.m_target, param.m_options);
#endif
        }

        static public void BuildPlayer(PlayerParam param)
        {
#if UNITY_5
            BuildPipeline.BuildPlayer(param.m_levels, param.m_locationPath, param.m_target, param.m_options);
#endif
        }

        // 获取 Data 目录
        static public string getDataPath(string path)
        {
            return Path.Combine(Application.dataPath, path);
        }

        // 获取当前目录
        protected static string getWorkPath(string path)
        {
            return Path.Combine(System.Environment.CurrentDirectory, path);
        }

        // 获取 pkg 打包工作目录
        public static string getPkgWorkPath(string path)
        {
            return Path.Combine(getPkgOutPath(), path);
        }

        const BuildTarget defaultValue = (BuildTarget)Int32.MaxValue;
        // 获取镜像目录
        public static string getImagePath(string path, BuildTarget buildTarget = defaultValue)
        {
            if (defaultValue == buildTarget)
            {
                buildTarget = EditorUserBuildSettings.activeBuildTarget;
            }

            string outputPath;
            outputPath = Path.Combine(getPkgOutPath(), ExportUtil.IMAGE_PATH);
            outputPath = Path.Combine(outputPath, ExportUtil.GetPlatformFolderForAssetBundles(buildTarget));
            if (string.IsNullOrEmpty(path))
            {
                outputPath = Path.Combine(outputPath, path);
            }
            return outputPath;
        }

        static public string getStreamingDataPath(string path, BuildTarget buildTarget = defaultValue)
        {
            if (defaultValue == buildTarget)
            {
                buildTarget = EditorUserBuildSettings.activeBuildTarget;
            }
            //return Application.streamingAssetsPath + "/" + path;
            //string outputPath = Application.dataPath.Substring(0, Application.dataPath.IndexOf("Assets"));
            string outputPath;
            //outputPath = Path.Combine(System.Environment.CurrentDirectory, ExportUtil.ASSET_BUNDLES_OUTPUT_PATH);
            outputPath = Path.Combine(getPkgOutPath(), ExportUtil.ASSET_BUNDLES_OUTPUT_PATH);
            outputPath = Path.Combine(outputPath, ExportUtil.GetPlatformFolderForAssetBundles(buildTarget));
            //outputPath = Path.Combine(outputPath, "StreamingAssets");
            //outputPath = Path.Combine(outputPath, Application.streamingAssetsPath);
            if (string.IsNullOrEmpty(path))
            {
                outputPath = Path.Combine(outputPath, path);
            }
            return outputPath;
        }

        public static string getPkgOutPath()
        {
            return Path.Combine(System.Environment.CurrentDirectory, ExportUtil.PKG_OUTPATH);
        }

        static public string getRelDataPath(string path)
        {
            return Path.Combine("Assets/", path);
        }

        static public void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch(Exception err)
                {
                    Debug.Log(string.Format("{0}{1}", "CreateDirectory Error: ", err.Message));
                }
            }
        }

        // 删除目录的时候，一定要关闭这个文件夹，否则删除文件夹可能出错
        static public void DeleteDirectory(string path, bool recursive = true)
        {
            if (Directory.Exists(path))
            {
                try
                {
                    Directory.Delete(path, recursive);
                }
                catch(Exception err)
                {
                    Debug.Log(string.Format("{0}{1}", "DeleteDirectory Error: ", err.Message));
                }
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
            path = normalPath(path);
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

        static public string getFileNameWithExt(string path)
        {
            path = normalPath(path);
            int slashIdx = path.LastIndexOf('/');
            if (-1 != slashIdx)
            {
                return path.Substring(slashIdx + 1, path.Length - slashIdx - 1);
            }
            else
            {
                return path;
            }
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
        public static void recursiveTraversalDir(string rootPath, Action<string> dispFile, Action<string> dispDir)
        {
            // 遍历目录回调
            if (dispDir != null)
            {
                dispDir(rootPath);
            }

            // 遍历所有文件
            traverseFilesInOneDir(rootPath, dispFile);

            // 遍历当前目录下的所有的文件夹
            DirectoryInfo theFolder = new DirectoryInfo(rootPath);
            DirectoryInfo[] dirInfo = theFolder.GetDirectories();
            foreach (DirectoryInfo NextFolder in dirInfo)
            {
                recursiveTraversalDir(NextFolder.FullName, dispFile, dispDir);
            }
        }

        // 处理当前目录下的所有文件
        public static void traverseFilesInOneDir(string dirPath, Action<string> dispFile)
        {
            DirectoryInfo theFolder = new DirectoryInfo(dirPath);

            //遍历文件
            FileInfo[] fileInfo = theFolder.GetFiles();
            foreach (FileInfo NextFile in fileInfo)  //遍历文件
            {
                if (dispFile != null)
                {
                    dispFile(NextFile.FullName);
                }
            }
        }

        // 遍历一个目录下的直属子目录
        public static void traverseSubDirInOneDir(string dirPath, Action<DirectoryInfo> dispDir)
        {
            // 遍历当前目录下的所有的所有子文件夹
            DirectoryInfo theFolder = new DirectoryInfo(dirPath);
            DirectoryInfo[] dirInfo = theFolder.GetDirectories();
            foreach (DirectoryInfo NextFolder in dirInfo)
            {
                if (dispDir != null)
                {
                    dispDir(NextFolder);
                }
            }
        }

        public static void copyDirectory(string sourceDirectory, string destDirectory)
        {
            //判断源目录和目标目录是否存在，如果不存在，则创建一个目录
            if (!Directory.Exists(sourceDirectory))
            {
                Directory.CreateDirectory(sourceDirectory);
            }
            if (!Directory.Exists(destDirectory))
            {
                Directory.CreateDirectory(destDirectory);
            }
            //拷贝文件
            copyFile(sourceDirectory, destDirectory);
           
            //拷贝子目录       
            //获取所有子目录名称
            string[] directionName = Directory.GetDirectories(sourceDirectory);
           
            foreach (string directionPath in directionName)
            {
                //根据每个子目录名称生成对应的目标子目录名称
                string directionPathTemp = destDirectory + "\\" + directionPath.Substring(sourceDirectory.Length + 1);
               
                //递归下去
                copyDirectory(directionPath, directionPathTemp);
            }                     
        }

        public static void copyFile(string sourceDirectory, string destDirectory)
        {
            //获取所有文件名称
            string[] fileName = Directory.GetFiles(sourceDirectory);
           
            foreach (string filePath in fileName)
            {
                //根据每个文件名称生成对应的目标文件名称
                string filePathTemp = destDirectory + "\\" + filePath.Substring(sourceDirectory.Length + 1);
               
                //若不存在，直接复制文件；若存在，覆盖复制
                if (File.Exists(filePathTemp))
                {
                    File.Copy(filePath, filePathTemp, true);
                }
                else
                {
                    File.Copy(filePath, filePathTemp);
                }
            }
        } 

        // 拷贝文件到 StreamingAssets 目录下
        public static void CopyAssetBundlesTo(string srcPath, BuildTarget target)
        {
            string platForm = GetPlatformFolderForAssetBundles(target);
            // Clear streaming assets folder.
            DeleteDirectory(Application.streamingAssetsPath);
            CreateDirectory(Application.streamingAssetsPath);
            // 放入平台单独的目录下
            //CreateDirectory(Path.Combine(Application.streamingAssetsPath, platForm));
            //copyDirectory(srcPath, Path.Combine(Application.streamingAssetsPath, platForm));
            copyDirectory(srcPath, Application.streamingAssetsPath);
        }

        static public bool getXmlAttrBool(XmlAttribute attr)
        {
            if (attr != null)
            {
                if (UtilApi.TRUE == attr.Value)
                {
                    return true;
                }
                else if (UtilApi.FALSE == attr.Value)
                {
                    return false;
                }
            }

            return false;
        }

        static public string getXmlAttrStr(XmlAttribute attr)
        {
            if (attr != null)
            {
                return attr.Value;
            }

            return "";
        }

        static public uint getXmlAttrUInt(XmlAttribute attr)
        {
            uint ret = 0;
            if (attr != null)
            {
                uint.TryParse(attr.Value, out ret);
            }

            return ret;
        }

        static public int getXmlAttrInt(XmlAttribute attr)
        {
            int ret = 0;
            if (attr != null)
            {
                int.TryParse(attr.Value, out ret);
            }

            return ret;
        }

        static public string rightSubStr(string origStr, string subStr)
        {
            int idx = origStr.IndexOf(subStr);
            if(idx != -1)
            {
                if(origStr.Length > idx + subStr.Length)
                {
                    if (subStr[subStr.Length - 1] != '/' && subStr[subStr.Length - 1] != '\\')
                    {
                        return origStr.Substring(idx + subStr.Length + 1);
                    }
                    else
                    {
                        return origStr.Substring(idx + subStr.Length);
                    }
                }
            }

            return "";
        }

        // 打包成 unity3d 后文件名字会变成小写，这里修改一下
        static public void modifyFileName(string path, string fileNameNoExt)
        {
            string srcFullPath = string.Format("{0}/{1}.{2}", path, fileNameNoExt.ToLower(), ExportUtil.UNITY3D);
            string destFullPath = string.Format("{0}/{1}.{2}", path, fileNameNoExt, ExportUtil.UNITY3D);

            if (File.Exists(srcFullPath))
            {
                File.Move(srcFullPath, destFullPath);
            }
            else
            {
                Debug.Log(string.Format("{0} 文件不存在", srcFullPath));
            }

            srcFullPath = string.Format("{0}/{1}.{2}.manifest", path, fileNameNoExt.ToLower(), ExportUtil.UNITY3D);
            destFullPath = string.Format("{0}/{1}.{2}.manifest", path, fileNameNoExt, ExportUtil.UNITY3D);

            if (File.Exists(srcFullPath))
            {
                File.Move(srcFullPath, destFullPath);
            }
            else
            {
                Debug.Log(string.Format("{0} 文件不存在", srcFullPath));
            }
        }

        // 删除一个文件
        static public void deleteFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}