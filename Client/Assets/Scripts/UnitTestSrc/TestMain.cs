namespace UnitTestSrc
{
    public class UnitTestMain
    {
        protected UnitTestBuffer m_pUnitTestBuffer = new UnitTestBuffer();
        protected TestZip m_pTestZip = new TestZip();
        protected TestEncrypt m_pTestEncrypt = new TestEncrypt();
        protected FilterTest m_filterTest = new FilterTest();
        protected TestVersion m_testVersion = new TestVersion();
        protected ThreadTest m_pThreadTest = new ThreadTest();
        protected TestFile m_pTestFile = new TestFile();
        protected TestAutoUpdate m_pTestAutoUpdate = new TestAutoUpdate();

        protected TestAssetBundles m_pTestAssetBundles = new TestAssetBundles();
        protected TestDataStruct m_pTestDataStruct = new TestDataStruct();

        public void run()
        {
            m_pTestFile.run();
            m_pThreadTest.run();
            m_pUnitTestBuffer.run();
            m_pTestZip.run();
            m_pTestEncrypt.run();
            m_filterTest.run();
            //m_testVersion.run();
            m_pTestAutoUpdate.run();
            m_pTestAssetBundles.run();

            m_pTestDataStruct.run();
        }
    }
}