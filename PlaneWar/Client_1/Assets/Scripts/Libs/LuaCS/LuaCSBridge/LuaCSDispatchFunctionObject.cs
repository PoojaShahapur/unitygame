namespace SDK.Lib
{
    public class LuaCSDispatchFunctionObject : LuaCSBridge
    {
        public uint mEventId;   // 事件唯一 Id

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


        public void setEventId(uint eventId)
        {
            this.mEventId = eventId;
        }

        public bool isEventIdEqual(uint eventId)
        {
            return this.mEventId == eventId;
        }
    }
}