namespace SDK.Lib
{
    public class LuaCSDispatchFunctionObject : LuaCSBridge
    {
        public LuaCSDispatchFunctionObject(string luaFile = "", string tableName = "")
            : base(luaFile, tableName)
        {

        }

        public void call(IDispatchObject dispObj)
        {
            if(m_luaTable != null && m_luaFunc != null)
            {
                this.callClassMethod("", "", dispObj);
            }
            else if(m_luaFunc != null)
            {
                m_luaFunc.Call(dispObj);
            }
        }
    }
}