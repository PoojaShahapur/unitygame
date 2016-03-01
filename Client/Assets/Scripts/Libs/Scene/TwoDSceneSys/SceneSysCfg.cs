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
            m_sceneWorldWidth = 0;
            m_sceneWorldDepth = 0;

            m_scenePageWidth = 0;
            m_scenePageDepth = 0;

            m_sceneTileWidth = 0;
            m_sceneTileDepth = 0;
        }

        public int getPageCountX()
        {
            return (m_sceneWorldWidth + m_scenePageWidth - 1) / m_scenePageWidth;
        }

        public int getPageCountY()
        {
            return (m_sceneWorldDepth + m_scenePageDepth - 1) / m_scenePageDepth;
        }

        public int getTileCountXPerPage()
        {
            return (m_scenePageWidth + m_sceneTileWidth - 1) / m_sceneTileWidth;
        }

        public int getTileCountYPerPage()
        {
            return (m_scenePageDepth + m_sceneTileDepth - 1) / m_sceneTileDepth;
        }

        public MVector2 convScenePageIdx2XYIdx(int pageIdx)
        {
            MVector2 ret = new MVector2();
            ret.x = pageIdx % getPageCountX();
            ret.y = pageIdx / getPageCountX();
            return ret;
        }

        public int convScenePageXYIdx2Idx(MVector2 xyIdx)
        {
            int idx = 0;
            idx = (int)(xyIdx.y * getPageCountX() + xyIdx.x);
            return idx;
        }

        public MVector2 convWorldXYPos2ScenePageXYIdx(float x, float y)
        {
            MVector2 ret = new MVector2();
            ret.x = x + m_scenePageWidth - 1 / m_scenePageWidth;
            ret.y = y + m_scenePageDepth - 1 / m_scenePageDepth;
            return ret;
        }

        // 转换世界坐标到场景索引
        public int convWorldXYPos2PageIdx(float x, float y)
        {
            MVector2 ret = convWorldXYPos2ScenePageXYIdx(x, y);
            int idx = convScenePageXYIdx2Idx(ret);
            return idx;
        }

        public MVector2 convWorldXYPos2PageXYPos(float x, float y)
        {
            MVector2 ret = new MVector2();
            MVector2 pageXYIdx = convWorldXYPos2ScenePageXYIdx(x, y);

            ret.x = x - pageXYIdx.x * m_scenePageWidth;
            ret.y = y - pageXYIdx.y * m_scenePageDepth;
            return ret;
        }

        public int convPageXYPos2TileIdx(float x, float y)
        {
            return (int)(((y + m_sceneTileDepth - 1) / m_sceneTileDepth) * getTileCountXPerPage() + (x + m_sceneTileWidth - 1) / m_sceneTileWidth);
        }

        public int convWorldXYPos2TileIdx(float x, float y)
        {
            MVector2 pageXYPos = convWorldXYPos2PageXYPos(x, y);
            int tileIdx = convPageXYPos2TileIdx(pageXYPos.x, pageXYPos.y);
            return tileIdx;
        }
    }
}