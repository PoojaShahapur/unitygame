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
            //testLua();
            testLoadLuaFile();
        }

        protected void testLua()
        {
            string path = "";
            TextRes textRes = null;
            //LuaState ls = new LuaState();
            LuaScriptMgr luaMgr = new LuaScriptMgr();
            path = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathLuaScript], "UtilDebug.txt");
            textRes = Ctx.m_instance.m_textResMgr.getAndSyncLoad<TextRes>(path);
            LuaScriptMgr.Instance.lua.DoString(textRes.text);

            LuaFunction reflf = LuaScriptMgr.Instance.lua.GetFunction("regPath");
            string luaPath = string.Format("{0}/{1}", UtilApi.getDataPath(), "Prefabs/Resources/LuaScript");
            UtilApi.normalPath(ref luaPath);
            object[] ret = reflf.Call(luaPath);

            luaPath = string.Format("{0}/{1}", UtilApi.getDataPath(), "Plugins/x86_64");
            reflf = LuaScriptMgr.Instance.lua.GetFunction("regCPath");
            ret = reflf.Call(luaPath);

            //path = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathLuaScript], "debugger.txt");
            //textRes = Ctx.m_instance.m_textResMgr.getAndSyncLoad<TextRes>(path);
            //ls.DoString(textRes.text);

            path = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathLuaScript], "TestLua.txt");
            textRes = Ctx.m_instance.m_textResMgr.getAndSyncLoad<TextRes>(path);
            LuaScriptMgr.Instance.lua.DoString(textRes.text);

            LuaFunction lf = LuaScriptMgr.Instance.lua.GetFunction("luaFunc");
            object[] r = lf.Call("2");
            string str = r[0].ToString();

            //LuaTable table = LuaScriptMgr.Instance.lua.GetTable("mimeself");
            //object _obj = table["encode"];
            //int aaa = 10;
        }

        protected void testLoadLuaFile()
        {
            LuaScriptMgr luaMgr = new LuaScriptMgr();
            string path = string.Format("{0}/{1}", UtilApi.getDataPath(), "Prefabs/Resources/LuaScript/UtilDebug.lua");
            LuaScriptMgr.Instance.lua.LoadFile(path);
        }
    }
}