using Game.UI;
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
            //testProjectMatrix();
            //testPrint();

            Ctx.mInstance.mScriptDynLoad.registerScriptType("Game.UI.UITerrainEdit", typeof(UITerrainEdit));
            testNewTerrain();
        }

        protected void testHeightMap()
        {
            //TestHeightMapData heightMapData = new TestHeightMapData();
			HeightMapData heightMapData = new HeightMapData();
            heightMapData.loadHeightMap("Terrain/terrain.png");
            heightMapData.getWidth();
            heightMapData.getHeight();
            float _height = heightMapData.getPixHeight(200, 200);

            end();
        }

        // 代码整理，将地形放到对应的场景里面了
        public void testSceneTerrain()
        {
            Ctx.mInstance.mSceneSys.createTerrain();

            // 操作摄像机
            GameObject camera = UtilApi.GoFindChildByName("MainCamera");
            GameObject man = UtilApi.GoFindChildByName("Cube");
            UtilApi.setPos(man.transform, Vector3.zero);
            Ctx.mInstance.mCamSys.setMainCamera(camera.GetComponent<Camera>());
            Ctx.mInstance.mCamSys.setCameraActor(man);
        }

        public void testPrint()
        {
            //TestHeightMapData heightMapData = new TestHeightMapData();
			HeightMapData heightMapData = new HeightMapData();
            heightMapData.loadHeightMap("Terrain/terrain.png");
            heightMapData.print();
        }

        // 测试投影矩阵
        public void testProjectMatrix()
        {
            Ctx.mInstance.mSceneSys.loadScene("TestHeightMap.unity", onResLoadScene);
        }

        public void onResLoadScene(IDispatchObject dispObj)
        {
            Scene scene = dispObj as Scene;
            //checkCamera();
            //Ctx.mInstance.mUiMgr.loadAndShow((UIFormId)100);
            testSceneTerrain();
        }

        // 测试四叉树
        protected void testQuadTree()
        {

        }

        protected void end()
        {

        }

        protected void testNewTerrain()
        {
            Ctx.mInstance.mSceneSys.loadScene("TestHeightMap.unity", onNewResLoadScene);
        }

        public void onNewResLoadScene(IDispatchObject dispObj)
        {
            Scene scene = dispObj as Scene;
            Ctx.mInstance.mSceneSys.createTerrain();

            // 操作摄像机
            GameObject camera = UtilApi.GoFindChildByName("MainCamera");
            GameObject man = UtilApi.GoFindChildByName("Cube");
            UtilApi.setPos(man.transform, Vector3.zero);
            Ctx.mInstance.mCamSys.setMainCamera(camera.GetComponent<Camera>());
            Ctx.mInstance.mCamSys.setCameraActor(man);

            Ctx.mInstance.mUiMgr.loadAndShow(UIFormId.eUITerrainEdit);

            //AuxTextLoader auxTextLoader = new AuxTextLoader();
            //auxTextLoader.syncLoad("Table/FrameTimerMgr.lua");
        }
    }
}