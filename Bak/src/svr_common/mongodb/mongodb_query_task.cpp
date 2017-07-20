#include "mongodb_query_task.h"
#include "mongodb_mgr.h"
#include "detail/mongodb_client.h"
#include "log.h"

MongoDbQueryTask::MongoDbQueryTask(const std::string &instName, const std::string &dbName, const std::string &collectionName, const std::string &queryString, const std::string &fieldsString)
	: m_instName(instName)
	, m_dbName(dbName)
	, m_collectionName(collectionName)
	, m_queryString(queryString)
	, m_fieldsString(fieldsString)
	, m_docType(e_mongodb_doc_type_json)
{
	m_taskInstName = "MongoDbQueryTask_" + m_instName;
}

MongoDbQueryTask::~MongoDbQueryTask()
{

}

void MongoDbQueryTask::Process(void *sharedObj)
{
	if (sharedObj == nullptr)
	{
		LOG_ERROR_TO("MongoDbQueryTask", Fmt("task process failure, no find mongodb obj"));
		return;
	}
	MongoDbClient *client = (MongoDbClient *)sharedObj;
	if (m_docType == e_mongodb_doc_type_json)
	{
		m_result = client->Find(m_dbName, m_collectionName, m_queryString, m_fieldsString);
	}
	else if (m_docType == e_mongodb_doc_type_bson)
	{
		m_resultB = client->FindB(m_dbName, m_collectionName, m_queryString, m_fieldsString);
	}
	else
	{
		assert(0);
	}
}

void MongoDbQueryTask::OnCompleted()
{
	if (m_docType == e_mongodb_doc_type_json)
	{
		if (m_cb)
		{
			m_cb(m_result);
		}
	}
	else if (m_docType == e_mongodb_doc_type_bson)
	{
		if (m_cbB)
		{
			m_cbB(m_resultB);
		}
		for (size_t i = 0; i < m_resultB.size(); i++)
		{
			if (m_resultB[i])
			{
				bson_destroy(m_resultB[i]);
			}
		}
		m_resultB.clear();
	}
}

void *MongoDbQueryTask::CreateSharedObj(unsigned index)
{
	auto &objs = MongoDbMgr::get_mutable_instance().GetDbInstance(m_instName + "_query");
	if (index >= objs.size())
	{
		return nullptr;
	}
	return objs[index].get();
}

std::shared_ptr<MongoDbQueryTask> MongoDbQueryTask::setReturnBson()
{
	m_docType = e_mongodb_doc_type_bson;
	return std::static_pointer_cast<MongoDbQueryTask>(shared_from_this());
}