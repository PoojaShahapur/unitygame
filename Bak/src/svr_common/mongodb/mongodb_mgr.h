#ifndef __MONGODB_MGR_HEAD__
#define __MONGODB_MGR_HEAD__

#include <memory>
#include <string>
#include <vector>
#include <unordered_map>
#include "singleton.h"

class MongoDbClient;

class MongoDbMgr : public Singleton<MongoDbMgr>
{
public:
	MongoDbMgr();
	~MongoDbMgr();

public:
	using InstSetType = std::vector<std::shared_ptr<MongoDbClient>>;

public:
	bool CreateDbInstance(const std::string &instName, const std::vector<std::string> &hosts, std::vector<unsigned> ports, const std::string &auth_database, const std::string &user, const std::string &pwd, const std::string &options = "");
	InstSetType &GetDbInstance(const std::string &instName);

private:
	bool CreateDbInstanceDetail(const std::string &instName, const std::vector<std::string> &hosts, std::vector<unsigned> ports, const std::string &auth_database, const std::string &user, const std::string &pwd, const std::vector<std::pair<std::string, std::string>> &options);
	std::unordered_map<std::string, InstSetType> m_clients;
};

#endif  //__MONGODB_MGR_HEAD__
