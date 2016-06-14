using System;
using System.Collections.Generic;

namespace SDK.Lib
{
    /**
     * @brief 本地版本系统
     */
    public class LocalVer : FileVerBase
    {
        public Dictionary<string, FileVerInfo> m_path2Ver_R_Dic;    // Resources 文件夹版本
        public Dictionary<string, FileVerInfo> m_path2Ver_S_Dic;    // StreamingAssets 文件夹版本
        public Dictionary<string, FileVerInfo> m_path2Ver_P_Dic;    // Persistent 文件夹版本

        protected MDataStream mMiniDataStream;      // Mini 版本
        protected MDataStream mRDataStream;         // Resources 版本
        protected MDataStream mSDataStream;         // StreamingAssets 版本
        protected MDataStream mPDataStream;         // Persistent 版本

        public LocalVer()
        {
            m_path2Ver_R_Dic = new Dictionary<string, FileVerInfo>();
            m_path2Ver_S_Dic = new Dictionary<string, FileVerInfo>();
            m_path2Ver_P_Dic = new Dictionary<string, FileVerInfo>();
        }

        public void load()
        {
            loadLocalRVer();
            loadLocalSVer();
            //loadLocalPVer();
        }

        public void loadMiniVerFile()
        {
            mMiniDataStream = new MDataStream(UtilPath.combine(MFileSys.getLocalWriteDir(), VerFileName.VER_MINI), onMiniLoadEventHandle);
        }

        protected void onMiniLoadEventHandle(IDispatchObject dispObj)
        {
            mMiniDataStream = dispObj as MDataStream;
            if (mMiniDataStream.isValid())
            {
                parseMiniFile(mMiniDataStream.readText());
            }
            mMiniDataStream.dispose();
            mMiniDataStream = null;

            mMiniLoadedDisp.dispatchEvent(null);
        }

        public void loadVerFile()
        {
            loadLocalPVer();
        }

        public void loadLocalRVer()
        {
            mRDataStream = new MDataStream(VerFileName.VER_R, onRVerLoaded);
        }

        public void onRVerLoaded(IDispatchObject dispObj)
        {
            mRDataStream = dispObj as MDataStream;
            if (mRDataStream.isValid())
            {
                loadFormText(mRDataStream.readText(), m_path2Ver_R_Dic);
            }
            mRDataStream.dispose();
            mRDataStream = null;
        }

        public void loadLocalSVer()
        {
            mSDataStream = new MDataStream(MFileSys.msStreamingAssetsPath + "/" + VerFileName.VER_S, onSVerLoaded);
        }

        public void onSVerLoaded(IDispatchObject dispObj)
        {
            mSDataStream = dispObj as MDataStream;
            if (mSDataStream.isValid())
            {
                loadFormText(mSDataStream.readText(), m_path2Ver_S_Dic);
            }

            mSDataStream.dispose();
            mSDataStream = null;
        }

        public void loadLocalPVer()
        {
            mPDataStream = new MDataStream(MFileSys.msPersistentDataPath + "/" + VerFileName.VER_P, onPVerLoaded);
        }

        public void onPVerLoaded(IDispatchObject dispObj)
        {
            mPDataStream = dispObj as MDataStream;
            if (mPDataStream.isValid())
            {
                loadFormText(mPDataStream.readText(), m_path2Ver_P_Dic);
            }

            mPDataStream.dispose();
            mPDataStream = null;

            mLoadedDisp.dispatchEvent(null);
        }

        public int getFileVerInfo(string origPath, ref FileVerInfo fileVerInfo)
        {
            // 在 Resources 中资源是大写，在 AssetBundles 中包含的资源名字是小写，但是 StreamingAssets 或者 Persistent 中不是 AssetBundles 形式的资源，仍然是大写
            string lowerOrigPath = origPath.ToLower();
            string md5 = "";
            ResLoadType resLoadType = ResLoadType.eLoadResource;

            // 这个目录只要有就记录
            if (m_path2Ver_P_Dic.ContainsKey(origPath))
            {
                analyzeHash(m_path2Ver_P_Dic[origPath], ResLoadType.eLoadLocalPersistentData, ref fileVerInfo, ref md5, ref resLoadType);
            }
            else if (m_path2Ver_P_Dic.ContainsKey(lowerOrigPath))
            {
                analyzeHash(m_path2Ver_P_Dic[lowerOrigPath], ResLoadType.eLoadLocalPersistentData, ref fileVerInfo, ref md5, ref resLoadType);
            }

            if (m_path2Ver_S_Dic.ContainsKey(origPath))
            {
                // 如果两个 Hash 码是相同，就说明资源定向在 StreamAsset 目录里面
                analyzeHash(m_path2Ver_S_Dic[origPath], ResLoadType.eLoadStreamingAssets, ref fileVerInfo, ref md5, ref resLoadType);
            }
            else if (m_path2Ver_S_Dic.ContainsKey(lowerOrigPath))
            {
                analyzeHash(m_path2Ver_S_Dic[lowerOrigPath], ResLoadType.eLoadStreamingAssets, ref fileVerInfo, ref md5, ref resLoadType);
            }

            if (m_path2Ver_R_Dic.ContainsKey(origPath))
            {
                analyzeHash(m_path2Ver_R_Dic[origPath], ResLoadType.eLoadResource, ref fileVerInfo, ref md5, ref resLoadType);
            }

            return (int)resLoadType;
        }

        // 比较 Hash 码
        protected void analyzeHash(
            FileVerInfo srcFileVerInfo, 
            ResLoadType defaultResLoadType, 
            ref FileVerInfo fileVerInfo, 
            ref string md5, 
            ref ResLoadType resLoadType
            )
        {
            if (md5 == srcFileVerInfo.m_fileMd5)
            {
                fileVerInfo = srcFileVerInfo;
                resLoadType = defaultResLoadType;
            }
            else if (string.IsNullOrEmpty(md5))
            {
                fileVerInfo = srcFileVerInfo;
                md5 = fileVerInfo.m_fileMd5;
                resLoadType = defaultResLoadType;
            }
        }
    }
}