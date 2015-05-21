using UnityEngine;

namespace SDK.Common
{
    public class AuxLayoutBase : AuxComponent
    {
        protected int m_elemWidth;          // 元素宽度
        protected int m_elemHeight;         // 元素高度
        protected GameObject m_contentGo;   // 放 Layout 中内容的 GameObject

        public int elemWidth
        {
            get
            {
                return m_elemWidth;
            }
            set
            {
                m_elemWidth = value;
            }
        }

        public int elemHeight
        {
            get
            {
                return m_elemHeight;
            }
            set
            {
                m_elemHeight = value;
            }
        }

        public GameObject contentGo
        {
            get
            {
                return m_contentGo;
            }
            set
            {
                m_contentGo = value;
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
            UtilApi.SetParent(go_, m_contentGo, false);

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