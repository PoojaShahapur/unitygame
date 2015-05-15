namespace Game.UI
{
    /**
     * @brief 每一页数据
     */
    public class PageInfo
    {
        public int m_curPageIdx;     // 当前显示的 Page 索引，下标从 0 开始
        protected int m_totalPage;     // 总共页数，从 1 开始，如果没有，就是 0
        public int m_cardCount;     // 当前页中总共卡牌数量

        public int totalPage
        {
            get
            {
                return m_totalPage;
            }
            set
            {
                m_totalPage = value;
            }
        }

        public int getTotalPageDesc()
        {
            if(m_totalPage == 0)
            {
                return 1;
            }

            return m_totalPage;
        }

        public bool canMovePreInCurTagPage()
        {
            return m_curPageIdx > 0;
        }

        public bool canMoveNextInCurTagPage()
        {
            return ((m_curPageIdx + 1) * (int)TuJianCardNumPerPage.eNum < m_cardCount);
        }
    }
}