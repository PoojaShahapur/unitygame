local M = {}

local log = require("log"):new("svc_login")
local pb = require("protobuf")
local User = require("user")
local user_mgr = require("user_mgr")
-- DEL local login_mgr = require("login.login_manager")

local mdb = require("database")
local json = require("json")
local serpent = require('serpent')
local rr = require("remote_run.remote_runner")
-- 全局唯一服务器id
local sessionid = 1
local gateway_limit = 500
local heart_beat_response = pb.encode("rpc.HeartBeatMsg", {})

log:debug("loading service...")

local function create_user_and_online(ctx, rpc_clt_id, openid, login_info)
        local user = User:new(rpc_clt_id, login_info)
		user_mgr.insert(user)
		local resp =  {
            result = "OK",
            account = user.account,
            header_imgid = user.header_imgid,
            header_imgurl = user.header_imgurl,
            level = user.level,
            short_url = user.shorturl,
            is_purgatory_unlock = user.can_in_pur,
            is_team_unlock = user.can_in_team,
            uid = user.uid,
        }
		c_rpc.reply_to(ctx, pb.encode("rpc.LoginResponse", resp))
        user:online()
end

local function replace_user_and_online(ctx, rpc_clt_id, login_info, u)
    local user = User:new(rpc_clt_id, login_info)
    user_mgr.replace(user)
	local resp =  {
        result = 0,
        account = user.account,
        header_imgid = user.header_imgid,
        header_imgurl = user.header_imgurl,
        level = user.level,
        short_url = user.shorturl,
        is_purgatory_unlock = user.can_in_pur,
        is_team_unlock = user.can_in_team,
        uid = user.uid,
    }
	c_rpc.reply_to(ctx, pb.encode("rpc.SwitchAccResponse", resp))
    user:online()
    u = nil--旧的用户设为空
end

-- 插入数据库失败,通知客户端
local function login_insertdb_fail(ctx)
	local resp = {result = "ERR_VERIFY_FAIL"}
	c_rpc.reply_to(ctx, pb.encode("rpc.LoginResponse", resp))
end

