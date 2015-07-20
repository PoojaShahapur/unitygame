using UnityEngine;
using UnityEngine.UI;

namespace SDK.Common
{
    public class AuxInputField : AuxComponent
    {
        protected InputField m_inputField;     // 输入

        public AuxInputField(GameObject pntNode, string path, BtnStyleID styleId = BtnStyleID.eBSID_None)
        {
            m_selfGo = UtilApi.TransFindChildByPObjAndPath(pntNode, path);
            m_inputField = UtilApi.getComByP<InputField>(pntNode, path);
        }

        public string text
        {
            get
            {
                return m_inputField.text;
            }
            set
            {
                m_inputField.text = value;
            }
        }
    }
}