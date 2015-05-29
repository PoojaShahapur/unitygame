using BehaviorLibrary.Components;
using Mono.Xml;
using SDK.Common;
using SDK.Lib;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

namespace BehaviorLibrary
{
    /**
     * @brief 行为树管理器，存放所有的行为树
     */
    public class BehaviorTreeMgr
    {
        protected BTFactory m_BTFactory = new BTFactory();
        protected Dictionary<string, BehaviorTree> m_id2BTDic = new Dictionary<string,BehaviorTree>();

        public BehaviorTree getBTByID(string id)
        {
            if(m_id2BTDic.ContainsKey(id))
            {
                return m_id2BTDic[id];
            }

            return null;
        }

        public void loadBT()
        {
            LoadParam param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
            param.m_path = Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathAIPath] + "AI";
            param.m_loadEventHandle = onLoadEventHandle;
            Ctx.m_instance.m_resLoadMgr.loadBundle(param);
            Ctx.m_instance.m_poolSys.deleteObj(param);
        }

        public void onLoadEventHandle(IDispatchObject dispObj)
        {
            ResItem res = dispObj as ResItem;
            if (res.refCountResLoadResultNotify.resLoadState.hasSuccessLoaded())
            {
                Ctx.m_instance.m_logSys.debugLog_1(LangItemID.eItem0, res.GetPath());

                string text = res.getText("TestAi");
                parseXml(text);
                text = res.getText("Test2Ai");
                parseXml(text);
            }
            else if (res.refCountResLoadResultNotify.resLoadState.hasFailed())
            {
                Ctx.m_instance.m_logSys.debugLog_1(LangItemID.eItem1, res.GetPath());
            }

            // 卸载资源
            Ctx.m_instance.m_resLoadMgr.unload(res.GetPath(), onLoadEventHandle);
        }

        protected void parseXml(string xmlStr)
        {
            SecurityParser xmlDoc = new SecurityParser();
            xmlDoc.LoadXml(xmlStr);

            SecurityElement rootNode = xmlDoc.ToXml();
            ArrayList behaviorTemplateNode = rootNode.Children;
            ArrayList behaviorTreeXmlList = null;
            SecurityElement xmlElemTpl;
            SecurityElement xmlElemBT;

            foreach (SecurityElement node in behaviorTemplateNode)  // 树列表，包括树和其它信息
            {
                xmlElemTpl = node;
                //xmlElemBT = xmlElemTpl.SelectSingleNode("BehaviorTree") as XmlElement;
                behaviorTreeXmlList = xmlElemTpl.Children;
                foreach (SecurityElement nodetree in behaviorTreeXmlList)
                {
                    xmlElemBT = nodetree;
                    m_id2BTDic[UtilApi.getXmlAttrStr(xmlElemBT, "name")] = new BehaviorTree(new BTRoot());
                    m_BTFactory.parseXml(m_id2BTDic[UtilApi.getXmlAttrStr(xmlElemBT, "name")], xmlElemBT);
                }
            }
        }
    }
}