using Mono.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Security;

namespace SDK.Lib
{
    public class LangMgr
    {
        protected LangID mLangID = LangID.zh_CN;                    // 当前语言，默认简体中文
        protected ArrayList mNodeList = null;                       // 整个的 xml 中 <t> 列表
        protected Dictionary<LangID, LangAttrItem> mId2FileName;    // 语言到文件名字的映射
        protected ArrayList mTmpEleList;                            // 临时的元素列表
        protected SecurityElement mTmpEle;                         // 临时的元素
        protected bool mIsLoaded = false;                          // 语言文件是否加载
        protected bool mHasItem = false;

        // 多线程访问
        protected MMutex mLoadMutex;

        public LangMgr()
        {
            mId2FileName = new Dictionary<LangID, LangAttrItem>();
            mLoadMutex = new MMutex(false, "LangMgr_Mutex");

            mId2FileName[LangID.zh_CN] = new LangAttrItem();
            mId2FileName[LangID.zh_CN].m_filePath = Ctx.mInstance.mCfg.mPathLst[(int)ResPathType.ePathLangXml] + "zh_CN.xml";
        }

        public string getText(LangTypeId typeId, LangItemID itemIdx)
        {
            if (!mIsLoaded)
            {
                // 多线程访问可能会有问题
                using (MLock mlock = new MLock(mLoadMutex))
                {
                    loadXml();
                }
            }

            string textStr = "";
            mHasItem = false;

            if(null != mNodeList)
            {
                if ((int)typeId < mNodeList.Count)
                {
                    mTmpEleList = (mNodeList[(int)typeId] as SecurityElement).Children;
                    if((int)itemIdx < mTmpEleList.Count)
                    {
                        mHasItem = true;
                        mTmpEle = mTmpEleList[(int)itemIdx] as SecurityElement;
                        //Ctx.mInstance.mShareData.m_retLangStr = mTmpEle.InnerText;
                        textStr = mTmpEle.Text;
                    }
                }
            }

            if (!mHasItem)
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
            if(!mIsLoaded)
            {
                mIsLoaded = true;
                LoadParam param = Ctx.mInstance.mPoolSys.newObject<LoadParam>();
                param.mLoadNeedCoroutine = false;
                param.mResNeedCoroutine = false;
                param.setPath(mId2FileName[mLangID].m_filePath);
                param.mLoadEventHandle = onLoadEventHandle;
                Ctx.mInstance.mResLoadMgr.loadAsset(param);
                Ctx.mInstance.mPoolSys.deleteObj(param);
            }
        }

        // 加载一个表完成
        public void onLoadEventHandle(IDispatchObject dispObj)
        {
            ResItem res = dispObj as ResItem;
            //Ctx.mInstance.mLogSys.debugLog_1(LangItemID.eItem0, res.GetPath());    // 这行执行的时候 mIsLoaded 设置加载标志，但是 mNodeList 还没有初始化
            Ctx.mInstance.mLogSys.log("local xml loaded");

            string text = res.getText(mId2FileName[mLangID].m_filePath);
            if (text != null)
            {
                SecurityParser SP = new SecurityParser();
                SP.LoadXml(text);
                SecurityElement SE = SP.ToXml();
                mNodeList = SE.Children;
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