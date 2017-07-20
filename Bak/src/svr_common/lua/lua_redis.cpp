/// Redis操作.
// @module c_redis

#include "lua_redis.h"

#include "log.h"
#include "redis/redis.h"  // for CRedis
#include "to_function.h"  // for ToFunction<>
#include "util.h"  // for GetRedis()

#include <LuaIntf/LuaIntf.h>
#include <hiredis/hiredis.h>  // to complete redisReply in ReplyCb

using LuaIntf::LuaRef;
using string = std::string;

namespace {

using Lua::ToFunction;
using Util::GetRedis;

void ArgsToStrVec(const LuaRef& luaArgs, StrVec& rArgs)
{
	rArgs.clear();
	if (!luaArgs) return;
	if (!luaArgs.isTable())
	{
		rArgs.emplace_back(luaArgs.toValue<string>());
		return;
	}
	int len = luaArgs.len();
	for (int i = 1; i <= len; ++i)
		rArgs.emplace_back(luaArgs[i].value<string>());
}

// luaArgs有可能是 nil, string, {string}, 或 function.
void Command(const string& sCommand, const string& sKey,
	const LuaRef& luaArgs, const LuaRef& luaReplyCb)
{
	StrVec vArgs;
	LuaRef luaReplyCb2 = luaReplyCb;

	// luaArgs为 function 时， luaReplyCb 必须为 nil， 回调为 luaArgs。
	if (luaArgs.isFunction())
	{
		if (luaReplyCb)
			throw LuaIntf::LuaException("Illegal arguments.");
		luaReplyCb2 = luaArgs;
	}
	else
	{
		ArgsToStrVec(luaArgs, vArgs);
	}
	auto replyCb = ToFunction<CRedis::ReplyCb>(luaReplyCb2);

	GetRedis().Command(sCommand, sKey, vArgs, replyCb);
}

void Set(const string& sKey, const string& sValue, const LuaRef& luaSetCb)
{
	// Default is empty callback.
	auto setCb = ToFunction<CRedis::SetCb>(luaSetCb);
	GetRedis().Set(sKey, sValue, setCb);
}

void Get(const string& sKey, const LuaRef& luaReplyStringCb)
{
	auto replyStringCb = ToFunction<CRedis::ReplyStringCb>(
		luaReplyStringCb);
	GetRedis().Get(sKey, replyStringCb);
}

int GetRedisReplyType(const redisReply* pReply)
{
	assert(pReply);
	return pReply->type;
}

long long GetRedisReplyInteger(const redisReply* pReply)
{
	assert(pReply);
	return pReply->integer;
}

string GetRedisReplyString(const redisReply* pReply)
{
	assert(pReply);
	int type = pReply->type;
	if (!pReply->str) return "";
	if (type == REDIS_REPLY_STRING ||
		type == REDIS_REPLY_STATUS ||
		type == REDIS_REPLY_ERROR)
		return string(pReply->str, pReply->len);
	return "";
}

LuaRef GetRedisReplyElements(lua_State* L, const redisReply& reply)
{
	assert(L);
	auto luaRet = LuaRef::createTable(L);
	if (REDIS_REPLY_ARRAY != reply.type) return luaRet;
	for (size_t i = 0; i < reply.elements; i++)
		luaRet[i+1] = reply.element[i];
	return luaRet;
}

}  // namespace

namespace LuaRedis {

void Bind(lua_State* L)
{
	assert(L);
	using namespace LuaIntf;
	LuaBinding(L).beginModule("c_redis")
		.addFunction("command", &Command,
			LUA_ARGS(string, string, _opt<LuaRef>, _opt<LuaRef>))
		.addFunction("set", &Set)
		.addFunction("get", &Get)
		.beginClass<redisReply>("RedisReply")
			.addPropertyReadOnly("type", &GetRedisReplyType)
			.addPropertyReadOnly("integer", &GetRedisReplyInteger)
			.addPropertyReadOnly("str", &GetRedisReplyString)
			.addPropertyReadOnly("elements", [L](const redisReply* pReply) {
				assert(pReply);
				return GetRedisReplyElements(L, *pReply);
			})
		.endClass()
	.endModule();
}

}  // namespace LuaRedis

/// 通用命令
// @function command
// @string command 命令,如："del", "get"
// @string key 键
// @tparam[opt] string|tab args 参数或参数列表
// @func[opt] cb 回调函数，`void (RedisReply reply)`
// @usage
// c_redis.command("del", "my_key", function(_) print("done") end)
// c_redis.command("set", "my_key", "value")
// c_redis.command("set", "my_key", 1234)  -- same as "1234"
// c_redis.command("set", "my_key", {"value"})
// c_redis command("lrange", "key_list" {2, 5})
// c_redis command("lrange", "key_list" {2, "5"})
// c_redis command("lrange", "key_list" {2, 5}, function(reply) print(#reply.elements) end)
// @see RedisReply
;
/// SET命令.
// @function set
// @string key 键
// @string value 值
// @func[opt] cb 回调函数，`void cb(bool ok)`
// @usage c_redis.set("my_key", "my_value", function(ok) print(ok) end)
;
/// GET命令
// @function get
// @string key 键
// @func[opt] cb 回调函数，`void cb(int reply_type, string reply)`<br>
//   其中 reply\_type = 0(OK), 1(nil), 2(error)
// @usage
// c_redis.get("my_key", function(reply_type, reply)
//     if 0 == reply_type then print("Got value: " .. reply)
//     elseif 1 == reply_type then print("No value.")
//     else print("Error: " .. reply)
//     end)
;
/// RedisReply结构.
// This is the reply object returned by redisCommand()
// @type RedisReply
;
/// 类型.
// @field type 类型
//     #define REDIS_REPLY_STRING 1
//     #define REDIS_REPLY_ARRAY 2
//     #define REDIS_REPLY_INTEGER 3
//     #define REDIS_REPLY_NIL 4
//     #define REDIS_REPLY_STATUS 5
//     #define REDIS_REPLY_ERROR 6
;
/// 数字.
// The integer when type is `REDIS_REPLY_INTEGER`。
// @field integer
;
/// 字符串.
// Used for both `REDIS_REPLY_ERROR` and `REDIS_REPLY_STRING`
// @field str
;
/// 数组.
// elements vector for `REDIS_REPLY_ARRAY`
// @field elements
