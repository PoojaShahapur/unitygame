// Author: Jin Qing (http://blog.csdn.net/jq0123)

#include "redis.h"

#include "cpp-hiredis-cluster/include/adapters/boostasioadapter.h"  // for Adapter (BoostAsioAdapter)
#include "cpp-hiredis-cluster/include/asynchirediscommand.h"  // for AsyncHiredisCommand
#include "cpp-hiredis-cluster/include/clusterexception.h"  // for ClusterException
#include "cpp-hiredis-cluster/include/hiredisprocess.h"  // for HiredisProcess
#include "log.h"

using Cmd = RedisCluster::AsyncHiredisCommand;

const char LOG_NAME[] = "CRedis";

CRedis::CRedis()
{
}

CRedis::~CRedis()
{
}

bool CRedis::Init(io_service& rIos, const std::string& sHost, uint16_t uPort)
{
	LOG_INFO(Fmt("Init redis: %s:%u") % sHost % uPort);
	m_pAdapter.reset(new Adapter(rIos));
	try
	{
		m_pCluster.reset(Cmd::createCluster(sHost.c_str(), uPort, *m_pAdapter));
		m_bInited = true;
	}
	catch (const RedisCluster::ClusterException &e)
	{
		LOG_ERROR("Cluster exception: " << e.what());
		return false;
	}

	return true;
}

static Cmd::Action HandleException(const RedisCluster::ClusterException &exception,
	RedisCluster::HiredisProcess::processState state)
{
	// Check the exception type.
	// Retry in case of non-critical exceptions.
	if (!dynamic_cast<const RedisCluster::CriticalException*>(&exception))
	{
		LOG_WARN("Exception in processing async redis callback: "
			<< exception.what() << " Retry...");
		// retry to send a command to redis node
		return Cmd::RETRY;
	}
	LOG_ERROR("Critical exception in processing async redis callback: "
		<< exception.what());
	return Cmd::FINISH;
}

void CRedis::CommandF(const std::string& sKey, const ReplyCb& replyCb,
	const char *pformat, ...)
{
	va_list ap;
	va_start(ap, pformat);
	const std::string& sCmd = Cmd::vformat(pformat, ap);
	va_end(ap);
	Cmd::commandStr(*m_pCluster, sKey, sCmd, replyCb, HandleException);
}

void CRedis::Command(const string& sCommand, const string& sKey,
	const StrVec& vArgs, const ReplyCb& replyCb)
{
	int argc = vArgs.size() + 2;
	std::vector<const char*> vArgv{ sCommand.data(), sKey.data() };
	std::vector<size_t> vArgvLen{ sCommand.size(), sKey.size() };
	for (const string& s : vArgs)
	{
		vArgv.emplace_back(s.data());
		vArgvLen.emplace_back(s.size());
	}
	Cmd::commandArgv(*m_pCluster, sKey, argc, &vArgv[0], &vArgvLen[0],
		replyCb, HandleException);
}

static void HandleSetReply(const redisReply *reply, const CRedis::SetCb& setCb)
{
	if (!setCb) return;
	if (reply && reply->type == REDIS_REPLY_STATUS)
	{
		const std::string OK("OK");
		if (OK == reply->str)
		{
			setCb(true);
			return;
		}
	}

	LOG_WARN("Set reply: " << (reply ? reply->str : "none"));
	setCb(false);
}

void CRedis::Set(const string& sKey, const string& sValue, const SetCb& setCb)
{
	assert(m_pCluster);
	if (!m_bInited) return;
	Cmd::commandf2(*m_pCluster, sKey,
		[setCb](const redisReply* reply) {
			HandleSetReply(reply, setCb);
		},
		HandleException,
		"set %b %b",
		sKey.data(), sKey.size(),
		sValue.data(), sValue.size());
}

static void HandleGetReply(const redisReply* reply,
	const CRedis::ReplyStringCb& hdlStrReply)
{
	if (!hdlStrReply) return;
	using Rt = CRedis::ReplyType;
	if (!reply)
	{
		hdlStrReply(Rt::ERR, "");
		return;
	}
	if (reply->type == REDIS_REPLY_NIL)
	{
		hdlStrReply(Rt::NIL, "");
		return;
	}
	std::string sReply(reply->str, reply->len);
	if (reply->type == REDIS_REPLY_STRING)
		hdlStrReply(Rt::OK, sReply);
	else
		hdlStrReply(Rt::ERR, sReply);
}

void CRedis::Get(const std::string& sKey, const ReplyStringCb& hdlStrRepl)
{
	assert(m_pCluster);
	if (!m_bInited) return;
	Cmd::commandf2(*m_pCluster, sKey,
		[hdlStrRepl](const redisReply* reply) {
			HandleGetReply(reply, hdlStrRepl);
		},
		HandleException,
		"get %b", sKey.data(), sKey.size());
}

