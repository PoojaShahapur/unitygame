using System.Collections;

namespace SDK.Lib
{
    public class SceneCfg
    {
        protected TerrainCfg mTerrainCfg;
        protected ArrayList mSceneNodeCfgArr;

        protected float mWidth;
        protected float mDepth;
        protected float mHeight;

        public SceneCfg()
        {
            this.mTerrainCfg = new TerrainCfg();
            this.mSceneNodeCfgArr = new ArrayList();

            this.mWidth = 3000;
            this.mDepth = 3000;
            this.mHeight = 3000;
        }

        public TerrainCfg terrainCfg
        {
            get
            {
                return mTerrainCfg;
            }
        }

        public void initSize()
        {
            this.mWidth = Ctx.mInstance.mSnowBallCfg.mXmlSnowBallCfg.mXmlItemMap.mWidth;
            this.mDepth = Ctx.mInstance.mSnowBallCfg.mXmlSnowBallCfg.mXmlItemMap.mWidth;
            this.mHeight = Ctx.mInstance.mSnowBallCfg.mXmlSnowBallCfg.mXmlItemMap.mWidth;
        }

        public void dispose()
        {

        }

        public void addSceneNode(SceneNodeCfg node)
        {
            this.mSceneNodeCfgArr.Add(node);
        }

        public float getWidth()
        {
            return this.mWidth;
        }

        public float getDepth()
        {
            return this.mDepth;
        }

        public float getHeight()
        {
            return this.mHeight;
        }
    }
}