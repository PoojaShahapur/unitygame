// Author: Jin Qing (http://blog.csdn.net/jq0123)

#ifndef SVR_COMMON_GAME_CLT_ID_H
#define SVR_COMMON_GAME_CLT_ID_H

#include <cstdint>
#include <string>

// 游戏客户端ID.
// 用(Base服, Base服上的RpcCltId) 唯一标识游戏客户端。
// Base服是指客户端直接连接的服务器。
// 对应 svr::ForwardRequest::GameCltId.
class CGameCltId
{
public:
	CGameCltId(uint16_t uBaseSvrId, uint64_t uBaseRpcCltId);
	virtual ~CGameCltId();
    CGameCltId& operator=(const CGameCltId&);

public:
	uint16_t GetBaseSvrId() const;
	uint64_t GetBaseRpcCltId() const;

public:
	std::string ToString() const;

	bool Equals(const CGameCltId& gameCltId) const;

	// 是否本服
	bool IsLocal() const;

private:
	uint16_t m_uBaseSvrId;  // 游戏客户端所连接的服务器(Base服)ID
	uint64_t m_uBaseRpcCltId;  // 游戏客户端在Base服上的连接号RpcCltId

#ifndef NDEBUG
	int m_nValid = 0x5a5a5a5a;
	bool IsValid() const { return m_nValid == 0x5a5a5a5a; }
#endif
};  // class CGameCltId

#endif  // SVR_COMMON_GAME_CLT_ID_H
