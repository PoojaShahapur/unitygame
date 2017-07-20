#include "mongodb_client.h"

#include "fmt.h"
#include "log.h"
#include "mongoc-uri.h"
#include "mongoc-uri-private.h"

#ifdef WIN32
#ifdef _DEBUG
#pragma comment(lib,"bsond-1.0.lib")
#pragma comment(lib,"mongocd-1.0.lib")
#pragma comment(lib,"libsasld.lib")
#pragma comment(lib,"libeay32.lib")
#pragma comment(lib,"ssleay32.lib")
#else
#pragma comment(lib,"bson-1.0.lib")
#pragma comment(lib,"mongoc-1.0.lib")
#pragma comment(lib,"libsasl.lib")
#pragma comment(lib,"libeay32.lib")
#pragma comment(lib,"ssleay32.lib")
#endif
#endif


MongoDbClient::MongoDbClient()
    : m_client(nullptr)
    , m_stateForDebug(0)
{
}

MongoDbClient::~MongoDbClient()
{
    if (m_client) { mongoc_client_destroy(m_client); m_client = nullptr; }
}

bool MongoDbClient::Init(const std::vector<std::string> &host, std::vector<unsigned> port, const std::string &auth_database, const std::string &user, const std::string &pwd, const std::vector<std::pair<std::string, std::string>> &options)
{
    assert(host.size() == port.size());
    assert(host.size() > 0);
    mongoc_uri_t *uri = nullptr;
    uri = mongoc_uri_new_for_host_port(host[0].c_str(), port[0]);
    for (size_t i = 1; i < host.size() && i < port.size(); i++)
    {
        mongoc_uri_append_host(uri, host[i].c_str(), port[i]);
    }
    if (user != "" && pwd != "")
    {
        mongoc_uri_set_username(uri, user.c_str());
        mongoc_uri_set_password(uri, pwd.c_str());
    }
    mongoc_uri_set_database(uri, auth_database.c_str());
    for (size_t i = 0; i < options.size(); i++)
    {
        mongoc_uri_set_option_as_utf8(uri, options[i].first.c_str(), options[i].second.c_str());
    }
    m_client = mongoc_client_new_from_uri(uri);
    mongoc_uri_destroy(uri);
    return m_client != nullptr;
}

std::vector<std::string> MongoDbClient::Find(const std::string &dbName, const std::string &collectionName, const std::string &queryString, const std::string &fieldsString)
{
    assert(m_stateForDebug == 0);
    std::vector<std::string> retvt;
    MongodbHelper objs;
    bson_error_t error;

    objs.query = bson_new_from_json(reinterpret_cast<const uint8_t*>(queryString.data()), queryString.size(), &error);
    if (!objs.query)
    {
        LOG_ERROR_TO("MongoClinet-Query", Fmt("query string parse failure, error = %s") % error.message);
        return retvt;
    }

    objs.collection = mongoc_client_get_collection(m_client, dbName.c_str(), collectionName.c_str());
    if (!objs.collection)
    {
        LOG_ERROR_TO("MongoClinet-Query", "call mongoc_client_get_collection failure");
        return retvt;
    }

    if (!fieldsString.empty())
    {
        objs.fields = bson_new_from_json(reinterpret_cast<const uint8_t*>(fieldsString.data()), fieldsString.size(), &error);
        if (!objs.fields)
        {
            LOG_ERROR_TO("MongoClinet-Query", Fmt("fields string parse failure, error = %s") % error.message);
            return retvt;
        }
    }
    objs.cursor = mongoc_collection_find(objs.collection, MONGOC_QUERY_NONE, 0, 0, 0, objs.query, objs.fields, NULL);
    if (!objs.cursor)
    {
        LOG_ERROR_TO("MongoClinet-Query", "call mongoc_collection_find failure");
        return retvt;
    }

    const bson_t *doc;
    while (mongoc_cursor_next(objs.cursor, &doc))
    {
        char * str = bson_as_json(doc, NULL);
        if (str)
        {
            retvt.push_back(str);
            bson_free(str);
        }
    }

    if (mongoc_cursor_error(objs.cursor, &error))
    {
        LOG_ERROR_TO("MongoClinet-Query", Fmt("Cursor Failure: %s") % error.message);
        return retvt;
    }

    return retvt;
}

