﻿using System.Collections.Generic;

namespace SDK.Common
{
    public class UIAttrs
    {
        public Dictionary<UIFormID, UIAttrItem> m_dicAttr = new Dictionary<UIFormID,UIAttrItem>();

        public UIAttrs()
        {
            m_dicAttr = new Dictionary<UIFormID, UIAttrItem>();
            m_dicAttr[UIFormID.UIPack] = new UIAttrItem();
            m_dicAttr[UIFormID.UIPack].m_LayerID = UILayerID.SecondLayer;
            m_dicAttr[UIFormID.UIPack].m_widgetPrefabName = "UIPack";
            m_dicAttr[UIFormID.UIPack].m_widgetPath = Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI] + m_dicAttr[UIFormID.UIPack].m_widgetPrefabName;

            m_dicAttr[UIFormID.UILogin] = new UIAttrItem();
            m_dicAttr[UIFormID.UILogin].m_LayerID = UILayerID.SecondLayer;
            m_dicAttr[UIFormID.UILogin].m_widgetPrefabName = "UILogin";
            m_dicAttr[UIFormID.UILogin].m_widgetPath = Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI] + m_dicAttr[UIFormID.UILogin].m_widgetPrefabName;

            m_dicAttr[UIFormID.UIHeroSelect] = new UIAttrItem();
            m_dicAttr[UIFormID.UIHeroSelect].m_LayerID = UILayerID.SecondLayer;
            m_dicAttr[UIFormID.UIHeroSelect].m_widgetPrefabName = "UIHeroSelect";
            m_dicAttr[UIFormID.UIHeroSelect].m_widgetPath = Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI] + m_dicAttr[UIFormID.UIHeroSelect].m_widgetPrefabName;

            m_dicAttr[UIFormID.UIBlurBg] = new UIAttrItem();
            m_dicAttr[UIFormID.UIBlurBg].m_LayerID = UILayerID.BtmLayer;
            m_dicAttr[UIFormID.UIBlurBg].m_widgetPrefabName = "UIBlurBg";
            m_dicAttr[UIFormID.UIBlurBg].m_widgetPath = Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI] + m_dicAttr[UIFormID.UIBlurBg].m_widgetPrefabName;
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