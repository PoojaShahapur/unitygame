using SDK.Lib;
using System.Collections.Generic;

namespace SDK.Common
{
    public class UIAttrs
    {
        public Dictionary<UIFormID, UIAttrItemBase> m_dicAttr = new Dictionary<UIFormID, UIAttrItemBase>();

        public UIAttrs()
        {
            m_dicAttr = new Dictionary<UIFormID, UIAttrItemBase>();
            m_dicAttr[UIFormID.UIPack] = new UIAttrItem();
            (m_dicAttr[UIFormID.UIPack] as UIAttrItem).m_LayerID = UILayerID.SecondLayer;
            m_dicAttr[UIFormID.UIPack].m_widgetPath = string.Format("{0}{1}{2}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI], "UIPack", ".prefab");

            m_dicAttr[UIFormID.UILogin] = new UIAttrItem();
            (m_dicAttr[UIFormID.UILogin] as UIAttrItem).m_LayerID = UILayerID.SecondLayer;
            m_dicAttr[UIFormID.UILogin].m_widgetPath = string.Format("{0}{1}{2}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI], "UILogin", ".prefab");

            m_dicAttr[UIFormID.UIHeroSelect] = new UIAttrItem();
            (m_dicAttr[UIFormID.UIHeroSelect] as UIAttrItem).m_LayerID = UILayerID.SecondLayer;
            m_dicAttr[UIFormID.UIHeroSelect].m_widgetPath = string.Format("{0}{1}{2}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI], "UIHeroSelect", ".prefab");

            m_dicAttr[UIFormID.UIBlurBg] = new UIAttrItem();
            (m_dicAttr[UIFormID.UIBlurBg] as UIAttrItem).m_LayerID = UILayerID.BtmLayer;
            m_dicAttr[UIFormID.UIBlurBg].m_widgetPath = string.Format("{0}{1}{2}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI], "UIBlurBg", ".prefab");

            m_dicAttr[UIFormID.UITest] = new UIAttrItem();
            (m_dicAttr[UIFormID.UITest] as UIAttrItem).m_LayerID = UILayerID.TopLayer;
            m_dicAttr[UIFormID.UITest].m_widgetPath = string.Format("{0}{1}{2}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI], "UITest", ".prefab");

            m_dicAttr[UIFormID.UIDZ] = new UIAttrItem();
            (m_dicAttr[UIFormID.UIDZ] as UIAttrItem).m_LayerID = UILayerID.SecondLayer;
            m_dicAttr[UIFormID.UIDZ].m_widgetPath = string.Format("{0}{1}{2}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI], "UIDZ", ".prefab");

            m_dicAttr[UIFormID.UIExtraOp] = new UIAttrItem();
            (m_dicAttr[UIFormID.UIExtraOp] as UIAttrItem).m_LayerID = UILayerID.SecondLayer;
            m_dicAttr[UIFormID.UIExtraOp].m_widgetPath = string.Format("{0}{1}{2}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI], "UIExtraOp", ".prefab");

            m_dicAttr[UIFormID.UIChat] = new UIAttrItem();
            (m_dicAttr[UIFormID.UIChat] as UIAttrItem).m_LayerID = UILayerID.SecondLayer;
            m_dicAttr[UIFormID.UIChat].m_widgetPath = string.Format("{0}{1}{2}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI], "UIChat", ".prefab");

            m_dicAttr[UIFormID.UIInfo] = new UIAttrItem();
            (m_dicAttr[UIFormID.UIInfo] as UIAttrItem).m_LayerID = UILayerID.ForthLayer;
            m_dicAttr[UIFormID.UIInfo].m_widgetPath = string.Format("{0}{1}{2}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI], "UIInfo", ".prefab");

            m_dicAttr[UIFormID.UILogicTest] = new UIAttrItem();
            (m_dicAttr[UIFormID.UILogicTest] as UIAttrItem).m_LayerID = UILayerID.TopLayer;
            m_dicAttr[UIFormID.UILogicTest].m_widgetPath = string.Format("{0}{1}{2}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI], "UILogicTest", ".prefab");

            m_dicAttr[UIFormID.UIJobSelect] = new UISceneAttrItem();
            m_dicAttr[UIFormID.UIJobSelect].m_widgetPath = string.Format("{0}{1}{2}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI], "UIJobSelect", ".prefab");
        }

        public string getPath(UIFormID id)
        {
            if (m_dicAttr.ContainsKey(id))
            {
                return m_dicAttr[id].m_widgetPath;
                //ret = Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI] + ret;
                //return ret;
            }

            return null;
        }

        // 通过路径获取
        public UIFormID GetFormIDByPath(string resPath, ResPathType pathType)
        {
            foreach(UIFormID id in m_dicAttr.Keys)
            {
                if (ResPathType.ePathComUI == pathType)
                {
                    if (m_dicAttr[id].m_widgetPath == resPath)
                    {
                        return id;
                    }
                }
                else if (ResPathType.ePathCodePath == pathType)
                {
                    if (m_dicAttr[id].m_codePath == resPath)
                    {
                        return id;
                    }
                }
            }

            return (UIFormID)0;       // 默认返回最大值
        }
    }
}