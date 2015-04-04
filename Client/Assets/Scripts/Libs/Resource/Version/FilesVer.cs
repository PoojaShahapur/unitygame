using SDK.Common;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDK.Lib
{
    public enum FilesVerType
    {
        eStreamingAssetsVer,
        ePersistentDataVer,
        eWebVer,
    }

    /**
     * @brief 一个位置的所有版本，StreamingAssets 、Application.persistentDataPath、web 
     */
    public class FilesVer
    {
        // MiniVersion 必须每一次从服务器上下载
        public const string MINIFILENAME = "MiniVersion.txt";
        public const string MINIFILENAMENOEXT = "MiniVersion";

        public const string FILENAME = "Version.txt";
        public const string FILENAMENOEXT = "Version";

        protected Dictionary<string, FileVerInfo> m_miniPath2HashDic = new Dictionary<string, FileVerInfo>();
        protected Dictionary<string, FileVerInfo> m_path2HashDic = new Dictionary<string, FileVerInfo>();

        public FilesVerType m_type;

        virtual public void loadMiniVerFile()
        {
            LoadParam param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
            param.m_path = MINIFILENAME;

            if (FilesVerType.eStreamingAssetsVer == m_type)
            {
                param.m_resLoadType = ResLoadType.eStreamingAssets;
            }
            else if (FilesVerType.ePersistentDataVer == m_type)
            {
                param.m_resLoadType = ResLoadType.ePersistentData;
            }
            else if (FilesVerType.eWebVer == m_type)
            {
                param.m_resLoadType = ResLoadType.eLoadWeb;
                param.m_path += "?ver=";
                param.m_path += UtilApi.Range(int.MinValue, int.MaxValue);
            }

            param.m_loaded = onLoadedMini;
            param.m_failed = onFailedMini;

            Ctx.m_instance.m_resLoadMgr.loadData(param);
            Ctx.m_instance.m_poolSys.deleteObj(param);
        }

        // 加载一个表完成
        protected void onLoadedMini(IDispatchObject resEvt)
        {
            IResItem m_res = resEvt as IResItem;                         // 类型转换
            byte[] textAsset = (m_res as DataResItem).getBytes();
            if (textAsset != null)
            {
                loadFormText(System.Text.Encoding.UTF8.GetString(textAsset), m_miniPath2HashDic);
            }

            // 卸载
            Ctx.m_instance.m_resLoadMgr.unload(MINIFILENAME);
        }

        protected void onFailedMini(IDispatchObject resEvt)
        {
            // 卸载
            Ctx.m_instance.m_resLoadMgr.unload(MINIFILENAME);
        }

        // 加载版本文件
        public void loadVerFile()
        {
            LoadParam param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
            param.m_path = FILENAME;

            if (FilesVerType.eStreamingAssetsVer == m_type)
            {
                param.m_resLoadType = ResLoadType.eStreamingAssets;
            }
            else if (FilesVerType.ePersistentDataVer == m_type)
            {
                param.m_resLoadType = ResLoadType.ePersistentData;
            }
            else if (FilesVerType.eWebVer == m_type)
            {
                param.m_resLoadType = ResLoadType.eLoadWeb;
            }

            param.m_loaded = onLoaded;
            param.m_failed = onFailed;

            Ctx.m_instance.m_resLoadMgr.loadData(param);
            Ctx.m_instance.m_poolSys.deleteObj(param);
        }

        // 加载一个表完成
        protected void onLoaded(IDispatchObject resEvt)
        {
            IResItem m_res = resEvt as IResItem;                         // 类型转换
            byte[] textAsset = (m_res as DataResItem).getBytes();
            if (textAsset != null)
            {
                loadFormText(System.Text.Encoding.UTF8.GetString(textAsset), m_path2HashDic);
            }

            // 卸载
            Ctx.m_instance.m_resLoadMgr.unload(FILENAME);
        }

        protected void onFailed(IDispatchObject resEvt)
        {
            // 卸载
            Ctx.m_instance.m_resLoadMgr.unload(FILENAME);
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