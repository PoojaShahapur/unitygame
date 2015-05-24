using SDK.Common;
using System;
using System.Collections.Generic;
using System.IO;

namespace SDK.Lib
{
    /**
     * @brief 打包资源项
     */
    public class ResListItem
    {
        public string m_origName;       // 原始的资源名字
        public string m_unity3dName;       // unity3d 文件名字
        public string m_pakName;       // Pak 文件名字
    }


    /**
     * @brief 打包系统
     */
    public class PakSys
    {
        public const string PAK_EXT = ".abc";       // 打包的文件扩展名字
        public const string FILE_LIST = "FileList.bytes";   // 文件列表文件名字

        protected Dictionary<string, ResListItem> m_path2PakDic = new Dictionary<string, ResListItem>();
        public Action m_pakCfgLoadDisp;

        public Dictionary<string, ResListItem> path2PakDic
        {
            get
            {
                return m_path2PakDic;
            }
        }

        virtual public void loadFile()
        {
            LoadParam param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
            LocalFileSys.modifyLoadParam(FILE_LIST, param);
            param.m_loadEventHandle = onLoadEventHandle;

            Ctx.m_instance.m_resLoadMgr.loadData(param);
            Ctx.m_instance.m_poolSys.deleteObj(param);
        }

        public void onLoadEventHandle(IDispatchObject dispObj)
        {
            ResItem res = dispObj as ResItem;
            if (res.refCountResLoadResultNotify.resLoadState.hasSuccessLoaded())
            {
                Ctx.m_instance.m_logSys.debugLog_1(LangItemID.eItem0, res.GetPath());

                byte[] textAsset = (res as DataResItem).getBytes();
                if (textAsset != null)
                {
                    loadFormText(System.Text.Encoding.UTF8.GetString(textAsset), m_path2PakDic);
                }

                // 卸载
                Ctx.m_instance.m_resLoadMgr.unload(res.GetPath(), onLoadEventHandle);

                if (m_pakCfgLoadDisp != null)
                {
                    m_pakCfgLoadDisp();
                }
            }
            else if (res.refCountResLoadResultNotify.resLoadState.hasFailed())
            {
                Ctx.m_instance.m_logSys.debugLog_1(LangItemID.eItem1, res.GetPath());
                // 卸载
                Ctx.m_instance.m_resLoadMgr.unload(res.GetPath(), onLoadEventHandle);

                if (m_pakCfgLoadDisp != null)
                {
                    m_pakCfgLoadDisp();
                }
            }
        }

        protected void loadFormText(string text, Dictionary<string, ResListItem> dic)
        {
            string[] lineSplitStr = { "\r\n" };
            string[] equalSplitStr = { "=" };
            string[] lineList = text.Split(lineSplitStr, StringSplitOptions.RemoveEmptyEntries);
            int lineIdx = 0;
            string[] equalList = null;

            ResListItem item;
            while (lineIdx < lineList.Length)
            {
                equalList = lineList[lineIdx].Split(equalSplitStr, StringSplitOptions.RemoveEmptyEntries);
                item = new ResListItem();
                item.m_origName = equalList[0];
                item.m_unity3dName = equalList[1];
                item.m_pakName = equalList[2];
                dic[equalList[0]] = item;
                ++lineIdx;
            }
        }

        // 获取当前资源所在的包文件名字
        public string getCurResPakPathByResPath(string resPath, LoadParam param)
        {
            //Ctx.m_instance.m_shareData.m_resInPakPath = resPath;
            //return Ctx.m_instance.m_shareData.m_resInPakPath;
#if PKG_RES_LOAD
            string retPath = resPath;

            if ("Module/AutoUpdate.prefab" == resPath)       // 自动更新模块更新还没有实现
            {
                param.m_resLoadType = ResLoadType.eStreamingAssets;
            }
            else
            {
                // 获取包的名字
                if (m_path2PakDic.ContainsKey(resPath))
                {
                    retPath = m_path2PakDic[resPath].m_pakName;
                }

                if(param != null)
                {
                    retPath = Ctx.m_instance.m_localFileSys.getAbsPathByRelPath(ref retPath, ref param.m_resLoadType);
                }
                else
                {
                    ResLoadType tmp = ResLoadType.eStreamingAssets;
                    retPath = Ctx.m_instance.m_localFileSys.getAbsPathByRelPath(ref retPath, ref tmp);
                }
            }
            return retPath;
#elif UnPKG_RES_LOAD
            if (param != null)
            {
                param.m_resLoadType = ResLoadType.eStreamingAssets;
            }
            return resPath;
#else
            return resPath;
#endif
        }
    }
}