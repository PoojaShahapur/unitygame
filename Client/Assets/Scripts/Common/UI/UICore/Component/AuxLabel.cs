using UnityEngine;
using UnityEngine.UI;

namespace SDK.Common
{
    /**
     * @brief ¸¨Öú Label
     */
    public class AuxLabel : AuxComponent
    {
        protected Text m_text;

        public AuxLabel(GameObject pntNode, string path, LabelStyleID styleId = LabelStyleID.eLSID_None)
        {
            m_selfGo = UtilApi.TransFindChildByPObjAndPath(pntNode, path);
            m_text = UtilApi.getComByP<Text>(pntNode, path);
        }

        public AuxLabel(GameObject selfNode, LabelStyleID styleId = LabelStyleID.eLSID_None)
        {
            m_selfGo = selfNode;
            m_text = UtilApi.getComByP<Text>(selfNode);
        }

        public AuxLabel(LabelStyleID styleId = LabelStyleID.eLSID_None)
        {
            
        }

        public void setSelfGo(GameObject pntNode, string path)
        {
            m_selfGo = UtilApi.TransFindChildByPObjAndPath(pntNode, path);
            m_text = UtilApi.getComByP<Text>(pntNode, path);
        }

        public string text
        {
            get
            {
                if (m_text != null)
                {
                    return m_text.text;
                }
                return "";
            }
            set
            {
                if (m_text != null)
                {
                    m_text.text = value;
                }
            }
        }

        public void changeSize()
        {
            m_text.rectTransform.sizeDelta = new Vector2(m_text.rectTransform.sizeDelta.x, m_text.preferredHeight);
        }
    }
}