using SDK.Lib;
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

        public const string ASSET_BUNDLES_OUTPUT_PATH = "AssetBundles";
        public const string IMAGE_PATH = "Image";
        public const string ASSETS = "Assets";
        public const string METAEXT = "meta";
        public const string PKG_OUTPATH = "PkgOutput";

        public const string FBX = "fbx";
        public const string AT = "@";
        static public string RESOURCES = "Resources";
        static public string SLASH = "/";

        // 获取 Data 目录
        static public string getDataPath(string path)
        {
            return Path.Combine(Application.dataPath, path);
        }

        // 获取当前目录
        static public string getWorkPath(string path)
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

        // 递归创建目录
        static public void recurseCreateStreamDirectory(string pathAndName)
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
                UtilPath.createDirectory(fullpath);

                slashIdx = leftpath.IndexOf("/");
            }
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

        // 拷贝文件到 StreamingAssets 目录下
        public static void CopyAssetBundlesTo(string srcPath, BuildTarget target)
        {
            string platForm = GetPlatformFolderForAssetBundles(target);
            UtilPath.deleteDirectory(Application.streamingAssetsPath);
            UtilPath.createDirectory(Application.streamingAssetsPath);
            // 放入平台单独的目录下
            //CreateDirectory(Path.Combine(Application.streamingAssetsPath, platForm));
            //copyDirectory(srcPath, Path.Combine(Application.streamingAssetsPath, platForm));
            UtilPath.copyDirectory(srcPath, Application.streamingAssetsPath, true);
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

        // 格式是 "0, 0, 0"
        static public Vector3 getXmlAttrVector3(XmlAttribute attr)
        {
            Vector3 ret = Vector3.zero;
            string[] scaleArr;
            if (attr != null)
            {
                scaleArr = attr.Value.Split(new []{','});
                ret = new Vector3(int.Parse(scaleArr[0]), int.Parse(scaleArr[1]), int.Parse(scaleArr[2]));
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

        static public void encodeLuaFile(string srcFile, string outFile, bool isWin)
        {
            if(!srcFile.ToLower().EndsWith(".lua"))
            {
                File.Copy(srcFile, outFile, true);
                return;
            }
            string luaexe = string.Empty;
            string args = string.Empty;
            string exedir = string.Empty;
            string currDir = Directory.GetCurrentDirectory();
            if(Application.platform == RuntimePlatform.WindowsEditor)
            {
                luaexe = "luajit.exe";
                args = "-b " + srcFile + " " + outFile;
                exedir = Application.dataPath + "/../../Tools/LuaEncoder/luajit/";
            }
            else if(Application.platform == RuntimePlatform.OSXEditor)
            {
                luaexe = "./luac";
                args = "-o " + outFile + " " + srcFile;
                exedir = Application.dataPath + "/../../Tools/LuaEncoder/luavm/";
            }
            Directory.SetCurrentDirectory(exedir);
            System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo();
            info.FileName = luaexe;
            info.Arguments = args;
            info.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            info.UseShellExecute = isWin;
            info.ErrorDialog = true;
            //Util.Log(info.FileName + " " + info.Arguments);

            System.Diagnostics.Process pro = System.Diagnostics.Process.Start(info);
            pro.WaitForExit();
            Directory.SetCurrentDirectory(currDir);
        }
    }
}