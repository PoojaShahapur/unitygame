using SDK.Common;
using System.Collections.Generic;

namespace SDK.Lib
{
    public class UIAttrs
    {
        public Dictionary<UIFormID, UIAttrItem> m_dicAttr = new Dictionary<UIFormID,UIAttrItem>();

        public UIAttrs()
        {
            m_dicAttr = new Dictionary<UIFormID, UIAttrItem>();
            m_dicAttr[UIFormID.UIBackPack] = new UIAttrItem();
            m_dicAttr[UIFormID.UIBackPack].m_LayerID = UILayerID.FirstLayer;
            m_dicAttr[UIFormID.UIBackPack].m_resPath = Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI] + "UIScrollForm" + ".unity3d";
            m_dicAttr[UIFormID.UIBackPack].m_prefabName = "UIScrollForm";
        }

        public string getPath(UIFormID id)
        {
            if (m_dicAttr.ContainsKey(id))
            {
                string ret = m_dicAttr[id].m_resPath;
                ret = Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI] + ret + ".unity3d";
                return ret;
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
                    if (m_dicAttr[id].m_resPath == resPath)
                    {
                        return id;
                    }
                }
                else if (ResPathType.ePathCodePath == pathType)
                {
                    if (m_dicAttr[id].m_logicPath == resPath)
                    {
                        return id;
                    }
                }
            }

            return (UIFormID)0;       // 默认返回最大值
        }
    }
}