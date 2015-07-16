using SDK.Common;

namespace SDK.Lib
{
    public class TestStaticHandle
    {
        static public void log(string desc)
        {
            Ctx.m_instance.m_logSys.log(desc);
        }
    }
}