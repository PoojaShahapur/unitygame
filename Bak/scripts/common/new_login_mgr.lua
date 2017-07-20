local M = {}
--多个网关的玩家,只能登陆一次 
--有openid登陆过,就存下来openid对应的svrid
--若该玩家已登陆过,再次请求登录,发消息通知旧网关下线,等待旧网关下线成功后通知新网关
local openid2gatewayid = {}--key:openid, value:该openid登录的网关
local svrid_syncok = {}--key:svrid,value:true代表服务器连接后,同步ok了该服务器的负载,
local pb = require("protobuf")
local login_fail_str = assert(pb.encode("svr.CanILoginResponse", {ok = 0}))
local login_success_str = assert(pb.encode("svr.CanILoginResponse", {ok = 1}))
local log = require("log"):new("new_login_mgr")
local rr = require("remote_run.remote_runner")

function M.can_i_login(ctx, svrid, openid)
    log:info("check login,svrid=%d,%s", svrid, openid)
    -- 没有同步完该服务器的负载,不能登录
    if svrid_syncok[svrid] ~= true then
        log:info("login failed,svrid=%d,%s", svrid, openid)
	    c_rpc.reply_to(ctx, login_fail_str)
    end

    if openid2gatewayid[openid] == nil then
        log:info("login success,svrid=%d,%s", svrid, openid)
	    c_rpc.reply_to(ctx, login_success_str)
        openid2gatewayid[openid] = svrid
    else
        -- 通知旧的网关把玩家踢了,踢成功了回复网关可以登录
        rr.run_mfa(openid2gatewayid[openid], "user_mgr", "erase_by_openid", {openid},
            function(ret)
                if ret then
                    openid2gatewayid[openid] = svrid
	                c_rpc.reply_to(ctx, login_success_str)
                end
            end)
    end
end

function M.offline(openid)
    openid2gatewayid[openid] = nil
end

-- 服务器断开连接
function M.server_disconnected(svrid)
    local need_erase_openid = {}
    for k,v in pairs(openid2gatewayid) do
        if v == svrid then
            table.insert(need_erase_openid, k)
        end
    end
    for i = 1,#need_erase_openid do
        openid2gatewayid[need_erase_openid[i]] = nil
    end
    log:info("svrid=%d 断开链接,清除所有登录记录", svrid)
end

function M.server_connected(svrid)
    if c_util.get_my_svr_id() == svrid then
        return
    end
    log:info("svrid=%d 链接,开始要数据", svrid)
    svrid_syncok[svrid] = false
    rr.run_mfa(svrid, "user_mgr", "give_session_login_data", {},
        function(datastr)
            M.sync_login_data(svrid, datastr)
        end)
end

-- run_mfa 没法
function M.sync_login_data(svrid, datastr)
    local all_openids = assert(pb.decode("svr.AllOpenids", datastr))
    local num = 0
    log:info("服务器启动同步登录数据, start,svrid=%d", svrid)
    for k,v in pairs(all_openids.openids) do
        openid2gatewayid[v] = svrid
        num = num + 1
        log:info("sync login,%s", v)
    end
    log:info("服务器启动同步登录数据, end,svrid=%d", svrid)
    if all_openids.is_sync_complete == 1 then
        svrid_syncok[svrid] = true
    end
end

return M
