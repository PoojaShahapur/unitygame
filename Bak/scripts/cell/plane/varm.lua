--[[
变量管理器,目前只有每日变量
未来会有每周，每月变量;到时候可以抽象出通用函数,比如时间是否到期之类的,统一管理
--]]

local Varm = {}
local log = require('log'):new('plane.varm')
local pb = require("protobuf")
local serpent = require('serpent')
local time = require("time")
local math_floor = math.floor

var_today_game_get_sugar = 1--今日游戏获得棒棒糖总数
var_today_share_get_sugar = 2--今日分享获得棒棒糖总数

local var_limit = {}
--每日游戏获得上限
var_limit[var_today_game_get_sugar] = 15
--每日分享获得上限
var_limit[var_today_share_get_sugar] = 20

function Varm:new(user)
    local tmp = {
        daily_vars = {},--key:varid, value:var的值
        owner = user,
    }
    setmetatable(tmp, self)
    self.__index = self

    return tmp
end

function Varm:add_daily_var(varid, num)
    local origin_var = self.daily_vars[varid]
    if origin_var == nil then
        self.daily_vars[varid] = num
    else
        self.daily_vars[varid] = origin_var + num
    end

    if self.daily_vars[varid] > var_limit[varid] then
        self.daily_vars[varid] = var_limit[varid]
    end

    return self.daily_vars[varid]
end

function Varm:get_daily_var(varid)
    return self.daily_vars[varid]
end

-- 零点回调
function Varm:zero_clock()
    log:info("zero clock varm")
    self.daily_vars = {}
    self.owner.need_save = true
end

function Varm:serialize()
    local serialize = {
        vars = {}
    }
    for k,v in pairs(self.daily_vars) do
        table.insert(serialize.vars, {varid = k, value = v})
    end
    local ret_str = assert(pb.encode("svr.AllVarSerialize", serialize))
    return ret_str
end

function Varm:unserialize(varstr, last_login_time)
    local now = os.time()
    --if math_floor(now / 86400) ~= math_floor(last_login_time / 86400) then
    if last_login_time ~= 0 and time.is_two_time_in_different_day(last_login_time, now) then
        self:zero_clock()
    else
        local unserialize = assert(pb.decode("svr.AllVarSerialize", varstr))
        for k,v in pairs(unserialize.vars) do
            self.daily_vars[v.varid] = v.value
        end
    end
end

return Varm
