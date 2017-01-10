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
            if(mLuaTable != null && mLuaFunc != null)
            {
                this.callClassMethod("", "", dispObj);
            }
            else if(mLuaFunc != null)
            {
                mLuaFunc.Call(dispObj);
            }
        }
    }
}