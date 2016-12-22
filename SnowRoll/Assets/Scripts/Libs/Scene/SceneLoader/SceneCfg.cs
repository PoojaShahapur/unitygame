using System.Collections;

namespace SDK.Lib
{
    public class SceneCfg
    {
        protected TerrainCfg mTerrainCfg;
        protected ArrayList mSceneNodeCfgArr;

        protected float mWidth;
        protected float mDepth;

        public SceneCfg()
        {
            this.mTerrainCfg = new TerrainCfg();
            this.mSceneNodeCfgArr = new ArrayList();

            this.mWidth = 3000;
            this.mDepth = 3000;
        }

        public TerrainCfg terrainCfg
        {
            get
            {
                return mTerrainCfg;
            }
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
    }
}