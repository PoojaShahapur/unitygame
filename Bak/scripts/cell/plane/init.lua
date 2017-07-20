-- init.lua
-- 小飞机模块初始化

local M = {}

local log = require("log"):new(...)

local config = require('config')
local excelm = require('excelm')
local shopm = require('plane.shopm')

local function init_rpc_router()
	-- 所有EnterRoom请求都转向功能服"PlaneBalancer",
	-- 由Balancer来指定房间服务器，据此再设置路由，结束后再重设。
	--c_rpc_router.set_mthd_function("plane.Plane", "EnterRoom", "PlaneBalancer")
end  -- init_rpc_router()

function M.loadconfig()
	config:loadconfig()
    excelm:load_objectbm()
    excelm:load_skinbm()
    excelm:load_levelbm()
end

function M.init()
	log:info("Init plane.")
    -- 在 common 里初始化
    --M.loadconfig()
    shopm.load_all_shop()
    --dofile('../scripts/cell/plane/item/10001.lua')
	init_rpc_router()
    --[[
    local http_client = require("httpclient")
    http_client:get('https://yf.ztgame.com/server.php', function (result)            
	        log:info("report_global_count, result:"..result)
        end)
    ]]
    --require("plane.balancer").init()
    --require("plane.hexagon_formation"):print_wrap_radius_map()
    --require("test.redis_test").test()
    --require("test.mongodb_test").test_binary_data()
end  -- init()

return M
