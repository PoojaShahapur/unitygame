--[[
皮肤管理器
将所有皮肤id以及已激活的颜色id发送给客户端
激活皮肤id,请求使用道具时候用到,遍历皮肤表中指定皮肤id字段,所有不需要花钱的皮肤都设置为激活状态
通知客户端添加一组已拥有的皮肤,皮肤id+颜色id
发送所有皮肤数据给客户端,打开我的皮肤列表时候用到,皮肤id,是否已拥有,已拥有的颜色列表;
请求使用某个皮肤id的颜色id
添加一个皮肤id到皮肤管理器中
通知皮肤id下的子皮肤激活
通知客户端使用结果

self.allskins[skinid] = {各个颜色的激活状态}
self.allskinuniqueids[uid] = {是否激活}
enter_room 时候,除了传账号,昵称之外,还需要传一个皮肤id
--]]

local Skinm = {}
local skinbm = require('excelm').skinbm
local objectbm = require('excelm').objectbm
local log = require('log'):new('plane.skinm')
local pb = require("protobuf")
local mdb = require("database")
local serpent = require('serpent')

function Skinm:new(user)
    local m = {
        skins = {},--key:皮肤表唯一编号;value:该皮肤的数据
        own_bullets = {},--玩家所有的子弹数据,key:子弹道具表id,value:过期时间戳,0代表永久
        owner = user,
        default_skinid = 0,
        default_bullet = 0,
    }

    setmetatable(m, self)
    self.__index = self

    return m
end

-- 激活皮肤id,服务器使用道具时候调用
function Skinm:active_skinid(skinid, need_send)
    local need_send = need_send or 1
    local skinid_tbl = skinbm.multiskin[skinid]
    if skinid_tbl == nil then
        log:info('%s,%s 请求激活皮肤表不存在的皮肤id=%d', self.owner.openid, self.owner.account, skinid)
        return 
    end

    -- 检查该皮肤是否已被激活
    for k,v in pairs(skinid_tbl) do
        if self.skins[v.id] ~= nil then
            self.owner:popMessageBox("你已激活过该皮肤,无需再激活啦!")
            return 
        end
    end
    local batch_add_skin = {
        skins = {},
    }

    -- 激活该皮肤id下所有不需要游戏币解锁的皮肤
    for k,v in pairs(skinid_tbl) do
        if v.money == 0 then
            local skin = {index = v.id, disappear_ts = 0}
            if v.tlimit ~= 0 then
                skin.disappear_ts = os.time() + v.tlimit
            end
            self.skins[v.id] = skin
            table.insert(batch_add_skin.skins, skin)
        end
    end

    if need_send == 1 then
        self.owner:rpc_request("plane.ObjectPush", "BatchAddSkins", pb.encode("plane.BatchSkinMsg", batch_add_skin))
        self:save()
    else
        self.owner.need_save = true
    end

end

-- 只要该 skinid 在皮肤表中有一种颜色被激活,就认为该皮肤已经被购买过了
function Skinm:is_skinid_actived(skinid)
    local skinid_tbl = skinbm.multiskin[skinid]
    if skinid_tbl == nil then
        return true--不能买这个皮肤,买了会出事
    end

    -- 检查该皮肤是否已被激活
    for k,v in pairs(skinid_tbl) do
        if self.skins[v.id] ~= nil then
            return true
        end
    end

    return false
end

function Skinm:is_bullet_owned(bullet_objid)
    return self.own_bullets[bullet_objid] ~= nil
end

function Skinm:active_bullet(bullet_objbase, need_send)
    local need = need_send or 1
    local expire_ts = 0
    if bullet_objbase.time_limit ~= 0 then
        expire_ts = math.floor(c_util.get_sys_ms() / 1000) + skin.time_limit
    end
    self.own_bullets[bullet_objbase.baseid] = expire_ts
    local bullet_msg = {baseid= bullet_objbase.baseid, disappear_ts = expire_ts}
    if need == 1 then
        self.owner:rpc_request("plane.ObjectPush", "AddOneBullet", pb.encode("plane.OneBulletMsg", bullet_msg))
        self:save()
    else
        self.owner.need_save = true
    end
end

-- 激活皮肤编号,客户端点界面激活时候调用
function Skinm:active_skin_index(index)
    local skin = skinbm[index]
    if skin == nil then
        log:info('%s,%s 请求激活皮肤表不存在的皮肤index=%d', self.owner.openid, self.owner.account, index)
        return
    end

    if self.skins[index] ~= nil then
        self.owner:popMessageBox('您已激活该颜色,不需要重复激活哦!')
        return
    end

    if skin.money ~= 0 and self.owner.money < skin.money then
        self.owner:popMessageBox('您的饼干不足,不能激活该皮肤!')
        return
    end

    self.owner:sub_money(skin.money)
    self.owner.log:info("饼干消耗|消耗%d,激活皮肤id %d", skin.money, index)
    self.owner:send_all_money_to_client()
    local newskin = {index = index, disappear_ts = 0}
    if skin.tlimit ~= 0 then
        newskin.disappear_ts = math.floor(c_util.get_sys_ms() / 1000) + skin.tlimit
    end
    self.skins[index] = newskin

    self.owner:rpc_request("plane.ObjectPush", "AddOneSkin", pb.encode("plane.OneSkinMsg", newskin))
    self:save()
end

