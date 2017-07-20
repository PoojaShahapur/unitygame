--region *.lua
--Date
--此文件由[BabeLua]插件自动生成



--endregion

local log = require("log"):new("database")

--[[
local test_server = {'222.73.63.209'}
local test_port = {27017}
local test_user = 'test'
local test_password = '123@123'
local test_db = 'giantserver_test'
--]]
local test_server = {'192.168.92.3'}
local test_port = {27017}
local test_user = 'plane'
local test_password = 'plane'
local test_db = 'plane'
local test_collection = 't_planeaccounts'
local test_options = ""

local mygamedb = "mygame_inst"
local mdb = require("mongodb"):new(mygamedb,test_server,test_port,test_db,test_user,test_password,test_options)

log:info("initialize database with    user:"..test_user.." db:"..test_db)
mdb.db = test_db
mdb.collection = test_collection
mdb.shorturl = 'http://192.168.93.187:8001/shorturl?url='
return mdb
