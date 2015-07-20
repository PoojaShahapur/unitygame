using UnityEngine;
using UnityEngine.UI;

namespace SDK.Common
{
    /**
     * @brief 垂直布局
     */
    public class AuxLayoutV : AuxLayoutBase
    {
        protected int m_rowCount;       // 总共行数

        public AuxLayoutV()
        {
            m_rowCount = 0;
        }

        public int rowCount
        {
            get
            {
                return m_rowCount;
            }
            set
            {
                m_rowCount = value;
            }
        }

        override public void addElem(GameObject go_, bool recalc = false)
        {
            ++m_rowCount;
            base.addElem(go_, recalc);
        }

        override public void removeAndDestroyElem(GameObject go_, bool recalc = false)
        {
            --m_rowCount;
            base.removeAndDestroyElem(go_, recalc);
        }

        override public void removeElem(GameObject go_, bool recalc = false)
        {
            --m_rowCount;
            base.removeElem(go_, recalc);
        }

        // 重新计算位置信息
        override public void reposition()
        {
            // 计算容器的大小
            RectTransform trans = m_selfGo.GetComponent<RectTransform>();
            VerticalLayoutGroup layout = m_selfGo.GetComponent<VerticalLayoutGroup>();
            trans.sizeDelta = new Vector2(elemWidth + layout.padding.left + layout.padding.right, m_rowCount * elemHeight + layout.spacing * (m_rowCount - 1) + layout.padding.top + layout.padding.bottom);
        }
    }
}