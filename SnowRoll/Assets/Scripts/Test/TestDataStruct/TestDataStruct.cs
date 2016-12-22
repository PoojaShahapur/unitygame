using SDK.Lib;

namespace UnitTest
{
    public class TestDataStruct
    {
        public void run()
        {
            testLockList();
        }

        protected void testLockList()
        {
            LockList<string> mList = new LockList<string>("TestLockList", 4);
            mList.Add("aaaaa");
            mList.Add("bbbbb");
            mList.Add("ccccc");

            mList.RemoveAt(1);
        }
    }
}