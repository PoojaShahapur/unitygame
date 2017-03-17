using Mono.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Security;

namespace SDK.Lib
{
    public class LangMgr
    {
        protected LangID mLangID = LangID.zh_CN;                    // ��ǰ���ԣ�Ĭ�ϼ�������
        protected ArrayList mNodeList = null;                       // ������ xml �� <t> �б�
        protected MDictionary<LangID, LangAttrItem> mId2FileName;    // ���Ե��ļ����ֵ�ӳ��
        protected ArrayList mTmpEleList;                            // ��ʱ��Ԫ���б�
        protected SecurityElement mTmpEle;                         // ��ʱ��Ԫ��
        protected bool mIsLoaded = false;                          // �����ļ��Ƿ����
        protected bool mHasItem = false;

        // ���̷߳���
        protected MMutex mLoadMutex;

        public LangMgr()
        {
            mId2FileName = new MDictionary<LangID, LangAttrItem>();
            mLoadMutex = new MMutex(false, "LangMgr_Mutex");

            mId2FileName[LangID.zh_CN] = new LangAttrItem();
            mId2FileName[LangID.zh_CN].m_filePath = Ctx.mInstance.mCfg.mPathLst[(int)ResPathType.ePathLangXml] + "zh_CN.xml";
        }

        public string getText(LangTypeId typeId, LangItemID itemIdx)
        {
            if (!mIsLoaded)
            {
                // ���̷߳��ʿ��ܻ�������
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
        //<!-- type ����һ������ item ����һ�˳��ǧ��Ҫ�ң���������  -->
        //<msg>
        //    <t>
        //        <i>���ݽṹ</i>
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

        // ����һ�������
        public void onLoadEventHandle(IDispatchObject dispObj)
        {
            ResItem res = dispObj as ResItem;
            //Ctx.mInstance.mLogSys.debugLog_1(LangItemID.eItem0, res.GetPath());    // ����ִ�е�ʱ�� mIsLoaded ���ü��ر�־������ mNodeList ��û�г�ʼ��
            
            string text = res.getText(mId2FileName[mLangID].m_filePath);
            if (text != null)
            {
                SecurityParser SP = new SecurityParser();
                SP.LoadXml(text);
                SecurityElement SE = SP.ToXml();
                mNodeList = SE.Children;
            }

            // ж����Դ
            Ctx.mInstance.mResLoadMgr.unload(res.getResUniqueId(), onLoadEventHandle);
        }

        public void onFailed(IDispatchObject dispObj)
        {
            ResItem res = dispObj as ResItem;
            Ctx.mInstance.mLogSys.debugLog_1(LangItemID.eItem1, res.getLoadPath());

            // ж����Դ
            Ctx.mInstance.mResLoadMgr.unload(res.getResUniqueId(), onLoadEventHandle);
        }
    }
}