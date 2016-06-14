using System;
using System.Collections.Generic;

namespace SDK.Lib
{
    /**
     * @brief 一个位置的所有版本 Resources，StreamingAssets 、Application.persistentDataPath、web 
     */
    public class ServerVer : FileVerBase
    {
        // MiniVersion 必须每一次从服务器上下载
        public Dictionary<string, FileVerInfo> m_path2HashDic;

        public FilesVerType m_type;

        public Action m_miniLoadedDisp;
        public Action m_miniFailedDisp;

        public Action m_LoadedDisp;
        public Action m_FailedDisp;

        public ServerVer()
        {
            m_path2HashDic = new Dictionary<string, FileVerInfo>();
        }

        virtual public void loadMiniVerFile(string ver = "")
        {
            AuxDownload auxDownload = new AuxDownload();
            auxDownload.load(VerFileName.VER_MINI, onMiniLoadEventHandle, 0, false, 0);
        }

        // 加载一个表完成
        protected void onMiniLoadEventHandle(IDispatchObject dispObj)
        {
            DownloadItem downloadItem = dispObj as DownloadItem;
            if (downloadItem.refCountResLoadResultNotify.resLoadState.hasSuccessLoaded())
            {
                byte[] textAsset = downloadItem.getBytes();
                if (textAsset != null)
                {
                    // Lzma 解压缩
                    //byte[] outBytes = null;
                    //uint outLen = 0;
                    //MLzma.DecompressStrLZMA(textAsset, (uint)textAsset.Length, ref outBytes, ref outLen);
                    parseMiniFile(System.Text.Encoding.UTF8.GetString(textAsset));
                }

                m_miniLoadedDisp();
            }
            else if (downloadItem.refCountResLoadResultNotify.resLoadState.hasFailed())
            {
                m_miniFailedDisp();
            }
        }

        // 加载版本文件
        public void loadVerFile(string ver = "")
        {
            AuxDownload auxDownload = new AuxDownload();
            auxDownload.load(VerFileName.VER_P, onLoadEventHandle, 0, false, 0);
        }

        // 加载一个表完成
        protected void onLoadEventHandle(IDispatchObject dispObj)
        {
            DownloadItem downloadItem = dispObj as DownloadItem;
            if (downloadItem.refCountResLoadResultNotify.resLoadState.hasSuccessLoaded())
            {
                Ctx.m_instance.m_logSys.debugLog_1(LangItemID.eItem0, downloadItem.getLoadPath());

                byte[] textAsset = downloadItem.getBytes();
                if (textAsset != null)
                {
                    loadFormText(System.Text.Encoding.UTF8.GetString(textAsset), m_path2HashDic);
                }

                m_LoadedDisp();
            }
            else if (downloadItem.refCountResLoadResultNotify.resLoadState.hasFailed())
            {
                Ctx.m_instance.m_logSys.debugLog_1(LangItemID.eItem1, downloadItem.getLoadPath());

                m_FailedDisp();
            }
        }
    }
}