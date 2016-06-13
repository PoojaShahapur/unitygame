using SDK.Lib;
using System.IO;

namespace UnitTest
{
    public class TestDataStream
    {
        protected MDataStream mStream;

        public TestDataStream()
        {
            
        }

        public void run()
        {
            //testEditor();

            mStream = new MDataStream(UtilPath.combine(MFileSys.msStreamingAssetsPath, "Test.txt"), onFileOpened);
        }

        public void onFileOpened(IDispatchObject dispObj)
        {
            mStream = dispObj as MDataStream;
            string text = mStream.readText();
            Ctx.m_instance.m_logSys.log("Text = " + text);
        }

        protected void testEditor()
        {
            string text = mStream.readText();
            Ctx.m_instance.m_logSys.log("Text = " + text);
        }
    }
}