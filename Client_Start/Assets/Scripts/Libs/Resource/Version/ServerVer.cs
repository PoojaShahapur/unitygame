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

        public ServerVer()
        {
            m_path2HashDic = new Dictionary<string, FileVerInfo>();
        }

        virtual public void loadMiniVerFile(string ver = "")
        {
            AuxDownload auxDownload = new AuxDownload();
            auxDownload.download(VerFileName.VER_MINI, onMiniLoadEventHandle, 0, false, 0);
        }

        // 加载一个表完成
        protected void onMiniLoadEventHandle(IDispatchObject dispObj)
        {
            AuxDownload downloadItem = dispObj as AuxDownload;
            if (downloadItem.hasSuccessLoaded())
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

                mIsMiniLoadSuccess = true;
            }
            else if (downloadItem.hasFailed())
            {
                mIsMiniLoadSuccess = false;
            }

            mMiniLoadedDisp.dispatchEvent(null);
        }

        // 加载版本文件
        public void loadVerFile(string ver = "")
        {
            AuxDownload auxDownload = new AuxDownload();
            auxDownload.download(VerFileName.VER_P, onLoadEventHandle, 0, false, 0);
        }

        // 加载一个表完成
        protected void onLoadEventHandle(IDispatchObject dispObj)
        {
            AuxDownload downloadItem = dispObj as AuxDownload;
            if (downloadItem.hasSuccessLoaded())
            {
                Ctx.m_instance.m_logSys.debugLog_1(LangItemID.eItem0, downloadItem.getLogicPath());

                byte[] textAsset = downloadItem.getBytes();
                if (textAsset != null)
                {
                    loadFormText(System.Text.Encoding.UTF8.GetString(textAsset), m_path2HashDic);
                }

                mIsVerLoadSuccess = true;
            }
            else if (downloadItem.hasFailed())
            {
                Ctx.m_instance.m_logSys.debugLog_1(LangItemID.eItem1, downloadItem.getLogicPath());

                mIsVerLoadSuccess = false;
            }

            mLoadedDisp.dispatchEvent(null);
        }
    }
}