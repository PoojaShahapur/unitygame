
local Bagm = {}
local log = require('log'):new('plane.bagm')
local pb = require("protobuf")
local Object = require('plane.object')
local table_insert = table.insert
local mdb = require("database")
local serpent = require('serpent')

-- Bagm 放在 user.lua 中进行管理
function Bagm:new(user)
    local bagm = {
        maxsize = 100,
        objs = {},--以uid作为key,obj作为value
        baseobjs = {},--以baseid作为key,obj作为value
        obj_count = 0,--道具个数
        owner = user,
    }

    setmetatable(bagm, self)
    self.__index = self
    return bagm
end

-- 包裹是否满了
function Bagm:isFull()
    return obj_count >= self.maxsize
end

function Bagm:get_obj_by_uid(uid)
    return self.objs[uid]
end

-- 按策划目前设计,baseid相同的道具就放在同一个格子
function Bagm:get_obj_by_baseid(baseid)
    return self.baseobjs[baseid]
end

-- 往包裹添加道具
-- 如果道具id相同,则系统自动将玩家道具合并
function Bagm:addObj(obj, need_send)
    local need_send = need_send or 1
    log:info('bagm add,baseid=%d,num=%d, need=%s', obj.baseid, obj.num, tostring(need_send))
    local tmpobj = self.objs[obj.uid]
    if tmpobj ~= nil then
        log:info('%s,%s重复添加道具,%d,%suid=%u', self.owner.openid, self.owner.account, obj.baseid, obj.name, obj.uid)
        return false
    end

    local baseobj = self.baseobjs[obj.baseid]
    -- 是否是更新道具数量
    local is_update_obj = false
    if baseobj ~= nil then--只需要更新包裹中的道具数量即可
        baseobj.num = baseobj.num + obj.num
        obj:destroy()
        is_update_obj = true
        -- 通知客户端更新道具
        self:update_one_object_to_client(baseobj)
    else
        self.objs[obj.uid] = obj
        self.baseobjs[obj.baseid] = obj
        self.obj_count = self.obj_count + 1
        -- 通知客户端加道具
        self:add_one_object_to_client(obj, need_send)
    end

    self:save()
end

-- 移除道具
function Bagm:remove(uid)
    local obj = self.objs[uid]
    if obj == nil then
        log:info('%s,%s移除道具时找不到uid=%u的道具', self.owner.openid, self.owner.account, uid)
        return false
    end

    obj:destroy()
    self.objs[obj.uid] = nil
    self.baseobjs[obj.baseid] = nil
    obj = nil
    self.obj_count = self.obj_count - 1
    -- 通知客户端删道具
    self:remove_one_object_to_client(uid)
end

function Bagm:remove_by_baseid(baseid)
    local obj = self.baseobjs[baseid]
    if obj == nil then
        log:info('%s,%s移除道具时找不到baseid=%u的道具', self.owner.openid, self.owner.account, baseid)
        return false
    end

    obj:destroy()
    self.objs[obj.uid] = nil
    self.baseobjs[obj.baseid] = nil
    self.obj_count = self.obj_count - 1
    -- 通知客户端删道具
    self:remove_one_object_to_client(obj.uid)
    obj = nil
end

function Bagm:use_obj(uid)
    local obj = self.objs[uid]
    if obj == nil then
        log:info('%s,%s使用道具时找不到uid=%u的道具', self.owner.openid, self.owner.account, uid)
        return false
    end

    local check_func = _G["check_use_" .. obj.baseid]
    if check_func ~= nil then
        if check_func(self.owner, obj) ~= true then
            log:info('道具检查失败,不能使用道具id=%d,uid=%u', obj.baseid, uid)
            return false
        end
    end

    -- 如果类型是皮肤
    if obj.objbase.type == 1 then
        self.owner.skinm:active_skinid(obj.objbase.activeskinid)
        self:remove_one_object_to_client(obj.uid)
        self.objs[uid] = nil
    end
    --[[
    local use_func = _G["use_" .. obj.baseid]
    if use_func ~= nil then
        use_func(self.owner, obj)
    else
        log:info('未找到道具使用脚本,不能使用道具id=%d,uid=%u', obj.baseid, uid)
        return false
    end
    ]]
