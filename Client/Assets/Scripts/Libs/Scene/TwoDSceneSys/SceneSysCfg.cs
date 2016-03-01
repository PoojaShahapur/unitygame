namespace SDK.Lib
{
    /**
     * @brief 当前场景的配置
     */
    public class SceneSysCfg
    {
        public int m_sceneWorldWidth; // 场景世界空间宽度
        public int m_sceneWorldDepth; // 场景世界空间高度

        public int m_scenePageWidth; // 场景页宽度
        public int m_scenePageDepth; // 场景页高度

        public int m_sceneTileWidth; // 场景Tile宽度
        public int m_sceneTileDepth; // 场景Tile高度

        public SceneSysCfg()
        {
            m_sceneWorldWidth = 3000;
            m_sceneWorldDepth = 1500;

            m_scenePageWidth = 300;
            m_scenePageDepth = 300;

            m_sceneTileWidth = 30;
            m_sceneTileDepth = 30;
        }

        public int getPageCountX()
        {
            int countX = 0;
            countX = (m_sceneWorldWidth + m_scenePageWidth - 1) / m_scenePageWidth;
            return countX;
        }

        public int getPageCountY()
        {
            int countY = 0;
            countY = (m_sceneWorldDepth + m_scenePageDepth - 1) / m_scenePageDepth;
            return countY;
        }

        public int getTileCountXPerPage()
        {
            return (m_scenePageWidth + m_sceneTileWidth - 1) / m_sceneTileWidth;
        }

        public int getTileCountYPerPage()
        {
            return (m_scenePageDepth + m_sceneTileDepth - 1) / m_sceneTileDepth;
        }

        public MPoint convScenePageIdx2XYIdx(int pageIdx)
        {
            MPoint ret = new MPoint();
            ret.x = pageIdx % getPageCountX();
            ret.y = pageIdx / getPageCountX();
            return ret;
        }

        public int convScenePageXYIdx2Idx(MPoint xyIdx)
        {
            int idx = 0;
            idx = (int)(xyIdx.y * getPageCountX() + xyIdx.x);
            return idx;
        }

        public MPoint convWorldXYPos2ScenePageXYIdx(float x, float y)
        {
            MPoint ret = new MPoint();
            ret.x = (int)(x / m_scenePageWidth);
            ret.y = (int)(y / m_scenePageDepth);
            return ret;
        }

        // 转换世界坐标到场景索引
        public int convWorldXYPos2PageIdx(float x, float y)
        {
            MPoint ret = convWorldXYPos2ScenePageXYIdx(x, y);
            int idx = convScenePageXYIdx2Idx(ret);
            return idx;
        }

        public MPoint convWorldXYPos2PageXYPos(float x, float y)
        {
            MPoint ret = new MPoint();
            MPoint pageXYIdx = convWorldXYPos2ScenePageXYIdx(x, y);

            ret.x = (int)(x - pageXYIdx.x * m_scenePageWidth);
            ret.y = (int)(y - pageXYIdx.y * m_scenePageDepth);
            return ret;
        }

        public int convPageXYPos2TileIdx(float x, float y)
        {
            return (int)((y / m_sceneTileDepth) * getTileCountXPerPage() + (x / m_sceneTileWidth));
        }

        public int convWorldXYPos2TileIdx(float x, float y)
        {
            MPoint pageXYPos = convWorldXYPos2PageXYPos(x, y);
            int tileIdx = convPageXYPos2TileIdx(pageXYPos.x, pageXYPos.y);
            return tileIdx;
        }
    }
}