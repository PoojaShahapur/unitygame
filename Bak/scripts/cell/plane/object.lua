local Object = {}

local objectbm = require('excelm').objectbm
local log = require("log"):new("plane.object")

-- 创建道具
function Object:new(baseid, num)
    local obj_base = objectbm[baseid]
    if obj_base == nil then
        log:info('企图创建不存在的道具id=%d', baseid)
        return nil
    end
    local obj = {
        baseid = baseid,
        uid = c_util.gen_objid(),
        name = obj_base.name,
        num = num,
        bind = 0,
        upgrade = 0,
        objbase = obj_base,
    }
    setmetatable(obj, self)
    self.__index = self
    log:info('创建道具,%d,%s,num=%d,uid=%u,bind=%d,upgrade=%d'
        , baseid, obj_base.name, obj.num, obj.uid, obj.bind, obj.upgrade)
    return obj
end

function Object:unserialize(baseid, uid, num, bind, upgrade)
    local obj_base = objectbm[baseid]
    if obj_base == nil then
        log:info('企图创建不存在的道具id=%d', baseid)
        return nil
    end
    local obj = {
        baseid = baseid,
        uid = uid,
        name = obj_base.name,
        num = num,
        bind = bind,
        upgrade = upgrade,
        objbase = obj_base,
    }
    setmetatable(obj, self)
    self.__index = self
    log:info('创建道具,%d,%s,num=%d,uid=%u,bind=%d,upgrade=%d'
        , baseid, obj_base.name, obj.num, obj.uid, obj.bind, obj.upgrade)
    return obj
end

function Object:destroy()
    log:info('销毁道具,%d,%s,num=%d,uid=%u,bind=%d,upgrade=%d'
        , self.baseid, self.objbase.name, self.num, self.uid, self.bind, self.upgrade)
end

-- 是否为皮肤道具
function Object.is_skin(base)
    return base.type == 1
end

-- 是否为子弹皮肤道具
function Object.is_bulletskin(base)
    return base.type == 2
end

function Object.is_money(base)
    return base.baseid == 1
end

function Object.is_ticket(base)
    return base.baseid == 2
end

function Object.is_plastic(base)
    return base.baseid == 3
end

-- 创建,消耗,销毁,更新道具都要加上日志
-- 道具销毁时候,直接设置为nil,打上日志即可
return Object
