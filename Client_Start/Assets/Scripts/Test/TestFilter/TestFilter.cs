using SDK.Lib;

namespace UnitTest
{
    public class FilterTest
    {
        public void run()
        {
            //testFilter();
            testMatch();
        }

        protected void testFilter()
        {
            string testStr = "aasbbsccsdddasbbsccs";
            Ctx.mInstance.mWordFilterManager.doFilter(ref testStr);
        }

        protected void testMatch()
        {
            string testStr = "aasbbsccsdddasbbsccs";
            Ctx.mInstance.mWordFilterManager.IsMatch(testStr);
        }
    }
}