-- 好友管理器,A玩家好友的定义:A玩家关注的玩家即为A玩家的好友
-- 目前只用来告诉客户端第x页的好友uid列表
-- 因为该管理器不涉及具体cell功能,就不放在scripts/cell/plane目录
-- 后面可能会开单独服务器做好友服务器
local Friendm = {}
local log = require("log"):new("friendm")

--匹配界面的好友页里面1页有5条数据
local one_page_data_num = 5

function Friendm:new(uid)
    local m = {
        myuid = uid,
        total_num = 0,
        friends_uids = {},--一堆uid列表
        online_uids = {},--在线uid列表
        followed_uids = {},--关注我的玩家列表,我的状态改变时候,需要通知关注我的人
    }

    setmetatable(m, self)
    self.__index = self

    return m
end

-- 上线,读取我的好友数据
-- 并通知我所有的在线好友更新我的在线状态
function Friendm:online(session_users)
	c_redis.command("zrange", "following:" .. self.myuid, {0, -1}, function(reply)
            if 2 == reply.type and #reply.elements > 0 then
                for k,v in pairs(reply.elements) do
                    if v.type == 1 then
                        local uid = tonumber(v.str)
                        self.total_num = self.total_num + 1
                        if session_users[uid] then
                            self.online_uids[uid] = 1
                        else
                            self.friends_uids[uid] = 1
                        end
                    else
                        log:info("解析关注列表 elements error,type=%d,integer=%d,str=%s", v.type,v.integer, v.str)
                    end
                end
            else
                log:error("获取好友失败zrange,return error type=%d,integer=%d,str=%s", reply.type,reply.integer, reply.str)
            end
        end)
	c_redis.command("zrange", "followed:" .. self.myuid, {0, -1}, function(reply)
            if 2 == reply.type and #reply.elements > 0 then
                for k,v in pairs(reply.elements) do
                    if v.type == 1 then
                        local uid = tonumber(v.str)
                        self.followed_uids[uid] = 1
                        local user = session_users[uid]
                        if user then
                            user.friendm:on_friend_online(self.myuid)
                        end
                    else
                        log:info("解析粉丝列表 elements error,type=%d,integer=%d,str=%s", v.type,v.integer, v.str)
                    end
                end
            else
                log:error("获取粉丝失败zrange,return error type=%d,integer=%d,str=%s", reply.type,reply.integer, reply.str)
            end
    end)
end

function Friendm:on_friend_online(uid)
    self.online_uids[uid] = 1
    self.friends_uids[uid] = nil
end

function Friendm:on_friend_offline(uid)
    self.online_uids[uid] = nil
    self.friends_uids[uid] = 1
end

function Friendm:on_add_friend(uid, is_new_friend_online)
    if is_new_friend_online == 1 then
        self.online_uids[uid] = 1
    else
        self.friends_uids[uid] = 1
    end
    self.total_num = self.total_num + 1
end

function Friendm:on_remove_friend(uid)
    if self.online_uids[uid] then
        self.online_uids[uid] = nil
    end
    if self.friends_uids[uid] then
        self.friends_uids[uid] = nil
    end
    self.total_num = self.total_num - 1
end

-- 下线,通知下我所有的在线好友我下线啦,交给session_user_state_mgr去做
function Friendm:offline(session_users)
    for uid, _ in pairs(self.followed_uids) do
        local user = session_users[uid]
        if user then
            user.friendm:on_friend_offline(self.myuid)
        end
    end
end

-- 获取x页的好友uid列表
-- 第一页返回的是数组 1-5 下标的uid
function Friendm:get_page_x_uids(page_x)
    local uid_array = {}
    for k,_ in pairs(self.online_uids) do
        table.insert(uid_array, k)
    end
    for k,_ in pairs(self.friends_uids) do
        table.insert(uid_array, k)
    end
    local len = #uid_array
    -- 玩家想请求第x页数据,至少需要有 5(x-1)+1好友
    local ret = {}
    local page_x_start_index = (page_x - 1) * one_page_data_num + 1
    if page_x_start_index > len then
        return ret
    end

    local page_x_end_index = page_x * one_page_data_num
    for i = page_x_start_index, page_x_end_index do
        table.insert(ret, uid_array[i])
    end
    return ret
end

return Friendm
