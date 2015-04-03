using SDK.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SDK.Lib
{
    /**
     * @brief 版本系统，文件格式   path=value
     */
    public class VersionSys
    {
        // MiniVersion 必须每一次从服务器上下载
        public const string MINIFILENAME = "MiniVersion.txt";
        public const string MINIFILENAMENOEXT = "MiniVersion";

        public const string FILENAME = "Version.txt";
        public const string FILENAMENOEXT = "Version";

        protected Dictionary<string, string> m_miniPath2HashDic = new Dictionary<string, string>();
        protected Dictionary<string, string> m_path2HashDic = new Dictionary<string, string>();

        public void loadMiniVerFile()
        {
            LoadParam param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
            param.m_path = string.Format("{0}/{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathTablePath], MINIFILENAME);
            param.m_prefabName = MINIFILENAME;
            param.m_loaded = onloadedMini;
            Ctx.m_instance.m_resLoadMgr.loadData(param);
            Ctx.m_instance.m_poolSys.deleteObj(param);
        }

        // 加载一个表完成
        protected void onloadedMini(IDispatchObject resEvt)
        {
            IResItem m_res = resEvt as IResItem;                         // 类型转换
            byte[] textAsset = (m_res as DataResItem).getBytes();
            if (textAsset != null)
            {
                loadFormText(System.Text.Encoding.UTF8.GetString(textAsset), m_miniPath2HashDic);
            }
        }

        // 加载版本文件
        public void loadVerFile()
        {
            // 首先从可写目录下加载
            if(Ctx.m_instance.m_localFileSys.isFileExist(string.Format("{0}/{1}", Ctx.m_instance.m_localFileSys.getLocalWriteDir(), FILENAME)))
            {
                loadFormLocalWrite();
            }
            else    // 从可读目录下读取
            {
                loadFormRes();
            }
        }

        protected void loadFormLocalWrite()
        {
            string text = Ctx.m_instance.m_localFileSys.LoadFileText(Ctx.m_instance.m_localFileSys.getLocalWriteDir(), FILENAME);

            loadFormText(text, m_path2HashDic);
        }

        protected void loadFormRes()
        {
            LoadParam param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
            param.m_path = Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathTablePath] + FILENAMENOEXT;
            param.m_prefabName = FILENAMENOEXT;
            param.m_loaded = onloaded;
            param.m_loadNeedCoroutine = false;
            param.m_resNeedCoroutine = false;
            Ctx.m_instance.m_resLoadMgr.loadResources(param);
            Ctx.m_instance.m_poolSys.deleteObj(param);
        }

        // 加载一个表完成
        protected void onloaded(IDispatchObject resEvt)
        {
            IResItem m_res = resEvt as IResItem;                         // 类型转换
            TextAsset textAsset = m_res.getObject("") as TextAsset;
            if (textAsset != null)
            {
                loadFormText(textAsset.text, m_path2HashDic);
            }
        }

        protected void loadFormText(string text, Dictionary<string, string> dic)
        {
            string[] lineSplitStr = { "\r\n" };
            string[] equalSplitStr = { "=" };
            string[] lineList = text.Split(lineSplitStr, StringSplitOptions.RemoveEmptyEntries);
            int lineIdx = 0;
            string[] equalList = null;

            while (lineIdx < lineList.Length)
            {
                equalList = lineList[lineIdx].Split(equalSplitStr, StringSplitOptions.RemoveEmptyEntries);
                dic[equalList[0]] = equalList[1];
                ++lineIdx;
            }
        }
    }
}