using UnityEngine;
using UnityEngine.UI;

namespace SDK.Lib
{
    /**
     * @brief 垂直布局
     */
    public class AuxLayoutV : AuxLayoutBase
    {
        protected int mRowCount;       // 总共行数

        public AuxLayoutV()
        {
            this.mRowCount = 0;
        }

        public int rowCount
        {
            get
            {
                return this.mRowCount;
            }
            set
            {
                this.mRowCount = value;
            }
        }

        override public void addElem(GameObject go_, bool recalc = false)
        {
            ++this.mRowCount;
            base.addElem(go_, recalc);
        }

        override public void removeAndDestroyElem(GameObject go_, bool recalc = false)
        {
            --this.mRowCount;
            base.removeAndDestroyElem(go_, recalc);
        }

        override public void removeElem(GameObject go_, bool recalc = false)
        {
            --this.mRowCount;
            base.removeElem(go_, recalc);
        }

        // 重新计算位置信息
        override public void reposition()
        {
            // 计算容器的大小
            RectTransform trans = this.mSelfGo.GetComponent<RectTransform>();
            VerticalLayoutGroup layout = this.mSelfGo.GetComponent<VerticalLayoutGroup>();
            trans.sizeDelta = new Vector2(elemWidth + layout.padding.left + layout.padding.right, this.mRowCount * elemHeight + layout.spacing * (this.mRowCount - 1) + layout.padding.top + layout.padding.bottom);
        }
    }
}