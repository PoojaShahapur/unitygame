#ifndef SVR_COMMON_RPC_RPC_cALL_CONTEXT_H
#define SVR_COMMON_RPC_RPC_cALL_CONTEXT_H

#include "game_clt_id.h"  // for CGameCltId

#include <cassert>  // for assert()

// Rpc调用的上下文
class CRpcCallContext final
{
public:
	// Todo: rename RpcCltId to SessionId
	// uRpcCltId is client session ID. Rpc客户ID，可能是另一个服务器。
	// uRpcId 是Rpc ID 顺序号，用于请求应答配对。
	CRpcCallContext(uint64_t uRpcCltId, uint32_t uRpcId);
	// 转发的上下文，ctx是当前Rpc上下文，gameCltId是游戏客户端ID(BaseSvrId+RpcCltId)
	CRpcCallContext(const CRpcCallContext& ctx, const CGameCltId& gameCltId);
	~CRpcCallContext()
	{
		assert(!((m_nValid = 0)));
	}

private:
	CRpcCallContext(uint64_t uRpcCltId, uint32_t uRpcId,
		const CGameCltId& gameCltId);

public:
	uint64_t GetRpcCltId() const { assert(IsValid()); return m_uRpcCltId; }
	uint32_t GetRpcId() const { assert(IsValid()); return m_uRpcId; }

	bool IsForwarded() const
	{
		assert(IsValid());
		return !m_gameCltId.IsLocal();
	}

	const CGameCltId& GetGameCltId() const
	{
		assert(IsValid());
		return m_gameCltId;
	}

private:
	const uint64_t m_uRpcCltId;
	const uint32_t m_uRpcId;

	// (uBaseSvrId, uBaseRpcCltId), (0,m_uRpcCltId)表示本服, 无转发
	const CGameCltId m_gameCltId;

#ifndef NDEBUG
	int m_nValid = 0x5a5a5a5a;
	bool IsValid() const { return 0x5a5a5a5a == m_nValid; }
#endif
};

#endif  // SVR_COMMON_RPC_RPC_cALL_CONTEXT_H
