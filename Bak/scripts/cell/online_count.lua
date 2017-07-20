-- online_count.lua
-- 全区在线人数计数
-- 定时向统计服发送本服人数。
-- 如果本服是统计服，则向GaSdk统计接口发送统计请求。
-- luacheck: no max line length
-- 接口文档: http://gasdk.ztgame.com/doc/index.html?file=002-%E5%A2%9E%E5%80%BC%E5%8A%9F%E8%83%BD/01-%E5%9C%A8%E7%BA%BF%E4%BA%BA%E6%95%B0%E5%AE%9E%E6%97%B6%E7%BB%9F%E8%AE%A1
-- GaSdk相关问题可联系： 李静

local M = {}

local user_mgr = require("user_mgr")
local rr = require("remote_run.remote_runner")
local http_client = require("httpclient")

local log = require("log"):new(...)
local timer_queue = c_timer_queue.CTimerQueue()
local COUNTER = "GlobalOnlineCounter"  -- 全局在线计数功能名
local my_svr_id = c_util.get_my_svr_id()
local svr_counts = {}  -- 各服人数 svr_counts[svr_id] = int

-- 向中央统计服报告本服人数
local function report_online_count()
    local local_count = user_mgr.get_count()
    local global_svr_id = c_util.get_function_svr_id(COUNTER)
    rr.run_mfa(global_svr_id, "online_count", "update_svr_count",
        {my_svr_id, local_count})
end  -- report_online_count()

local function get_total_online_count()
    local total = 0
    for _, count in pairs(svr_counts) do
        total = total + count
    end
    return total
end  -- get_total_online_count()

-- 向GaSdk报告全区在线人数
local login_key = "fbf9a68914fdf172737cd72b547d508c"

local function report_global_count()
    local global_svr_id = c_util.get_function_svr_id(COUNTER)
    if my_svr_id ~= global_svr_id then
        return  -- 只有中央统计服才会向GaSdk报告
    end

    local total = get_total_online_count()
    -- 清空人数。
    svr_counts = {}
    log:info("Total online count: " .. total);
    
    local game_id = 5187
    local zone_id = 1
    local channel_id = 0 -- 渠道ID
    local sign = c_util.md5(""..game_id.."&"..total.."&"..zone_id.."&"..login_key)
    local url = string.format("http://stat.mztgame.com/game/online?game_id=%d&zone_id=%d&number=%d&channel_id=%d&sign=%s",game_id,zone_id,total,channel_id,sign)
    http_client:get(url, function (result)            
	        log:info("report_global_count, result:"..result)
        end)
end  -- report_global_count()

function M.init()
    -- 每29s发送一次。如果有变化就发送会太频繁。
    timer_queue:insert_repeat_now(29000, report_online_count)
    -- 接口请求频率： 每分钟一次
    timer_queue:insert_repeat_now(60000, report_global_count)
end  -- init()

function M.update_svr_count(svr_id, online_count)
    svr_counts[svr_id] = online_count
end  -- update_svr_count()

return M
