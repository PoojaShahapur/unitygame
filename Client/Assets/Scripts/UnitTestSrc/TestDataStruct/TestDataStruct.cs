using SDK.Lib;

namespace UnitTestSrc
{
    public class TestDataStruct
    {
        public void run()
        {
            testLockList();
        }

        protected void testLockList()
        {
            LockList<string> m_list = new LockList<string>("TestLockList", 4);
            m_list.Add("aaaaa");
            m_list.Add("bbbbb");
            m_list.Add("ccccc");

            m_list.RemoveAt(1);
        }
    }
}