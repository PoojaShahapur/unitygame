using UnityEngine;
using UnityEngine.UI;

namespace SDK.Lib
{
    public class AuxInputField : AuxWindow
    {
        protected InputField mInputField;     // 输入

        public AuxInputField(GameObject go = null, BtnStyleID styleId = BtnStyleID.eBSID_None)
        {
            this.mSelfGo = go;
            this.updateCom();
        }

        public AuxInputField(GameObject pntNode, string path, BtnStyleID styleId = BtnStyleID.eBSID_None)
        {
            this.mSelfGo = UtilApi.TransFindChildByPObjAndPath(pntNode, path);
            this.mInputField = UtilApi.getComByP<InputField>(pntNode, path);
        }

        public void setSelfGo(GameObject pntNode, string path)
        {
            this.mSelfGo = UtilApi.TransFindChildByPObjAndPath(pntNode, path);
            this.updateCom();
        }

        public void updateCom()
        {
            if(null != this.mSelfGo)
            {
                this.mInputField = UtilApi.getComByP<InputField>(this.mSelfGo);
            }
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

        public string getText()
        {
            return this.mInputField.text;
        }

        public void setText(string value)
        {
            this.mInputField.text = value;
        }

        public Color getColor()
        {
            return this.mInputField.textComponent.color;
        }

        public void setColor(Color value)
        {
            this.mInputField.textComponent.color = value;
        }
    }
}