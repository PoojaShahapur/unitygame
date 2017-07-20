#ifndef __MONGODB_BULK_TASK_HEAD__
#define __MONGODB_BULK_TASK_HEAD__

#include <mongoc.h>
#include <string>
#include <vector>
#include <functional>
#include "multi_task/task.h"
#include "mongodb_define.h"

//MongoDbBulkTask任务， 保持同一数据库同一张表，按序执行数据库更改操作，且不失性能

struct S_MongoDbDataType;

class MongoDbBulkTask : public MultiTask::TaskBase
{
public:
    MongoDbBulkTask(const std::string &instName, const std::string &dbName, const std::string &collectionName, unsigned type, const std::string &queryString, const std::string &docString, E_MONGODB_DOC_TYPE docType);
    ~MongoDbBulkTask();

public:
    const char *GetName() override { return m_taskInstName.c_str(); }
    void Process(void *sharedObj) override;
    void OnCompleted() override;
    void *CreateSharedObj(unsigned index) override;
    void Go() override;

private:
    std::string m_instName;
    std::string m_taskInstName;
    std::string m_dbName;
    std::string m_collectionName;
    unsigned m_type;
    std::string m_queryString;
    std::string m_docString;
    E_MONGODB_DOC_TYPE m_docType;
    std::string m_id;
    bool m_bFirst;

    using DataSetType = std::vector<S_MongoDbDataType>;
    DataSetType m_datas;

public:
    using BulkCBType = std::function<void(int result)>;
    std::shared_ptr<MultiTask::TaskBase> SetBulkCB(const BulkCBType & cb) { m_cb = cb; return shared_from_this(); }
    BulkCBType &GetBulkCB() { return m_cb; }
private:
    BulkCBType m_cb;
    int m_fail;
    int m_fail2;
};

class MongoDbBulkTask_Remove : public MongoDbBulkTask
{
public:
    MongoDbBulkTask_Remove(const std::string &instName, const std::string &dbName, const std::string &collectionName, const std::string &queryString);
};

class MongoDbBulkTask_Insert : public MongoDbBulkTask
{
public:
    MongoDbBulkTask_Insert(const std::string &instName, const std::string &dbName, const std::string &collectionName, const std::string &docString, E_MONGODB_DOC_TYPE docType);
};

class MongoDbBulkTask_Update : public MongoDbBulkTask
{
public:
    MongoDbBulkTask_Update(const std::string &instName, const std::string &dbName, const std::string &collectionName, const std::string &queryString, const std::string &docString, E_MONGODB_DOC_TYPE docType);
};

#define MongoRemove(...) std::make_shared<MongoDbBulkTask_Remove>(__VA_ARGS__)
#define MongoInsert(...) std::make_shared<MongoDbBulkTask_Insert>(__VA_ARGS__, e_mongodb_doc_type_json)
#define MongoUpdate(...) std::make_shared<MongoDbBulkTask_Update>(__VA_ARGS__, e_mongodb_doc_type_json)
#define MongoInsertB(...) std::make_shared<MongoDbBulkTask_Insert>(__VA_ARGS__, e_mongodb_doc_type_bson)
#define MongoUpdateB(...) std::make_shared<MongoDbBulkTask_Update>(__VA_ARGS__, e_mongodb_doc_type_bson)


#endif  //__MONGODB_BULK_TASK_HEAD__
