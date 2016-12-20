namespace UnitTest
{
    public class TestMain
    {
        protected UnitTestBuffer mUnitTestBuffer;
        protected TestZip mTestZip;
        protected TestEncrypt mTestEncrypt;
        protected FilterTest mFilterTest;
        protected TestVersion mTestVersion;
        protected ThreadTest mThreadTest;
        protected TestFile mTestFile;
        protected TestAutoUpdate mTestAutoUpdate;

        protected TestAssetBundles mTestAssetBundles;
        protected TestDataStruct mTestDataStruct;
        protected TestTable mTestTable;
        protected TestXml mTestXml;
        protected TestResources mTestResources;
        protected TestLoad mTestLoad;
        protected TestNet mTestNet;
        protected TestAni mTestAni;
        protected TestLogic mTestLogic;
        protected TestLua mTestLua;
        protected TestNavMesh mTestNavMesh;
        protected TestAStar mTestAStar;
        protected TestProtoBuf mTestProtoBuf;
        protected TestDraw mTestDraw;

        protected TestHeightMap mTestHeightMap;
        protected TestCoordinateConv mTestCoordinateConv;
        protected TestTime mTestTime;

        protected TestCameraMan mTestCameraMan;
        protected TestIOControl mTestIOControl;
        protected TestIsometric mTestIsometric;
        protected TestCoroutinePrefabIns mTestCoroutinePrefabIns;
        protected TestCoroutineTask mTestCoroutineTask;
        protected TestDataStream mTestDataStream;

        public TestMain()
        {
            mUnitTestBuffer = new UnitTestBuffer();
            mTestZip = new TestZip();
            mTestEncrypt = new TestEncrypt();
            mFilterTest = new FilterTest();
            mTestVersion = new TestVersion();
            mThreadTest = new ThreadTest();
            mTestFile = new TestFile();
            mTestAutoUpdate = new TestAutoUpdate();

            mTestAssetBundles = new TestAssetBundles();
            mTestDataStruct = new TestDataStruct();
            mTestTable = new TestTable();
            mTestXml = new TestXml();
            mTestResources = new TestResources();
            mTestLoad = new TestLoad();
            mTestNet = new TestNet();
            mTestAni = new TestAni();
            mTestLogic = new TestLogic();
            mTestLua = new TestLua();
            mTestNavMesh = new TestNavMesh();
            mTestAStar = new TestAStar();
            mTestProtoBuf = new TestProtoBuf();
            mTestDraw = new TestDraw();

            mTestHeightMap = new TestHeightMap();
            mTestCoordinateConv = new TestCoordinateConv();
            mTestTime = new TestTime();

            mTestCameraMan = new TestCameraMan();
            mTestIOControl = new TestIOControl();
            mTestIsometric = new TestIsometric();
            mTestCoroutinePrefabIns = new TestCoroutinePrefabIns();
            mTestCoroutineTask = new TestCoroutineTask();
            mTestDataStream = new TestDataStream();
        }

        public void run()
        {
            //mTestFile.run();
            //mThreadTest.run();
            //mUnitTestBuffer.run();
            //mTestZip.run();
            //mTestEncrypt.run();
            //mFilterTest.run();
            //mTestVersion.run();
            //mTestAutoUpdate.run();
            //mTestAssetBundles.run();

            //mTestDataStruct.run();
            //mTestTable.run();
            //mTestXml.run();
            //mTestResources.run();
            mTestLoad.run();
            //mTestNet.run();
            //mTestAni.run();

            //mTestLogic.run();
            //mTestLua.run();
            //mTestNavMesh.run();
            //mTestAStar.run();
            //mTestProtoBuf.run();

            //mTestDraw.run();
            //mTestHeightMap.run();
            //mTestCoordinateConv.run();
            //mTestTime.run();
            //mTestCameraMan.run();
            //mTestIOControl.run();
            //mTestIsometric.run();
            //mTestCoroutinePrefabIns.run();
            //mTestCoroutineTask.run();
            //mTestDataStream.run();
        }
    }
}