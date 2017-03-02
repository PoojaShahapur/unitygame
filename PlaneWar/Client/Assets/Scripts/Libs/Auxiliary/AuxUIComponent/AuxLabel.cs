using UnityEngine;
using UnityEngine.UI;

namespace SDK.Lib
{
    /**
     * @brief 辅助 Label
     */
    public class AuxLabel : AuxWindow
    {
        protected Text mText;
        protected LabelStyleBase mLabelStyle;

        protected bool mIsStrInvalid;
        protected string mStr;

        // 初始构造
        public AuxLabel(GameObject pntNode, string path, LabelStyleID styleId = LabelStyleID.eLSID_None)
        {
            this.mStr = "";
            this.mIsStrInvalid = false;

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
            this.mStr = "";
            this.mIsStrInvalid = false;

            this.mSelfGo = selfNode;
            this.mText = UtilApi.getComByP<Text>(selfNode);
        }

        public AuxLabel(LabelStyleID styleId = LabelStyleID.eLSID_None)
        {
            this.mStr = "";
            this.mIsStrInvalid = false;
            this.mSelfGo = null;
        }

        // 修改组件
        public void setSelfGo(GameObject pntNode, string path)
        {
            this.selfGo = UtilApi.TransFindChildByPObjAndPath(pntNode, path);
        }

        override protected void onSelfChanged()
        {
            this.mText = UtilApi.getComByP<Text>(this.mSelfGo);

            if (this.mIsStrInvalid)
            {
                this.mText.text = this.mStr;
            }
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

        public void setText(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (this.mStr != value)
                {
                    this.mStr = value;

                    if (this.mText != null)
                    {
                        this.mIsStrInvalid = false;
                        this.mText.text = value;
                    }
                    else
                    {
                        this.mIsStrInvalid = true;
                    }
                }
            }
        }

        public string getText()
        {
            if (this.mText != null)
            {
                return this.mText.text;
            }
            return "";
        }

        public void setColor(Color color)
        {
            this.mText.color = color;
        }

        public Color getColor()
        {
            return this.mText.color;
        }

        public void changeSize(float scale)
        {
            if (this.mText != null)
            {
                //this.mText.rectTransform.sizeDelta = new Vector2(this.mText.rectTransform.sizeDelta.x, this.mText.preferredHeight);
                this.mText.rectTransform.localScale = new Vector2(scale, scale);
            }
        }
    }
}