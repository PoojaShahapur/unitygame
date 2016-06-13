using System;
using System.Collections.Generic;

namespace SDK.Lib
{
    /**
     * @brief 一个位置的所有版本 Resources，StreamingAssets 、Application.persistentDataPath、web 
     */
    public class ServerVer
    {
        // MiniVersion 必须每一次从服务器上下载
        public const string MINIFILENAME = "VerMini.txt";
        public const string MINIFILENAMENOEXT = "VerMini";

        public const string FILENAME = "VerFile.txt";
        public const string FILENAMENOEXT = "VerFile";

        public Dictionary<string, FileVerInfo> m_miniPath2HashDic = new Dictionary<string, FileVerInfo>();
        public Dictionary<string, FileVerInfo> m_path2HashDic = new Dictionary<string, FileVerInfo>();

        public FilesVerType m_type;

        public Action m_miniLoadedDisp;
        public Action m_miniFailedDisp;

        public Action m_LoadedDisp;
        public Action m_FailedDisp;

        virtual public void loadMiniVerFile(string ver = "")
        {
            LoadParam param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
            param.setPath(MINIFILENAME);

            if (FilesVerType.eStreamingAssetsVer == m_type)
            {
                param.m_resLoadType = ResLoadType.eLoadStreamingAssets;
            }
            else if (FilesVerType.ePersistentDataVer == m_type)
            {
                param.m_resLoadType = ResLoadType.eLoadLocalPersistentData;
                param.m_version = ver;
            }
            else if (FilesVerType.eWebVer == m_type)
            {
                param.m_resLoadType = ResLoadType.eLoadWeb;
                //param.m_version = UtilApi.Range(int.MinValue, int.MaxValue).ToString();
                param.m_version = ver;
            }

            param.m_loadEventHandle = onMiniLoadEventHandle;

            Ctx.m_instance.m_resLoadMgr.loadData(param);
            Ctx.m_instance.m_poolSys.deleteObj(param);
        }

        // 加载一个表完成
        protected void onMiniLoadEventHandle(IDispatchObject dispObj)
        {
            ResItem res = dispObj as ResItem;
            if (res.refCountResLoadResultNotify.resLoadState.hasSuccessLoaded())
            {
                byte[] textAsset = (res as DataResItem).getBytes("");
                if (textAsset != null)
                {
                    // Lzma 解压缩
                    //byte[] outBytes = null;
                    //uint outLen = 0;
                    //MLzma.DecompressStrLZMA(textAsset, (uint)textAsset.Length, ref outBytes, ref outLen);
                    loadFormText(System.Text.Encoding.UTF8.GetString(textAsset), m_miniPath2HashDic);
                }

                // 卸载
                Ctx.m_instance.m_resLoadMgr.unload(MINIFILENAME, onMiniLoadEventHandle);

                m_miniLoadedDisp();
            }
            else if (res.refCountResLoadResultNotify.resLoadState.hasFailed())
            {
                // 卸载
                Ctx.m_instance.m_resLoadMgr.unload(MINIFILENAME, onMiniLoadEventHandle);
                m_miniFailedDisp();
            }
        }

        // 加载版本文件
        public void loadVerFile(string ver = "")
        {
            LoadParam param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
            param.setPath(FILENAME);

            if (FilesVerType.eStreamingAssetsVer == m_type)
            {
                param.m_resLoadType = ResLoadType.eLoadStreamingAssets;
            }
            else if (FilesVerType.ePersistentDataVer == m_type)
            {
                param.m_resLoadType = ResLoadType.eLoadLocalPersistentData;
                param.m_version = ver;
            }
            else if (FilesVerType.eWebVer == m_type)
            {
                param.m_resLoadType = ResLoadType.eLoadWeb;
                param.m_version = ver;
            }

            param.m_loadEventHandle = onLoadEventHandle;

            Ctx.m_instance.m_resLoadMgr.loadData(param);
            Ctx.m_instance.m_poolSys.deleteObj(param);
        }

        // 加载一个表完成
        protected void onLoadEventHandle(IDispatchObject dispObj)
        {
            ResItem res = dispObj as ResItem;
            if (res.refCountResLoadResultNotify.resLoadState.hasSuccessLoaded())
            {
                Ctx.m_instance.m_logSys.debugLog_1(LangItemID.eItem0, res.getLoadPath());

                byte[] textAsset = (res as DataResItem).getBytes("");
                if (textAsset != null)
                {
                    loadFormText(System.Text.Encoding.UTF8.GetString(textAsset), m_path2HashDic);
                }

                // 卸载
                Ctx.m_instance.m_resLoadMgr.unload(FILENAME, onLoadEventHandle);
                m_LoadedDisp();
            }
            else if (res.refCountResLoadResultNotify.resLoadState.hasFailed())
            {
                Ctx.m_instance.m_logSys.debugLog_1(LangItemID.eItem1, res.getLoadPath());
                // 卸载
                Ctx.m_instance.m_resLoadMgr.unload(FILENAME, onLoadEventHandle);
                m_FailedDisp();
            }
        }

        protected void loadFormText(string text, Dictionary<string, FileVerInfo> dic)
        {
            string[] lineSplitStr = { "\r\n" };
            string[] equalSplitStr = { "=" };
            string[] lineList = text.Split(lineSplitStr, StringSplitOptions.RemoveEmptyEntries);
            int lineIdx = 0;
            string[] equalList = null;
            FileVerInfo fileInfo;
            while (lineIdx < lineList.Length)
            {
                equalList = lineList[lineIdx].Split(equalSplitStr, StringSplitOptions.RemoveEmptyEntries);
                fileInfo = new FileVerInfo();
                fileInfo.m_fileMd5 = equalList[1];
                fileInfo.m_fileSize = Int32.Parse(equalList[2]);
                dic[equalList[0]] = fileInfo;
                ++lineIdx;
            }
        }
    }
}