end


-- 玩家请求包裹数据,将包裹内所有道具发送给客户端
function Bagm:send_all_objects_to_client()
    local batch_add_msg = {
        is_request = 1,
        addobjs = {},
    }

    for k,v in pairs(self.objs) do
        local obj = {
            baseid = v.baseid,
            uid = v.uid,
            name = v.name,
            num = v.num,
            bind = v.bind,
            upgrade = v.upgrade,
        }
        table_insert(batch_add_msg.addobjs, obj)
    end
    self.owner:rpc_request("plane.ObjectPush", "BatchAddObjects", pb.encode("plane.BatchAddObjectsMsg", batch_add_msg))
end

-- 将所有的道具序列化为字符串,存入 mongodb
function Bagm:serialize()
    local serialize = {
        objs = {},
    }
    for k,v in pairs(self.objs) do
        local obj = {
            baseid = v.baseid,
            uid = v.uid,
            num = v.num,
            bind = v.bind,
            upgrade = v.upgrade,
        }
        table_insert(serialize.objs, obj)
    end
    local ret_str = assert(pb.encode("svr.AllObjSerialize", serialize))
    return ret_str
end

function Bagm:save()
    local all_obj = self:serialize()
    log:info('save bag%s, all_obj=%d', self.account,string.len(all_obj))
    if string.len(all_obj) ~= 0 then
        mdb:update_b(mdb.db, mdb.collection, {_id=self.owner.openid}, nil, {objstr = all_obj})
    end
end

-- 从 string 反序列化出所有道具,并依次创建出来
function Bagm:unserialize(objstr)
    local allobjs = assert(pb.decode("svr.AllObjSerialize", objstr))
    local need_print = false
    for k,v in pairs(allobjs.objs) do
        local obj = Object:unserialize(v.baseid, v.uid, v.num, v.bind, v.upgrade)
        if obj ~= nil then
            self.objs[obj.uid] = obj
            self.baseobjs[obj.baseid] = obj
            self.obj_count = self.obj_count + 1
        else
            need_print = true
        end
    end
    if need_print == true then
        log:info("%s,%s反序列化道具失败,objstr=%s", self.owner.openid, self.owner.account, objstr)
    end
end

function Bagm:print_all_objs()
    for k,v in pairs(self.objs) do
        log:info('%s,%s, 包裹道具打印,%d,%s,%lu,num=%d,bind=%d,upgrade=%d'
            ,self.owner.openid, self.owner.account, v.baseid, v.name, v.uid, v.num, v.bind, v.upgrade)
    end
end

-- 通知客户端添加个道具
function Bagm:add_one_object_to_client(obj, need_send)
    local need_send = need_send or 1
    local add_msg = {
        addobj = {},
    }

    local tmp = {}
    tmp.baseid = obj.baseid
    tmp.uid = obj.uid
    tmp.name = obj.name
    tmp.num = obj.num
    tmp.bind = obj.bind
    tmp.upgrade = obj.upgrade
    add_msg.addobj = tmp
    if need_send == 1 then
        self.owner:rpc_request("plane.ObjectPush", "AddOneObject", pb.encode("plane.AddOneObjectMsg", add_msg))
    end
end

-- 通知客户端移除道具
function Bagm:remove_one_object_to_client(uid)
    local remove_msg = {
        uid = uid,
    }
    self.owner:rpc_request("plane.ObjectPush", "RemoveOneObject", pb.encode("plane.RemoveOneObjectMsg", remove_msg))
end

-- 通知客户端更新道具
function Bagm:update_one_object_to_client(obj)
    local update_msg = {
        uid = obj.uid,
        newnum = obj.num,
        newbind = obj.bind,
        newupgrade = obj.upgrade
    }
    self.owner:rpc_request("plane.ObjectPush", "UpdateOneObject", pb.encode("plane.UpdateObjectMsg", update_msg))
end

return Bagm
