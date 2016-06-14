using System;
using System.IO;

namespace SDK.Lib
{
    /**
     * @brief 版本系统，文件格式   path=value
     */
    public class VersionSys
    {
        public ServerVer m_serverVer = new ServerVer();
        public LocalVer m_localVer;

        public Action m_miniLoadResultDisp;
        public Action m_LoadResultDisp;
        public bool m_needUpdateVerFile;

        public string m_miniVer;    // mini 版本文件版本号

        public VersionSys()
        {
            m_serverVer.m_type = FilesVerType.eWebVer;
            m_miniVer = UtilApi.Range(0, int.MaxValue).ToString();

            m_serverVer = new ServerVer();
            m_localVer = new LocalVer();
        }

        public void loadMiniVerFile()
        {
            m_localVer.m_miniLoadedDisp = onLocalMiniLoaded;
            m_localVer.m_miniFailedDisp = onLocalMiniFailed;
            m_localVer.loadMiniVerFile();
        }

        public void loadVerFile()
        {
            m_localVer.m_LoadedDisp = onVerLoaded;
            m_localVer.m_FailedDisp = onVerFailed;

            m_localVer.loadVerFile();
        }

        public void onLocalMiniLoaded()
        {
            m_serverVer.m_miniLoadedDisp = onWebMiniLoaded;
            m_serverVer.m_miniFailedDisp = onWebMiniFailed;
            m_serverVer.loadMiniVerFile(m_miniVer);
        }

        public void onLocalMiniFailed()
        {
            m_serverVer.m_miniLoadedDisp = onWebMiniLoaded;
            m_serverVer.m_miniFailedDisp = onWebMiniFailed;
            m_serverVer.loadMiniVerFile(m_miniVer);
        }

        public void onWebMiniLoaded()
        {
            // 删除旧 mini 版本，修改新版本文件名字
            //UtilPath.deleteFile(Path.Combine(MFileSys.getLocalWriteDir(), VerFileName.VER_P));
            // 修改新的版本文件名字
            //UtilPath.renameFile(UtilLogic.combineVerPath(Path.Combine(MFileSys.getLocalWriteDir(), VerFileName.VER_MINI), m_miniVer), Path.Combine(MFileSys.getLocalWriteDir(), VerFileName.VER_MINI));

            m_needUpdateVerFile = (m_localVer.mFileVerInfo.m_fileMd5 != m_serverVer.mFileVerInfo.m_fileMd5);      // 如果版本不一致，需要重新加载
            //m_needUpdateVerFile = true;         // 测试强制更新
            m_miniLoadResultDisp();
        }

        public void onWebMiniFailed()
        {

        }

        public void onVerLoaded()
        {
            if (m_needUpdateVerFile)
            {
                m_serverVer.m_LoadedDisp = onWebVerLoaded;
                m_serverVer.m_FailedDisp = onWebVerFailed;
                string ver = m_serverVer.mFileVerInfo.m_fileMd5;
                m_serverVer.loadVerFile(ver);
            }
            else
            {
                m_LoadResultDisp();
            }
        }

        public void onVerFailed()
        {
            if (m_needUpdateVerFile)
            {
                m_serverVer.m_LoadedDisp = onWebVerLoaded;
                m_serverVer.m_FailedDisp = onWebVerFailed;
                string ver = m_serverVer.mFileVerInfo.m_fileMd5;
                m_serverVer.loadVerFile(ver);
            }
            else
            {
                m_LoadResultDisp();
            }
        }

        public void onWebVerLoaded()
        {
            m_LoadResultDisp();
        }

        public void onWebVerFailed()
        {
            m_LoadResultDisp();
        }

        public string getFileVer(string path)
        {
            if(m_needUpdateVerFile)
            {
                if (m_serverVer.m_path2HashDic.ContainsKey(path))
                {
                    return m_serverVer.m_path2HashDic[path].m_fileMd5;
                }
            }
            else
            {
                if (m_localVer.m_path2Ver_P_Dic.ContainsKey(path))
                {
                    return m_localVer.m_path2Ver_P_Dic[path].m_fileMd5;
                }
            }

            return "";
        }

        public void loadLocalVer()
        {
            m_localVer.load();
        }
    }
}