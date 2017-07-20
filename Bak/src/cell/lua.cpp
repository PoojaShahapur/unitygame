#include "lua.h"
#include "normal_room.h"
#include "purgatory_room.h"
#include "timer_queue/timer_queue_root.h"
#include <LuaIntf/LuaIntf.h>
#include "bullet_group.h"

namespace LuaIntf
{
    LUA_USING_LIST_TYPE(std::vector)
    LUA_USING_SHARED_PTR_TYPE(std::shared_ptr)
}

/*
std::shared_ptr<CommonRoom> createRoom(uint32_t id, uint32_t holdNumber)
{
    return std::make_shared<CommonRoom>(id, holdNumber);
}
*/

namespace CellLua{
    uint32_t playerExitRoom(const std::string &gameClientStr)
    {
        auto it = CommonRoom::m_playerSharedPtrMap.find(gameClientStr);
        if (it == CommonRoom::m_playerSharedPtrMap.end())
        {
            return 0;
        }
        uint32_t roomid = it->second->m_pRoom->m_id;
        if (it->second->m_pRoom->m_roomMode == RoomMode_Purgatory)
        {
            it->second->m_pRoom->playerExit(it->second);
        }
        else
        {
            it->second->onOffline();
        }
        return roomid;
    }

    void Bind(lua_State *L)
    {
	    assert(L);
	    using namespace LuaIntf;
	    LuaBinding(L).beginModule("c_room")
		    //.addFunction("createRoom", &createRoom)
		    .addFunction("exitRoom", &playerExitRoom)
		    .beginClass<Room>("Room")
			    //.addConstructor(LUA_ARGS(uint32_t, uint32_t))
                // 下面的代码每次进房间都会报错 bad weak ptr
			    .addConstructor(LUA_SP(std::shared_ptr<Room>), LUA_ARGS(uint32_t))
			    // Todo: addPropertyReadOnly?
			    .addFunction("playerEnter", &CommonRoom::playerEnter)
			    .addFunction("playerReconnect", &CommonRoom::playerReconnect)
			    .addFunction("playerExit", &CommonRoom::playerExit)
			    .addFunction("getmvpid", &CommonRoom::getMVPPlayerUid)
			    .addFunction("get_room_left_can_in_num", &CommonRoom::getLeftCanInPlayerNumber)
			    .addFunction("destroy", &CommonRoom::onDestroy)
			    .addFunction("lua_broadcast", &CommonRoom::luaBroadcast)
			    .addFunction("send_result_data", &CommonRoom::sendRankResultData)
                // lua中时间到了,要将房间内所有玩家的room置为空
			    .addFunction("get_all_player_cltid_str"
                        , &CommonRoom::getAllPlayerGameClientIDStrings
                        , LUA_ARGS(_out<std::vector<std::string>& >)
                        )
			    .addFunction("get_all_player_rankdata" , &CommonRoom::getAllPlayerRankData
                        , LUA_ARGS(_out<std::vector<LuaRankData>& >))
			    .addProperty("nonai_players_count", &CommonRoom::GetPlayersCount)
			    .addProperty("room_id", &CommonRoom::GetRoomID)
			    .addProperty("room_mode", &CommonRoom::GetRoomMode)
		    .endClass()
		    .beginClass<TeamRoom>("TeamRoom")
			    .addConstructor(LUA_SP(std::shared_ptr<TeamRoom>), LUA_ARGS(uint32_t))
			    .addFunction("canThisTeamInRoom", &TeamRoom::canThisTeamInRoom)
			    .addFunction("getmvpid", &CommonRoom::getMVPPlayerUid)
			    .addFunction("lua_broadcast", &CommonRoom::luaBroadcast)
			    .addFunction("playerReconnect", &CommonRoom::playerReconnect)
			    .addProperty("room_mode", &CommonRoom::GetRoomMode)
			    .addFunction("get_all_player_rankdata" , &CommonRoom::getAllPlayerRankData
                        , LUA_ARGS(_out<std::vector<LuaRankData>& >))
			    .addFunction("get_all_player_cltid_str"
                        , &CommonRoom::getAllPlayerGameClientIDStrings
                        , LUA_ARGS(_out<std::vector<std::string>& >)
                        )
		    .endClass()
		    .beginClass<PurgatoryRoom>("PurgatoryRoom")
			    .addConstructor(LUA_SP(std::shared_ptr<PurgatoryRoom>), LUA_ARGS(uint32_t))
			    .addProperty("room_mode", &CommonRoom::GetRoomMode)
			    .addFunction("get_all_player_rankdata" , &CommonRoom::getAllPlayerRankData
                        , LUA_ARGS(_out<std::vector<LuaRankData>& >))
			    .addFunction("lua_broadcast", &CommonRoom::luaBroadcast)
			    .addFunction("playerEnter", &CommonRoom::playerEnter)
			    .addFunction("playerReconnect", &CommonRoom::playerReconnect)
		    .endClass()
		    .beginClass<LuaRankData>("LuaRankData")
			    .addVariableRef("id", &LuaRankData::m_id, false)
			    .addVariableRef("rank", &LuaRankData::m_rank, false)
			    .addVariableRef("nickname", &LuaRankData::m_nickname, false)
			    .addVariableRef("account", &LuaRankData::m_account, false)
			    .addVariableRef("isai", &LuaRankData::m_isai, false)
			    .addVariableRef("base_svr_id", &LuaRankData::m_serverid, false)
			    .addVariableRef("base_rpc_clt_id", &LuaRankData::m_rpcid, false)
			    .addVariableRef("score", &LuaRankData::m_score, false)
			    .addVariableRef("total_killnum", &LuaRankData::m_totalKillNumber, false)
			    .addVariableRef("destroynum", &LuaRankData::m_destroyNumber, false)
			    .addVariableRef("highest_combo", &LuaRankData::m_highestComboNumber, false)
		    .endClass()
	    .endModule();
    }
}
