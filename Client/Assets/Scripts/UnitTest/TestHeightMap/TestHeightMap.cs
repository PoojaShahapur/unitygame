using SDK.Lib;
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
            MTerrainOne terPage = new MTerrainOne();
            terPage.buildPage();
        }

        public void testTerrainPageMulti()
        {
            MTerrainMulti terPage = new MTerrainMulti();
            terPage.buildPage();        // 生成地形
            terPage.buildQuadTree();    // 生成四叉树
            Ctx.m_instance.m_camSys.setMCamera(Camera.main);
            terPage.updateClip();
        }

        // 代码整理，将地形放到对应的场景里面了
        public void testSceneTerrain()
        {
            Ctx.m_instance.m_sceneSys.createTerrain();

            // 操作摄像机
            GameObject camera = UtilApi.GoFindChildByName("MainCamera");
            GameObject man = UtilApi.GoFindChildByName("Cube");
            UtilApi.setPos(man.transform, Vector3.zero);
            Ctx.m_instance.m_camSys.setMainCamera(camera.GetComponent<Camera>());
            Ctx.m_instance.m_camSys.setCameraActor(man);
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
            //Ctx.m_instance.m_uiMgr.loadAndShow((UIFormID)100);
            testSceneTerrain();
        }

        public void checkCamera()
        {
            MTestCamera camera = new MTestCamera(Camera.main);
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