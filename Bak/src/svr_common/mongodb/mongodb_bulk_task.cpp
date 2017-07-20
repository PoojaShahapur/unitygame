#include "mongodb_bulk_task.h"
#include "mongodb_mgr.h"
#include "detail/mongodb_bulk_data.h"
#include "detail/mongodb_client.h"
#include "log.h"

MongoDbBulkTask::MongoDbBulkTask(const std::string &instName, const std::string &dbName, const std::string &collectionName, unsigned type, const std::string &queryString, const std::string &docString, E_MONGODB_DOC_TYPE docType)
    : m_instName(instName)
    , m_dbName(dbName)
    , m_collectionName(collectionName)
    , m_type(type)
    , m_queryString(queryString)
    , m_docString(docString)
    , m_docType(docType)
    , m_bFirst(true)
    , m_fail(0)
    , m_fail2(0)
{
    m_taskInstName = "MongoDbBulkTask_" + m_instName;
    m_id = MongoDbBulkData::GetId(m_instName, m_dbName, m_collectionName);
}

MongoDbBulkTask::~MongoDbBulkTask()
{
    //LOG_INFO_TO("MongoDbBulkTask", "enter ~MongoDbBulkTask()");
}

void MongoDbBulkTask::Process(void *sharedObj)
{
    if (sharedObj == nullptr)
    {
        LOG_ERROR_TO("MongoDbBulkTask", Fmt("task process failure, no find mongodb obj"));
        m_fail = 1;
        return;
    }
    MongoDbClient *client = (MongoDbClient *)sharedObj;
    bool createRet = client->BulkCreate(m_dbName, m_collectionName);
    if (!createRet)
    {
        m_fail = 1;
        return;
    }

    if (!m_datas.empty())
    {
        auto &datas = m_datas;
        for (size_t i = 0; i < datas.size(); i++)
        {
            auto taskobj = std::static_pointer_cast<MongoDbBulkTask>(datas[i].taskobj);
            switch (datas[i].type)
            {
            case e_bulkopt_remove:
                taskobj->m_fail = client->BulkOptRemove(datas[i].query);
                break;
            case e_bulkopt_insert:
                taskobj->m_fail = client->BulkOptInsert(datas[i].doc, datas[i].docType);
                break;
            case e_bulkopt_update:
                taskobj->m_fail = client->BulkOptUpdate(datas[i].query, datas[i].doc, datas[i].docType, true);
                break;
            case e_bulkopt_replace:
                taskobj->m_fail = client->BuikOptReplace(datas[i].query, datas[i].doc, datas[i].docType, true);
                break;
            }
        }
    }
    m_fail2 = client->BulkExecute();
}

void MongoDbBulkTask::OnCompleted()
{
    // call cb
    for (size_t i = 0; i < m_datas.size(); i++)
    {
        auto taskobj = std::static_pointer_cast<MongoDbBulkTask>(m_datas[i].taskobj);
        if (taskobj->GetBulkCB())
        {
            taskobj->GetBulkCB()(taskobj->m_fail != 0 ? taskobj->m_fail : m_fail2);
        }
    }

    // do next bulk
    MongoDbBulkData::get_mutable_instance().SetDataState(m_id, e_bulkopt_state_idle);
    auto task = std::make_shared<MongoDbBulkTask>(m_instName, m_dbName, m_collectionName, 0, "", "", e_mongodb_doc_type_unknow);
    task->m_bFirst = false;
    task->Go();
}

void *MongoDbBulkTask::CreateSharedObj(unsigned index)
{
    auto &objs = MongoDbMgr::get_mutable_instance().GetDbInstance(m_instName + "_bulk");
    if (index >= objs.size())
    {
        return nullptr;
    }
    return objs[index].get();
}

void MongoDbBulkTask::Go()
{
    if (m_bFirst)
    {
        m_bFirst = false;
        switch (m_type)
        {
        case e_bulkopt_remove:
            MongoDbBulkData::get_mutable_instance().BulkOptRemove(m_id, m_queryString, shared_from_this());
            break;
        case e_bulkopt_insert:
            MongoDbBulkData::get_mutable_instance().BulkOptInsert(m_id, m_docString, m_docType, shared_from_this());
            break;
        case e_bulkopt_update:
            MongoDbBulkData::get_mutable_instance().BulkOptUpdate(m_id, m_queryString, m_docString, m_docType, shared_from_this());
            break;
        case e_bulkopt_replace:
            MongoDbBulkData::get_mutable_instance().BuikOptReplace(m_id, m_queryString, m_docString, m_docType, shared_from_this());
            break;
        }
    }

    unsigned state = MongoDbBulkData::get_mutable_instance().GetDataState(m_id);
    if (state == e_bulkopt_state_idle)
    {
        auto &datas = MongoDbBulkData::get_mutable_instance().GetData(m_id);
        if (!datas.empty())
        {
            m_datas.clear();
            m_datas = std::move(datas);
            MongoDbBulkData::get_mutable_instance().RemoveData(m_id);
            MongoDbBulkData::get_mutable_instance().SetDataState(m_id, e_bulkopt_state_working);
            MultiTask::TaskBase::Go();
        }
    }
}


MongoDbBulkTask_Remove::MongoDbBulkTask_Remove(const std::string &instName, const std::string &dbName, const std::string &collectionName, const std::string &queryString)
    : MongoDbBulkTask(instName, dbName, collectionName, e_bulkopt_remove, queryString, "", e_mongodb_doc_type_unknow)
{}

MongoDbBulkTask_Insert::MongoDbBulkTask_Insert(const std::string &instName, const std::string &dbName, const std::string &collectionName, const std::string &docString, E_MONGODB_DOC_TYPE docType)
    : MongoDbBulkTask(instName, dbName, collectionName, e_bulkopt_insert, "", docString, docType)
{}


MongoDbBulkTask_Update::MongoDbBulkTask_Update(const std::string &instName, const std::string &dbName, const std::string &collectionName, const std::string &queryString, const std::string &docString, E_MONGODB_DOC_TYPE docType)
    : MongoDbBulkTask(instName, dbName, collectionName, e_bulkopt_update, queryString, docString, docType)
{}
