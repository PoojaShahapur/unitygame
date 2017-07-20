#pragma once

#include "rpc/rpc_service.h"  // for CRpcService

// plane/plane.proto
class CSvcPlane final : public CRpcService
{
public:
	CSvcPlane();

public:
	// 服务名，对应proto文件中的service, 带包名。
	std::string GetServiceName() const override { return "plane.Plane"; }

private:
	void EnterRoom(const CRpcCallContext& ctx, const std::string& sContent);
	void JoinRoom(const CRpcCallContext& ctx, const std::string& sContent);
	void TurnTo(const CRpcCallContext& ctx, const std::string& sContent);
	void Fire(const CRpcCallContext& ctx, const std::string& sContent);
	void StopMove(const CRpcCallContext& ctx, const std::string& sContent);
	void ReqRankData(const CRpcCallContext& ctx, const std::string& sContent);
	void SmallPlaneDie(const CRpcCallContext& ctx, const std::string& sContent);
	void Split(const CRpcCallContext& ctx, const std::string& sContent);
	void RunGMCmd(const CRpcCallContext& ctx, const std::string& sContent);
	void BackHall(const CRpcCallContext& ctx, const std::string& sContent);
	void ReconnectEnterRoom(const CRpcCallContext& ctx, const std::string& sContent);
};  // class CSvcPlane