-- Return LoginResponse.
local function login(ctx, request)
	local rpc_clt_id = ctx:get_rpc_clt_id()
	log:info("Login. account=%q rpc_clt_id=%u", request.account, rpc_clt_id)
	if user_mgr.get_count() > gateway_limit then  -- Todo: config max_user
		local resp = {result = "ERR_SERVER_FULL"}
		c_rpc.reply_to(ctx, pb.encode("rpc.LoginResponse", resp))
		return
	end

	--[[ 暂时禁用manager登录验证，待客户端实现。
	如需测试，
	先运行testClient执行test.loginManager()命令模拟用户登录到服务器Manager
	再用猫狗大战客户端（同样的用户名）去登录服务器（30秒超时）

	registerdata = login_mgr.get_center_register_data(request.account)
	if not registerdata then
		log:debug("Login not register manager . account=%q rpc_clt_id=%u", request.account, rpc_clt_id)
		local resp = {result = "ERR_M_NOT_LOGIN_MANAGER"}
		c_rpc.reply_to(ctx, pb.encode("rpc.LoginResponse", resp))
		return
	end
	]]
    local myopenid = '1-' .. request.account
    log:info('openid=%s login', myopenid)
	local old_clt_id = user_mgr.get_rpc_clt_id_by_openid(myopenid)
	if old_clt_id ~= nil then -- 该账号竟然已经登录了本服务器
        if rpc_clt_id ~= old_clt_id then
	        -- 此账号已经登陆,踢掉线上用户
	        log:info("Kick multi-login client. CltId=" .. old_clt_id)
		    c_util.disconnect_game_client(old_clt_id);  -- 断开连接
        end
	end
    local user = user_mgr.get_user(rpc_clt_id)
    if user ~= nil then
        return
    end
    local entity = {openid = myopenid, account = request.account }
    local req = {
        svrid = c_util.get_my_svr_id(),
        openid = entity.openid,
    }
    local req_str = assert(pb.encode("svr.LoginRequest", req))
	c_rpc.request_svr(sessionid, "svr.RunLua", "CanILogin", req_str, function(resp)
        local ret = assert(pb.decode("svr.CanILoginResponse", resp))
        log:info("openid %s,session ret result=%d", entity.openid, ret.ok)
        if ret.ok == 1 then--我可以登录
	        mdb:query_b(mdb.db, mdb.collection, {_id = entity.openid}, '', function (results)
                local login_info = nil
                if #results > 0 then
                    login_info = mdb:get_table(results[1])
                end
                -- 账号不存在，向redis要个userid,要成功了创建user
                if login_info == nil then
	                c_redis.command("incr", "planeuid", function(reply)
                        --若返回成功,则插入mongodb,待mongodb返回成功后,创建user并返回登录成功消息
		                if 3 == reply.type then
		                    local user_info = User:make_db_data(entity.openid, reply.integer)
		                    log:info("往redis中插入对应关系openid=%s,userid=%d", entity.openid, reply.integer)
                            c_redis.set("ufo:" .. user_info.account, reply.integer,function(reply_type)
                                if reply_type == true then
		                            mdb:insert(mdb.db, mdb.collection, user_info, function(result)
                                        if result ~= 0 then
                                            log:info('角色对应关系%s,%d建立,但写入数据库失败,userinfo=%s'
                                                , entity.openid, reply.integer, user_info)
                                        end
                                    end)
                                    create_user_and_online(ctx, rpc_clt_id, entity.openid, user_info)
                                else
                                    login_insertdb_fail(ctx)
                                    log:info("往redis中插入 %s:%d 时失败,禁止玩家登录", user_info.account, reply.integer)
                                end
                            end)
                            --log:info('logininfo=%s', serpent.block(login_info))
		                else
                            login_insertdb_fail(ctx)
		                    log:error("[login] Redis insert key-value %s:%d error", user_info.account, reply.integer)
		                end  -- if
                    end)
                else
                    --log:info('logininfo=%s', serpent.block(login_info))
                    create_user_and_online(ctx, rpc_clt_id, entity.openid, login_info)
                    mdb:update(mdb.db, mdb.collection, {_id=entity.openid}, {last_login_time = os.time()})
                end
	        end)
        else
	        local resp = {result = "ERR_MULTI_LOGIN"}
	        c_rpc.reply_to(ctx, pb.encode("rpc.LoginResponse", resp))
        end
    end)
end  -- login()


