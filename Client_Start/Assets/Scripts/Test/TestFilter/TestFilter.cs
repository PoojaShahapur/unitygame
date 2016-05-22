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
            Ctx.m_instance.m_wordFilterManager.doFilter(ref testStr);
        }

        protected void testMatch()
        {
            string testStr = "aasbbsccsdddasbbsccs";
            Ctx.m_instance.m_wordFilterManager.IsMatch(testStr);
        }
    }
}