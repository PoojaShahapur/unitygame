using UnityEngine;

namespace SDK.Lib
{
    public class AuxLayoutBase : AuxWindow
    {
        protected int mElemWidth;          // 元素宽度
        protected int mElemHeight;         // 元素高度

        public int elemWidth
        {
            get
            {
                return this.mElemWidth;
            }
            set
            {
                this.mElemWidth = value;
            }
        }

        public int elemHeight
        {
            get
            {
                return this.mElemHeight;
            }
            set
            {
                this.mElemHeight = value;
            }
        }

        public void hideLayout()
        {
            UtilApi.SetActive(pntGo, false);
        }

        public void showLayout()
        {
            UtilApi.SetActive(pntGo, true);
        }

        virtual public void addElem(GameObject go_, bool recalc = false)
        {
            UtilApi.SetParent(go_, this.mSelfGo, false);

            if (recalc)
            {
                reposition();
            }
        }

        virtual public void removeAndDestroyElem(GameObject go_, bool recalc = false)
        {
            UtilApi.Destroy(go_);
            if (recalc)
            {
                reposition();
            }
        }

        virtual public void removeElem(GameObject go_, bool recalc = false)
        {
            if (recalc)
            {
                reposition();
            }
        }

        virtual public void reposition()
        {

        }
    }
}