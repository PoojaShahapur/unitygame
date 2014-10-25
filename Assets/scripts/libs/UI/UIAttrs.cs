using SDK.Common;
using System.Collections.Generic;

namespace SDK.Lib
{
    public class UIAttrs
    {
        public Dictionary<UIFormID, UIAttrItem> m_dicAttr;

        public void UIPathFunc()
        {
            m_dicAttr = new Dictionary<UIFormID, UIAttrItem>();
            m_dicAttr[UIFormID.UIBackPack] = new UIAttrItem();
            m_dicAttr[UIFormID.UIBackPack].m_LayerID = UILayerID.FirstLayer;
            m_dicAttr[UIFormID.UIBackPack].m_path = "UIBackPack";
        }

        public string getPath(UIFormID id)
        {
            if (m_dicAttr.ContainsKey(id))
            {
                string ret = m_dicAttr[id].m_path;
                ret = "asset/ui/" + ret + ".aaa";
                return ret;
            }

            return null;
        }
    }
}
