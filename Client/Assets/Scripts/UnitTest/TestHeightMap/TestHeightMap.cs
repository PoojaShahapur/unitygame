using SDK.Lib;

namespace UnitTest
{
    /**
     * @brief 测试高度图
     */
    public class TestHeightMap
    {
        public void run()
        {
            testHeightMap();
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

        protected void end()
        {

        }
    }
}