using UnityEngine;
using UnityEngine.UI;

namespace SDK.Lib
{
    public class AuxInputField : AuxWindow
    {
        protected InputField mInputField;     // 输入

        public AuxInputField(GameObject pntNode, string path, BtnStyleID styleId = BtnStyleID.eBSID_None)
        {
            this.mSelfGo = UtilApi.TransFindChildByPObjAndPath(pntNode, path);
            this.mInputField = UtilApi.getComByP<InputField>(pntNode, path);
        }

        public string text
        {
            get
            {
                return this.mInputField.text;
            }
            set
            {
                this.mInputField.text = value;
            }
        }
    }
}