using UnityEngine;
using UnityEngine.UI;

namespace SDK.Lib
{
    /**
     * @brief ���� Label
     */
    public class AuxLabel : AuxWindow
    {
        protected Text mText;
        protected LabelStyleBase mLabelStyle;

        // ��ʼ����
        public AuxLabel(GameObject pntNode, string path, LabelStyleID styleId = LabelStyleID.eLSID_None)
        {
            this.mSelfGo = UtilApi.TransFindChildByPObjAndPath(pntNode, path);
            this.mText = UtilApi.getComByP<Text>(pntNode, path);
            this.mLabelStyle = Ctx.mInstance.mWidgetStyleMgr.GetWidgetStyle<LabelStyleBase>(WidgetStyleID.eWSID_Text, (int)styleId);

            if(this.mLabelStyle.needClearText())
            {
                this.mText.text = "";
            }
        }

        public AuxLabel(GameObject selfNode, LabelStyleID styleId = LabelStyleID.eLSID_None)
        {
            this.mSelfGo = selfNode;
            this.mText = UtilApi.getComByP<Text>(selfNode);
        }

        public AuxLabel(LabelStyleID styleId = LabelStyleID.eLSID_None)
        {
            
        }

        // �����޸�
        public void setSelfGo(GameObject pntNode, string path)
        {
            this.mSelfGo = UtilApi.TransFindChildByPObjAndPath(pntNode, path);
            this.mText = UtilApi.getComByP<Text>(pntNode, path);
        }

        public string text
        {
            get
            {
                if (this.mText != null)
                {
                    return this.mText.text;
                }
                return "";
            }
            set
            {
                if (this.mText != null)
                {
                    this.mText.text = value;
                }
            }
        }

        public void changeSize()
        {
            this.mText.rectTransform.sizeDelta = new Vector2(this.mText.rectTransform.sizeDelta.x, this.mText.preferredHeight);
        }
    }
}