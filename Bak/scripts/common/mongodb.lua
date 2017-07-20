--- Mongodb操作
-- @module mongodb

local MongoDB = {}

local log = require("log"):new("mongodb_inst")
local event_handler = require("event_handler.event_handler")
local json = require("json")
local bson = require("bson")

local mongodb_create_inst = c_mongodb.create_inst
local mongodb_remove = c_mongodb.remove
local mongodb_insert = c_mongodb.insert
local mongodb_update = c_mongodb.update
local mongodb_query = c_mongodb.query
local mongodb_insert_b = c_mongodb.insert_b
local mongodb_update_b = c_mongodb.update_b
local mongodb_query_b = c_mongodb.query_b

local function tojson(obj)
	local doc = ''
	if type(obj)=="table" then
		doc = json.encode(obj)
	else
		doc = obj
	end
	return doc
end

function MongoDB:new(inst_name, host, port, auth, user, pswd, options)
	assert("table" == type(self))
	assert(not inst_name or "string" == type(inst_name))
	local inst = {}
	inst.inst_name = inst_name
	setmetatable(inst, self)
	self.__index = self
	inst.is_init = mongodb_create_inst(inst_name, host, port, auth, user, pswd, options)	--该方法可重入，使用者不需要考虑 inst_name相同等问题
	inst.inst_name = inst_name
	return inst
end

function MongoDB:remove(dbname, collection, _query, cb)
	if self.is_init==false then
		log.error("call mongodb_create_inst failed! please check host, port, user, pswd!")
		return
	end
    local id = mongodb_remove(self.inst_name, dbname, collection, tojson(_query))
    if cb ~= nil then
        dispatcher.register_once_handler("__mongo_bulk_" .. tostring(id), cb)
    end
end

function MongoDB:insert(dbname, collection, doc, cb)
	if self.is_init==false then
		log.error("call mongodb_create_inst failed! please check host, port, user, pswd!")
		return
	end
	local id = mongodb_insert(self.inst_name, dbname, collection, tojson(doc))
	if cb ~= nil then
		event_handler.register_once("__mongo_bulk_" .. tostring(id), cb)
	end
end

function MongoDB:update(dbname, collection, _query, doc, cb)
	if self.is_init==false then
		log.error("call mongodb_create_inst failed! please check host, port, user, pswd!")
		return
	end
	local _doc = string.format('{"$set":%s}', tojson(doc))
	local id = mongodb_update(self.inst_name, dbname, collection, tojson(_query), _doc)
	if cb ~= nil then
		event_handler.register_once("__mongo_bulk_" .. tostring(id), cb)
	end
end

function MongoDB:query(dbname, collection, _query, fields, cb)
	if self.is_init==false then
		log.error("call mongodb_create_inst failed! please check host, port, user, pswd!")
		return
	end
	local id = mongodb_query(self.inst_name, dbname, collection, tojson(_query), tojson(fields))
	event_handler.register_once("__mongo_query_" .. tostring(id), cb)
end

function MongoDB:insert_b(dbname, collection, doc, cb)
	if self.is_init==false then
		log.error("call mongodb_create_inst failed! please check host, port, user, pswd!")
		return
	end
	local _doc = bson.encode(doc)
	local id = mongodb_insert_b(self.inst_name, dbname, collection, _doc)
	if cb ~= nil then
		event_handler.register_once("__mongo_bulk_" .. tostring(id), cb)
	end
end

function MongoDB:update_b(dbname, collection, _query, modifier, doc , cb)
	if self.is_init==false then
		log.error("call mongodb_create_inst failed! please check host, port, user, pswd!")
		return
	end
    local modifier_str = modifier or "$set"
	local _table = {}
	_table[modifier_str] = doc
	local _doc = bson.encode(_table)
	local id = mongodb_update_b(self.inst_name, dbname, collection, tojson(_query), _doc)
	if cb ~= nil then
		event_handler.register_once("__mongo_bulk_" .. tostring(id), cb)
    else
		event_handler.register_once("__mongo_bulk_" .. tostring(id), function () end)
	end
end

function MongoDB:query_b(dbname, collection, _query, fields, cb)
	if self.is_init==false then
		log.error("call mongodb_create_inst failed! please check host, port, user, pswd!")
		return
	end
	local id = mongodb_query_b(self.inst_name, dbname, collection, tojson(_query), tojson(fields))
	event_handler.register_once("__mongo_query_" .. tostring(id), cb)
end

function MongoDB:get_table(data)
	if data == nil then return nil end
	return bson.decode(data)
end

return MongoDB
