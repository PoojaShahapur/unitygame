using SDK.Common;
namespace UnitTestSrc
{
    public class FilterTest
    {
        public void run()
        {
            testFilter();
        }

        public void testFilter()
        {
            string testStr = "aasbbsccsdddasbbsccs";
            Ctx.m_instance.m_wordFilterManager.doFilter(ref testStr);
        }
    }
}