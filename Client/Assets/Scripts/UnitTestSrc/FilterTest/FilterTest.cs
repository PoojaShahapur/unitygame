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
            string testStr = "aasbbsccsddd";
            Ctx.m_instance.m_wordFilterManager.doFilter(ref testStr);
        }
    }
}