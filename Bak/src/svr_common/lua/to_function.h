#include "log.h"

#include <LuaIntf/LuaIntf.h>

namespace Lua {

using LuaRef = LuaIntf::LuaRef;

template<class Function>
class LuaFunction final
{
public:
	explicit LuaFunction(const LuaRef& luaFunction) : m_luaFun(luaFunction)
	{
		assert(luaFunction.isFunction());
	}

public:
	using result_type = typename Function::result_type;

	template<class... Types>
	result_type operator()(Types... args) const
	{
		const char LOG_NAME[] = "LuaFunction";
		LuaRef luaFun(m_luaFun);  // copy from const
		try
		{
			return luaFun.call<result_type>(std::forward<Types>(args)...);
		}
		catch (const LuaIntf::LuaException& e)
		{
			LOG_ERROR("Lua exception in lua function call: " << e.what());
		}
		catch (std::exception& e)
		{
			LOG_ERROR("Exception in Lua function call: " << e.what());
		}
		catch (...)
		{
			LOG_ERROR("Exception in Lua function call.");
		}

		return result_type();
	}

public:
	const LuaRef m_luaFun;
};

template <class Function>
Function ToFunction(const LuaRef& luaFunction)
{
	// Default is empty.
	if (!luaFunction)
		return Function();  // skip nil
	if (luaFunction.isFunction())
		return LuaFunction<Function>(luaFunction);
	LOG_WARN_TO("ToFunction", "Lua function expected, but got "
		<< luaFunction.typeName());
	return Function();
}

}  // namespace Lua
