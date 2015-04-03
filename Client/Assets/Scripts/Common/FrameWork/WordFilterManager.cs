﻿using SDK.Lib;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace SDK.Common
{
    public class WordFilterManager
    {
        protected string[] m_filterArr;

        public void loadFile()
        {
            string name = "forbidWords";
            LoadParam param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
            param.m_path = Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathTablePath] + name;
            param.m_prefabName = name;
            param.m_loaded = onloaded;
            param.m_loadNeedCoroutine = false;
            param.m_resNeedCoroutine = false;
            Ctx.m_instance.m_resLoadMgr.loadResources(param);
            Ctx.m_instance.m_poolSys.deleteObj(param);
        }

        // 加载一个表完成
        public void onloaded(IDispatchObject resEvt)
        {
            IResItem m_res = resEvt as IResItem;                         // 类型转换
            TextAsset textAsset = m_res.getObject("") as TextAsset;
            
            if (textAsset != null)
            {
                string[] lineSplitStr = {"\r\n"};
                string[] tabSplitStr = { "\t" };
                string[] lineList = textAsset.text.Split(lineSplitStr, StringSplitOptions.RemoveEmptyEntries);
                int lineIdx = 0;
                string[] tabList = null;

                m_filterArr = new string[lineList.Length];

                while (lineIdx < lineList.Length)
                {
                    tabList = lineList[lineIdx].Split(tabSplitStr, StringSplitOptions.RemoveEmptyEntries);
                    m_filterArr[lineIdx] = tabList[2];
                    ++lineIdx;
                }
            }
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
    }
}