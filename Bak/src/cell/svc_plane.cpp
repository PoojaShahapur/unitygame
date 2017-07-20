#include "svc_plane.h"

#include "log.h"
#include "app.h"
#include "util.h"
#include "plane/plane.pb.h"
#include "rpc/rpc_call_context.h"  // for GetRpcId()
#include "rpc/rpc_helper.h"  // for PushRpcResp()
#include "asio/asio_server4c.h"// GetEventHandler()
#include "common_room.h"
#include "bullet_group.h"
#include "timer_queue/timer_queue_root.h"
#include <LuaIntf/LuaIntf.h>  // for LuaRef

const char LOG_NAME[] = "CSvcPlane";

// Cpp服务示例。

CSvcPlane::CSvcPlane()
{
	RegisterMethod("EnterRoom",
		[this](const CRpcCallContext& ctx, const std::string& sContent) {
			EnterRoom(ctx, sContent);
		});
	RegisterMethod("JoinRoom",
		[this](const CRpcCallContext& ctx, const std::string& sContent) {
			JoinRoom(ctx, sContent);
		});
	RegisterMethod("TurnTo",
		[this](const CRpcCallContext& ctx, const std::string& sContent) {
			TurnTo(ctx, sContent);
		});
	RegisterMethod("Fire",
		[this](const CRpcCallContext& ctx, const std::string& sContent) {
			Fire(ctx, sContent);
		});
	RegisterMethod("StopMove",
		[this](const CRpcCallContext& ctx, const std::string& sContent) {
			StopMove(ctx, sContent);
		});
	RegisterMethod("ReqRankData",
		[this](const CRpcCallContext& ctx, const std::string& sContent) {
			ReqRankData(ctx, sContent);
		});
	RegisterMethod("SmallPlaneDie",
		[this](const CRpcCallContext& ctx, const std::string& sContent) {
			SmallPlaneDie(ctx, sContent);
		});
	RegisterMethod("Split",
		[this](const CRpcCallContext& ctx, const std::string& sContent) {
			Split(ctx, sContent);
		});
	RegisterMethod("RunGMCmd",
		[this](const CRpcCallContext& ctx, const std::string& sContent) {
			RunGMCmd(ctx, sContent);
		});
	RegisterMethod("BackHall",
		[this](const CRpcCallContext& ctx, const std::string& sContent) {
			BackHall(ctx, sContent);
		});
	RegisterMethod("ReconnectEnterRoom",
		[this](const CRpcCallContext& ctx, const std::string& sContent) {
			ReconnectEnterRoom(ctx, sContent);
		});
}

void CSvcPlane::ReconnectEnterRoom(const CRpcCallContext& ctx, const std::string& sContent)
{
    CApp::get_const_instance().GetSvr4C().HandleRpcRequestInLua(
            ctx, "plane.Plane", "ReconnectEnterRoom", sContent);
}

void CSvcPlane::EnterRoom(const CRpcCallContext& ctx, const std::string& sContent)
{
	LOG_INFO("EnterRoom");
    CApp::get_const_instance().GetSvr4C().HandleRpcRequestInLua(
            ctx, "plane.Plane", "EnterRoom", sContent);
}

void CSvcPlane::JoinRoom(const CRpcCallContext& ctx, const std::string& sContent)
{
	LOG_INFO("JoinRoom");
    CApp::get_const_instance().GetSvr4C().HandleRpcRequestInLua(
            ctx, "plane.Plane", "JoinRoom", sContent);
}
void CSvcPlane::TurnTo(const CRpcCallContext& ctx, const std::string& sContent)
{
    // 找到该 user 所在房间
	//LOG_INFO("TurnTo");
    auto it = CommonRoom::m_playerSharedPtrMap.find(ctx.GetGameCltId().ToString());
    if (it == CommonRoom::m_playerSharedPtrMap.end())
    {
        return;
    }
	    plane::TurnToMsg req;
        ctx.GetGameCltId().ToString();
	    if (!RpcHelper::ParseMsgFromStr(sContent, req))
		    return;
        it->second->HandleTurnToMsg(req.angle());
}
void CSvcPlane::Fire(const CRpcCallContext& ctx, const std::string& sContent)
{
	LOG_INFO("Fire");
    auto it = CommonRoom::m_playerSharedPtrMap.find(ctx.GetGameCltId().ToString());
    if (it == CommonRoom::m_playerSharedPtrMap.end())
    {
        return;
    }
        it->second->HandleFireMsg();
}
void CSvcPlane::StopMove(const CRpcCallContext& ctx, const std::string& sContent)
{
    auto it = CommonRoom::m_playerSharedPtrMap.find(ctx.GetGameCltId().ToString());
    if (it == CommonRoom::m_playerSharedPtrMap.end())
    {
        return;
    }
	    plane::MoveInfo req;
	    if (!RpcHelper::ParseMsgFromStr(sContent, req))
        {
		    return;
        }
        it->second->HandleMoveMsg(req.is_stop(), req.angle());
}
void CSvcPlane::ReqRankData(const CRpcCallContext& ctx, const std::string& sContent)
{
    auto it = CommonRoom::m_playerSharedPtrMap.find(ctx.GetGameCltId().ToString());
    if (it == CommonRoom::m_playerSharedPtrMap.end())
    {
        return;
    }
    it->second->HandleReqRankMsg();
}
void CSvcPlane::SmallPlaneDie(const CRpcCallContext& ctx, const std::string& sContent)
{
	LOG_INFO("SmallPlaneDie");
	plane::PlaneDieMsg req;
	if (!RpcHelper::ParseMsgFromStr(sContent, req))
		return;
    auto it = CommonRoom::m_playerSharedPtrMap.find(ctx.GetGameCltId().ToString());
    if (it == CommonRoom::m_playerSharedPtrMap.end())
    {
        return;
    }
    it->second->HandleSmallPlaneDieMsg(req.planeid());
}
void CSvcPlane::Split(const CRpcCallContext& ctx, const std::string& sContent)
{
    auto it = CommonRoom::m_playerSharedPtrMap.find(ctx.GetGameCltId().ToString());
    if (it == CommonRoom::m_playerSharedPtrMap.end())
    {
        return;
    }
    it->second->HandleSplitMsg();
}
void CSvcPlane::RunGMCmd(const CRpcCallContext& ctx, const std::string& sContent)
{
    LOG_INFO("RunGMCmd");
    /*
	using LuaIntf::LuaRef;
	LuaRef require(Util::GetLuaState(), "require");
	try
	{
		LuaRef handler = require.call<LuaRef>("rpc_request_handler");
		const CRpcCallContext ctx_copy(ctx);
		handler.dispatchStatic("handle", ctx_copy, "plane.Plane", "RunGMCmd", sContent);
		// Todo: Register Lua service directly. No rpc_request_handler.
	}
	catch (const LuaIntf::LuaException& e)
	{
		LOG_ERROR("Failed to call lua rpc_request_handler.handle(), "
			<< e.what());
	}
    */
    CApp::get_const_instance().GetSvr4C().HandleRpcRequestInLua(
            ctx, "plane.Plane", "RunGMCmd", sContent);
}
void CSvcPlane::BackHall(const CRpcCallContext& ctx, const std::string& sContent)
{
	LOG_INFO("BackHall");
    CApp::get_const_instance().GetSvr4C().HandleRpcRequestInLua(
            ctx, "plane.Plane", "BackHall", sContent);
}
