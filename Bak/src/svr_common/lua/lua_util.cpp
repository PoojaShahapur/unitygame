#include "lua_util.h"

#include "util.h"  // for GetMySvrId()

#include <LuaIntf/LuaIntf.h>

#include <cassert>

#ifdef ENABLE_GVOE_GPROFTOOLS
#include <profiler.h>
#endif

namespace LuaUtil {

    void Bind(lua_State* L)
    {
        assert(L);
        // 所有导出模块带前缀"c_"
        LuaIntf::LuaBinding(L).beginModule("c_util")
            .addFunction("get_my_svr_id", &Util::GetMySvrId)
            .addFunction("get_rand_svr_id", &Util::GetRandSvrId)
            .addFunction("get_ms", &Util::GetMs)
            .addFunction("get_sys_ms", &Util::getSystemMs)
            .addFunction("gen_objid", &Util::genObjectID)
            .addFunction("verify_giant_login", &Util::VerifyGiantLogin)
            .addFunction("get_function_svr_id", &Util::GetFunctionSvrId)
			.addFunction("is_valid_svr_id", &Util::IsValidSvrId)
			.addFunction("md5", &Util::md5)
			.addFunction("disconnect_game_client", &Util::DisconnectGameClient)

#ifdef ENABLE_GVOE_GPROFTOOLS
            .addFunction("gpreftools_heap_dump", &Profiler::heap_profiler_dump)
            .addFunction("gpreftools_flush", &Profiler::profiler_flush)
#endif

            .endModule();
    }

}  // namespace LuaServer
