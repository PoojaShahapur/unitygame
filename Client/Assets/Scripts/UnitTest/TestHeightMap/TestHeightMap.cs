﻿using SDK.Lib;
using UnityEngine;

namespace UnitTest
{
    /**
     * @brief 测试高度图
     */
    public class TestHeightMap
    {
        public void run()
        {
            //testHeightMap();
            //testTerrainPageOne();
            //testTerrainPageMulti();
            testProjectMatrix();
            //testPrint();
        }

        protected void testHeightMap()
        {
            HeightMapData heightMapData = new HeightMapData();
            heightMapData.loadHeightMap("Terrain/terrain.png");
            heightMapData.getWidth();
            heightMapData.getHeight();
            float _height = heightMapData.getPixHeight(200, 200);

            end();
        }

        public void testTerrainPageOne()
        {
            TerrainPageOne terPage = new TerrainPageOne();
            terPage.buildPage();
        }

        public void testTerrainPageMulti()
        {
            TerrainPageMulti terPage = new TerrainPageMulti();
            terPage.buildPage();        // 生成地形
            terPage.buildQuadTree();    // 生成四叉树
            Ctx.m_instance.m_camSys.getCamera().setCamera(Camera.main);

        }

        public void testPrint()
        {
            HeightMapData heightMapData = new HeightMapData();
            heightMapData.loadHeightMap("Terrain/terrain.png");
            heightMapData.print();
        }

        // 测试投影矩阵
        public void testProjectMatrix()
        {
            Ctx.m_instance.m_sceneSys.loadScene("TestHeightMap.unity", onResLoadScene);
        }

        public void onResLoadScene(Scene scene)
        {
            //checkCamera();
            Ctx.m_instance.m_uiMgr.loadAndShow((UIFormID)100);
            testTerrainPageMulti();
        }

        public void checkCamera()
        {
            MCamera camera = new MCamera(Camera.main);
        }

        // 测试四叉树
        protected void testQuadTree()
        {

        }

        protected void end()
        {

        }
    }
}