-- 玩家上线,将所有拥有的皮肤数据发送给客户端
function Skinm:online()
    if self.default_skinid == 0 then
        self:active_skinid(1, 0)
        --self:active_skinid(2, 0)
        --self:active_skinid(3, 0)
        self.default_skinid = 1
        self:save()
    end
    if self.default_bullet == 0 then
        self.default_bullet = 20001
        local objbase = objectbm[20001]
        if objbase ~= nil then
            self:active_bullet(objbase, 0)
        end
    end
    local online_skin = {
        cur_skinindex = self.default_skinid,
        allskins = {},
        cur_bulletid = self.default_bullet,
        bullets = {},
    }
    for k,v in pairs(self.skins) do
        table.insert(online_skin.allskins, v)
    end
    for k,v in pairs(self.own_bullets) do
        local bullet = {baseid = k, disappear_ts = v}
        table.insert(online_skin.bullets, bullet)
    end
    self.owner:rpc_request("plane.ObjectPush", "OnlineSendAllSkins", pb.encode("plane.OnlineSendSkinMsg", online_skin))
end

-- 将所有的皮肤和子弹填充成index发送给客户端
function Skinm:full_skin_page_msg()
    local ret = {}
    for k,v in pairs(self.skins) do
        table.insert(ret, k)
    end
    for k,v in pairs(self.own_bullets) do
        table.insert(ret, k)
    end

    return ret
end

-- 每5秒钟检查皮肤是否到期
function Skinm:loop5Second(cur_ts)
    local need_remove_skinid = {}
    local need_remove_bullet = {}
    local need_save = false
    for k,v in pairs(self.skins) do
        if v.disappear_ts >= cur_ts then
            table.insert(need_remove_skinid, k)
        end
    end

    if #need_remove_skinid == 1 then
        local remove_one_skin = {
            index = need_remove_skinid[1]
        }
        self.owner:rpc_request("plane.ObjectPush", "RemoveOneSkin", pb.encode("plane.OneSkinIndexMsg", remove_one_skin))
        need_save = true
    elseif #need_remove_skinid > 1 then
        self.owner:rpc_request("plane.ObjectPush", "BatchRemoveSkins", pb.encode("plane.BatchSkinIndexMsg", need_remove_skinid))
        need_save = true
    end

    for i = 1,#need_remove_skinid do
        self.skins[need_remove_skinid[i]] = nil
    end

    for k,v in pairs(self.own_bullets) do
        if v >= cur_ts then
            self.own_bullets[k] = nil
            self.owner:rpc_request("plane.ObjectPush", "RemoveOneBullet", pb.encode("plane.OneSkinIndexMsg", {index = k}))
            need_save = true
        end
    end

    if need_save == true then
        self:save()
    end
end

function Skinm:save()
    self.owner.need_save = true
    --[[
    local all_skin = self:serialize()
    self.owner.log:info('save skin,%s,%s, all_skin=%d', self.owner.openid,self.owner.account,string.len(all_skin))
    if string.len(all_skin) ~= 0 then
        mdb:update_b(mdb.db, mdb.collection, {_id=self.owner.openid}, nil, {skinstr = all_skin})
    end
    ]]
end

function Skinm:serialize()
    local serialize = {
        default_skinid = self.default_skinid,
        skins = {},
        default_bullet = self.default_bullet,
        bullets = {},
    }
    for k,v in pairs(self.skins) do
        table.insert(serialize.skins, v)
    end
    for k,v in pairs(self.own_bullets) do
        table.insert(serialize.bullets, {objid = k, disappear_ts = v})
    end
    local ret_str = assert(pb.encode("svr.AllSkinSerialize", serialize))
    return ret_str
end

function Skinm:unserialize(skinstr)
    local unserialize = assert(pb.decode("svr.AllSkinSerialize", skinstr))
    --log:info("unser=%s", serpent.block(unserialize.skins))
    self.default_skinid = unserialize.default_skinid
    for k,v in pairs(unserialize.skins) do
        self.skins[v.index] = {
            index = v.index,
            disappear_ts = (v.disappear_ts==nil and 0 or v.disappear_ts),
        }
    end
    self.default_bullet = unserialize.default_bullet
    for k,v in pairs(unserialize.bullets) do
        self.own_bullets[v.objid] = v.disappear_ts
    end
end

-- 客户端请求使用index的皮肤
function Skinm:use_skin(skin_index)
    -- 查看该皮肤我是否已拥有,若已拥有,则通知客户端使用成功;没有,则弹出提示。
    local skindata = self.skins[skin_index]
    if skindata == nil then
        self.owner:popMessageBox('您尚未拥有该皮肤,不能使用该皮肤哦!')
        return
    end

    self.default_skinid = skin_index

    local result = {
        use_result = 1,
        new_skinindex = skin_index,
    }
    self.owner:rpc_request("plane.ObjectPush", "NotifyUseSkinResult", pb.encode("plane.UseSkinResultMsg", result))
end

function Skinm:use_bullet(bullet_index)
    local bullet = self.own_bullets[bullet_index]
    if bullet == nil then
        self.owner:popMessageBox('您尚未拥有该子弹,不能使用该子弹哦!')
        return
    end

    self.default_bullet = bullet_index
    local result = {
        use_result = 1,
        new_skinindex = bullet_index,
    }
    self.owner:rpc_request("plane.ObjectPush", "NotifyUseBulletResult", pb.encode("plane.UseSkinResultMsg", result))
end
return Skinm
