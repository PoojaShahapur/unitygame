#include "rpc_req_handler.h"

#include "log.h"
#include "pb/rpc/rpc.pb.h"  // for RpcRequest
#include "rpc_call_context.h"  // for CRpcCallContext
#include "rpc_forward/rpc_forwarder.h"  // for RpcForwarder
#include "rpc_forward/rpc_router.h"  // for CRpcRouter
#include "rpc_service.h"  // for GetServiceName()
#include "util.h"  // for GetLuaState()

#include <LuaIntf/LuaIntf.h>  // for LuaRef

const char LOG_NAME[] = "CRpcReqHandler";

CRpcReqHandler::CRpcReqHandler()
{
}

CRpcReqHandler::~CRpcReqHandler()
{
}

// 注册服务。
// sServiceName须包含包名，如"rpc.TestSvc".
// pService允许为空，表示忽略该服务。
void CRpcReqHandler::RegisterService(
	const std::string& sServiceName,
	RpcServiceSptr pService)
{
	m_mapService[sServiceName] = pService;
}

void CRpcReqHandler::RegisterService(RpcServiceSptr pService)
{
	if (pService)
		RegisterService(pService->GetServiceName(), pService);
}

void CRpcReqHandler::HandleRpcRequest(uint64_t uRpcCltId,
	const rpc::RpcRequest& req)
{
	HandleRpcRequest(CRpcCallContext(uRpcCltId, req.id()),
		req.service(), req.method(), req.content());
}

// 直发和转发Rpc统一处理接口. ctx.IsForwarded()可判断是否转发.
void CRpcReqHandler::HandleRpcRequest(const CRpcCallContext& ctx,
	const std::string& sService, const std::string& sMethod,
	const std::string& sContent)
{
	// 优先转发. 服务器内部互连将禁止转发。仅允许直接客户端请求转发。
	if (!m_bDisableForward &&
		TryToForwardRpcRequest(ctx, sService, sMethod, sContent))
		return;

	// 依次尝试: C++服务, Lua服务
	auto itr = m_mapService.find(sService);
	if (itr != m_mapService.end())
	{
		const RpcServiceSptr& pSvc = (*itr).second;
		if (!pSvc) return;  // 允许注册空服务来禁止服务
		pSvc->CallMethod(ctx, sMethod, sContent);
		return;
	}

	HandleRpcRequestInLua(ctx, sService, sMethod, sContent);
}

void CRpcReqHandler::HandleRpcRequestInLua(const CRpcCallContext& ctx,
	const std::string& sService, const std::string& sMethod,
	const std::string& sContent)
{
	using LuaIntf::LuaRef;
	LuaRef require(Util::GetLuaState(), "require");
	try
	{
		LuaRef handler = require.call<LuaRef>("rpc_request_handler");
		const CRpcCallContext ctx_copy(ctx);
		handler.dispatchStatic("handle", ctx_copy, sService, sMethod, sContent);
		// Todo: Register Lua service directly. No rpc_request_handler.
	}
	catch (const LuaIntf::LuaException& e)
	{
		LOG_ERROR("Failed to call lua rpc_request_handler.handle(), "
			<< e.what());
	}
}

bool CRpcReqHandler::TryToForwardRpcRequest(const CRpcCallContext& ctx,
	const std::string& sService, const std::string& sMethod,
	const std::string& sContent) const
{
	// 仅允许直连客户端请求转发。
	// 不允许多次转发(防止循环)。
	if (ctx.IsForwarded())
		return false;

	//LOG_DEBUG("TryToForward: " << sService << "." << sMethod);
	uint16_t uCellSvrId = CRpcRouter::get_const_instance().GetDstSvrId(
		ctx.GetRpcCltId(), sService, sMethod);
	if (0 == uCellSvrId || Util::IsMySvrId(uCellSvrId))
		return false;  // 无转发

	RpcForwarder::ForwardTo(uCellSvrId, ctx, sService, sMethod, sContent);
	return true;
}
