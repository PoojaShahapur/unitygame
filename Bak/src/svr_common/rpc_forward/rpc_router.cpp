#include "rpc_router.h"

#include "cluster/cluster.h"  // for CCluster
#include "log.h"

const char LOG_NAME[] = "CRpcRouter";

CRpcRouter::CRpcRouter()
{
}

CRpcRouter::~CRpcRouter()
{
}

uint16_t CRpcRouter::GetDstSvrId(uint64_t uRpcCltId,
	const std::string& sService, const std::string& sMethod) const
{
	// 先在自己路由表中查找
	const Router* pCltRtr = GetCltRouter(uRpcCltId);
	if (pCltRtr)
	{
		uint16_t uSvrId = GetDstSvrIdFromRouter(*pCltRtr, sService, sMethod);
		if (uSvrId)
			return uSvrId;
	}

	// 再在全局功能服路由表中查找
	const string* pFunction = GetFunction(sService, sMethod);
	if (!pFunction)
		return 0;
	return CCluster::get_const_instance().GetSvrId(*pFunction);
}

uint16_t CRpcRouter::GetIdFromMap(const Str2Id& str2Id, const string& sKey) const
{
	auto itr = str2Id.find(sKey);
	if (itr == str2Id.end())
		return 0;
	return (*itr).second;
}

const std::string* CRpcRouter::GetStrFromMap(
	const Str2Str& str2Str, const string& sKey) const
{
	auto itr = str2Str.find(sKey);
	if (itr == str2Str.end())
		return 0;
	return &((*itr).second);
}

static inline std::string GetSvcMthdStr(
	const std::string& sService,
	const std::string& sMethod)
{
	return sService + "." + sMethod;
}

const std::string* CRpcRouter::GetFunction(
	const string& sService, const string& sMethod) const
{
	const string& sSvcMthd = GetSvcMthdStr(sService, sMethod);
	const string* pFunction = GetStrFromMap(m_mthd2Function, sSvcMthd);
	if (pFunction)
		return pFunction;
	return GetStrFromMap(m_svc2Function, sService);
}

void CRpcRouter::SetMthdDstSvrId(uint64_t uRpcCltId, const string& sService,
	const string& sMethod, uint16_t uSvrId)
{
	const string& sSvcMthd = GetSvcMthdStr(sService, sMethod);
	LOG_DEBUG(Fmt("Set method route: Clt%u %s -> %u")
		% uRpcCltId % sSvcMthd % uSvrId);
	m_rpcCltId2Router[uRpcCltId].mthd2SvrId[sSvcMthd] = uSvrId;
}

void CRpcRouter::SetSvcDstSvrId(uint64_t uRpcCltId,
	const string& sService, uint16_t uSvrId)
{
	LOG_DEBUG(Fmt("Set service route: Clt%u %s -> %u")
		% uRpcCltId % sService % uSvrId);
	m_rpcCltId2Router[uRpcCltId].svc2SvrId[sService] = uSvrId;
}

void CRpcRouter::SetSvcFunction(const string& sService, const string& sFunction)
{
	LOG_DEBUG(Fmt("Set service function: %s -> %s") % sService % sFunction);
	m_svc2Function[sService] = sFunction;
}

void CRpcRouter::ResetSvcFunction(const string& sService)
{
	LOG_DEBUG(Fmt("Reset service function: %s") % sService);
	m_svc2Function.erase(sService);
}

void CRpcRouter::SetMthdFunction(const string& sService, const string& sMethod,
	const string& sFunction)
{
	const string& sSvcMthd = GetSvcMthdStr(sService, sMethod);
	LOG_DEBUG(Fmt("Set method function: %s -> %s") % sSvcMthd % sFunction);
	m_mthd2Function[sSvcMthd] = sFunction;
}

void CRpcRouter::ResetMthdFunction(const string& sService, const string& sMethod)
{
	const string& sSvcMthd = GetSvcMthdStr(sService, sMethod);
	LOG_DEBUG(Fmt("Reset method function: %s") % sSvcMthd);
	m_mthd2Function.erase(sSvcMthd);
}

// 客户端断开时删除客户相关路由。
void CRpcRouter::EraseClient(uint64_t uRpcCltId)
{
	LOG_DEBUG("Erase router of session_" << uRpcCltId);
	m_rpcCltId2Router.erase(uRpcCltId);
}

// 集群中有服务器关停时遍历删除相关路由。
void CRpcRouter::EraseSvrId(uint16_t uSvrId)
{
	LOG_INFO("Erase all routers to svr_" << uSvrId);
	for (auto& rElem : m_rpcCltId2Router)
	{
		Router& rRouter = rElem.second;
		EraseIdInStr2Id(uSvrId, rRouter.svc2SvrId);
		EraseIdInStr2Id(uSvrId, rRouter.mthd2SvrId);
	}
}

void CRpcRouter::EraseIdInStr2Id(uint16_t uId, Str2Id& rStr2Id) const
{
	for (auto& rElem : rStr2Id)
	{
		if (rElem.second == uId)
			rElem.second = 0;
	}
}

const CRpcRouter::Router* CRpcRouter::GetCltRouter(uint64_t uRpcCltId) const
{
	auto itr = m_rpcCltId2Router.find(uRpcCltId);
	if (itr == m_rpcCltId2Router.end()) return nullptr;
	return &(*itr).second;
}

uint16_t CRpcRouter::GetDstSvrIdFromRouter(const Router& router,
	const string& sService, const string& sMethod) const
{
	// 先按方法查找，如果没有则按服务查找
	const string& sSvcMthd = GetSvcMthdStr(sService, sMethod);
	uint16_t uSvrId = GetIdFromMap(router.mthd2SvrId, sSvcMthd);
	if (uSvrId) return uSvrId;
	return GetIdFromMap(router.svc2SvrId, sService);
}

