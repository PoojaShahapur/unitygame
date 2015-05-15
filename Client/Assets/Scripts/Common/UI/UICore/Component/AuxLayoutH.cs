using UnityEngine;
using UnityEngine.UI;
namespace SDK.Common
{
    /**
     * @brief 垂直布局
     */
    public class AuxLayoutH : AuxLayoutBase
    {
        protected int m_colCount;       // 总共列数

        public AuxLayoutH()
        {
            m_colCount = 0;
        }

        public int colCount
        {
            get
            {
                return m_colCount;
            }
            set
            {
                m_colCount = value;
            }
        }

        public void addElem(GameObject go_, bool recalc = false)
        {
            ++m_colCount;
            base.addElem(go_, recalc);
        }

        public void removeAndDestroyElem(GameObject go_, bool recalc = false)
        {
            --m_colCount;
            base.removeAndDestroyElem(go_, recalc);
        }

        // 重新计算位置信息
        public void reposition()
        {
            // 计算容器的大小
            RectTransform trans = pntGo.GetComponent<RectTransform>();
            HorizontalLayoutGroup layout = pntGo.GetComponent<HorizontalLayoutGroup>();
            trans.sizeDelta = new Vector2(m_colCount * elemWidth + layout.spacing * (m_colCount - 1) + layout.padding.left + layout.padding.right, elemHeight + layout.padding.top + layout.padding.bottom);
        }
    }
}