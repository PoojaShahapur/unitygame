using SDK.Common;
using System;
using System.Collections.Generic;
using System.IO;

namespace SDK.Lib
{
    /**
     * @brief 打包系统
     */
    public class PakSys
    {
        public const string PAK_EXT = ".abc";       // 打包的文件扩展名字

        protected Dictionary<string, string> m_path2PakDic = new Dictionary<string, string>();
        public Action m_pakCfgLoadDisp;

        public Dictionary<string, string> path2PakDic
        {
            get
            {
                return m_path2PakDic;
            }
        }

        virtual public void loadFile()
        {
            LoadParam param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
            LocalFileSys.modifyLoadParam("File2Dir.bytes", param);
            param.m_loaded = onLoaded;
            param.m_failed = onFailed;

            Ctx.m_instance.m_resLoadMgr.loadData(param);
            Ctx.m_instance.m_poolSys.deleteObj(param);
        }

        public void onLoaded(IDispatchObject resEvt)
        {
            IResItem m_res = resEvt as IResItem;                         // 类型转换
            Ctx.m_instance.m_logSys.debugLog_1(LangItemID.eItem0, m_res.GetPath());

            byte[] textAsset = (m_res as DataResItem).getBytes();
            if (textAsset != null)
            {
                loadFormText(System.Text.Encoding.UTF8.GetString(textAsset), m_path2PakDic);
            }

            // 卸载
            Ctx.m_instance.m_resLoadMgr.unload(m_res.GetPath());

            if(m_pakCfgLoadDisp != null)
            {
                m_pakCfgLoadDisp();
            }
        }

        public void onFailed(IDispatchObject resEvt)
        {
            IResItem m_res = resEvt as IResItem;                         // 类型转换
            Ctx.m_instance.m_logSys.debugLog_1(LangItemID.eItem1, m_res.GetPath());
            // 卸载
            Ctx.m_instance.m_resLoadMgr.unload(m_res.GetPath());

            if (m_pakCfgLoadDisp != null)
            {
                m_pakCfgLoadDisp();
            }
        }

        protected void loadFormText(string text, Dictionary<string, string> dic)
        {
            string[] lineSplitStr = { "\n" };
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
                    retPath = m_path2PakDic[resPath];
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