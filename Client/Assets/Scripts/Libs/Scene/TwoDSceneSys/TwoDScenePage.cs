namespace SDK.Lib
{
    public enum PageDirect
    {
        eLeft = 0,      // 左边
        eRight = 0,     // 右边
        eTop = 0,       // 顶边
        eBottom = 0,    // 底边
    }

    public class TwoDScenePage
    {
        protected MTwoDAxisAlignedBox m_box;
        protected MList<TwoDSceneTile> m_sceneTileList;
        protected bool m_bInit;      // 是否初始化
        protected MList<SceneEntityBase> m_entityList;
        protected MList<TwoDSceneTile> m_curVisibleTileList;    // 当前可见的Tile的列表
        protected MList<TwoDSceneTile> m_willVisibleTileList;    // 将要可见的Tile的列表
        protected int m_pageIndex;      // 当前 Page 的索引
        protected bool m_isVisible;     // 当前 Page 是否可见

        public TwoDScenePage()
        {
            m_box = new MTwoDAxisAlignedBox();
            m_sceneTileList = new MList<TwoDSceneTile>();
            m_entityList = new MList<SceneEntityBase>();
        }

        public void init()
        {
            int idx = 0;
            int idy = 0;

            int tileCountX = Ctx.m_instance.m_twoDSceneMgr.getSceneSysCfg().getTileCountXPerPage();
            int tileCountY = Ctx.m_instance.m_twoDSceneMgr.getSceneSysCfg().getTileCountYPerPage();

            while (idy < tileCountY)
            {
                idx = 0;
                while (idx < tileCountX)
                {
                    m_sceneTileList[idy * tileCountX + idx] = new TwoDSceneTile();
                    m_sceneTileList[idy * tileCountX + idx].setTileIdx(idy * tileCountX + idx);
                    m_sceneTileList[idy * tileCountX + idx].init();
                    ++idx;
                }
                ++idy;
            }
        }

        // 显示的时候调用
        public void show()
        {

        }

        // 隐藏的时候调用
        public void hide()
        {

        }

        public void setPageIdx(int idx)
        {
            m_pageIndex = idx;
        }

        public int getPageIdx()
        {
            return m_pageIndex;
        }

        public void addToPage(SceneEntityBase entity)
        {
            m_entityList.Add(entity);   // 检查是否重复添加

            if (m_bInit)
            {
                // 直接添加的具体 Tile 中去
                float x = entity.getWorldPosX();
                float y = entity.getWorldPosY();
                TwoDSceneTile tile = getSceneTile(x, y);
                tile.addToTile(entity);
            }
        }

        public TwoDSceneTile getSceneTile(float x, float y)
        {
            int tileIdx = Ctx.m_instance.m_twoDSceneMgr.getSceneSysCfg().convPageXYPos2TileIdx(x, y);
            return m_sceneTileList[tileIdx];
        }

        public void addWillVisibleTile(float x, float y)
        {
            int tileIdx = Ctx.m_instance.m_twoDSceneMgr.m_sceneSysCfg.convWorldXYPos2TileIdx(x, y);
            if(!isInWillVisibleList(tileIdx))
            {
                m_willVisibleTileList.Add(m_sceneTileList[tileIdx]);
            }
        }

        public void updateVisible()
        {
            int len = m_curVisibleTileList.Count();
            int index = len - 1;
            while(index >= 0)
            {
                if(isInWillVisibleList(m_curVisibleTileList[index].getTileIdx()))
                {
                    m_curVisibleTileList.RemoveAt(index);
                }
                else
                {
                    m_curVisibleTileList[index].hide();
                    m_curVisibleTileList.RemoveAt(index);
                }
                --index;
            }
            index = 0;
            while(index < m_willVisibleTileList.Count())
            {
                m_willVisibleTileList[index].show();
                m_curVisibleTileList.Add(m_willVisibleTileList[index]);
            }
            m_willVisibleTileList.Clear();
        }

        public bool isInWillVisibleList(int tileIdx)
        {
            foreach (TwoDSceneTile tile in m_willVisibleTileList.list())
            {
                if(tile.getTileIdx() == tileIdx)
                {
                    return true;
                }
            }

            return false;
        }
    }
}