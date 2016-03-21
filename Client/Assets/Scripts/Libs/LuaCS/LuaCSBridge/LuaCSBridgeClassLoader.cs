using LuaInterface;

namespace SDK.Lib
{
    /**
     * @brief Lua 类文件加载器
     */
    public class LuaCSBridgeClassLoader : LuaCSBridge
    {
        // MyLua/Libs/Core/ClassLoader.lua
        protected bool m_bLoaded = false;       // lua 文件是否加载

        public LuaCSBridgeClassLoader()
            : base("", "GlobalNS.ClassLoader")
        {
            if (!m_bLoaded)
            {
                init();
                m_bLoaded = true;
            }
        }

        public LuaTable loadModule(string file)
        {
            return this.CallTableMethod("loadClass", file)[0] as LuaTable;
        }
    }
}