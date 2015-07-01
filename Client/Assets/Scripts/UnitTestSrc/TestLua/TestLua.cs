using LuaInterface;
using SDK.Common;
using SDK.Lib;

namespace UnitTestSrc
{
    public class TestLua
    {
        public TestLua()
        {
            
        }

        public void run()
        {
            testLua();
        }

        protected void testLua()
        {
            string path = "";
            TextRes textRes = null;
            LuaState ls = new LuaState();

            path = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathLuaScript], "UtilDebug.txt");
            textRes = Ctx.m_instance.m_textResMgr.getAndSyncLoad<TextRes>(path);
            ls.DoString(textRes.text);

            LuaFunction reflf = ls.GetFunction("regPath");
            object[] ret = reflf.Call("E:/Work/Code20150402/client/trunk/Client/Assets/Prefabs/Resources/LuaScript");

            //path = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathLuaScript], "debugger.txt");
            //textRes = Ctx.m_instance.m_textResMgr.getAndSyncLoad<TextRes>(path);
            //ls.DoString(textRes.text);

            path = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathLuaScript], "TestLua.txt");
            textRes = Ctx.m_instance.m_textResMgr.getAndSyncLoad<TextRes>(path);
            ls.DoString(textRes.text);

            LuaFunction lf = ls.GetFunction("luaFunc");
            object[] r = lf.Call("2");
            string str = r[0].ToString();
        }
    }
}