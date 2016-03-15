﻿using QuadTree;
using UnityEngine;

namespace SDK.Lib
{
    public class Scene
    {
        protected SceneCfg m_sceneCfg;
        protected string m_file;
        //protected QuadTree<Tile> m_quadTree;      // 地形四叉树
        protected ZoneSys m_zoneSys;
        protected MTerrain m_terrain;            // 地形

        public Scene()
        {
            m_sceneCfg = new SceneCfg();
            m_zoneSys = new ZoneSys();
        }

        public ZoneSys zoneSys
        {
            get
            {
                return m_zoneSys;
            }
            set
            {
                m_zoneSys = value;
            }
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

            Ctx.m_instance.m_terrainGroup.defineTerrain(0, 0);
            Ctx.m_instance.m_terrainGroup.loadTerrain(0, 0, true);
            Ctx.m_instance.m_terrainGroup.updateAABB(0, 0);
            //Ctx.m_instance.m_terrainGroup.showTerrain(0, 0);


            Ctx.m_instance.m_camSys.setLocalCamera(Camera.main);
            //Ctx.m_instance.m_terrainGroup.cullTerrain(0, 0, Ctx.m_instance.m_camSys.getMCamera());
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