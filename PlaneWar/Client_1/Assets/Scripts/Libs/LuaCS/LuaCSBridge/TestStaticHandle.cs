namespace SDK.Lib
{
    public class TestStaticHandle
    {
        static public void log(string desc)
        {
            if (MacroDef.ENABLE_LOG)
            {
                Ctx.mInstance.mLogSys.log(desc);
            }
        }
    }
}