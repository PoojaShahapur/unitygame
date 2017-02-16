using SDK.Lib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEditor;
using UnityEngine;

namespace EditorTool
{
    class UtilEditor
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
            return UtilPath.combine(Application.dataPath, path);
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
            outputPath = Path.Combine(getPkgOutPath(), UtilEditor.IMAGE_PATH);
            outputPath = Path.Combine(outputPath, UtilEditor.GetPlatformFolderForAssetBundles(buildTarget));
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
            //outputPath = Path.Combine(System.Environment.CurrentDirectory, UtilEditor.ASSET_BUNDLES_OUTPUT_PATH);
            outputPath = Path.Combine(getPkgOutPath(), UtilEditor.ASSET_BUNDLES_OUTPUT_PATH);
            outputPath = Path.Combine(outputPath, UtilEditor.GetPlatformFolderForAssetBundles(buildTarget));
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
            return Path.Combine(System.Environment.CurrentDirectory, UtilEditor.PKG_OUTPATH);
        }

        static public string getRelDataPath(string path)
        {
            return Path.Combine(UtilEditor.ASSETS + UtilEditor.SLASH, path);
        }

        static public string convAbsPath2AssetPath(string fullPath)
        {
            string assetPath = "";

            int idx = fullPath.IndexOf(UtilEditor.ASSETS);
            assetPath = fullPath.Substring(idx, fullPath.Length - idx);

            return assetPath;
        }

        /**
         * @brief 转换相对目录到 Assets 目录
         */
        static public string conRelPath2AssetPath(string relPath)
        {
            string assetPath = "";

            int idx = relPath.IndexOf(UtilEditor.ASSETS);

            if(-1 == idx)   // 如果没有，就添加
            {
                assetPath = string.Format("{0}/{1}", UtilEditor.ASSETS, relPath);
            }
            else
            {
                if(0 != idx)
                {
                    assetPath = relPath.Substring(idx + UtilEditor.ASSETS.Length + 1);
                }
                else
                {
                    assetPath = relPath;
                }
            }
            
            return assetPath;
        }

        /**
         * @brief 转换 asset 目录到完整目录
         */
        static public string convAssetPath2FullPath(string path)
        {
            string ret = "";
            int assetIdx = path.IndexOf(UtilEditor.ASSETS);

            // 如果有 Assets 目录
            if (-1 != assetIdx)
            {
                //path = UtilPath.combine(UtilEditor.ASSETS, path);
                path = path.Substring(assetIdx + 1, path.Length - (assetIdx + UtilEditor.ASSETS.Length + 1));
            }

            ret = UtilEditor.getDataPath(path);

            return ret;
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
                // warning CS0618: `UnityEditor.BuildTarget.WebPlayerStreamed' is obsolete: `WebPlayerStreamed has been removed in 5.4'
                //case BuildTarget.WebPlayer:
                //case BuildTarget.WebPlayerStreamed:
                //    return "";
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
                // warning CS0618: `UnityEditor.BuildTarget.WebPlayer' is obsolete: `WebPlayer has been removed in 5.4'
                //case BuildTarget.WebPlayer:
                //    return "WebPlayer";
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
            //string platForm = GetPlatformFolderForAssetBundles(target);
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

        static public string getAssetBundlesOutpath(BuildTarget target)
        {
            string targetFolder = UtilEditor.GetPlatformFolderForAssetBundles(target);

            string sourcePath = UtilPath.combine(UtilEditor.getOutPutRootPath(), UtilApi.ASSETBUNDLES, targetFolder);

            return sourcePath;
        }

        // 获取输出根目录，所有自己写的文件都要放在这个文件的子文件夹下
        static public string getOutPutRootPath()
        {
            return UtilPath.combine(UtilPath.getCurrentDirectory(), "OutPut");
        }

        static public string getAssetBundlesPath(BuildTarget target)
        {
            string targetFolder = UtilEditor.GetPlatformFolderForAssetBundles(target);

            string sourcePath = UtilPath.combine(getOutPutRootPath(), UtilApi.ASSETBUNDLES, targetFolder);

            return sourcePath;
        }

        static public string getAssetBundlesManifestPath(BuildTarget target)
        {
            string targetFolder = UtilEditor.GetPlatformFolderForAssetBundles(target);

            string sourcePath = UtilPath.combine(getOutPutRootPath(), UtilApi.ASSETBUNDLES, targetFolder);
            sourcePath = sourcePath + "/" + targetFolder + UtilApi.DOTUNITY3D;

            return sourcePath;
        }

        static public string getBuildOutPath()
        {
            string sourcePath = UtilPath.combine(UtilEditor.getOutPutRootPath(), "BuildOut");
            return sourcePath;
        }

        // 获取克执行的二进制文件的输出目录
        static public string getBinPath(BuildTarget target)
        {
            string targetFolder = UtilEditor.GetPlatformFolderForAssetBundles(target);
            string sourcePath = UtilPath.combine(getOutPutRootPath(), "Bin", targetFolder);

            return sourcePath;
        }

        static public void renameManifestFile(BuildTarget target)
        {
            string targetFolder = UtilEditor.GetPlatformFolderForAssetBundles(target);
            string manifestSrcName = UtilPath.combine(UtilEditor.getOutPutRootPath(),
                                     UtilApi.ASSETBUNDLES,
                                     targetFolder,
                                     targetFolder
                                    );

            string manifestDestName = UtilPath.combine(UtilEditor.getOutPutRootPath(),
                                                 UtilApi.ASSETBUNDLES,
                                                 targetFolder,
                                                 targetFolder + UtilApi.DOTUNITY3D
                                                );
            UtilPath.renameFile(manifestSrcName, manifestDestName);
        }

        /// <summary>
        /// 获取贴图设置
        /// </summary>
        public static TextureImporter GetTextureSettings(string path)
        {
            TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
            //Texture Type
            textureImporter.textureType = TextureImporterType.Default;
            //Non power of2
            textureImporter.npotScale = TextureImporterNPOTScale.ToNearest;
            //PlatformTextureSettings
            textureImporter.SetPlatformTextureSettings("iPhone", 1024, TextureImporterFormat.PVRTC_RGBA4);
            textureImporter.SetPlatformTextureSettings("Android", 1024, TextureImporterFormat.ETC2_RGBA8);
            return textureImporter;
        }

        /// <summary>
        /// 循环设置选择的贴图
        /// </summary>
        private static void LoopSetTexture()
        {
            UnityEngine.Object[] textures = GetSelectedTextures();
            foreach (Texture2D texture in textures)
            {
                //获取资源路径
                string path = AssetDatabase.GetAssetPath(texture);
                TextureImporter texImporter = GetTextureSettings(path);
                TextureImporterSettings tis = new TextureImporterSettings();
                texImporter.ReadTextureSettings(tis);
                texImporter.SetTextureSettings(tis);
                AssetDatabase.ImportAsset(path);
            }
        }

        /// <summary>
        /// 获取Resources下的贴图
        /// </summary>
        /// <returns></returns>
        private static UnityEngine.Object[] GetSelectedTextures()
        {
            UnityEngine.Object[] textureAll;
            var textures = Resources.LoadAll("", typeof(Texture2D));
            int countAll = textures.Length;
            textureAll = new UnityEngine.Object[countAll];
            for (int i = 0; i < countAll; i++)
            {
                textureAll[i] = textures[i] as UnityEngine.Object;
            }
            return textureAll;
        }

        private static void LoopSetTexture2()
        {
            string[] fileInfo = GetTexturePath();
            int length = fileInfo.Length;
            for (int i = 0; i < length; i++)
            {
                //获取资源路径
                string path = fileInfo[i];
                TextureImporter texImporter = GetTextureSettings(path);
                TextureImporterSettings tis = new TextureImporterSettings();
                texImporter.ReadTextureSettings(tis);
                texImporter.SetTextureSettings(tis);
                AssetDatabase.ImportAsset(path);
            }
        }

        private static string[] GetTexturePath()
        {
            //jpg
            System.Collections.ArrayList jpgList = GetResourcesPath("*.jpg");
            int jpgLength = jpgList.Count;
            //png
            System.Collections.ArrayList pngList = GetResourcesPath("*.png");
            int pngLength = pngList.Count;
            //tga
            System.Collections.ArrayList tgaList = GetResourcesPath("*.tga");
            int tgaLength = tgaList.Count;
            string[] filePath = new string[jpgLength + pngLength + tgaLength];
            for (int i = 0; i < jpgLength; i++)
            {
                filePath[i] = jpgList[i].ToString();
            }
            for (int i = 0; i < pngLength; i++)
            {
                filePath[i + jpgLength] = pngList[i].ToString();
            }
            for (int i = 0; i < tgaLength; i++)
            {
                filePath[i + jpgLength + pngLength] = tgaList[i].ToString();
            }
            return filePath;
        }

        private static string PATH = "E:\\tcb\\program\\tcb\\tcbclient\\";

        /// <summary>
        /// 获取指定后掇后的文件路径
        /// </summary>
        /// <param name="fileType"></param>
        /// <returns></returns>
        private static System.Collections.ArrayList GetResourcesPath(string fileType)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(PATH + "Assets\\Resources");
            System.Collections.ArrayList filePath = new System.Collections.ArrayList();
            foreach (FileInfo fi in directoryInfo.GetFiles(fileType, SearchOption.AllDirectories))
            {
                string path = fi.DirectoryName + "\\" + fi.Name;
                path = path.Remove(0, PATH.Length);
                path = path.Replace("\\", "/");
                filePath.Add(path);
            }
            return filePath;
        }

        public static Sprite getSpriteByAssetPath(string texName, string spriteName)
        {
            UnityEngine.Object[] objectArray = AssetDatabase.LoadAllAssetsAtPath(texName);
            Sprite sprite = null;
            bool isFind = false;

            int idx = 0;
            int len = objectArray.Length;

            while(idx < len)
            {
                sprite = objectArray[idx] as Sprite;

                if(null != sprite)
                {
                    if(sprite.name == spriteName)
                    {
                        isFind = true;
                        break;
                    }
                }

                ++idx;
            }

            if(!isFind)
            {
                sprite = null;
            }

            return sprite;
        }

        public static void SaveAssets()
        {
            AssetDatabase.SaveAssets();
        }

        public static void Refresh()
        {
            AssetDatabase.Refresh();
        }
    }
}