using SDK.Common;
using System;
using System.IO;
namespace SDK.Lib
{
    /**
     * @brief 版本系统，文件格式   path=value
     */
    public class VersionSys
    {
        public FilesVer m_webVer = new FilesVer();
        public FilesVer m_localVer = new FilesVer();

        public Action m_miniLoadResultDisp;
        public Action m_LoadResultDisp;
        public bool m_needUpdateVerFile;

        public string m_miniVer;    // mini 版本文件版本号

        public VersionSys()
        {
            m_webVer.m_type = FilesVerType.eWebVer;
            m_miniVer = UtilApi.Range(0, int.MaxValue).ToString();
        }

        public void loadMiniVerFile()
        {
            if (UtilApi.fileExistNoVer(Path.Combine(Ctx.m_instance.m_localFileSys.getLocalWriteDir(), FilesVer.MINIFILENAME)))
            {
                m_localVer.m_type = FilesVerType.ePersistentDataVer;
            }
            else
            {
                m_localVer.m_type = FilesVerType.eStreamingAssetsVer;
            }

            m_localVer.m_miniLoadedDisp = onLocalMiniLoaded;
            m_localVer.m_miniFailedDisp = onLocalMiniFailed;
            m_localVer.loadMiniVerFile();
        }

        public void loadVerFile()
        {
            if (UtilApi.fileExistNoVer(Path.Combine(Ctx.m_instance.m_localFileSys.getLocalWriteDir(), FilesVer.FILENAME)))
            {
                m_localVer.m_type = FilesVerType.ePersistentDataVer;
            }
            else
            {
                m_localVer.m_type = FilesVerType.eStreamingAssetsVer;
            }

            m_localVer.m_LoadedDisp = onVerLoaded;
            m_localVer.m_FailedDisp = onVerFailed;

            string ver = m_localVer.m_miniPath2HashDic[FilesVer.FILENAME].m_fileMd5;
            m_localVer.loadVerFile(ver);
        }

        public void onLocalMiniLoaded()
        {
            m_webVer.m_miniLoadedDisp = onWebMiniLoaded;
            m_webVer.m_miniFailedDisp = onWebMiniFailed;
            m_webVer.loadMiniVerFile(m_miniVer);
        }

        public void onLocalMiniFailed()
        {
            m_webVer.m_miniLoadedDisp = onWebMiniLoaded;
            m_webVer.m_miniFailedDisp = onWebMiniFailed;
            m_webVer.loadMiniVerFile(m_miniVer);
        }

        public void onWebMiniLoaded()
        {
            // 删除旧 mini 版本，修改新版本文件名字
            UtilApi.delFile(Path.Combine(Ctx.m_instance.m_localFileSys.getLocalWriteDir(), FilesVer.FILENAME));
            // 修改新的版本文件名字
            UtilApi.renameFile(UtilApi.combineVerPath(Path.Combine(Ctx.m_instance.m_localFileSys.getLocalWriteDir(), FilesVer.MINIFILENAME), m_miniVer), Path.Combine(Ctx.m_instance.m_localFileSys.getLocalWriteDir(), FilesVer.MINIFILENAME));

            m_needUpdateVerFile = (m_localVer.m_miniPath2HashDic[FilesVer.FILENAME].m_fileMd5 != m_webVer.m_miniPath2HashDic[FilesVer.FILENAME].m_fileMd5);      // 如果版本不一致，需要重新加载
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
                m_webVer.m_LoadedDisp = onWebVerLoaded;
                m_webVer.m_FailedDisp = onWebVerFailed;
                string ver = m_webVer.m_miniPath2HashDic[FilesVer.FILENAME].m_fileMd5;
                m_webVer.loadVerFile(ver);
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
                m_webVer.m_LoadedDisp = onWebVerLoaded;
                m_webVer.m_FailedDisp = onWebVerFailed;
                string ver = m_webVer.m_miniPath2HashDic[FilesVer.FILENAME].m_fileMd5;
                m_webVer.loadVerFile(ver);
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
                if (m_webVer.m_path2HashDic.ContainsKey(path))
                {
                    return m_webVer.m_path2HashDic[path].m_fileMd5;
                }
            }
            else
            {
                if (m_localVer.m_path2HashDic.ContainsKey(path))
                {
                    return m_localVer.m_path2HashDic[path].m_fileMd5;
                }
            }

            return "";
        }
    }
}