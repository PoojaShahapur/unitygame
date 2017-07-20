// Author: Jin Qing (http://blog.csdn.net/jq0123)

#ifndef SVR_COMMON_REDIS_H
#define SVR_COMMON_REDIS_H

#include "asio/asio_fwd.h"  // for io_service
#include "str_vec.h"  // for StrVec

#include <cstdint>  // for uint16_t
#include <functional>  // for function<>
#include <memory>  // for unique_ptr
#include <string>

namespace RedisCluster {
class BoostAsioAdapter;

template<typename redisConnection>
class DefaultContainer;

template <typename redisConnection, typename ConnectionContainer>
class Cluster;
}

struct redisAsyncContext;
struct redisReply;

// Async redis operations.
class CRedis
{
public:
	CRedis();
	virtual ~CRedis();

public:
	using string = std::string;

public:
	// 初始化是阻塞式的。仅第1次调用有效，忽略重复调用。
	using io_service = boost::asio::io_service;
	bool Init(io_service& rIos, const string& sHost, uint16_t uPort);

public:
	template <class T>
	using function = std::function<T>;

	// 通用的redis应答回调，用于复杂的不常用的命令
	using ReplyCb = function<void (const redisReply* reply)>;

	// As redisCommand(). The format is like fprintf with additional "%b".
	// CommandF("foo", "SET foo %b", value, (size_t)valuelen);
	void CommandF(const std::string& sKey, const ReplyCb& replyCb,
		const char *pformat, ...);

	void Command(const string& sCommand, const string& sKey,
		const StrVec& vArgs, const ReplyCb& replyCb = ReplyCb());

	inline void Command(const string& sCommand, const string& sKey,
		const string& sArg0, const ReplyCb& replyCb = ReplyCb());
	inline void Command(const string& sCommand, const string& sKey,
		const string& sArg0, const string& sArg1,
		const ReplyCb& replyCb = ReplyCb());
	inline void Command(const string& sCommand, const string& sKey,
		const string& sArg0, const string& sArg1, const string& sArg2,
		const ReplyCb& replyCb = ReplyCb());
	inline void Command(const string& sCommand, const string& sKey,
		const string& sArg0, const string& sArg1, const string& sArg2,
		const string& sArg3, const ReplyCb& replyCb = ReplyCb());
	inline void Command(const string& sCommand, const string& sKey,
		const string& sArg0, const string& sArg1, const string& sArg2,
		const string& sArg3, const string& sArg4,
		const ReplyCb& replyCb = ReplyCb());
	inline void Command(const string& sCommand, const string& sKey,
		const string& sArg0, const string& sArg1, const string& sArg2,
		const string& sArg3, const string& sArg4, const string& sArg5,
		const ReplyCb& replyCb = ReplyCb());

	enum class ReplyType
	{
		OK,  // redis replys string/integer/array
		NIL,  // redis replys nil
		ERR,  // redis replys error status
	};

	// 简单的常用命令会自动解析reply, 使用更易用的回调。
	using ReplyStringCb = function<void (ReplyType, const string& sReply)>;
	using ReplyIntegerCb = function<void (ReplyType, long long llResult, const string& sError)>;
	// using ReplyStatusCb = function<void (bool ok, const string& sError)>;

	// set key value [EX seconds] [PX milliseconds] [NX|XX]
	using SetCb = function<void (bool ok)>;
	void Set(const string& sKey, const string& sValue,
		const SetCb& setCb = SetCb());

	void Get(const std::string& sKey,
		const ReplyStringCb& hdlStrRepl = ReplyStringCb());

private:
	using Adapter = RedisCluster::BoostAsioAdapter;
	std::unique_ptr<Adapter> m_pAdapter;
	using Cluster = RedisCluster::Cluster<redisAsyncContext,
		RedisCluster::DefaultContainer<redisAsyncContext> >;
	std::unique_ptr<Cluster> m_pCluster;
	bool m_bInited = false;
};  // class CRedis

void CRedis::Command(const string& sCommand, const string& sKey,
	const string& sArg0, const ReplyCb& replyCb)
{
	Command(sCommand, sKey, StrVec{sArg0}, replyCb);
}

void CRedis::Command(const string& sCommand, const string& sKey,
	const string& sArg0, const string& sArg1, const ReplyCb& replyCb)
{
	Command(sCommand, sKey, StrVec{sArg0, sArg1}, replyCb);
}

void CRedis::Command(const string& sCommand, const string& sKey,
	const string& sArg0, const string& sArg1, const string& sArg2,
	const ReplyCb& replyCb)
{
	Command(sCommand, sKey, StrVec{sArg0, sArg1, sArg2}, replyCb);
}

void CRedis::Command(const string& sCommand, const string& sKey,
	const string& sArg0, const string& sArg1, const string& sArg2,
	const string& sArg3, const ReplyCb& replyCb)
{
	Command(sCommand, sKey, StrVec{sArg0, sArg1, sArg2, sArg3}, replyCb);
}

void CRedis::Command(const string& sCommand, const string& sKey,
	const string& sArg0, const string& sArg1, const string& sArg2,
	const string& sArg3, const string& sArg4, const ReplyCb& replyCb)
{
	Command(sCommand, sKey, StrVec{sArg0, sArg1, sArg2, sArg3, sArg4}, replyCb);
}

void CRedis::Command(const string& sCommand, const string& sKey,
	const string& sArg0, const string& sArg1, const string& sArg2,
	const string& sArg3, const string& sArg4, const string& sArg5,
	const ReplyCb& replyCb)
{
	Command(sCommand, sKey, StrVec{
		sArg0, sArg1, sArg2, sArg3, sArg4, sArg5}, replyCb);
}

#endif  // SVR_COMMON_REDIS_H
