using UnityEngine;
using UnityEngine.UI;

namespace SDK.Lib
{
    public class AuxScrollbar : AuxComponent
    {
        protected Scrollbar m_scrollbar;       // 滚动条

        public AuxScrollbar(GameObject pntNode, string path, BtnStyleID styleId = BtnStyleID.eBSID_None)
        {
            m_selfGo = UtilApi.TransFindChildByPObjAndPath(pntNode, path);
            m_scrollbar = UtilApi.getComByP<Scrollbar>(pntNode, path);
        }

        public float value
        {
            get
            {
                return m_scrollbar.value;
            }
            set
            {
                m_scrollbar.value = 0;
            }
        }
    }
}