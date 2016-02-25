using SDK.Lib;
using System.IO;

namespace UnitTest
{
    public class TestFile
    {
        public void run()
        {
            //testApi();
            testLoadFile();
        }

        protected void testApi()
        {
            UtilApi.recureCreateSubDir("E:", "aaa\\bbb\\ccc.txt");
            string path = UtilApi.versionPath("E:\\aaa\\bbb\\ccc.txt", "2011");
            UtilApi.delFileNoVer("E:\\aaa\\bbb\\ccc.txt");
        }

        protected void testLoadFile()
        {
            string path = Path.Combine(Ctx.m_instance.m_localFileSys.getLocalDataDir(), "Resources/Table/CardBase_client.bytes");
            Ctx.m_instance.m_localFileSys.LoadFileByte(path);
        }
    }
}