local M = {}

local pb = require("protobuf")
local log = require("log"):new("plane.svc_object")
local user_mgr = require("user_mgr")
local shopm = require('plane.shopm')

function M.RequestBagObjects(ctx, content)
    local clt_id = ctx:get_rpc_clt_id()
    local user = user_mgr.get_user(clt_id)
    if user ~= nil then
        user.bagm:send_all_objects_to_client()
    else
        log:debug('请求查看包裹时候,找不到client id=%d的玩家', clt_id)
    end
end

function M.UseObject(ctx, content)
    local req = assert(pb.decode("plane.UseObjectMsg", content))
    local clt_id = ctx:get_rpc_clt_id()
    local user = user_mgr.get_user(clt_id)
    if user ~= nil then
        user.bagm:use_obj(req.uid)
    else
        log:debug('请求查看包裹时候,找不到client id=%d的玩家', clt_id)
    end
end

function M.ReqShopItemList(ctx, content)
    local req = assert(pb.decode("plane.ShopIDMsg", content))
    local clt_id = ctx:get_rpc_clt_id()
    local user = user_mgr.get_user(clt_id)
    if user ~= nil then
        shopm.send_all_items_in_shop(req.shopid, user)
    else
        log:debug('请求查看商店时候,找不到client id=%d的玩家', clt_id)
    end
end

function M.ReqByShopItem(ctx, content)
    local req = assert(pb.decode("plane.BuyMsg", content))
    local clt_id = ctx:get_rpc_clt_id()
    local user = user_mgr.get_user(clt_id)
    if user ~= nil then
        local item = shopm.get_item(req.shopid, req.pos)
        user:buy_item(item.baseid, item.money, item.ticket, item.plastic)
    else
        log:debug('请求查看商店时候,找不到client id=%d的玩家', clt_id)
    end
end

function M.ReqUseSkin(ctx, content)
    local req = assert(pb.decode("plane.OneSkinIndexMsg", content))
    local clt_id = ctx:get_rpc_clt_id()
    local user = user_mgr.get_user(clt_id)
    if user ~= nil then
        user.skinm:use_skin(req.index)
    else
        log:debug('ReqUseSkin,找不到client id=%d的玩家', clt_id)
    end
end

function M.ActiveSkin(ctx, content)
    local req = assert(pb.decode("plane.OneSkinIndexMsg", content))
    local clt_id = ctx:get_rpc_clt_id()
    local user = user_mgr.get_user(clt_id)
    if user ~= nil then
        user.skinm:active_skin_index(req.index)
    else
        log:debug('ActiveSkin,找不到client id=%d的玩家', clt_id)
    end
end

function M.ReqUseBullet(ctx, content)
    local req = assert(pb.decode("plane.OneSkinIndexMsg", content))
    local clt_id = ctx:get_rpc_clt_id()
    local user = user_mgr.get_user(clt_id)
    if user ~= nil then
        user.skinm:use_bullet(req.index)
    else
        log:debug('ActiveBullet,找不到client id=%d的玩家', clt_id)
    end
end

require("rpc_request_handler").register_service("plane.Object", M)
return M
