using UnityEngine;

namespace SDK.Lib
{
    public class Scene : IDispatchObject
    {
        protected SceneCfg m_sceneCfg;
        protected string m_file;
        protected MTerrain m_terrain;            // 地形

        public Scene()
        {
            m_sceneCfg = new SceneCfg();
        }

        public SceneCfg sceneCfg
        {
            get
            {
                return m_sceneCfg;
            }
        }

        public string file
        {
            get
            {
                return m_file;
            }
            set
            {
                m_file = value;
            }
        }

        public void createTerrain()
        {
            //m_terrain = new MTerrainMulti();
            //m_terrain.buildPage();        // 生成地形
            //m_terrain.buildQuadTree();    // 生成四叉树

            //Ctx.m_instance.m_camSys.setMCamera(Camera.main);
            //m_terrain.updateClip();
            Ctx.m_instance.mSceneNodeGraph.init();
            Ctx.m_instance.m_sceneManager.addUpdateTask();
            Ctx.m_instance.m_terrainBufferSys.loadSceneCfg("S1000");
            string terrainId = "";

            if (Ctx.m_instance.mTerrainGlobalOption.mIsReadFile)
            {
                Ctx.m_instance.m_terrainBufferSys.loadNeedRes();
            }

            int pageIdx = 0;
            int pageIdy = 0;
            while(pageIdy < Ctx.m_instance.mTerrainGlobalOption.mTerrainPageCount)
            {
                pageIdx = 0;
                while (pageIdx < Ctx.m_instance.mTerrainGlobalOption.mTerrainPageCount)
                {
                    terrainId = Ctx.m_instance.m_terrainBufferSys.getTerrainId(pageIdx, pageIdy);
                    if(string.IsNullOrEmpty(terrainId))
                    {
                        terrainId = "T1000";
                    }
                    Ctx.m_instance.m_terrainGroup.defineTerrain(pageIdx, pageIdy, terrainId);
                    Ctx.m_instance.m_terrainGroup.loadTerrain(pageIdx, pageIdy, true);
                    if (!Ctx.m_instance.mTerrainGlobalOption.mNeedCull)
                    {
                        Ctx.m_instance.m_terrainGroup.updateAABB(pageIdx, pageIdy);
                        Ctx.m_instance.m_terrainGroup.showTerrain(pageIdx, pageIdy);
                    }
                    if (Ctx.m_instance.mTerrainGlobalOption.mNeedSaveScene)
                    {
                        Ctx.m_instance.m_terrainGroup.serializeTerrain(pageIdx, pageIdy);
                    }
                    ++pageIdx;
                }
                ++pageIdy;
            }

            Ctx.m_instance.m_camSys.setLocalCamera(Camera.main);
            if(Ctx.m_instance.mTerrainGlobalOption.mNeedCull)
            {
                Ctx.m_instance.m_camSys.invalidCamera();
            }
        }

        public void updateClip()
        {
            Ctx.m_instance.m_terrainGroup.cullTerrain(0, 0, Ctx.m_instance.m_camSys.getLocalCamera());
        }

        public float getHeightAt(float x, float z)
        {
            if (m_terrain != null)
            {
                return m_terrain.getHeightAtWorldPosition(x, 0, z);
            }

            return 0;
        }
    }
}