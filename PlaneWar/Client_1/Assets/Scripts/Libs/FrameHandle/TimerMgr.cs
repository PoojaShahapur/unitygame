using LuaInterface;
using System;

/**
 * @brief 定时器管理器
 */
namespace SDK.Lib
{
    public class TimerMgr : DelayHandleMgrBase
    {
        protected MList<TimerItemBase> mTimerList;     // 当前所有的定时器列表

        public TimerMgr()
        {
            this.mTimerList = new MList<TimerItemBase>();
        }

        override public void init()
        {

        }

        override public void dispose()
        {

        }

        protected override void addObject(IDelayHandleItem delayObject, float priority = 0.0f)
        {
            // 检查当前是否已经在队列中
            if (!this.mTimerList.Contains(delayObject as TimerItemBase))
            {
                if (this.isInDepth())
                {
                    base.addObject(delayObject, priority);
                }
                else
                {
                    this.mTimerList.Add(delayObject as TimerItemBase);
                }
            }
        }

        protected override void removeObject(IDelayHandleItem delayObject)
        {
            // 检查当前是否在队列中
            if (this.mTimerList.Contains(delayObject as TimerItemBase))
            {
                (delayObject as TimerItemBase).mDisposed = true;

                if (this.isInDepth())
                {
                    base.removeObject(delayObject);
                }
                else
                {
                    foreach (TimerItemBase item in this.mTimerList.list())
                    {
                        if (UtilApi.isAddressEqual(item, delayObject))
                        {
                            this.mTimerList.Remove(item);
                            break;
                        }
                    }
                }
            }
        }

        // 从 Lua 中添加定时器，这种定时器尽量整个定时器周期只与 Lua 通信一次
        public void addTimer(TimerItemBase delayObject, float priority = 0.0f)
        {
            this.addObject(delayObject, priority);
        }

        public void addTimer(LuaTable luaTimer)
        {
            LuaTable table = luaTimer["pthis"] as LuaTable;
            LuaFunction function = luaTimer["func"] as LuaFunction;

            TimerItemBase timer = new TimerItemBase();
            timer.mTotalTime = Convert.ToSingle(luaTimer["totaltime"]);
            timer.setLuaFunctor(table, function);

            this.addTimer(timer);
        }

        public void removeTimer(TimerItemBase timer)
        {
            this.removeObject(timer);
        }

        public void Advance(float delta)
        {
            this.incDepth();

            foreach (TimerItemBase timerItem in this.mTimerList.list())
            {
                if (!timerItem.isClientDispose())
                {
                    timerItem.OnTimer(delta);
                }

                if (timerItem.mDisposed)        // 如果已经结束
                {
                    this.removeObject(timerItem);
                }
            }

            this.decDepth();
        }
    }
}