using SDK.Common;

namespace UnitTestSrc
{
    public class TestFile
    {
        public void run()
        {
            //testApi();
        }

        protected void testApi()
        {
            UtilApi.recureCreateSubDir("E:", "aaa\\bbb\\ccc.txt");
            string path = UtilApi.versionPath("E:\\aaa\\bbb\\ccc.txt", "2011");
            UtilApi.delFileNoVer("E:\\aaa\\bbb\\ccc.txt");
        }
    }
}