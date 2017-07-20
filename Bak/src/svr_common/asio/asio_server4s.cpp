/*----------------------------------------------------------------
// Copyright (C) 2016-2017 巨人网络
//
// 模块名：asio_server4s
// 创建者：Jin Qing (http://blog.csdn.net/jq0123)
// 修改者列表：
// 创建日期：2017.2.17
// 模块描述：Asio服务器用于服务器连接
//----------------------------------------------------------------*/

#include "asio_server4s.h"

#include "rpc/rpc_req_handler.h"  // for DisableForward()

const char LOG_NAME[] = "CAsioServer4S";

CAsioServer4S::CAsioServer4S(io_service& rIos)
	: CAsioServer(rIos)
{
	assert(m_pRpcReqHandler);
	// 服务器内部服务禁止转发
	m_pRpcReqHandler->DisableForward();
}

CAsioServer4S::~CAsioServer4S()
{
}

