/*----------------------------------------------------------------
// Copyright (C) 2016 巨人网络
//
// 模块名：asio_server
// 创建者：Jin Qing (http://blog.csdn.net/jq0123)
// 修改者列表：
// 创建日期：2017.2.17
// 模块描述：Asio服务器用于游戏客户端连接
//----------------------------------------------------------------*/

#include "asio_server4c.h"

#include "rpc/rpc_req_handler.h"  // for HandleRpcRequest()

const char LOG_NAME[] = "CAsioServer4C";

CAsioServer4C::CAsioServer4C(io_service& rIos)
	: CAsioServer(rIos)
{
	setSecureTransportEnabled(true);
}

CAsioServer4C::~CAsioServer4C()
{
}

void CAsioServer4C::HandleForwardedRpcRequest(const CRpcCallContext& ctx,
	const std::string& sService, const std::string& sMethod,
	const std::string& sContent)
{
	m_pRpcReqHandler->HandleRpcRequest(ctx, sService, sMethod, sContent);
}
