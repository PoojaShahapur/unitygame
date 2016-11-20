using LuaInterface;

namespace SDK.Lib
{
    /**
     * @brief lua 内存分配
     */
    public class LuaCSBridgeMalloc : LuaCSBridge
    {
        public LuaCSBridgeMalloc(string luaScriptPath, string luaScriptTableName)
            : base(luaScriptPath, luaScriptTableName)
        {
            init();
        }

        public LuaTable malloc(params object[] args)
        {
            return callClassMethod("", "new")[0] as LuaTable;
        }
    }
}