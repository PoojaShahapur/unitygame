namespace UnitTestSrc
{
    public class UnitTestMain
    {
        public UnitTestBuffer m_pUnitTestBuffer = new UnitTestBuffer();
        public TestZip m_pTestZip = new TestZip();
        public TestEncrypt m_pTestEncrypt = new TestEncrypt();

        public void run()
        {
            //m_pUnitTestBuffer.run();
            //m_pTestZip.run();
            m_pTestEncrypt.run();
        }
    }
}