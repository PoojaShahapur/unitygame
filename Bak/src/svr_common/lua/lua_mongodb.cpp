#include "lua_mongodb.h"

#include "lua/event_to_lua.h"
#include "fmt.h"
#include "mongodb/mongodb_query_task.h"
#include "mongodb/mongodb_bulk_task.h"
#include "mongodb/mongodb_mgr.h"

#include <LuaIntf/LuaIntf.h>

#include <functional>

namespace LuaIntf
{
	LUA_USING_LIST_TYPE(std::vector)
}


namespace {

	bool mongo_create_inst(const std::string& dbinst, const std::vector<std::string> &host, std::vector<unsigned> port, const std::string &auth_database, const std::string &user, const std::string &pwd, const std::string &options = "")
	{
		std::string options_ = "";
		if (host.size() > 1)
		{
			options_ = "replicaSet=" + dbinst;
			if (options.size() != 0)
			{
				options_ += "&" + options;
			}
		}
		else
		{
			options_ = options;
		}
		return MongoDbMgr::get_mutable_instance().CreateDbInstance(dbinst, host, port, auth_database, user, pwd, options_);
	}

    static uint32_t g_op_id = 0;
    void __mongo_bulk(uint32_t id, int result)
    {
        std::string lua_event_id = (Fmt("__mongo_bulk_%u") % id).str();
        CEventToLua().Dispatch(lua_event_id, result);
    }

    uint32_t mongo_remove(const std::string& dbinst, const std::string &dbname, const std::string &collection, const std::string &query)
	{
		MongoRemove(dbinst, dbname, collection, query)->SetBulkCB(std::bind(__mongo_bulk, ++g_op_id, std::placeholders::_1))->Go();
        return g_op_id;
	}

	uint32_t mongo_insert(const std::string& dbinst, const std::string &dbname, const std::string &collection, const std::string &doc)
	{
		MongoInsert(dbinst, dbname, collection, doc)->SetBulkCB(std::bind(__mongo_bulk, ++g_op_id, std::placeholders::_1))->Go();
        return g_op_id;
	}

    uint32_t mongo_update(const std::string& dbinst, const std::string &dbname, const std::string &collection, const std::string &query, const std::string &doc)
	{
		MongoUpdate(dbinst, dbname, collection, query, doc)->SetBulkCB(std::bind(__mongo_bulk, ++g_op_id, std::placeholders::_1))->Go();
        return g_op_id;
	}

    uint32_t mongo_insert_by_bson(const std::string& dbinst, const std::string &dbname, const std::string &collection, const std::string &doc)
	{
		MongoInsertB(dbinst, dbname, collection, doc)->SetBulkCB(std::bind(__mongo_bulk, ++g_op_id, std::placeholders::_1))->Go();
        return g_op_id;
	}

    uint32_t mongo_update_by_bson(const std::string& dbinst, const std::string &dbname, const std::string &collection, const std::string &query, const std::string &doc)
	{
		MongoUpdateB(dbinst, dbname, collection, query, doc)->SetBulkCB(std::bind(__mongo_bulk, ++g_op_id, std::placeholders::_1))->Go();
        return g_op_id;
	}

	void __mongo_query(uint32_t id, const std::vector<std::string> &retdata)
	{
		std::string lua_event_id = (Fmt("__mongo_query_%u") % id).str();
		CEventToLua().Dispatch(lua_event_id, retdata);
	}

	static uint32_t g_query_id = 0;

	uint32_t mongo_query(const std::string &dbinst, const std::string &dbname, const std::string &collection, const std::string &query = "{}", const std::string &fields = "")
	{
		MongoQuery(dbinst, dbname, collection, query, fields)->SetQueryCB(std::bind(__mongo_query, ++g_query_id, std::placeholders::_1))->Go();
		return g_query_id;
	}

	void __mongo_query_b(uint32_t id, const std::vector<bson_t *> &retdata)
	{
		std::vector<std::string> datas;
		for (size_t i = 0; i < retdata.size(); i++)
		{
			const char* ptr = (const char*)bson_get_data(retdata[i]);
			unsigned len = retdata[i]->len;
			if (ptr != nullptr)
			{
				datas.emplace_back(std::string(ptr, len));
			}
			else
			{
				datas.emplace_back("");
			}
		}
		__mongo_query(id, datas);
	}

	uint32_t mongo_query_by_bson(const std::string &dbinst, const std::string &dbname, const std::string &collection, const std::string &query = "{}", const std::string &fields = "")
	{
		MongoQuery(dbinst, dbname, collection, query, fields)->SetQueryCBB(std::bind(__mongo_query_b, ++g_query_id, std::placeholders::_1))->Go();
		return g_query_id;
	}

}  // namespace

namespace LuaMongoDb {

	void Bind(lua_State* L)
	{
		assert(L);
		LuaIntf::LuaBinding(L).beginModule("c_mongodb")
			.addFunction("create_inst", &mongo_create_inst)
			.addFunction("remove", &mongo_remove)
			.addFunction("insert", &mongo_insert)
			.addFunction("update", &mongo_update)
			.addFunction("query", &mongo_query)
			.addFunction("insert_b", &mongo_insert_by_bson)
			.addFunction("update_b", &mongo_update_by_bson)
			.addFunction("query_b", &mongo_query_by_bson)
			.endModule();
	}

}  // namespace LuaMongoDb

