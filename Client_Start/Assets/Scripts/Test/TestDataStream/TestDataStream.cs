using SDK.Lib;
using System.IO;

namespace UnitTest
{
    public class TestDataStream
    {
        protected MDataStream mStream;

        public TestDataStream()
        {
            mStream = new MDataStream(UtilPath.combine(MFileSys.msStreamingAssetsPath, "Test.txt"), FileMode.Open);
        }

        public void run()
        {
            testEditor();
        }

        protected void testEditor()
        {
            string text = mStream.readText();
            Ctx.m_instance.m_logSys.log("Text = " + text);
        }
    }
}