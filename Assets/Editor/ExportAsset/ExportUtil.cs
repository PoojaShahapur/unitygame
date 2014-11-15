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

        public const string PREFAB = "prefab";
        public const string TEXTASSET = "textasset";

        public const string TRUE = "true";
        public const string FALSE = "false";

        public const string DOTUNITY3D = ".unity3d";
        public const string UNITY3D = "unity3d";

        static public void BuildAssetBundle(AssetBundleParam param)
        {
            BuildPipeline.BuildAssetBundle(param.m_mainAsset, param.m_assets, param.m_pathName, param.m_assetBundleOptions, param.m_targetPlatform);
        }

        static public void BuildStreamedSceneAssetBundle(StreamedSceneAssetBundleParam param)
        {
            BuildPipeline.BuildStreamedSceneAssetBundle(param.m_levels, param.m_locationPath, param.m_target, param.m_options);
        }

        static public string getDataPath(string path)
        {
            return Application.dataPath + "/" + path;
        }

        static public string getStreamingDataPath(string path)
        {
            return Application.streamingAssetsPath + "/" + path;
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

        static public bool getXmlAttrBool(XmlAttribute attr)
        {
            if (attr != null)
            {
                if (TRUE == attr.Value)
                {
                    return true;
                }
                else if (FALSE == attr.Value)
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
                    return path.Substring(0, dotIdx - 1);
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
    }
}