-- Return LoginResponse.
local function login_giant(ctx, request)
	local rpc_clt_id = ctx:get_rpc_clt_id()

	log:info("Login. entities=%q rpc_clt_id=%u,sign=%s", request.entities_str, rpc_clt_id, request.sign)
	if user_mgr.get_count() > gateway_limit then  -- Todo: config max_user
		local resp = {result = "ERR_SERVER_FULL"}
		c_rpc.reply_to(ctx, pb.encode("rpc.LoginResponse", resp))
		return
	end

    local entity = {}
    for k,v in string.gmatch(request.entities_str, "([%w_]+)=([A-Za-z0-9_-]+)") do
        entity[k] = v
    end

    local verify_result = c_util.verify_giant_login(request.entities_str, request.sign)
    if not verify_result or entity.openid == nil then
	    log:info("Login failed. entities=%q rpc_clt_id=%u", request.entities_str, rpc_clt_id)
		local resp = {result = "ERR_VERIFY_FAIL"}
		c_rpc.reply_to(ctx, pb.encode("rpc.LoginResponse", resp))
        return
    end

    -- 验证是否重复登录
	local old_clt_id = user_mgr.get_rpc_clt_id_by_openid(entity.openid)
	if old_clt_id ~= nil then -- 该账号竟然已经登录了本服务器
        if rpc_clt_id ~= old_clt_id then
	        -- 此账号已经登陆,踢掉线上用户
	        log:info("Kick multi-login client. CltId=" .. old_clt_id)
		    c_util.disconnect_game_client(old_clt_id);  -- 断开连接
        end
	end

    local user = user_mgr.get_user(rpc_clt_id)
    if user ~= nil then
        return
    end

    -- 如果玩家版本低于 2.0.0,禁止登陆
    if request.majorv < 2 then
	    local resp = {result = "OK"}
	    c_rpc.reply_to(ctx, pb.encode("rpc.LoginResponse", resp))
	    c_rpc.request_clt(ctx:get_game_clt_id(), "plane.ObjectPush", "PopMessageBox"
            , pb.encode("plane.MessageBoxMsg", {content = "你当前的客户端版本已停用,请更新到最新版本!!"}), nil)
        return
    end

    local req = {
        svrid = c_util.get_my_svr_id(),
        openid = entity.openid,
    }
    local req_str = assert(pb.encode("svr.LoginRequest", req))
	c_rpc.request_svr(sessionid, "svr.RunLua", "CanILogin", req_str, function(resp)
        local ret = assert(pb.decode("svr.CanILoginResponse", resp))
        log:info("openid %s,session ret result=%d", entity.openid, ret.ok)
        if ret.ok == 1 then--我可以登录
	        mdb:query_b(mdb.db, mdb.collection, {_id = entity.openid}, '', function (results)
                local login_info = nil
                if #results > 0 then
                    login_info = mdb:get_table(results[1])
                end
                -- 账号不存在，向redis要个userid,要成功了创建user
                if login_info == nil then
	                c_redis.command("incr", "planeuid", function(reply)
                        --若返回成功,则插入mongodb,待mongodb返回成功后,创建user并返回登录成功消息
		                if 3 == reply.type then
		                    local user_info = User:make_db_data(entity.openid, reply.integer)
		                    log:info("往redis中插入对应关系openid=%s,userid=%d", entity.openid, reply.integer)
                            c_redis.set("ufo:" .. user_info.account, reply.integer,function(reply_type)
                                if reply_type == true then
		                            mdb:insert(mdb.db, mdb.collection, user_info, function(result)
                                        if result ~= 0 then
                                            log:info('角色对应关系%s,%d建立,但写入数据库失败,userinfo=%s'
                                                , entity.openid, reply.integer, user_info)
                                        end
                                    end)
                                    create_user_and_online(ctx, rpc_clt_id, entity.openid, user_info)
                                else
                                    login_insertdb_fail(ctx)
                                    log:info("往redis中插入 %s:%d 时失败,禁止玩家登录", user_info.account, reply.integer)
                                end
                            end)
                            --log:info('logininfo=%s', serpent.block(login_info))
		                else
                            login_insertdb_fail(ctx)
		                    log:error("[login] Redis insert key-value %s:%d error", user_info.account, reply.integer)
		                end  -- if
                    end)
                else
                    --log:info('logininfo=%s', serpent.block(login_info))
                    create_user_and_online(ctx, rpc_clt_id, entity.openid, login_info)
                    mdb:update(mdb.db, mdb.collection, {_id=entity.openid}, {last_login_time = os.time()})
                end
	        end)
        else
	        local resp = {result = "ERR_MULTI_LOGIN"}
	        c_rpc.reply_to(ctx, pb.encode("rpc.LoginResponse", resp))
        end
    end)
    
end  -- login_giant()

function M.Login(ctx, content)
	local req = assert(pb.decode("rpc.LoginRequest", content))
	login(ctx, req)
end  -- Login()

function M.Login_Giant(ctx, content)
    local req = assert(pb.decode("rpc.LoginRequest_Giant", content))
    login_giant(ctx, req)
end

local function logout(ctx, request)
    log:info("Logout. account=%q rpc_clt_id=%u", request.account, ctx:get_rpc_clt_id())
end

