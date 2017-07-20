#include "event_handler.h"

#include "cluster/cluster.h"  // for DeleteCell()
#include "log.h"
#include "util.h"  // for SetServerIdEnv()
#include "lua/event_to_lua.h"  // for CEventToLua
#include "rpc_forward/rpc_router.h"  // for CRpcRouter

#include <cassert>

const char LOG_NAME[] = "CEventHandler";

CEventHandler::CEventHandler()
	: m_pEventToLua(new CEventToLua)
{
}

CEventHandler::~CEventHandler()
{
	assert(m_pEventToLua);
}

void CEventHandler::OnClientConnected(uint64_t uCltId,
	const std::string& sCltAddr, uint16_t uCltPort)
{
	assert(m_pEventToLua);
	m_pEventToLua->Dispatch("ClientConnected", uCltId, sCltAddr, uCltPort);
}

void CEventHandler::OnClientDisconnected(uint64_t uCltId)
{
	assert(m_pEventToLua);
	m_pEventToLua->Dispatch("ClientDisconnected", uCltId);
	CRpcRouter::get_mutable_instance().EraseClient(uCltId);
}

void CEventHandler::OnS2sDisconnected(uint16_t uSvrId)
{
	LOG_DEBUG("OnS2sDisconnected: " << uSvrId);
	CCluster::get_mutable_instance().DeleteCell(uSvrId);
	CRpcRouter::get_mutable_instance().EraseSvrId(uSvrId);
    if (1 == Util::GetMySvrId())
    {
	    m_pEventToLua->Dispatch("OnS2sDisconnected", uSvrId);
    }
}

void CEventHandler::OnS2sConnected(uint16_t uSvrId)
{
	LOG_DEBUG("OnS2sConnected: " << uSvrId);
	CCluster::get_mutable_instance().OnS2sConnected(uSvrId);
    if (1 == Util::GetMySvrId())
    {
	    m_pEventToLua->Dispatch("OnS2sConnected", uSvrId);
    }
}

