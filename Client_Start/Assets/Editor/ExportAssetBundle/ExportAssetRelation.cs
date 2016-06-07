using SDK.Lib;
using UnityEditor;
using UnityEngine;

namespace EditorTool
{
    /**
     * @brief 导出资源关系
     */
    public class ExportAssetRelation
    {
        protected string mCurPath;      // 当前目录
        protected string mOutFileName;  // 输出配置文件名字
        protected BuildTarget mBuildTarget; // 编译平台
        protected MDataStream mDataStream;
        protected AssetBundleManifest mAssetBundleManifest;
        protected AssetBundle mManifestAB;

        public void setCurPath(string value)
        {
            mCurPath = UtilPath.normalPath(value);
        }

        public void setOutFileName(string value)
        {
            mOutFileName = UtilPath.normalPath(value);
        }

        public void setBuildTarget(BuildTarget buildTarget)
        {
            mBuildTarget = buildTarget;
        }

        public void buildOutFile()
        {
            string mainfestPath = UtilEditor.getAssetBundlesManifestPath(mBuildTarget);
            mManifestAB = AssetBundle.LoadFromFile(mainfestPath);
            mAssetBundleManifest = mManifestAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

            if (UtilPath.existFile(mOutFileName))
            {
                UtilPath.deleteFile(mOutFileName);
            }
            mDataStream = new MDataStream(mOutFileName);

            UtilPath.traverseDirectory(mCurPath, "", null, onFileHandle, true);

            mDataStream.dispose();
            mManifestAB.Unload(true);
        }

        public void onFileHandle(string fullPath, string fileName, string destFullPath)
        {
            fullPath = UtilPath.normalPath(fullPath);

            string strContent = "";
            string extName = UtilPath.getFileExt(fullPath);
            AssetBundle ab = null;
            string[] nameArr = null;
            string[] depNameArr = null;
            string abName = "";
            string assetListName = "";
            string depListName = "";

            abName = fullPath.Replace(mCurPath + "/", "");
            depNameArr = mAssetBundleManifest.GetAllDependencies(abName);
            foreach(string item in depNameArr)
            {
                if (depListName.Length > 0)
                {
                    depListName = depListName + ",";
                }
                depListName = depListName + item;
            }

            if (extName == UtilApi.UNITY3D)
            {
                ab = AssetBundle.LoadFromFile(fullPath);
                nameArr = ab.GetAllAssetNames();

                foreach(string assetNameItem in nameArr)
                {
                    if(assetListName.Length > 0)
                    {
                        assetListName = assetListName + ",";
                    }
                    assetListName = assetListName + assetNameItem;
                }
            }

            strContent = string.Format("{0}={1}={2}", abName, assetListName, depListName);

            mDataStream.writeLine(strContent);
        }
    }
}