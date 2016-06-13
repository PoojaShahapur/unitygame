using System;
using System.Collections.Generic;

namespace SDK.Lib
{
    /**
     * @brief 本地版本系统
     */
    public class LocalVer : FileVerBase
    {
        public Dictionary<string, FileVerInfo> m_path2Ver_R_Dic;
        public Dictionary<string, FileVerInfo> m_path2Ver_S_Dic;
        public Dictionary<string, FileVerInfo> m_path2Ver_P_Dic;

        public Action m_miniLoadedDisp;
        public Action m_miniFailedDisp;

        public Action m_LoadedDisp;
        public Action m_FailedDisp;

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
            FilesVerType filesVerType = FilesVerType.ePersistentDataVer;

            if (UtilPath.fileExistNoVer(UtilPath.combine(MFileSys.getLocalWriteDir(), ServerVer.MINIFILENAME)))
            {
                filesVerType = FilesVerType.ePersistentDataVer;
            }
            else
            {
                filesVerType = FilesVerType.eStreamingAssetsVer;
            }
        }

        public void loadVerFile()
        {
            FilesVerType filesVerType = FilesVerType.ePersistentDataVer;

            if (UtilPath.fileExistNoVer(UtilPath.combine(MFileSys.getLocalWriteDir(), ServerVer.FILENAME)))
            {
                filesVerType = FilesVerType.ePersistentDataVer;
            }
            else
            {
                filesVerType = FilesVerType.eStreamingAssetsVer;
            }

            loadLocalPVer();
        }

        public void loadLocalRVer()
        {
            AuxTextLoader textLoader = new AuxTextLoader();
            textLoader.syncLoad("Version_R.txt");

            loadFormText(textLoader.getText(), m_path2Ver_R_Dic);

            textLoader.dispose();
            textLoader = null;
        }

        public void loadLocalSVer()
        {
            AuxTextLoader textLoader = new AuxTextLoader();
            textLoader.syncLoad("Version_S.txt");

            loadFormText(textLoader.getText(), m_path2Ver_S_Dic);

            textLoader.dispose();
            textLoader = null;
        }

        public void loadLocalPVer()
        {
            AuxTextLoader textLoader = new AuxTextLoader();
            textLoader.syncLoad("Version_P.txt");

            loadFormText(textLoader.getText(), m_path2Ver_P_Dic);

            textLoader.dispose();
            textLoader = null;
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