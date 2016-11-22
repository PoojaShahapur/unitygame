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

            //Ctx.mInstance.mCamSys.setMCamera(Camera.main);
            //m_terrain.updateClip();
            Ctx.mInstance.mSceneNodeGraph.init();
            Ctx.mInstance.mSceneManager.addUpdateTask();
            Ctx.mInstance.mTerrainBufferSys.loadSceneCfg("S1000");
            string terrainId = "";

            if (Ctx.mInstance.mTerrainGlobalOption.mIsReadFile)
            {
                Ctx.mInstance.mTerrainBufferSys.loadNeedRes();
            }

            int pageIdx = 0;
            int pageIdy = 0;
            while(pageIdy < Ctx.mInstance.mTerrainGlobalOption.mTerrainPageCount)
            {
                pageIdx = 0;
                while (pageIdx < Ctx.mInstance.mTerrainGlobalOption.mTerrainPageCount)
                {
                    terrainId = Ctx.mInstance.mTerrainBufferSys.getTerrainId(pageIdx, pageIdy);
                    if(string.IsNullOrEmpty(terrainId))
                    {
                        terrainId = "T1000";
                    }
                    Ctx.mInstance.mTerrainGroup.defineTerrain(pageIdx, pageIdy, terrainId);
                    Ctx.mInstance.mTerrainGroup.loadTerrain(pageIdx, pageIdy, true);
                    if (!Ctx.mInstance.mTerrainGlobalOption.mNeedCull)
                    {
                        Ctx.mInstance.mTerrainGroup.updateAABB(pageIdx, pageIdy);
                        Ctx.mInstance.mTerrainGroup.showTerrain(pageIdx, pageIdy);
                    }
                    if (Ctx.mInstance.mTerrainGlobalOption.mNeedSaveScene)
                    {
                        Ctx.mInstance.mTerrainGroup.serializeTerrain(pageIdx, pageIdy);
                    }
                    ++pageIdx;
                }
                ++pageIdy;
            }

            Ctx.mInstance.mCamSys.setLocalCamera(Camera.main);
            if(Ctx.mInstance.mTerrainGlobalOption.mNeedCull)
            {
                Ctx.mInstance.mCamSys.invalidCamera();
            }
        }

        public void updateClip()
        {
            Ctx.mInstance.mTerrainGroup.cullTerrain(0, 0, Ctx.mInstance.mCamSys.getLocalCamera());
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