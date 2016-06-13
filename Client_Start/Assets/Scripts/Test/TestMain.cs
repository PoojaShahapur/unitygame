namespace UnitTest
{
    public class TestMain
    {
        protected UnitTestBuffer m_pUnitTestBuffer;
        protected TestZip m_pTestZip;
        protected TestEncrypt m_pTestEncrypt;
        protected FilterTest m_filterTest;
        protected TestVersion m_testVersion;
        protected ThreadTest m_pThreadTest;
        protected TestFile m_pTestFile;
        protected TestAutoUpdate m_pTestAutoUpdate;

        protected TestAssetBundles m_pTestAssetBundles;
        protected TestDataStruct m_pTestDataStruct;
        protected TestTable m_pTestTable;
        protected TestXml m_pTestXml;
        protected TestResources m_pTestResources;
        protected TestLoad m_pTestLoad;
        protected TestNet m_pTestNet;
        protected TestAni m_pTestAni;
        protected TestLogic m_pTestLogic;
        protected TestLua m_pTestLua;
        protected TestNavMesh m_pTestNavMesh;
        protected TestAStar m_pTestAStar;
        protected TestProtoBuf m_pTestProtoBuf;
        protected TestDraw m_pTestDraw;

        protected TestHeightMap m_testHeightMap;
        protected TestCoordinateConv m_testCoordinateConv;
        protected TestTime m_testTime;

        protected TestCameraMan m_testCameraMan;
        protected TestIOControl m_testIOControl;
        protected TestIsometric m_testIsometric;
        protected TestCoroutinePrefabIns mTestCoroutinePrefabIns;
        protected TestCoroutineTask mTestCoroutineTask;
        protected TestDataStream mTestDataStream;

        public TestMain()
        {
            m_pUnitTestBuffer = new UnitTestBuffer();
            m_pTestZip = new TestZip();
            m_pTestEncrypt = new TestEncrypt();
            m_filterTest = new FilterTest();
            m_testVersion = new TestVersion();
            m_pThreadTest = new ThreadTest();
            m_pTestFile = new TestFile();
            m_pTestAutoUpdate = new TestAutoUpdate();

            m_pTestAssetBundles = new TestAssetBundles();
            m_pTestDataStruct = new TestDataStruct();
            m_pTestTable = new TestTable();
            m_pTestXml = new TestXml();
            m_pTestResources = new TestResources();
            m_pTestLoad = new TestLoad();
            m_pTestNet = new TestNet();
            m_pTestAni = new TestAni();
            m_pTestLogic = new TestLogic();
            m_pTestLua = new TestLua();
            m_pTestNavMesh = new TestNavMesh();
            m_pTestAStar = new TestAStar();
            m_pTestProtoBuf = new TestProtoBuf();
            m_pTestDraw = new TestDraw();

            m_testHeightMap = new TestHeightMap();
            m_testCoordinateConv = new TestCoordinateConv();
            m_testTime = new TestTime();

            m_testCameraMan = new TestCameraMan();
            m_testIOControl = new TestIOControl();
            m_testIsometric = new TestIsometric();
            mTestCoroutinePrefabIns = new TestCoroutinePrefabIns();
            mTestCoroutineTask = new TestCoroutineTask();
            mTestDataStream = new TestDataStream();
        }

        public void run()
        {
            //m_pTestFile.run();
            //m_pThreadTest.run();
            //m_pUnitTestBuffer.run();
            //m_pTestZip.run();
            //m_pTestEncrypt.run();
            //m_filterTest.run();
            //m_testVersion.run();
            //m_pTestAutoUpdate.run();
            //m_pTestAssetBundles.run();

            //m_pTestDataStruct.run();
            //m_pTestTable.run();
            //m_pTestXml.run();
            //m_pTestResources.run();
            m_pTestLoad.run();
            //m_pTestNet.run();
            //m_pTestAni.run();

            //m_pTestLogic.run();
            //m_pTestLua.run();
            //m_pTestNavMesh.run();
            //m_pTestAStar.run();
            //m_pTestProtoBuf.run();

            //m_pTestDraw.run();
            //m_testHeightMap.run();
            //m_testCoordinateConv.run();
            //m_testTime.run();
            //m_testCameraMan.run();
            //m_testIOControl.run();
            //m_testIsometric.run();
            //mTestCoroutinePrefabIns.run();
            //mTestCoroutineTask.run();
            //mTestDataStream.run();
        }
    }
}