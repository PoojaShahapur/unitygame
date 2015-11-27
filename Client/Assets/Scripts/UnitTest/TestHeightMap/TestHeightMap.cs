﻿using SDK.Lib;

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
            testTerrainPageMulti();
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
            terPage.buildPage();
        }

        public void testPrint()
        {
            HeightMapData heightMapData = new HeightMapData();
            heightMapData.loadHeightMap("Terrain/terrain.png");
            heightMapData.print();
        }

        protected void end()
        {

        }
    }
}