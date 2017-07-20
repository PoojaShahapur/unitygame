#ifndef SVR_COMMON_LUA_EVENT_TO_LUA_H
#define SVR_COMMON_LUA_EVENT_TO_LUA_H

#include "log.h"

#include <LuaIntf/LuaIntf.h>

#include <string>

// Dispatch event to lua.
class CEventToLua
{
public:
	CEventToLua();
	virtual ~CEventToLua();

public:
	template <typename R = void, typename... P>
	R Dispatch(const std::string& sEventName, P&&... args);

private:
	LuaIntf::LuaRef m_handler;
};  // CEventToLua

/*
调用 Lua event_handler.handle() 并返回结果（可多值）。示例：
	int a, b;
	std::tie(a, b) = eventToLua.Dispatch<std::tuple<int, int>>(
		"Event.Test", arg1, arg2);
以上代码如同调用 Lua:
	local a, b = event_handler.handle("Event.Text", arg1, arg2)
*/
template <typename R, typename... P>
R CEventToLua::Dispatch(const std::string& sEventName, P&&... args)
{
	if (!m_handler) return;
	try
	{
		return m_handler.dispatchStatic<R>("handle", sEventName,
			std::forward<P>(args)...);
	}
	catch (const LuaIntf::LuaException& e)
	{
		LOG_ERROR_TO("CEventToLua",
			"Failed to call event_handler.handle(), " << e.what());
	}
}  // handle()

#endif  // SVR_COMMON_LUA_EVENT_TO_LUA_H
