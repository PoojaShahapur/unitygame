using SDK.Lib;
using System;
using System.IO;
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
        protected string mMainfestName;
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
            mMainfestName = UtilEditor.GetPlatformFolderForAssetBundles(buildTarget) + UtilApi.DOTUNITY3D;
        }

        public void buildOutFile()
        {
            string mainfestPath = UtilEditor.getAssetBundlesManifestPath(mBuildTarget);
            mManifestAB = AssetBundle.LoadFromFile(mainfestPath);
            mAssetBundleManifest = mManifestAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            //mManifestAB.Unload(true);       // 立即卸载掉，数据会丢失

            try
            {
                if (UtilPath.existFile(mOutFileName))
                {
                    UtilPath.deleteFile(mOutFileName);
                }
                else
                {
                    // 确保写入的目录是存在的
                    string outPath = UtilPath.getFilePathNoName(mOutFileName);
                    if (!UtilPath.existDirectory(outPath))
                    {
                        UtilPath.createDirectory(outPath);
                    }
                }

                mDataStream = new MDataStream(mOutFileName, null, FileMode.CreateNew, FileAccess.Write);

                UtilPath.traverseDirectory(mCurPath, "", null, onFileHandle, true);

                mDataStream.dispose();
                mDataStream = null;
                mManifestAB.Unload(true);
            }
            catch(Exception exp)
            {
                Debug.Log("Exception " + exp.Message);
                if(mDataStream != null)
                {
                    mDataStream.dispose();
                    mDataStream = null;
                }
                mManifestAB.Unload(true);
            }
        }

        public void onFileHandle(string fullPath, string fileName, string destFullPath)
        {
            // 如果是清单文件名字，直接返回
            if(fileName == mMainfestName)
            {
                return;
            }

            fullPath = UtilPath.normalPath(fullPath);

            string strContent = "";
            string extName = UtilPath.getFileExt(fullPath);
            AssetBundle ab = null;
            string[] includeAssetArr = null;
            string[] depNameArr = null;
            string abName = "";
            string assetListName = "";
            string depListName = "";

            string assetPathPrefix = UtilEditor.getAssetBundlesOutpath(mBuildTarget) + "/assets/";

            if (extName == UtilApi.UNITY3D)
            {
                abName = fullPath.Replace(mCurPath + "/", "");
                depNameArr = mAssetBundleManifest.GetAllDependencies(abName);
                foreach (string item in depNameArr)
                {
                    if (depListName.Length > 0)
                    {
                        depListName = depListName + ",";
                    }
                    depListName = depListName + item;
                }

                ab = AssetBundle.LoadFromFile(fullPath);
                includeAssetArr = ab.GetAllAssetNames();
                ab.Unload(true);

                string replaceItemName = "";
                foreach (string assetNameItem in includeAssetArr)
                {
                    replaceItemName = assetNameItem.Replace("assets/resources/", "");
                    if (assetListName.Length > 0)
                    {
                        assetListName = assetListName + ",";
                    }
                    assetListName = assetListName + replaceItemName;
                }

                // 如果有资源，就是正常的 AssetBundles 包
                if (!string.IsNullOrEmpty(assetListName))
                {
                    strContent = string.Format("{0}={1}={2}", abName, assetListName, depListName);
                }
                else
                {
                    // 如果是空，就是场景的 AssetBundles 包，这个时候， AB 包中资源的名字是空值
                    fullPath = fullPath.Replace(assetPathPrefix, "");
                    fullPath = UtilPath.getFilePathNoExt(fullPath);
                    assetListName = fullPath + UtilApi.DOTUNITY;
                    strContent = string.Format("{0}={1}={2}", abName, assetListName, depListName);
                }

                mDataStream.writeLine(strContent);
            }
        }
    }
}