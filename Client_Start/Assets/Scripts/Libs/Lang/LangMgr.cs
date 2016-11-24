using Mono.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Security;

namespace SDK.Lib
{
    public class LangMgr
    {
        protected LangID m_langID = LangID.zh_CN;           // 当前语言，默认简体中文
        protected ArrayList m_nodeList = null;                   // 整个的 xml 中 <t> 列表
        protected Dictionary<LangID, LangAttrItem> mId2FileName = new Dictionary<LangID, LangAttrItem>();  // 语言到文件名字的映射
        protected ArrayList m_tmpEleList;         // 临时的元素列表
        protected SecurityElement m_tmpEle;              // 临时的元素
        protected bool m_isLoaded = false;                  // 语言文件是否加载
        protected bool m_hasItem = false;

        // 多线程访问
        protected MMutex m_loadMutex = new MMutex(false, "LangMgr_Mutex");

        public LangMgr()
        {
            mId2FileName[LangID.zh_CN] = new LangAttrItem();
            mId2FileName[LangID.zh_CN].m_filePath = Ctx.mInstance.mCfg.mPathLst[(int)ResPathType.ePathLangXml] + "zh_CN.xml";
        }

        public string getText(LangTypeId typeId, LangItemID itemIdx)
        {
            if (!m_isLoaded)
            {
                // 多线程访问可能会有问题
                using (MLock mlock = new MLock(m_loadMutex))
                {
                    loadXml();
                }
            }

            string textStr = "";
            m_hasItem = false;

            if(null != m_nodeList)
            {
                if ((int)typeId < m_nodeList.Count)
                {
                    m_tmpEleList = (m_nodeList[(int)typeId] as SecurityElement).Children;
                    if((int)itemIdx < m_tmpEleList.Count)
                    {
                        m_hasItem = true;
                        m_tmpEle = m_tmpEleList[(int)itemIdx] as SecurityElement;
                        //Ctx.mInstance.mShareData.m_retLangStr = m_tmpEle.InnerText;
                        textStr = m_tmpEle.Text;
                    }
                }
            }

            if (!m_hasItem)
            {
                //Ctx.mInstance.mShareData.m_retLangStr = "default string";
                textStr = "default string";
            }

            return textStr;
        }

        //<?xml version="1.0" encoding="utf-8"?>
        //<!-- type 就是一个功能 item 就是一项，顺序千万不要乱，否则都乱了  -->
        //<msg>
        //    <t>
        //        <i>数据结构</i>
        //    </t>
        //</msg>
        public void loadXml()
        {
            if(!m_isLoaded)
            {
                m_isLoaded = true;
                LoadParam param = Ctx.mInstance.mPoolSys.newObject<LoadParam>();
                param.mLoadNeedCoroutine = false;
                param.mResNeedCoroutine = false;
                param.setPath(mId2FileName[m_langID].m_filePath);
                param.mLoadEventHandle = onLoadEventHandle;
                Ctx.mInstance.mResLoadMgr.loadAsset(param);
                Ctx.mInstance.mPoolSys.deleteObj(param);
            }
        }

        // 加载一个表完成
        public void onLoadEventHandle(IDispatchObject dispObj)
        {
            ResItem res = dispObj as ResItem;
            //Ctx.mInstance.mLogSys.debugLog_1(LangItemID.eItem0, res.GetPath());    // 这行执行的时候 m_isLoaded 设置加载标志，但是 m_nodeList 还没有初始化
            Ctx.mInstance.mLogSys.log("local xml loaded");

            string text = res.getText(mId2FileName[m_langID].m_filePath);
            if (text != null)
            {
                SecurityParser SP = new SecurityParser();
                SP.LoadXml(text);
                SecurityElement SE = SP.ToXml();
                m_nodeList = SE.Children;
            }

            // 卸载资源
            Ctx.mInstance.mResLoadMgr.unload(res.getResUniqueId(), onLoadEventHandle);
        }

        public void onFailed(IDispatchObject dispObj)
        {
            ResItem res = dispObj as ResItem;
            Ctx.mInstance.mLogSys.debugLog_1(LangItemID.eItem1, res.getLoadPath());

            // 卸载资源
            Ctx.mInstance.mResLoadMgr.unload(res.getResUniqueId(), onLoadEventHandle);
        }
    }
}