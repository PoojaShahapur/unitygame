#ifndef __MONGODB_BULK_DATA_HEAD__
#define __MONGODB_BULK_DATA_HEAD__

#include <string>
#include <vector>
#include <unordered_map>

#include "singleton.h"
#include "fmt.h"
#include "multi_task/task.h"

enum E_OperateState
{
    e_bulkopt_state_idle = 0,
    e_bulkopt_state_working = 1,
};

enum E_OperateEnum
{
    e_bulkopt_remove = 1,
    e_bulkopt_insert = 2,
    e_bulkopt_update = 3,
    e_bulkopt_replace = 4,
};

struct S_MongoDbDataType
{
    E_OperateEnum type;
    std::string query;
    std::string doc;
    int docType;
    std::shared_ptr<MultiTask::TaskBase> taskobj;

    S_MongoDbDataType(
        E_OperateEnum type_,
        const std::string &query_,
        const std::string &doc_,
        int docType_,
        const std::shared_ptr<MultiTask::TaskBase> &taskobj_)
        : type(type_)
        , query(std::move(query_))
        , doc(std::move(doc_))
        , docType(docType_)
        , taskobj(taskobj_)
    {
    }

    S_MongoDbDataType(S_MongoDbDataType&& other)
        : type(other.type)
        , query(std::move(other.query))
        , doc(std::move(other.doc))
        , docType(std::move(other.docType))
        , taskobj(other.taskobj)
    {
    }

    S_MongoDbDataType(const S_MongoDbDataType& other) = delete;
    S_MongoDbDataType& operator=(const S_MongoDbDataType& other) = delete;
};

using MongoDbDataTypeVector = std::vector<S_MongoDbDataType>;

class MongoDbBulkData : public Singleton<MongoDbBulkData>
{
public:
    void BulkOptRemove(const std::string &id, const std::string &queryString, const std::shared_ptr<MultiTask::TaskBase> &taskobj);
    void BulkOptInsert(const std::string &id, const std::string &docString, int docType, const std::shared_ptr<MultiTask::TaskBase> &taskobj);
    void BulkOptUpdate(const std::string &id, const std::string &queryString, const std::string &docString, int docType, const std::shared_ptr<MultiTask::TaskBase> &taskobj);
    void BuikOptReplace(const std::string &id, const std::string &queryString, const std::string &docString, int docType, const std::shared_ptr<MultiTask::TaskBase> &taskobj);

public:
    MongoDbDataTypeVector &GetData(const std::string &id);
    void RemoveData(const std::string &id);
    unsigned GetDataState(const std::string &id);
    void SetDataState(const std::string &id, unsigned state);

public:
    static inline std::string GetId(const std::string &instName, const std::string &dbName, const std::string &collectionName)
    {
        return (Fmt("%s_%s_%s") % instName % dbName % collectionName).str();
    }

private:
    std::unordered_map<std::string, MongoDbDataTypeVector> m_datas;
    std::unordered_map<std::string, unsigned> m_dataState;
};

#endif  //__MONGODB_BULK_DATA_HEAD__
