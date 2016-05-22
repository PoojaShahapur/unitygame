using UnityEngine;
using UnityEngine.UI;

namespace SDK.Lib
{
    /**
     * @brief 辅助 Label
     */
    public class AuxLabel : AuxWindow
    {
        protected Text m_text;
        protected LabelStyleBase m_labelStyle;

        // 初始构造
        public AuxLabel(GameObject pntNode, string path, LabelStyleID styleId = LabelStyleID.eLSID_None)
        {
            m_selfGo = UtilApi.TransFindChildByPObjAndPath(pntNode, path);
            m_text = UtilApi.getComByP<Text>(pntNode, path);
            m_labelStyle = Ctx.m_instance.m_widgetStyleMgr.GetWidgetStyle<LabelStyleBase>(WidgetStyleID.eWSID_Text, (int)styleId);
            if(m_labelStyle.needClearText())
            {
                m_text.text = "";
            }
        }

        public AuxLabel(GameObject selfNode, LabelStyleID styleId = LabelStyleID.eLSID_None)
        {
            m_selfGo = selfNode;
            m_text = UtilApi.getComByP<Text>(selfNode);
        }

        public AuxLabel(LabelStyleID styleId = LabelStyleID.eLSID_None)
        {
            
        }

        // 后期修改
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