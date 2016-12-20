namespace SDK.Lib
{
    public class TestStaticHandle
    {
        static public void log(string desc)
        {
            Ctx.mInstance.mLogSys.log(desc);
        }
    }
}