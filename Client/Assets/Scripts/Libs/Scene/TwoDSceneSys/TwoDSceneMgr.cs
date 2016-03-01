namespace SDK.Lib
{
    /**
     * @brief 所有的场景数据都是左下角是原点，右上角是最大值
     */
    public class TwoDSceneMgr
    {
        public SceneSysCfg m_sceneSysCfg;
        protected MTwoDAxisAlignedBox m_box;
        protected MList<TwoDScenePage> m_scenePageList;
        protected bool m_bInit;      // 是否初始化
        protected MList<TwoDScenePage> m_curVisiblePageList;    // 当前可见的页的列表
        protected MList<TwoDScenePage> m_willVisiblePageList;   // 将要可见的页的列表
        protected int m_camPageIdx;         // 相机所在的 Page 索引
        protected int m_camTileIdx;         // 相机所在的 Tile 索引

        public TwoDSceneMgr()
        {
            m_sceneSysCfg = new SceneSysCfg();
            m_box = new MTwoDAxisAlignedBox();
            m_scenePageList = new MList<TwoDScenePage>();

            m_curVisiblePageList = new MList<TwoDScenePage>();
            m_willVisiblePageList = new MList<TwoDScenePage>();
        }

        public SceneSysCfg getSceneSysCfg()
        {
            return m_sceneSysCfg;
        }

        public void init()
        {
            int idx = 0;
            int idy = 0;

            int pageCountX = m_sceneSysCfg.getPageCountX();
            int pageCountY = m_sceneSysCfg.getPageCountY();

            while (idy < pageCountY)
            {
                idx = 0;
                while (idx < pageCountX)
                {
                    m_scenePageList[idy * pageCountX + idx] = new TwoDScenePage();
                    m_scenePageList[idy * pageCountX + idx].setPageIdx(idy * pageCountX + idx);
                    m_scenePageList[idy * pageCountX + idx].init();
                    ++idx;
                }
                ++idy;
            }
        }

        // 添加到场景
        public void addToScene(SceneEntityBase entity)
        {
            float x = entity.getWorldPosX();
            float y = entity.getWorldPosY();

            TwoDScenePage page = getScenePage(x, y);
            page.addToPage(entity);
        }

        // 从场景移除
        public void removeFromScene()
        {

        }

        public TwoDScenePage getScenePage(float x, float y)
        {
            int pageIdx = m_sceneSysCfg.convWorldXYPos2PageIdx(x, y);
            if(pageIdx < m_scenePageList.Count())
            {
                return m_scenePageList[pageIdx];
            }

            return null;
        }

        public TwoDScenePage getSceneTileByIdAndDirect(int pageIdx, PageDirect dir)
        {
            int newPageIdx = 0;
            MVector2 pageXYIdx = m_sceneSysCfg.convScenePageIdx2XYIdx(pageIdx);
            if(PageDirect.eLeft == dir)
            {
                if(pageXYIdx.x > 0)
                {
                    return m_scenePageList[pageIdx - 1];
                }
            }
            else if (PageDirect.eRight == dir)
            {
                if (pageXYIdx.x < m_sceneSysCfg.getPageCountX())
                {
                    return m_scenePageList[pageIdx + 1];
                }
            }
            else if (PageDirect.eTop == dir)
            {
                if (pageXYIdx.y < m_sceneSysCfg.getPageCountY())
                {
                    pageXYIdx.y += 1;
                    newPageIdx = m_sceneSysCfg.convScenePageXYIdx2Idx(pageXYIdx);
                    return m_scenePageList[newPageIdx];
                }
            }
            else if (PageDirect.eBottom == dir)
            {
                if (pageXYIdx.y > 0)
                {
                    pageXYIdx.y -= 1;
                    newPageIdx = m_sceneSysCfg.convScenePageXYIdx2Idx(pageXYIdx);
                    return m_scenePageList[newPageIdx];
                }
            }
            return null;
        }

        // 更新场景管理器
        public void update(float x, float y)
        {
            int pageIdx = m_sceneSysCfg.convWorldXYPos2PageIdx(x, y);
            int tileIdx = m_sceneSysCfg.convWorldXYPos2TileIdx(x, y);

            if(m_camPageIdx != pageIdx && m_camTileIdx != tileIdx)
            {
                m_camPageIdx = pageIdx;
                m_camTileIdx = tileIdx;

                float lbx = x - m_sceneSysCfg.m_sceneTileWidth / 2;
                float lby = y - m_sceneSysCfg.m_sceneTileDepth / 2;

                int idx = 0;
                int idy = 0;
                while(idy < 3)
                {
                    idx = 0;
                    while(idx < 3)
                    {
                        addWillVisiblePage(lbx + idx * (m_sceneSysCfg.m_sceneTileWidth / 2), lby + idy * (m_sceneSysCfg.m_sceneTileDepth / 2));
                        ++idx;
                    }
                    ++idy;
                }

                updateVisible();
            }
        }

        public void addWillVisiblePage(float x, float y)
        {
            int pageIdx = m_sceneSysCfg.convWorldXYPos2PageIdx(x, y);
            if (!isInWillVisibleList(pageIdx))
            {
                m_willVisiblePageList.Add(m_scenePageList[pageIdx]);
            }
            m_scenePageList[pageIdx].addWillVisibleTile(x, y);
        }

        public void updateVisible()
        {
            int len = m_curVisiblePageList.Count();
            int index = len - 1;
            while (index >= 0)
            {
                if (isInWillVisibleList(m_curVisiblePageList[index].getPageIdx()))
                {
                    m_curVisiblePageList.RemoveAt(index);
                }
                else
                {
                    m_curVisiblePageList[index].hide();
                    m_curVisiblePageList.RemoveAt(index);
                }
                --index;
            }
            index = 0;
            while (index < m_willVisiblePageList.Count())
            {
                m_willVisiblePageList[index].updateVisible();
                m_willVisiblePageList[index].show();
                m_curVisiblePageList.Add(m_willVisiblePageList[index]);
            }
            m_willVisiblePageList.Clear();
        }

        public bool isInWillVisibleList(int tileIdx)
        {
            foreach (TwoDScenePage page in m_willVisiblePageList.list())
            {
                if (page.getPageIdx() == tileIdx)
                {
                    return true;
                }
            }

            return false;
        }
    }
}