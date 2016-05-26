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
            UtilPath.recureCreateSubDir("E:", "aaa\\bbb\\ccc.txt");
            string path = UtilPath.versionPath("E:\\aaa\\bbb\\ccc.txt", "2011");
            UtilPath.delFileNoVer("E:\\aaa\\bbb\\ccc.txt");
        }

        protected void testLoadFile()
        {
            string path = Path.Combine(MFileSys.getLocalDataDir(), "Resources/Table/CardBase_client.bytes");
            Ctx.m_instance.m_fileSys.LoadFileByte(path);
        }
    }
}