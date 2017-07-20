#include "mongodb_bulk_data.h"
#include "mongodb/mongodb_define.h"

void MongoDbBulkData::BulkOptRemove(const std::string &id, const std::string &queryString, const std::shared_ptr<MultiTask::TaskBase> &taskobj)
{
    m_datas[id].emplace_back(
        S_MongoDbDataType{ e_bulkopt_remove, queryString , "", e_mongodb_doc_type_unknow, taskobj }
    );
}

void MongoDbBulkData::BulkOptInsert(const std::string &id, const std::string &docString, int docType, const std::shared_ptr<MultiTask::TaskBase> &taskobj)
{
    m_datas[id].emplace_back(
        S_MongoDbDataType{ e_bulkopt_insert, "" , docString, docType, taskobj }
    );
}

void MongoDbBulkData::BulkOptUpdate(const std::string &id, const std::string &queryString, const std::string &docString, int docType, const std::shared_ptr<MultiTask::TaskBase> &taskobj)
{
    m_datas[id].emplace_back(
        S_MongoDbDataType{ e_bulkopt_update, queryString , docString, docType, taskobj }
    );
}

void MongoDbBulkData::BuikOptReplace(const std::string &id, const std::string &queryString, const std::string &docString, int docType, const std::shared_ptr<MultiTask::TaskBase> &taskobj)
{
    m_datas[id].emplace_back(
        S_MongoDbDataType{ e_bulkopt_replace, queryString , docString, docType, taskobj }
    );
}

MongoDbDataTypeVector &MongoDbBulkData::GetData(const std::string &id)
{
    static MongoDbDataTypeVector g_empty_data;
    auto it = m_datas.find(id);
    if (it != m_datas.end())
    {
        return it->second;
    }
    return g_empty_data;
}

void MongoDbBulkData::RemoveData(const std::string &id)
{
    m_datas.erase(id);
}

unsigned MongoDbBulkData::GetDataState(const std::string &id)
{
    auto it = m_dataState.find(id);
    if (it != m_dataState.end())
    {
        return it->second;
    }
    return e_bulkopt_state_idle;
}

void MongoDbBulkData::SetDataState(const std::string &id, unsigned state)
{
    m_dataState[id] = state;
}