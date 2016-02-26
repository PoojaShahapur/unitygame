using System.Collections;

namespace SDK.Lib
{
    public class SceneCfg
    {
        protected TerrainCfg m_terrainCfg;
        protected ArrayList m_sceneNodeCfgArr;

        public SceneCfg()
        {
            m_terrainCfg = new TerrainCfg();
            m_sceneNodeCfgArr = new ArrayList();
        }

        public TerrainCfg terrainCfg
        {
            get
            {
                return m_terrainCfg;
            }
        }

        public void addSceneNode(SceneNodeCfg node)
        {
            m_sceneNodeCfgArr.Add(node);
        }
    }
}