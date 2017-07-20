#ifndef __MONGODB_QUERY_TASK_HEAD__
#define __MONGODB_QUERY_TASK_HEAD__

#include <mongoc.h>
#include <string>
#include <vector>
#include <functional>
#include "multi_task/task.h"
#include "mongodb_define.h"

class MongoDbQueryTask : public MultiTask::TaskBase
{
public:
	MongoDbQueryTask(const std::string &instName, const std::string &dbName, const std::string &collectionName, const std::string &queryString = "{}", const std::string &fieldsString = "");
	~MongoDbQueryTask();

	using QueryCBType = std::function<void(const std::vector<std::string> &)>;
	std::shared_ptr<MultiTask::TaskBase> SetQueryCB(const QueryCBType & cb) { m_cb = cb; return shared_from_this(); }

	using QueryCBTypeB = std::function<void(const std::vector<bson_t *> &)>;
	std::shared_ptr<MultiTask::TaskBase> SetQueryCBB(const QueryCBTypeB & cb) { m_docType = e_mongodb_doc_type_bson; m_cbB = cb; return shared_from_this(); }
public:
	const char *GetName() override { return m_taskInstName.c_str(); }
	void Process(void *sharedObj) override;
	void OnCompleted() override;
	void *CreateSharedObj(unsigned index) override;

public:
	std::shared_ptr<MongoDbQueryTask> setReturnBson();

private:
	std::string m_instName;
	std::string m_dbName;
	std::string m_collectionName;
	std::string m_queryString;
	std::string m_fieldsString;
	std::vector<std::string> m_result;
	std::vector<bson_t *> m_resultB;
	QueryCBType m_cb;
	QueryCBTypeB m_cbB;
	E_MONGODB_DOC_TYPE m_docType;
	std::string m_taskInstName;
};

#define MongoQuery(...) std::make_shared<MongoDbQueryTask>(__VA_ARGS__)
#define MongoQueryB(...) std::make_shared<MongoDbQueryTask>(__VA_ARGS__)->setReturnBson()

#endif  //__MONGODB_QUERY_TASK_HEAD__
