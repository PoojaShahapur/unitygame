using Mono.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Security;

namespace SDK.Lib
{
    public class LangMgr
    {
        protected LangID m_langID = LangID.zh_CN;           // ��ǰ���ԣ�Ĭ�ϼ�������
        protected ArrayList m_nodeList = null;                   // ������ xml �� <t> �б�
        protected Dictionary<LangID, LangAttrItem> m_ID2FileName = new Dictionary<LangID, LangAttrItem>();  // ���Ե��ļ����ֵ�ӳ��
        protected ArrayList m_tmpEleList;         // ��ʱ��Ԫ���б�
        protected SecurityElement m_tmpEle;              // ��ʱ��Ԫ��
        protected bool m_isLoaded = false;                  // �����ļ��Ƿ����
        protected bool m_hasItem = false;

        // ���̷߳���
        protected MMutex m_loadMutex = new MMutex(false, "LangMgr_Mutex");

        public LangMgr()
        {
            m_ID2FileName[LangID.zh_CN] = new LangAttrItem();
            m_ID2FileName[LangID.zh_CN].m_filePath = Ctx.mInstance.mCfg.mPathLst[(int)ResPathType.ePathLangXml] + "zh_CN.xml";
        }

        public string getText(LangTypeId typeId, LangItemID itemIdx)
        {
            if (!m_isLoaded)
            {
                // ���̷߳��ʿ��ܻ�������
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
        //<!-- type ����һ������ item ����һ�˳��ǧ��Ҫ�ң���������  -->
        //<msg>
        //    <t>
        //        <i>���ݽṹ</i>
        //    </t>
        //</msg>
        public void loadXml()
        {
            if(!m_isLoaded)
            {
                m_isLoaded = true;
                LoadParam param = Ctx.mInstance.mPoolSys.newObject<LoadParam>();
                param.m_loadNeedCoroutine = false;
                param.m_resNeedCoroutine = false;
                param.setPath(m_ID2FileName[m_langID].m_filePath);
                param.m_loadEventHandle = onLoadEventHandle;
                Ctx.mInstance.mResLoadMgr.loadAsset(param);
                Ctx.mInstance.mPoolSys.deleteObj(param);
            }
        }

        // ����һ�������
        public void onLoadEventHandle(IDispatchObject dispObj)
        {
            ResItem res = dispObj as ResItem;
            //Ctx.mInstance.mLogSys.debugLog_1(LangItemID.eItem0, res.GetPath());    // ����ִ�е�ʱ�� m_isLoaded ���ü��ر�־������ m_nodeList ��û�г�ʼ��
            Ctx.mInstance.mLogSys.log("local xml loaded");

            string text = res.getText(m_ID2FileName[m_langID].m_filePath);
            if (text != null)
            {
                SecurityParser SP = new SecurityParser();
                SP.LoadXml(text);
                SecurityElement SE = SP.ToXml();
                m_nodeList = SE.Children;
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