#ifndef __MONGODB_CLIENT_HEAD__
#define __MONGODB_CLIENT_HEAD__

#include <mongoc.h>
#include <string>
#include <memory>
#include <vector>

#include "multi_task/task.h"
#include "mongodb_helper.h"

//wrap class for mongo-c-driver api

class MongoDbClient
{
public:
    MongoDbClient();
    ~MongoDbClient();

    bool Init(const std::vector<std::string> &host, std::vector<unsigned> port, const std::string &auth_database, const std::string &user, const std::string &pwd, const std::vector<std::pair<std::string, std::string>> &options);

    // for find
    std::vector<std::string> Find(const std::string &dbName, const std::string &collectionName, const std::string &queryString = "{}", const std::string &fieldsString = "");
    std::vector<bson_t *> FindB(const std::string &dbName, const std::string &collectionName, const std::string &queryString = "{}", const std::string &fieldsString = "");

    // for bulk operation
    bool BulkCreate(const std::string &dbName, const std::string &collectionName);
    int BulkOptRemove(const std::string &queryString);
    int BulkOptInsert(const std::string &docString, int docType);
    int BulkOptUpdate(const std::string &queryString, const std::string &docString, int docType, bool upsert);
    int BuikOptReplace(const std::string &queryString, const std::string &docString, int docType, bool upsert);
    int BulkExecute();

private:
    mongoc_client_t *m_client;
    MongodbHelper m_bulkObjs;
    unsigned m_stateForDebug;
};

#endif  //__MONGODB_CLIENT_HEAD__
