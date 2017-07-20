#include "mongodb_mgr.h"

#include "detail/mongodb_client.h"
#include "multi_task/multi_task_mgr.h"
#include "log.h"

#include <boost/algorithm/string.hpp>

MongoDbMgr::MongoDbMgr()
{
	mongoc_init();
}

MongoDbMgr::~MongoDbMgr()
{
	mongoc_cleanup();
}

bool MongoDbMgr::CreateDbInstance(const std::string &instName, const std::vector<std::string> &host, std::vector<unsigned> port, const std::string &auth_database, const std::string &user, const std::string &pwd, const std::string &options)
{
	std::vector<std::pair<std::string, std::string>> options_;
	if (options.size() != 0)
	{
		std::vector<std::string> items;
		boost::algorithm::split(items, options, boost::is_any_of("&;"), boost::algorithm::token_compress_on);
		for (size_t i = 0; i < items.size(); i++)
		{
			std::vector<std::string> keyvalue;
			boost::algorithm::split(keyvalue, items[i], boost::is_any_of("="), boost::algorithm::token_compress_on);
			if (keyvalue.size() >= 2)
			{
				options_.push_back({ keyvalue[0], keyvalue[1] });
			}
		}
	}

	bool r1 = CreateDbInstanceDetail(instName + "_bulk", host, port, auth_database, user, pwd, options_);
	bool r2 = CreateDbInstanceDetail(instName + "_query", host, port, auth_database, user, pwd, options_);
	return r1 & r2;
}

bool MongoDbMgr::CreateDbInstanceDetail(const std::string &instName, const std::vector<std::string> &host, std::vector<unsigned> port, const std::string &auth_database, const std::string &user, const std::string &pwd, const std::vector<std::pair<std::string, std::string>> &options)
{
	if (m_clients.find(instName) != m_clients.end())
	{
		return true;
	}
	unsigned instNum = MultiTask::Mgr::get_mutable_instance().GetThreadNum();
	auto &clients = m_clients.insert(std::make_pair(instName, InstSetType())).first->second;
	for (size_t i = 0; i < instNum; i++)
	{
		auto client = std::make_shared<MongoDbClient>();
		bool ret = client->Init(host, port, auth_database, user, pwd, options);
		if (!ret)
		{
			LOG_ERROR_TO("MongoDbMgr", Fmt("crteate db instance failure, memory error"));
			m_clients.erase(instName);
			return false;
		}
		clients.emplace_back(client);
	}
	return true;
}

MongoDbMgr::InstSetType &MongoDbMgr::GetDbInstance(const std::string &instName)
{
	static InstSetType g_empty_inst;
	auto it = m_clients.find(instName);
	if (it == m_clients.end())
	{
		return g_empty_inst;
	}
	return it->second;
}