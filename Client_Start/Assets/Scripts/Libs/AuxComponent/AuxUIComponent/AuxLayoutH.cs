using UnityEngine;
using UnityEngine.UI;

namespace SDK.Lib
{
    /**
     * @brief 垂直布局
     */
    public class AuxLayoutH : AuxLayoutBase
    {
        protected int mColCount;       // 总共列数

        public AuxLayoutH()
        {
            this.mColCount = 0;
        }

        public int colCount
        {
            get
            {
                return this.mColCount;
            }
            set
            {
                this.mColCount = value;
            }
        }

        override public void addElem(GameObject go_, bool recalc = false)
        {
            ++this.mColCount;
            base.addElem(go_, recalc);
        }

        override public void removeAndDestroyElem(GameObject go_, bool recalc = false)
        {
            --this.mColCount;
            base.removeAndDestroyElem(go_, recalc);
        }

        override public void removeElem(GameObject go_, bool recalc = false)
        {
            --this.mColCount;
            base.removeElem(go_, recalc);
        }

        // 重新计算位置信息
        override public void reposition()
        {
            // 计算容器的大小
            RectTransform trans = this.mSelfGo.GetComponent<RectTransform>();
            HorizontalLayoutGroup layout = this.mSelfGo.GetComponent<HorizontalLayoutGroup>();
            trans.sizeDelta = new Vector2(this.mColCount * elemWidth + layout.spacing * (this.mColCount - 1) + layout.padding.left + layout.padding.right, elemHeight + layout.padding.top + layout.padding.bottom);
        }
    }
}