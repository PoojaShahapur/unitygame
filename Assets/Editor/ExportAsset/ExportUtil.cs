using System;
using System.IO;
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
    }
}