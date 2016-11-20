using LuaInterface;
using SDK.Lib;

namespace UnitTest
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
            //testLuaByteBuffer();
            //testLuaByteBufferNeg();
            //testSysEndian();
            //testGet_GTable();
            testLuaCsCall();
        }

        protected void testLua()
        {
            string path = "";
            TextRes textRes = null;
            //LuaState ls = new LuaState();
            LuaScriptMgr luaMgr = new LuaScriptMgr();
            path = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathLuaScript], "UtilDebug.txt");
            textRes = Ctx.m_instance.m_texMgr.getAndSyncLoad<TextRes>(path);
            Ctx.m_instance.m_luaSystem.lua.DoString(textRes.text);

            LuaFunction reflf = Ctx.m_instance.m_luaSystem.lua.GetFunction("regPath");
            string luaPath = string.Format("{0}/{1}", UtilApi.getDataPath(), "Resources/LuaScript");
            luaPath = UtilPath.normalPath(luaPath);
            object[] ret = reflf.Call(luaPath);

            luaPath = string.Format("{0}/{1}", UtilApi.getDataPath(), "Plugins/x86_64");
            reflf = Ctx.m_instance.m_luaSystem.lua.GetFunction("regCPath");
            ret = reflf.Call(luaPath);

            //path = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathLuaScript], "debugger.txt");
            //textRes = Ctx.m_instance.m_textResMgr.getAndSyncLoad<TextRes>(path);
            //ls.DoString(textRes.text);

            path = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathLuaScript], "TestLua.txt");
            textRes = Ctx.m_instance.m_texMgr.getAndSyncLoad<TextRes>(path);
            Ctx.m_instance.m_luaSystem.lua.DoString(textRes.text);

            LuaFunction lf = Ctx.m_instance.m_luaSystem.lua.GetFunction("luaFunc");
            object[] r = lf.Call("2");
            string str = r[0].ToString();

            //LuaTable table = Ctx.m_instance.m_luaSystem.lua.GetTable("mimeself");
            //object _obj = table["encode"];
            //int aaa = 10;
        }

        protected void testLoadLuaFile()
        {
            //LuaScriptMgr luaMgr = new LuaScriptMgr();
            //luaMgr.Start();
            LuaScriptMgr luaMgr = Ctx.m_instance.m_luaSystem.getLuaScriptMgr();

            string path = "";
            luaMgr.GetMainState().DoFile("Test/UtilDebug.lua");

            LuaFunction reflf = Ctx.m_instance.m_luaSystem.lua.GetFunction("regPath");
            string luaPath = string.Format("{0}/{1}", UtilApi.getDataPath(), "Resources/LuaScript");
            luaPath = UtilPath.normalPath(luaPath);
            object[] ret = reflf.Call(luaPath);

            luaPath = string.Format("{0}/{1}", UtilApi.getDataPath(), "Plugins/x86_64");
            reflf = luaMgr.GetMainState().GetFunction("regCPath");
            ret = reflf.Call(luaPath);

            path = string.Format("{0}/{1}", UtilApi.getDataPath(), "Resources/LuaScript/UtilDebug.lua");
            luaMgr.GetMainState().DoFile("Test/TestLua.lua");

            //LuaFunction reflf = Ctx.m_instance.m_luaScriptMgr.lua.GetFunction("addVarArg");
            //object[] ret = reflf.Call(10, 30);
            //int aaa = 10;
        }

        protected void testLocalLua()
        {
            LuaScriptMgr luaMgr = Ctx.m_instance.m_luaSystem.getLuaScriptMgr();
            //luaMgr.lua.DoFile("Common/Prerequisites.lua");
            luaMgr.GetMainState().DoFile("Test/TestLua.lua");

            //Process.Start("\"D:\\ProgramFiles(x86)\\Lua\\5.1\\lua\" -e \"require('debugger')('192.168.122.64', '10000');\" E:\\Work\\Code20150402\\client\\trunk\\Client\\Assets\\Lua\\LuaScript\\TestLua.lua");
            //Process.Start("D:\\ProgramFiles(x86)\\Lua\\5.1\\lua");
            //Process.Start("E:\\Work\\Code20150402\\client\\trunk\\Client\\Assets\\Lua\\LuaScript\\TestLua.lua");
            // Process.Start("E:/Self/Self/unity/unitygame/Client/Assets/Scripts/UnitTestSrc/TestLua/Start.bat");
        }

        protected void testLuaBindFile()
        {
            LuaScriptMgr luaMgr = Ctx.m_instance.m_luaSystem.getLuaScriptMgr();
            luaMgr.GetMainState().DoFile("Common/Prerequisites.lua");

            LuaCSBridge _luaCSBridge = new LuaCSBridge("", "testTable");
            string path = "Test/TestLuaBind.lua";      // 
            Ctx.m_instance.m_luaSystem.doFile(path);                      // 添加函数，如果 "TestLuaBind.lua" 文件直接调用了一个函数，例如 luaFunc(10) ，执行 DoFile 后返回值是 null ，注意这一点，但是自己手工调用这个函数却有返回值的。
            object[] ret = _luaCSBridge.callTableMethod("", "tableFunc", 10);

            object member = _luaCSBridge.getMember("tableData");
        }

        protected void testLuaByteBuffer()
        {
            ByteBuffer byteBuffer = new ByteBuffer();
            byteBuffer.luaCSBridgeByteBuffer = new LuaCSBridgeByteBuffer();

            byteBuffer.writeInt16(257);
            byteBuffer.writeInt32(2147483647);
            byteBuffer.writeInt16(-86);
            byteBuffer.writeMultiByte("asdfasdf", GkEncode.eUTF8, 16);
            byteBuffer.writeMultiByte("测试啊", GkEncode.eUTF8, 16);
            //byteBuffer.luaCSBridgeByteBuffer.callClassMethod("tableFunc");
            byteBuffer.luaCSBridgeByteBuffer.updateLuaByteBuffer(byteBuffer);
            byteBuffer.luaCSBridgeByteBuffer.callClassMethod("", "TestOut");
            //bu.luaCSBridgeByteBuffer.callClassMethod("dumpAllBytes");

            object _int16 = byteBuffer.luaCSBridgeByteBuffer.callClassMethod("", "readInt16FromCS");
            object _int32 = byteBuffer.luaCSBridgeByteBuffer.callClassMethod("", "readInt32FromCS");
            object _int16Neg = byteBuffer.luaCSBridgeByteBuffer.callClassMethod("", "readInt16FromCS");
            object _strEn = byteBuffer.luaCSBridgeByteBuffer.callClassMethod("", "readMultiByteFromCS");
            object _strChs = byteBuffer.luaCSBridgeByteBuffer.callClassMethod("", "readMultiByteFromCS");
            int aaa = 0;
        }

        // 测试符号数
        protected void testLuaByteBufferNeg()
        {
            ByteBuffer byteBuffer = new ByteBuffer();
            byteBuffer.luaCSBridgeByteBuffer = new LuaCSBridgeByteBuffer();

            byteBuffer.writeUnsignedInt8(123);
            byteBuffer.writeInt16(-7894);
            byteBuffer.writeInt32(456789132);
            byteBuffer.writeInt32(-789445678);
            byteBuffer.luaCSBridgeByteBuffer.updateLuaByteBuffer(byteBuffer);
            byteBuffer.luaCSBridgeByteBuffer.callClassMethod("", "TestOut");

            object _int8Neg = byteBuffer.luaCSBridgeByteBuffer.callClassMethod("", "readInt8FromCS");
            object _int16Neg = byteBuffer.luaCSBridgeByteBuffer.callClassMethod("", "readInt16FromCS");
            object _int32Pos = byteBuffer.luaCSBridgeByteBuffer.callClassMethod("", "readInt32FromCS");
            object _int32Neg = byteBuffer.luaCSBridgeByteBuffer.callClassMethod("", "readInt32FromCS");
            int aaa = 0;
        }

        protected void testSysEndian()
        {
            ByteBuffer byteBuffer = new ByteBuffer();
            byteBuffer.luaCSBridgeByteBuffer = new LuaCSBridgeByteBuffer();
            byteBuffer.luaCSBridgeByteBuffer.setSysEndian(23);

            byteBuffer.writeInt32(-789445678);
            byteBuffer.luaCSBridgeByteBuffer.updateLuaByteBuffer(byteBuffer);
            byteBuffer.luaCSBridgeByteBuffer.callClassMethod("", "TestOut");

            object _int32Neg = byteBuffer.luaCSBridgeByteBuffer.callClassMethod("", "readInt32FromCS");
            int aaa = 0;
        }

        protected void testGet_GTable()
        {
            LuaTable luaTable = Ctx.m_instance.m_luaSystem.getLuaTable("_G");
        }

        // 下面函数仅仅是说明用法，不能真正运行
        private void testFunc()
        {
            LuaFunction luaFunc = null;
            luaFunc.Call();
            luaFunc.Dispose();
            luaFunc = null;
        }

        private void testPCall()
        {
            LuaFunction luaFunc = null;
            //int oldTop = luaFunc.BeginPCall();       // 将函数压栈
            //LuaInterface.LuaDLL.lua_pushinteger(Ctx.m_instance.m_luaSystem.lua.L, 10);
            //if (luaFunc.PCall(oldTop, 1))
            //{
            //    luaFunc.EndPCall(oldTop);
            //}
            luaFunc.Call(10);
        }

        // 测试 Lua 中回调 CS 中保存 Lua 中的表的出来
        public void testLuaCsCall()
        {

        }
    }
}