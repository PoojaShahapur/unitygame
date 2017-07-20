#ifndef SVR_COMMON_UTIL_H
#define SVR_COMMON_UTIL_H

#include <cassert>
#include <cstdint>  // for uint16_t
#include <string>

class CRedis;
struct lua_State;

namespace Util {

bool SetServerIdEnv(uint16_t uServerId);

namespace detail {
uint16_t GetMySvrId();
}  // namespace detail

inline uint16_t GetMySvrId()  // 导出到 lua get_my_svr_id()
{
	static uint16_t s_uSvrId = detail::GetMySvrId();
	return s_uSvrId;
}

uint16_t GetRandSvrId();  // 导出到 lua get_rand_svr_id()
lua_State* GetLuaState();
CRedis& GetRedis();

// 是否本服ID
inline bool IsMySvrId(uint16_t uServerId)
{
	return uServerId == GetMySvrId();
}

// 毫秒时戳值
uint64_t GetMs();
uint64_t genObjectID();
uint64_t getSystemMs();

bool VerifyGiantLogin(const std::string& entites_str, const std::string& sign);
std::string md5(const std::string& data);
std::string GenerateRandToken(bool isBinary = true);
void Encrypt_XOR(char* szData, size_t uSize, const char* szKey, size_t keySize);

// 获取功能服ID
uint16_t GetFunctionSvrId(const std::string& sFunction);

// 检查服务器ID是否有效,(该服务器是否还在集群中)
bool IsValidSvrId(uint16_t uSvrId);

// 服务器主动关闭当前的游戏客户端连接. 只能是本服客户端。
void DisconnectGameClient(uint64_t uCltId);

}  // namespace Util
#endif  // SVR_COMMON_UITL_H
