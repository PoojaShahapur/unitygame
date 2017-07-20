#ifndef __MONGODB_HELPER_HEAD__
#define __MONGODB_HELPER_HEAD__

#include <mongoc.h>

struct MongodbHelper
{
	MongodbHelper()
		: doc(nullptr)
		, query(nullptr)
		, fields(nullptr)
		, reply(nullptr)
		, collection(nullptr)
		, cursor(nullptr)
		, bulk(nullptr)
	{}

	~MongodbHelper()
	{
		Release();
	}

	void Release()
	{
		if (doc) { bson_destroy(doc);doc = nullptr; }
		if (query) { bson_destroy(query);query = nullptr; }
		if (fields) { bson_destroy(fields);fields = nullptr; }
		if (reply) { bson_destroy(reply);reply = nullptr; }
		if (cursor) { mongoc_cursor_destroy(cursor);cursor = nullptr; }
		if (bulk) { mongoc_bulk_operation_destroy(bulk);bulk = nullptr; }
		if (collection) { mongoc_collection_destroy(collection);collection = nullptr; }
	}

	bson_t *doc;
	bson_t *query;
	bson_t *fields;
	bson_t *reply;
	mongoc_collection_t *collection;
	mongoc_cursor_t *cursor;
	mongoc_bulk_operation_t *bulk;
};

#endif  //__MONGODB_HELPER_HEAD__