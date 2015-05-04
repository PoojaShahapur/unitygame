using SDK.Lib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

namespace SDK.Common
{
    public class WordFilterManager
    {
        protected string[] m_filterArr;

        public void loadFile()
        {
            string name = "forbidWords.txt";
            LoadParam param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
            LocalFileSys.modifyLoadParam(Path.Combine(Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathWord], name), param);
            param.m_loaded = onLoaded;
            param.m_failed = onFailed;
            param.m_loadNeedCoroutine = false;
            param.m_resNeedCoroutine = false;
            Ctx.m_instance.m_resLoadMgr.loadResources(param);
            Ctx.m_instance.m_poolSys.deleteObj(param);
        }

        // 加载一个表完成
        public void onLoaded(IDispatchObject resEvt)
        {
            IResItem m_res = resEvt as IResItem;                         // 类型转换
            Ctx.m_instance.m_logSys.debugLog_1(LangItemID.eItem0, m_res.GetPath());
            string text = m_res.getText("");
            
            if (text != null)
            {
                string[] lineSplitStr = {"\r\n"};
                string[] tabSplitStr = { "\t" };
                string[] lineList = text.Split(lineSplitStr, StringSplitOptions.RemoveEmptyEntries);
                int lineIdx = 0;
                string[] tabList = null;

                m_filterArr = new string[lineList.Length];

                while (lineIdx < lineList.Length)
                {
                    tabList = lineList[lineIdx].Split(tabSplitStr, StringSplitOptions.RemoveEmptyEntries);
                    m_filterArr[lineIdx] = tabList[1];
                    ++lineIdx;
                }
            }

            // 卸载资源
            Ctx.m_instance.m_resLoadMgr.unload(m_res.GetPath());
        }

        public void onFailed(IDispatchObject resEvt)
        {
            IResItem m_res = resEvt as IResItem;                         // 类型转换
            Ctx.m_instance.m_logSys.debugLog_1(LangItemID.eItem1, m_res.GetPath());

            // 卸载资源
            Ctx.m_instance.m_resLoadMgr.unload(m_res.GetPath());
        }

        public bool doFilter(ref string str)
        {
            if (m_filterArr == null)
            {
                loadFile();
            }

            return doRegexFilter(ref str);
        }

        // 过滤字符串
        public bool doRegexFilter(ref string str)
        {
            foreach(string item in m_filterArr)
            {
                str = Regex.Replace(str, item, "*", RegexOptions.IgnoreCase);
            }
            return true;
        }

        public bool doMatchFilter(string str)
        {
            foreach (string item in m_filterArr)
            {
                str = Regex.Replace(str, item, "*", RegexOptions.IgnoreCase);
            }
            return true;
        }

        // 判断是否有过滤文字
        public bool IsMatch(string str)
        {
            if (m_filterArr == null)
            {
                loadFile();
            }

            foreach (string item in m_filterArr)
            {
                if(Regex.IsMatch(str, item, RegexOptions.IgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
    }
}