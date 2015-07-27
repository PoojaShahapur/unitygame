using LuaInterface;
using SDK.Common;
using SDK.Lib;
using System.Diagnostics;

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
            //testLoadLuaFile();
            //testLocalLua();
            //testLuaBindFile();
            testLuaByteBuffer();
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
            //LuaScriptMgr luaMgr = new LuaScriptMgr();
            //luaMgr.Start();
            LuaScriptMgr luaMgr = Ctx.m_instance.m_luaMgr;

            string path = "";
            luaMgr.lua.DoFile("LuaScript/UtilDebug.lua");

            LuaFunction reflf = LuaScriptMgr.Instance.lua.GetFunction("regPath");
            string luaPath = string.Format("{0}/{1}", UtilApi.getDataPath(), "Prefabs/Resources/LuaScript");
            UtilApi.normalPath(ref luaPath);
            object[] ret = reflf.Call(luaPath);

            luaPath = string.Format("{0}/{1}", UtilApi.getDataPath(), "Plugins/x86_64");
            reflf = luaMgr.lua.GetFunction("regCPath");
            ret = reflf.Call(luaPath);

            path = string.Format("{0}/{1}", UtilApi.getDataPath(), "Prefabs/Resources/LuaScript/UtilDebug.lua");
            luaMgr.lua.DoFile("LuaScript/TestLua.lua");

            //LuaFunction reflf = LuaScriptMgr.Instance.lua.GetFunction("addVarArg");
            //object[] ret = reflf.Call(10, 30);
            //int aaa = 10;
        }

        protected void testLocalLua()
        {
            //Process.Start("\"D:\\ProgramFiles(x86)\\Lua\\5.1\\lua\" -e \"require('debugger')('192.168.122.64', '10000');\" E:\\Work\\Code20150402\\client\\trunk\\Client\\Assets\\Lua\\LuaScript\\TestLua.lua");
            //Process.Start("D:\\ProgramFiles(x86)\\Lua\\5.1\\lua");
            //Process.Start("E:\\Work\\Code20150402\\client\\trunk\\Client\\Assets\\Lua\\LuaScript\\TestLua.lua");
            Process.Start("E:/Start.bat");
        }

        protected void testLuaBindFile()
        {
            LuaCSBridge _luaCSBridge = new LuaCSBridge("testTable");
            string path = "LuaScript/TestLuaBind.lua";      // 
            _luaCSBridge.DoFile(path);                      // 添加函数，如果 "TestLuaBind.lua" 文件直接调用了一个函数，例如 luaFunc(10) ，执行 DoFile 后返回值是 null ，注意这一点，但是自己手工调用这个函数却有返回值的。
            object[] ret = _luaCSBridge.CallMethod("tableFunc", 10);

            object member = _luaCSBridge.GetMember("tableData");
        }

        protected void testLuaByteBuffer()
        {
            ByteBuffer ba = new ByteBuffer();
            ba.luaCSBridgeByteBuffer = new LuaCSBridgeByteBuffer();

            ba.writeInt16(234);
            ba.writeInt32(567);
            //ba.luaCSBridgeByteBuffer.CallClassMethod("tableFunc");
            ba.luaCSBridgeByteBuffer.updateLuaByteBuffer(ba);
            ba.luaCSBridgeByteBuffer.CallClassMethod("TestOut");
            //ba.luaCSBridgeByteBuffer.CallClassMethod("dumpAllBytes");

            object _int16 = ba.luaCSBridgeByteBuffer.CallClassMethod("readInt16FromCS");
            int aaa = 0;
        }
    }
}