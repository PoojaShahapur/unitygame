using LuaInterface;
using System;

namespace SDK.Lib
{
    /**
     * @brief 事件回调函数只能添加一次
     */
    public class AddOnceEventDispatch : EventDispatch
    {
        public AddOnceEventDispatch(int eventId_ = 0)
            : base(eventId_)
        {

        }

        override public void addEventHandle(ICalleeObject pThis, MAction<IDispatchObject> handle, LuaTable luaTable = null, LuaFunction luaFunction = null)
        {
            // 这个判断说明相同的函数只能加一次，但是如果不同资源使用相同的回调函数就会有问题，但是这个判断可以保证只添加一次函数，值得，因此不同资源需要不同回调函数
            if (!isExistEventHandle(pThis, handle, luaTable, luaFunction))
            {
                base.addEventHandle(pThis, handle, luaTable, luaFunction);
            }
        }
    }
}