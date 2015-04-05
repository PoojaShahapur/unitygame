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

        public Action<bool> m_miniLoadResultDisp;
        public Action m_LoadResultDisp;
        public bool m_needUpdateVer;

        public VersionSys()
        {
            m_webVer.m_type = FilesVerType.eWebVer;
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
            m_localVer.loadVerFile();
        }

        public void onLocalMiniLoaded()
        {
            m_webVer.m_miniLoadedDisp = onWebMiniLoaded;
            m_webVer.m_miniFailedDisp = onWebMiniFailed;
            m_webVer.loadMiniVerFile();
        }

        public void onLocalMiniFailed()
        {
            m_webVer.m_miniLoadedDisp = onWebMiniLoaded;
            m_webVer.m_miniFailedDisp = onWebMiniFailed;
            m_webVer.loadMiniVerFile();
        }

        public void onWebMiniLoaded()
        {
            m_needUpdateVer = (m_localVer.m_miniPath2HashDic[FilesVer.FILENAME] != m_webVer.m_miniPath2HashDic[FilesVer.FILENAME]);      // 如果版本不一致，需要重新加载
            m_miniLoadResultDisp(m_needUpdateVer);
        }

        public void onWebMiniFailed()
        {

        }

        public void onVerLoaded()
        {
            if (m_needUpdateVer)
            {
                m_webVer.m_LoadedDisp = onWebVerLoaded;
                m_webVer.m_FailedDisp = onWebVerFailed;
                m_webVer.loadVerFile();
            }
        }

        public void onVerFailed()
        {
            if (m_needUpdateVer)
            {
                m_webVer.m_LoadedDisp = onWebVerLoaded;
                m_webVer.m_FailedDisp = onWebVerFailed;
                m_webVer.loadVerFile();
            }
        }

        public void onWebVerLoaded()
        {
            m_LoadResultDisp();
        }

        public void onWebVerFailed()
        {

        }
    }
}