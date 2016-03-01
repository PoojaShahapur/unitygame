namespace SDK.Lib
{
    public class TwoDSceneTile
    {
        protected MTwoDAxisAlignedBox m_box;
        protected bool m_bInit;      // 是否初始化
        protected int m_tileIndex;  // 当前 Tile 索引
        protected MList<SceneEntityBase> m_entityList;
        protected bool m_isVisible;     // 当前 Tile 是否可见

        public void init()
        {

        }

        // 显示的时候调用
        public void show()
        {
            if (!m_isVisible)
            {
                m_isVisible = true;
                if (!m_bInit)
                {
                    init();
                }
            }
        }

        // 隐藏的时候调用
        public void hide()
        {

        }

        public void setTileIdx(int idx)
        {
            m_tileIndex = idx;
        }

        public int getTileIdx()
        {
            return m_tileIndex;
        }

        public void addToTile(SceneEntityBase entity)
        {
            if (m_entityList.IndexOf(entity) == -1)
            {
                m_entityList.Add(entity);   // 检查是否存在
            }
        }

        public void removeFromTile(SceneEntityBase entity)
        {
            m_entityList.Remove(entity);
        }
    }
}