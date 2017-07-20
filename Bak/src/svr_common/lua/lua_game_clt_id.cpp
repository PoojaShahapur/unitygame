#include "lua_game_clt_id.h"

#include "game_clt_id.h"  // for CGameCltId

#include <LuaIntf/LuaIntf.h>

namespace LuaGameCltId {

void Bind(lua_State* L)
{
	assert(L);
	using namespace LuaIntf;
	LuaBinding(L).beginModule("c_game_clt_id")
		.beginClass<CGameCltId>("CGameCltId")
			.addConstructor(LUA_ARGS(uint16_t, uint64_t))
			// Todo: addPropertyReadOnly?
			.addProperty("base_svr_id", &CGameCltId::GetBaseSvrId)
			.addProperty("base_rpc_clt_id", &CGameCltId::GetBaseRpcCltId)
			.addFunction("to_string", &CGameCltId::ToString)
			.addFunction("equals", &CGameCltId::Equals)
			.addFunction("is_local", &CGameCltId::IsLocal)
		.endClass()
	.endModule();
}

}  // namespace LuaGameCltId
