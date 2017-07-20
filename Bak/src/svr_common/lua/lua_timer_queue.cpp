#include "lua_timer_queue.h"

#include "log.h"
#include "timer_queue/timer_queue_root.h"  // for TimerQueueRoot

#include <LuaIntf/LuaIntf.h>  // for LuaRef

const char LOG_NAME[] = "LuaTimerQueue";

LuaTimerQueue::LuaTimerQueue()
{
	LOG_DEBUG("LuaTimerQueue() " << this);
	Reset();
}

LuaTimerQueue::~LuaTimerQueue()
{
	LOG_DEBUG("~LuaTimerQueue() " << this);
}

static void OnLuaTimer(const LuaIntf::LuaRef& luaAct)
{
	LuaIntf::LuaRef luaAct2 = luaAct;  // no const
	try
	{
		luaAct2();
	}
	catch(const LuaIntf::LuaException& e)
	{
		LOG_ERROR("Exception in Lua timer action, " << e.what());
	}
}

LuaTimerQueue::TimerId LuaTimerQueue::InsertRepeatFromNow(
	float fSecFromNow, float fIntervalSec, const LuaRef & luaAct)
{
	luaAct.checkFunction();
	assert(m_pTimerQueue);
	return m_pTimerQueue->InsertRepeatFromNow(
		static_cast<unsigned int>(fSecFromNow),
		static_cast<unsigned int>(fIntervalSec),
		[luaAct]() { OnLuaTimer(luaAct); });
}

void LuaTimerQueue::Erase(TimerId id)
{
	assert(m_pTimerQueue);
	if (0 == id) return;
	m_pTimerQueue->Erase(id);
}

void LuaTimerQueue::EraseAll()
{
	Reset();
}

void LuaTimerQueue::Reset()
{
	m_pTimerQueue.reset(new TimerQueue);  // unique_ptr

	// 挂到根队列上
	TimerQueueRoot& rTimerQueueRoot = TimerQueueRoot::Get();
	m_pTimerQueue->SetParent(&rTimerQueueRoot);
}

void LuaTimerQueue::Bind(lua_State* L)
{
	assert(L);
	using namespace LuaIntf;
	LuaBinding(L).beginModule("c_timer_queue").beginClass<LuaTimerQueue>("CTimerQueue")
		.addConstructor(LUA_ARGS())
		.addFunction("insert_single_from_now", &LuaTimerQueue::InsertSingleFromNow)
		.addFunction("insert_repeat_now", &LuaTimerQueue::InsertRepeatNow)
		.addFunction("insert_repeat_from_now", &LuaTimerQueue::InsertRepeatFromNow)
		.addFunction("erase", &LuaTimerQueue::Erase, LUA_ARGS(_opt<TimerId>))
		.addFunction("erase_all", &LuaTimerQueue::EraseAll)
	.endClass().endModule();
}


