using LuaInterface;
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
            //testLoadLuaFile();
            testLocalLua();
            //testLuaBindFile();
            //testLuaByteBuffer();
            //testLuaByteBufferNeg();
            //testSysEndian();
            //testGet_GTable();
        }

        protected void testLua()
        {
            string path = "";
            TextRes textRes = null;
            //LuaState ls = new LuaState();
            LuaScriptMgr luaMgr = new LuaScriptMgr();
            path = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathLuaScript], "UtilDebug.txt");
            textRes = Ctx.m_instance.m_texMgr.getAndSyncLoad<TextRes>(path);
            Ctx.m_instance.m_luaScriptMgr.lua.DoString(textRes.text);

            LuaFunction reflf = Ctx.m_instance.m_luaScriptMgr.lua.GetFunction("regPath");
            string luaPath = string.Format("{0}/{1}", UtilApi.getDataPath(), "Prefabs/Resources/LuaScript");
            UtilApi.normalPath(ref luaPath);
            object[] ret = reflf.Call(luaPath);

            luaPath = string.Format("{0}/{1}", UtilApi.getDataPath(), "Plugins/x86_64");
            reflf = Ctx.m_instance.m_luaScriptMgr.lua.GetFunction("regCPath");
            ret = reflf.Call(luaPath);

            //path = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathLuaScript], "debugger.txt");
            //textRes = Ctx.m_instance.m_textResMgr.getAndSyncLoad<TextRes>(path);
            //ls.DoString(textRes.text);

            path = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathLuaScript], "TestLua.txt");
            textRes = Ctx.m_instance.m_texMgr.getAndSyncLoad<TextRes>(path);
            Ctx.m_instance.m_luaScriptMgr.lua.DoString(textRes.text);

            LuaFunction lf = Ctx.m_instance.m_luaScriptMgr.lua.GetFunction("luaFunc");
            object[] r = lf.Call("2");
            string str = r[0].ToString();

            //LuaTable table = Ctx.m_instance.m_luaScriptMgr.lua.GetTable("mimeself");
            //object _obj = table["encode"];
            //int aaa = 10;
        }

        protected void testLoadLuaFile()
        {
            //LuaScriptMgr luaMgr = new LuaScriptMgr();
            //luaMgr.Start();
            LuaScriptMgr luaMgr = Ctx.m_instance.m_luaScriptMgr;

            string path = "";
            luaMgr.lua.DoFile("Test/UtilDebug.lua");

            LuaFunction reflf = Ctx.m_instance.m_luaScriptMgr.lua.GetFunction("regPath");
            string luaPath = string.Format("{0}/{1}", UtilApi.getDataPath(), "Prefabs/Resources/LuaScript");
            UtilApi.normalPath(ref luaPath);
            object[] ret = reflf.Call(luaPath);

            luaPath = string.Format("{0}/{1}", UtilApi.getDataPath(), "Plugins/x86_64");
            reflf = luaMgr.lua.GetFunction("regCPath");
            ret = reflf.Call(luaPath);

            path = string.Format("{0}/{1}", UtilApi.getDataPath(), "Prefabs/Resources/LuaScript/UtilDebug.lua");
            luaMgr.lua.DoFile("Test/TestLua.lua");

            //LuaFunction reflf = Ctx.m_instance.m_luaScriptMgr.lua.GetFunction("addVarArg");
            //object[] ret = reflf.Call(10, 30);
            //int aaa = 10;
        }

        protected void testLocalLua()
        {
            LuaScriptMgr luaMgr = Ctx.m_instance.m_luaScriptMgr;
            //luaMgr.lua.DoFile("Common/Prerequisites.lua");
            luaMgr.lua.DoFile("Test/TestLua.lua");

            //Process.Start("\"D:\\ProgramFiles(x86)\\Lua\\5.1\\lua\" -e \"require('debugger')('192.168.122.64', '10000');\" E:\\Work\\Code20150402\\client\\trunk\\Client\\Assets\\Lua\\LuaScript\\TestLua.lua");
            //Process.Start("D:\\ProgramFiles(x86)\\Lua\\5.1\\lua");
            //Process.Start("E:\\Work\\Code20150402\\client\\trunk\\Client\\Assets\\Lua\\LuaScript\\TestLua.lua");
            // Process.Start("E:/Self/Self/unity/unitygame/Client/Assets/Scripts/UnitTestSrc/TestLua/Start.bat");
        }

        protected void testLuaBindFile()
        {
            LuaScriptMgr luaMgr = Ctx.m_instance.m_luaScriptMgr;
            luaMgr.lua.DoFile("Common/Prerequisites.lua");

            LuaCSBridge _luaCSBridge = new LuaCSBridge("testTable");
            string path = "Test/TestLuaBind.lua";      // 
            _luaCSBridge.DoFile(path);                      // 添加函数，如果 "TestLuaBind.lua" 文件直接调用了一个函数，例如 luaFunc(10) ，执行 DoFile 后返回值是 null ，注意这一点，但是自己手工调用这个函数却有返回值的。
            object[] ret = _luaCSBridge.CallMethod("tableFunc", 10);

            object member = _luaCSBridge.GetMember("tableData");
        }

        protected void testLuaByteBuffer()
        {
            ByteBuffer bu = new ByteBuffer();
            bu.luaCSBridgeByteBuffer = new LuaCSBridgeByteBuffer();

            bu.writeInt16(257);
            bu.writeInt32(2147483647);
            bu.writeInt16(-86);
            bu.writeMultiByte("asdfasdf", GkEncode.UTF8, 16);
            bu.writeMultiByte("测试啊", GkEncode.UTF8, 16);
            //bu.luaCSBridgeByteBuffer.CallClassMethod("tableFunc");
            bu.luaCSBridgeByteBuffer.updateLuaByteBuffer(bu);
            bu.luaCSBridgeByteBuffer.CallClassMethod("TestOut");
            //bu.luaCSBridgeByteBuffer.CallClassMethod("dumpAllBytes");

            object _int16 = bu.luaCSBridgeByteBuffer.CallClassMethod("readInt16FromCS");
            object _int32 = bu.luaCSBridgeByteBuffer.CallClassMethod("readInt32FromCS");
            object _int16Neg = bu.luaCSBridgeByteBuffer.CallClassMethod("readInt16FromCS");
            object _strEn = bu.luaCSBridgeByteBuffer.CallClassMethod("readMultiByteFromCS");
            object _strChs = bu.luaCSBridgeByteBuffer.CallClassMethod("readMultiByteFromCS");
            int aaa = 0;
        }

        // 测试符号数
        protected void testLuaByteBufferNeg()
        {
            ByteBuffer bu = new ByteBuffer();
            bu.luaCSBridgeByteBuffer = new LuaCSBridgeByteBuffer();

            bu.writeUnsignedInt8(123);
            bu.writeInt16(-7894);
            bu.writeInt32(456789132);
            bu.writeInt32(-789445678);
            bu.luaCSBridgeByteBuffer.updateLuaByteBuffer(bu);
            bu.luaCSBridgeByteBuffer.CallClassMethod("TestOut");

            object _int8Neg = bu.luaCSBridgeByteBuffer.CallClassMethod("readInt8FromCS");
            object _int16Neg = bu.luaCSBridgeByteBuffer.CallClassMethod("readInt16FromCS");
            object _int32Pos = bu.luaCSBridgeByteBuffer.CallClassMethod("readInt32FromCS");
            object _int32Neg = bu.luaCSBridgeByteBuffer.CallClassMethod("readInt32FromCS");
            int aaa = 0;
        }

        protected void testSysEndian()
        {
            ByteBuffer bu = new ByteBuffer();
            bu.luaCSBridgeByteBuffer = new LuaCSBridgeByteBuffer();
            bu.luaCSBridgeByteBuffer.setSysEndian(23);

            bu.writeInt32(-789445678);
            bu.luaCSBridgeByteBuffer.updateLuaByteBuffer(bu);
            bu.luaCSBridgeByteBuffer.CallClassMethod("TestOut");

            object _int32Neg = bu.luaCSBridgeByteBuffer.CallClassMethod("readInt32FromCS");
            int aaa = 0;
        }

        protected void testGet_GTable()
        {
            LuaTable luaTable = Ctx.m_instance.m_luaScriptMgr.GetLuaTable("_G");
        }
    }
}