std::vector<bson_t *> MongoDbClient::FindB(const std::string &dbName, const std::string &collectionName, const std::string &queryString, const std::string &fieldsString)
{
    assert(m_stateForDebug == 0);
    std::vector<bson_t *> retvt;
    MongodbHelper objs;
    bson_error_t error;

    objs.query = bson_new_from_json(reinterpret_cast<const uint8_t*>(queryString.data()), queryString.size(), &error);
    if (!objs.query)
    {
        LOG_ERROR_TO("MongoClinet-Query", Fmt("query string parse failure, error = %s") % error.message);
        return retvt;
    }

    objs.collection = mongoc_client_get_collection(m_client, dbName.c_str(), collectionName.c_str());
    if (!objs.collection)
    {
        LOG_ERROR_TO("MongoClinet-Query", "call mongoc_client_get_collection failure");
        return retvt;
    }

    if (!fieldsString.empty())
    {
        objs.fields = bson_new_from_json(reinterpret_cast<const uint8_t*>(fieldsString.data()), fieldsString.size(), &error);
        if (!objs.fields)
        {
            LOG_ERROR_TO("MongoClinet-Query", Fmt("fields string parse failure, error = %s") % error.message);
            return retvt;
        }
    }
    objs.cursor = mongoc_collection_find(objs.collection, MONGOC_QUERY_NONE, 0, 0, 0, objs.query, objs.fields, NULL);
    if (!objs.cursor)
    {
        LOG_ERROR_TO("MongoClinet-Query", "call mongoc_collection_find failure");
        return retvt;
    }

    const bson_t *doc;
    while (mongoc_cursor_next(objs.cursor, &doc))
    {
        retvt.push_back(bson_copy(doc));
    }

    if (mongoc_cursor_error(objs.cursor, &error))
    {
        LOG_ERROR_TO("MongoClinet-Query", Fmt("Cursor Failure: %s") % error.message);
        return retvt;
    }

    return retvt;
}

bool MongoDbClient::BulkCreate(const std::string &dbName, const std::string &collectionName)
{
    if (m_bulkObjs.bulk || m_bulkObjs.collection)
    {
        LOG_ERROR_TO("MongoClinet-Bulk", Fmt("bulk state failure"));
    }
    m_bulkObjs = MongodbHelper();
    m_bulkObjs.collection = mongoc_client_get_collection(m_client, dbName.c_str(), collectionName.c_str());
    if (!m_bulkObjs.collection)
    {
        LOG_ERROR_TO("MongoClinet-Bulk", "call mongoc_client_get_collection failure");
        return false;
    }

    m_bulkObjs.bulk = mongoc_collection_create_bulk_operation(m_bulkObjs.collection, true, NULL);
    m_stateForDebug = 1;
    return true;
}

int MongoDbClient::BulkOptRemove(const std::string &queryString)
{
    if (!m_bulkObjs.bulk)
    {
        LOG_ERROR_TO("MongoClinet-Bulk", "bulk no init");
        return 1;
    }

    MongodbHelper objs;
    bson_error_t error;
    objs.query = bson_new_from_json(reinterpret_cast<const uint8_t*>(queryString.data()), queryString.size(), &error);
    if (!objs.query)
    {
        LOG_ERROR_TO("MongoClinet-BulkOptRemove", Fmt("query string parse failure, error = %s") % error.message);
        return 1;
    }
    mongoc_bulk_operation_remove(m_bulkObjs.bulk, objs.query);
    return 0;
}

int MongoDbClient::BulkOptInsert(const std::string &docString, int docType)
{
    if (!m_bulkObjs.bulk)
    {
        LOG_ERROR_TO("MongoClinet-Bulk", "bulk no init");
        return 1;
    }

    MongodbHelper objs;
    bson_error_t error;
    if (docType == 1)
    {
        objs.doc = bson_new_from_json(reinterpret_cast<const uint8_t*>(docString.data()), docString.size(), &error);

    }
    else if (docType == 2)
    {
        strncpy(error.message, "bson data error", sizeof(error.message));
        objs.doc = bson_new_from_data(reinterpret_cast<const uint8_t*>(docString.data()), docString.size());
    }
    else
    {
        strncpy(error.message, "doc type error", sizeof(error.message));
    }
    if (!objs.doc)
    {
        LOG_ERROR_TO("MongoClinet-BulkOptInsert", Fmt("doc string parse failure, error = %s") % error.message);
        return 1;
    }
    mongoc_bulk_operation_insert(m_bulkObjs.bulk, objs.doc);
    return 0;
}

