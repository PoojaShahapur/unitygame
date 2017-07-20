#ifndef SVR_COMMON_LUA_LUA_TIMER_QUEUE_H
#define SVR_COMMON_LUA_LUA_TIMER_QUEUE_H

#include <memory>  // for unique_ptr<>

namespace LuaIntf {
class LuaRef;
}

class TimerQueue;
struct lua_State;

// 导出到Lua中使用的定时器队列。
class LuaTimerQueue
{
public:
	LuaTimerQueue();
	virtual ~LuaTimerQueue();

public:
	static void Bind(lua_State* L);

public:
	using LuaRef = LuaIntf::LuaRef;
	using TimerId = uint64_t;  // 0为无效值

	// 添加定时器，分为单次定时器和重复定时器. uMsFromNow是多少ms后开始。
	// uIntervalMs为间隔时间，不应该太小，建议最小100ms。
	TimerId InsertSingleFromNow(float fSecFromNow, const LuaRef & luaAct)
	{
		return InsertRepeatFromNow(fSecFromNow, 0, luaAct);
	}
	TimerId InsertRepeatNow(float fIntervalSec, const LuaRef & luaAct)
	{
		return InsertRepeatFromNow(0, fIntervalSec, luaAct);
	}
	TimerId InsertRepeatFromNow(float fSecFromNow,
		float fIntervalSec, const LuaRef & luaAct);

public:
	void Erase(TimerId id);
	// 因为Lua对象不会立即删除，所以须主动调用EraseAll()来清空定时器。
	void EraseAll();

private:
	void Reset();

private:
	std::unique_ptr<TimerQueue> m_pTimerQueue;
};  // class LuaTimerQueue

#endif  // SVR_COMMON_LUA_LUA_TIMER_QUEUE_H
