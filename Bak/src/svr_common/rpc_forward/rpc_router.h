#ifndef __RPC_ROUTER_HEAD__
#define __RPC_ROUTER_HEAD__

#include "singleton.h"  // for Singleton

#include <cstdint>  // for uint16_t
#include <string>
#include <unordered_map>

// Rcp路由表。根据服务名和方法名查找目的服ID.
// 不同客户端有不同的路由表.
// 全局功能服路由会动态选举目的服。
// 客户端路由优先于全局功能服路由。
// 方法名路由优先于服务名路由。
class CRpcRouter : public Singleton<CRpcRouter>
{
public:
	CRpcRouter();
	virtual ~CRpcRouter();

public:
	using string = std::string;

public:
	// return 0 if no route.
	uint16_t GetDstSvrId(uint64_t uRpcCltId,
		const string& sService, const string& sMethod) const;

	// 设置特定方法的路由，服务的路由不变
	void SetMthdDstSvrId(uint64_t uRpcCltId, const string& sService,
		const string& sMethod, uint16_t uSvrId);
	void ResetMthdDstSvrId(uint64_t uRpcCltId, const string& sService,
		const string& sMethod)
	{
		SetMthdDstSvrId(uRpcCltId, sService, sMethod, 0);  // 设为0表示重置。
	}

	// 设置服务的路由，特定方法的路由不变
	void SetSvcDstSvrId(uint64_t uRpcCltId,
		const string& sService, uint16_t uSvrId);
	void ResetSvcDstSvrId(uint64_t uRpcCltId, const string& sService)
	{
		SetSvcDstSvrId(uRpcCltId, sService, 0);  // 设为0表示重置。
	}

	// 设置服务的功能服。功能服将动态选举。
	void SetSvcFunction(const string& sService, const string& sFunction);
	void ResetSvcFunction(const string& sService);
	// 设置服务方法的功能服。功能服将动态选举。
	void SetMthdFunction(const string& sService, const string& sMethod,
		const string& sFunction);
	void ResetMthdFunction(const string& sService, const string& sMethod);

	// 客户端断开时删除客户相关路由。
	void EraseClient(uint64_t uRpcCltId);
	// 集群中有服务器关停时遍历删除相关路由。
	void EraseSvrId(uint16_t uSvrId);

private:
	struct Router;
	const Router* GetCltRouter(uint64_t uRpcCltId) const;

	// return 0 if no route.
	uint16_t GetDstSvrIdFromRouter(const Router& router,
		const string& sService, const string& sMethod) const;

private:
	using Str2Id = std::unordered_map<string, uint16_t>;
	// 没有则返回0
	uint16_t GetIdFromMap(const Str2Id& str2Id, const string& s) const;
	// 遍历删除Str2Id中ID项，用于路由表中删除目的服ID.
	void EraseIdInStr2Id(uint16_t uId, Str2Id& rStr2Id) const;

	using Str2Str = std::unordered_map<string, string>;
	const string* GetStrFromMap(const Str2Str& str2Str, const string& sKey) const;
	const string* GetFunction(const string& sService, const string& sMethod) const;

private:
	struct Router
	{
		// 多数服务是全部转发，个别方法可能会转发到另一个服。
		// 如副本配对请求会发到配对服务器，而其他请求会发到副本服。
		using Svc2SvrId = Str2Id;  // 服务->SvrId
		using Mthd2SvrId = Str2Id;  // 服务方法->SvrId
		Svc2SvrId svc2SvrId;  // 服务转发
		Mthd2SvrId mthd2SvrId;  // 服务方法名转发
	};

	// 按客户端不同而路由不同, uRpcCltId -> Router
	using RpcCltId2Router = std::unordered_map<uint64_t, Router>;
	RpcCltId2Router m_rpcCltId2Router;

	// 所有客户端都相同的功能服路由
	using Svc2Function = Str2Str;
	using Mthd2Function = Str2Str;
	Svc2Function m_svc2Function;
	Mthd2Function m_mthd2Function;
};  // class CRpcRouter

#endif  // __RPC_ROUTER_HEAD__