function M.Logout(ctx, content)
    local req = assert(pb.decode("rpc.LogoutRequest", content))
    c_util.disconnect_game_client(ctx:get_rpc_clt_id()) -- 断开连接
end

function M.HeartBeat(ctx, content)
	--c_rpc.reply_to(ctx, heart_beat_response)
end

function switch_fail(ctx, failcode)
	local resp = {result = failcode}
	c_rpc.reply_to(ctx, pb.encode("rpc.SwitchAccResponse", resp))
end

function M.SwitchAcc(ctx, content)
    local request = assert(pb.decode("rpc.SwitchAccMsg", content))
    local clt_id = ctx:get_rpc_clt_id()
	log:info("Switch acc. entities=%q rpc_clt_id=%u,sign=%s", request.entities_str, clt_id, request.sign)
    local user = user_mgr.get_user(clt_id)
    if user == nil then
        switch_fail(ctx, 2)
        return
    end

    local entity = {}
    for k,v in string.gmatch(request.entities_str, "([%w_]+)=([A-Za-z0-9_-]+)") do
        entity[k] = v
    end

    if entity['account'] ~= nil and entity['account'] == user.account then
        return
    end

    local verify_result = c_util.verify_giant_login(request.entities_str, request.sign)
    if not verify_result or entity.openid == nil then
	    log:info("Switch failed. entities=%q rpc_clt_id=%u", request.entities_str, clt_id)
        switch_fail(ctx, 2)
        return
    end

    local req = {
        svrid = c_util.get_my_svr_id(),
        openid = entity.openid,
    }
    local req_str = assert(pb.encode("svr.LoginRequest", req))
	c_rpc.request_svr(sessionid, "svr.RunLua", "CanILogin", req_str, function(resp)
        local ret = assert(pb.decode("svr.CanILoginResponse", resp))
        log:info("switch account,openid %s,session ret result=%d", entity.openid, ret.ok)
        if ret.ok == 1 then--我可以登录
	        mdb:query_b(mdb.db, mdb.collection, {_id = entity.openid}, '', function (results)
                local login_info = nil
                if #results > 0 then
                    login_info = mdb:get_table(results[1])
                end
                -- 账号不存在，向redis要个userid,要成功了创建user
                if login_info == nil then
	                c_redis.command("incr", "planeuid", function(reply)
                        --若返回成功,则插入mongodb,待mongodb返回成功后,创建user并返回登录成功消息
		                if 3 == reply.type then
		                    local user_info = User:make_db_data(entity.openid, reply.integer)
		                    log:info("往redis中插入对应关系openid=%s,userid=%d", entity.openid, reply.integer)
                            c_redis.set("ufo:" .. user_info.account, reply.integer,function(reply_type)
                                if reply_type == true then
		                            mdb:insert(mdb.db, mdb.collection, user_info, function(result)
                                        if result ~= 0 then
                                            log:info('角色对应关系%s,%d建立,但写入数据库失败,userinfo=%s'
                                                , entity.openid, reply.integer, user_info)
                                        end
                                    end)
                                    replace_user_and_online(ctx, clt_id, user_info, user)
                                else
                                    switch_fail(ctx, 2)
                                    log:info("往redis中插入 %s:%d 时失败,禁止玩家登录", user_info.account, reply.integer)
                                end
                            end)
                            --log:info('logininfo=%s', serpent.block(login_info))
		                else
                            switch_fail(ctx, 2)
		                    log:error("[login] Redis insert key-value %s:%d error", user_info.account, reply.integer)
		                end  -- if
                    end)
                else
                    --log:info('logininfo=%s', serpent.block(login_info))
                    replace_user_and_online(ctx, clt_id, login_info, user)
                    mdb:update(mdb.db, mdb.collection, {_id=myopenid}, {last_login_time = os.time()})
                end
	        end)
        else
            switch_fail(ctx, 1)
        end
    end)
end

require("rpc_request_handler").register_service("rpc.Login", M)
return M