int MongoDbClient::BulkOptUpdate(const std::string &queryString, const std::string &docString, int docType, bool upsert)
{
    if (!m_bulkObjs.bulk)
    {
        LOG_ERROR_TO("MongoClinet-Bulk", "bulk no init");
        return 1;
    }

    MongodbHelper objs;
    bson_error_t error;
    objs.query = bson_new_from_json(reinterpret_cast<const uint8_t*>(queryString.data()), queryString.size(), &error);
    if (!objs.query)
    {
        LOG_ERROR_TO("MongoClinet-BulkOptUpdate", Fmt("query string parse failure, error = %s") % error.message);
        return 1;
    }
    if (docType == 1)
    {
        objs.doc = bson_new_from_json(reinterpret_cast<const uint8_t*>(docString.data()), docString.size(), &error);

    }
    else if (docType == 2)
    {
        strncpy(error.message, "bson data error", sizeof(error.message));
        objs.doc = bson_new_from_data(reinterpret_cast<const uint8_t*>(docString.data()), docString.size());
    }
    else
    {
        strncpy(error.message, "doc type error", sizeof(error.message));
    }
    if (!objs.doc)
    {
        LOG_ERROR_TO("MongoClinet-BulkOptUpdate", Fmt("doc string parse failure, error = %s") % error.message);
        return 1;
    }
    mongoc_bulk_operation_update(m_bulkObjs.bulk, objs.query, objs.doc, upsert);
    return 0;
}

int MongoDbClient::BuikOptReplace(const std::string &queryString, const std::string &docString, int docType, bool upsert)
{
    if (!m_bulkObjs.bulk)
    {
        LOG_ERROR_TO("MongoClinet-Bulk", "bulk no init");
        return 1;
    }

    MongodbHelper objs;
    bson_error_t error;
    objs.query = bson_new_from_json(reinterpret_cast<const uint8_t*>(queryString.data()), queryString.size(), &error);
    if (!objs.query)
    {
        LOG_ERROR_TO("MongoClinet-BuikOptReplace", Fmt("query string parse failure, error = %s") % error.message);
        return 1;
    }
    if (docType == 1)
    {
        objs.doc = bson_new_from_json(reinterpret_cast<const uint8_t*>(docString.data()), docString.size(), &error);

    }
    else if (docType == 2)
    {
        strncpy(error.message, "bson data error", sizeof(error.message));
        objs.doc = bson_new_from_data(reinterpret_cast<const uint8_t*>(docString.data()), docString.size());
    }
    else
    {
        strncpy(error.message, "doc type error", sizeof(error.message));
    }
    if (!objs.doc)
    {
        LOG_ERROR_TO("MongoClinet-BuikOptReplace", Fmt("doc string parse failure, error = %s") % error.message);
        return 1;
    }
    mongoc_bulk_operation_replace_one(m_bulkObjs.bulk, objs.query, objs.doc, upsert);
    return 0;
}

int MongoDbClient::BulkExecute()
{
    if (!m_bulkObjs.bulk)
    {
        LOG_ERROR_TO("MongoClinet-Bulk", "bulk no init");
        return 1;
    }
    MongodbHelper objs;
    bson_error_t error;
    objs.reply = bson_new();
    unsigned ret = mongoc_bulk_operation_execute(m_bulkObjs.bulk, objs.reply, &error);
    if (ret == 0) {
        char *str = bson_as_json(objs.reply, NULL);
        LOG_ERROR_TO("MongoClinet-BulkExecute", Fmt("Error: %s, Reply: %s") % error.message % str);
        bson_free(str);
    }
    m_bulkObjs.Release();
    m_stateForDebug = 0;
    return ret == 0 ? 1 : 0;
}