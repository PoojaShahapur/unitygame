﻿using UnityEngine;

namespace SDK.Lib
{
    public class Scene : IDispatchObject
    {
        protected SceneCfg mSceneCfg;
        protected string mFile;
        protected MTerrain mTerrain;            // 地形

        public Scene()
        {
            mSceneCfg = new SceneCfg();
        }

        public SceneCfg sceneCfg
        {
            get
            {
                return mSceneCfg;
            }
        }

        public string file
        {
            get
            {
                return mFile;
            }
            set
            {
                mFile = value;
            }
        }

        public void createTerrain()
        {
            //mTerrain = new MTerrainMulti();
            //mTerrain.buildPage();        // 生成地形
            //mTerrain.buildQuadTree();    // 生成四叉树

            //Ctx.mInstance.mCamSys.setMCamera(Camera.main);
            //mTerrain.updateClip();
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
            if (mTerrain != null)
            {
                return mTerrain.getHeightAtWorldPosition(x, 0, z);
            }

            return 0;
        }
    }
}