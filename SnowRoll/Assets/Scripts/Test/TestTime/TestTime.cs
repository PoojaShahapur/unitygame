using SDK.Lib;

namespace UnitTest
{
    public class TestTime
    {
        public void run()
        {
            this.testFormat();
        }

        protected void testFormat()
        {
            int time = 0;
            string str = "";

            time = 366 * 24 * 60 * 60;
            str = UtilLogic.formatTime(time);

            time = 360 * 24 * 60 * 60;
            str = UtilLogic.formatTime(time);

            time = 25 * 60 * 60;
            str = UtilLogic.formatTime(time);

            time = 20 * 60 * 60;
            str = UtilLogic.formatTime(time);

            time = 80 * 60;
            str = UtilLogic.formatTime(time);

            time = 50 * 60;
            str = UtilLogic.formatTime(time);

            time = 80;
            str = UtilLogic.formatTime(time);

            time = 20;
            str = UtilLogic.formatTime(time);
        }
    }
}