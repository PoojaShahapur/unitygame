using SDK.Lib;
using System.Collections.Generic;

namespace SDK.Common
{
    public class UIAttrs
    {
        public Dictionary<UIFormID, UIAttrItemBase> m_dicAttr = new Dictionary<UIFormID, UIAttrItemBase>();

        public UIAttrs()
        {
            // ****************** 第二层开始 ***********************
            m_dicAttr = new Dictionary<UIFormID, UIAttrItemBase>();
            m_dicAttr[UIFormID.eUIPack] = new UIAttrItem();
            (m_dicAttr[UIFormID.eUIPack] as UIAttrItem).m_LayerID = UILayerID.SecondLayer;
            m_dicAttr[UIFormID.eUIPack].m_widgetPath = string.Format("{0}{1}/{2}{3}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI], "UIPack", "UIPack", ".prefab");

            m_dicAttr[UIFormID.eUILogin] = new UIAttrItem();
            (m_dicAttr[UIFormID.eUILogin] as UIAttrItem).m_LayerID = UILayerID.SecondLayer;
            m_dicAttr[UIFormID.eUILogin].m_widgetPath = string.Format("{0}{1}/{2}{3}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI], "UILogin", "UILogin", ".prefab");

            m_dicAttr[UIFormID.eUIHeroSelect] = new UIAttrItem();
            (m_dicAttr[UIFormID.eUIHeroSelect] as UIAttrItem).m_LayerID = UILayerID.SecondLayer;
            m_dicAttr[UIFormID.eUIHeroSelect].m_widgetPath = string.Format("{0}{1}/{2}{3}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI], "UIHeroSelect", "UIHeroSelect", ".prefab");

            m_dicAttr[UIFormID.eUIBlurBg] = new UIAttrItem();
            (m_dicAttr[UIFormID.eUIBlurBg] as UIAttrItem).m_LayerID = UILayerID.BtmLayer;
            m_dicAttr[UIFormID.eUIBlurBg].m_widgetPath = string.Format("{0}{1}/{2}{3}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI], "UIBlurBg", "UIBlurBg", ".prefab");

            m_dicAttr[UIFormID.eUITest] = new UIAttrItem();
            (m_dicAttr[UIFormID.eUITest] as UIAttrItem).m_LayerID = UILayerID.TopLayer;
            m_dicAttr[UIFormID.eUITest].m_widgetPath = string.Format("{0}{1}/{2}{3}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI], "UITest", "UITest", ".prefab");

            m_dicAttr[UIFormID.eUIDZ] = new UIAttrItem();
            (m_dicAttr[UIFormID.eUIDZ] as UIAttrItem).m_LayerID = UILayerID.SecondLayer;
            m_dicAttr[UIFormID.eUIDZ].m_widgetPath = string.Format("{0}{1}/{2}{3}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI], "UIDZ", "UIDZ", ".prefab");

            m_dicAttr[UIFormID.eUIExtraOp] = new UIAttrItem();
            (m_dicAttr[UIFormID.eUIExtraOp] as UIAttrItem).m_LayerID = UILayerID.SecondLayer;
            m_dicAttr[UIFormID.eUIExtraOp].m_widgetPath = string.Format("{0}{1}/{2}{3}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI], "UIExtraOp", "UIExtraOp", ".prefab");

            m_dicAttr[UIFormID.eUIChat] = new UIAttrItem();
            (m_dicAttr[UIFormID.eUIChat] as UIAttrItem).m_LayerID = UILayerID.SecondLayer;
            m_dicAttr[UIFormID.eUIChat].m_widgetPath = string.Format("{0}{1}/{2}{3}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI], "UIChat", "UIChat", ".prefab");

            m_dicAttr[UIFormID.eUIJobSelect] = new UIAttrItem();
            (m_dicAttr[UIFormID.eUIJobSelect] as UIAttrItem).m_LayerID = UILayerID.SecondLayer;
            m_dicAttr[UIFormID.eUIJobSelect].m_widgetPath = string.Format("{0}{1}/{2}{3}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI], "UIJobSelect", "UIJobSelect", ".prefab");

            m_dicAttr[UIFormID.eUITuJian] = new UIAttrItem();
            (m_dicAttr[UIFormID.eUITuJian] as UIAttrItem).m_LayerID = UILayerID.SecondLayer;
            m_dicAttr[UIFormID.eUITuJian].m_widgetPath = string.Format("{0}{1}/{2}{3}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI], "UITuJian", "UITuJian", ".prefab");

            m_dicAttr[UIFormID.eUIMain] = new UIAttrItem();
            (m_dicAttr[UIFormID.eUIMain] as UIAttrItem).m_LayerID = UILayerID.SecondLayer;
            m_dicAttr[UIFormID.eUIMain].m_widgetPath = string.Format("{0}{1}/{2}{3}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI], "UIMain", "UIMain", ".prefab");

            m_dicAttr[UIFormID.eUIHero] = new UIAttrItem();
            (m_dicAttr[UIFormID.eUIHero] as UIAttrItem).m_LayerID = UILayerID.SecondLayer;
            m_dicAttr[UIFormID.eUIHero].m_widgetPath = string.Format("{0}{1}/{2}{3}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI], "UIHero", "UIHero", ".prefab");

            m_dicAttr[UIFormID.eUIOpenPack] = new UIAttrItem();
            (m_dicAttr[UIFormID.eUIOpenPack] as UIAttrItem).m_LayerID = UILayerID.SecondLayer;
            m_dicAttr[UIFormID.eUIOpenPack].m_widgetPath = string.Format("{0}{1}/{2}{3}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI], "UIOpenPack", "UIOpenPack", ".prefab");

            // ****************** 第二层结束 ***********************

            // ****************** 第四层开始 ***********************
            m_dicAttr[UIFormID.eUIInfo] = new UIAttrItem();
            (m_dicAttr[UIFormID.eUIInfo] as UIAttrItem).m_LayerID = UILayerID.ForthLayer;
            m_dicAttr[UIFormID.eUIInfo].m_widgetPath = string.Format("{0}{1}/{2}{3}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI], "UIInfo", "UIInfo", ".prefab");
            // ****************** 第四层结束 ***********************

            // ****************** 顶层开始 ***********************
            m_dicAttr[UIFormID.eUILogicTest] = new UIAttrItem();
            (m_dicAttr[UIFormID.eUILogicTest] as UIAttrItem).m_LayerID = UILayerID.TopLayer;
            m_dicAttr[UIFormID.eUILogicTest].m_widgetPath = string.Format("{0}{1}/{2}{3}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI], "UILogicTest", "UILogicTest", ".prefab");
            // ****************** 顶层结束 ***********************
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