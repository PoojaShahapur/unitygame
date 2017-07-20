local M = {}
local pb = require("protobuf")
local log = require("log"):new("plane.svc_object")
local user_mgr = require("user_mgr")
local shopm = require('plane.shopm')
--local serpent = require('serpent')

function M.ChangeHeaderImg(ctx, content)
    local req = assert(pb.decode("plane.ImgIDMsg", content))
    local clt_id = ctx:get_rpc_clt_id()
    local user = user_mgr.get_user(clt_id)
    if user ~= nil then
        user:set_headerimg(req.imgid, '')
    else
        log:debug('更换头像id=%d时,找不到client id=%d的玩家', req.imgid, clt_id)
    end
end

function M.ChangePersonalInfo(ctx, content)
    local req = assert(pb.decode("plane.PersonalInfoMsg", content))
    local clt_id = ctx:get_rpc_clt_id()
    local user = user_mgr.get_user(clt_id)
    if user ~= nil then
        user:change_personalinfo(req)
    else
        log:debug('设置个人资料时,找不到client id=%d的玩家', clt_id)
    end
end

function M.ChangeAccount(ctx, content)
    local req = assert(pb.decode("plane.AccountMsg", content))
    local clt_id = ctx:get_rpc_clt_id()
    local user = user_mgr.get_user(clt_id)
    if user ~= nil then
        user:change_account(ctx, req.account)
    else
        log:debug('设置个人资料时,找不到client id=%d的玩家', clt_id)
    end
end
function M.ChangeSign(ctx, content)
    local req = assert(pb.decode("plane.SignMsg", content))
    local clt_id = ctx:get_rpc_clt_id()
    local user = user_mgr.get_user(clt_id)
    if user ~= nil then
        user:change_sign(req.sign)
    else
        log:debug('设置个人资料时,找不到client id=%d的玩家', clt_id)
    end
end

function M.ReqMainPageInfo(ctx, content)
    local req = assert(pb.decode("plane.UidMsg", content))
    local clt_id = ctx:get_rpc_clt_id()
    local user = user_mgr.get_user(clt_id)
    if user ~= nil then
        user:send_account_main_page_info(ctx, req.uid)
    else
        log:debug('ReqMainPageInfo,找不到client id=%d的玩家', clt_id)
    end
end

function M.ViewGamePageInfo(ctx, content)
    local req = assert(pb.decode("plane.AccountMsg", content))
    local clt_id = ctx:get_rpc_clt_id()
    local user = user_mgr.get_user(clt_id)
    if user ~= nil then
        user:send_game_page_info(ctx, req.account)
    else
        log:debug('ReqMainPageInfo,找不到client id=%d的玩家', clt_id)
    end
end

function M.ViewFollowingList(ctx, content)
    local req = assert(pb.decode("plane.FollowUnfollowViewMsg", content))
    local clt_id = ctx:get_rpc_clt_id()
    local user = user_mgr.get_user(clt_id)
    if user ~= nil then
        user:view_follow(ctx, req.account, req.page_num, 0)
    else
        log:debug('查看 %s的关注列表,找不到client id=%d的玩家', req.account, clt_id)
    end
end

function M.ViewFollowedList(ctx, content)
    local req = assert(pb.decode("plane.FollowUnfollowViewMsg", content))
    local clt_id = ctx:get_rpc_clt_id()
    local user = user_mgr.get_user(clt_id)
    if user ~= nil then
        user:view_follow(ctx, req.account, req.page_num, 1)
    else
        log:debug('查看 %s的粉丝列表,找不到client id=%d的玩家', req.account, clt_id)
    end
end

function M.Follow(ctx, content)
    local req = assert(pb.decode("plane.UidMsg", content))
    local clt_id = ctx:get_rpc_clt_id()
    local user = user_mgr.get_user(clt_id)
    if user ~= nil then
        user:follow(ctx, req.uid)
    else
        log:debug('follow %s,找不到client id=%d的玩家', req.account, clt_id)
    end
end

function M.UnFollow(ctx, content)
    local req = assert(pb.decode("plane.UidMsg", content))
    local clt_id = ctx:get_rpc_clt_id()
    local user = user_mgr.get_user(clt_id)
    if user ~= nil then
        user:unfollow(ctx, req.uid)
    else
        log:debug('unfollow %s,找不到client id=%d的玩家', req.account,clt_id)
    end
end

function M.FindFriend(ctx, content)
    local req = assert(pb.decode("plane.AccountMsg", content))
    local clt_id = ctx:get_rpc_clt_id()
    local user = user_mgr.get_user(clt_id)
    if user ~= nil then
        user:find_friend_by_acc(ctx, req.account)
    else
        log:debug('找朋友 %s,找不到client id=%d的玩家', req.account,clt_id)
    end
end

function M.FindFriendInFollowing(ctx, content)
    local req = assert(pb.decode("plane.AccountMsg", content))
    local clt_id = ctx:get_rpc_clt_id()
    local user = user_mgr.get_user(clt_id)
    if user ~= nil then
        user:find_friend_in_follow(ctx, req.account)
    else
        log:debug('找朋友 %s,找不到client id=%d的玩家', req.account,clt_id)
    end
end

function M.FindFriendInFollowed(ctx, content)
    local req = assert(pb.decode("plane.AccountMsg", content))
    local clt_id = ctx:get_rpc_clt_id()
    local user = user_mgr.get_user(clt_id)
    if user ~= nil then
        user:find_friend_in_follow(ctx, req.account, 1)
    else
        log:debug('找朋友 %s,找不到client id=%d的玩家', req.account,clt_id)
    end
end

function M.ViewSkinPageInfo(ctx, content)
    local req = assert(pb.decode("plane.AccountMsg", content))
    local clt_id = ctx:get_rpc_clt_id()
    local user = user_mgr.get_user(clt_id)
    if user ~= nil then
        user:send_skin_page_info(ctx, req.account)
    else
        log:debug('unfollow %s,找不到client id=%d的玩家', req.account,clt_id)
    end
end

function M.GetShareData(ctx, content)
    local clt_id = ctx:get_rpc_clt_id()
    local user = user_mgr.get_user(clt_id)
    if user ~= nil then
        user:send_share_data(ctx)
    else
        log:debug('GetShareData找不到client id=%d的玩家', clt_id)
    end
end

function M.GetPurgatoryRank(ctx, content)
    local req = assert(pb.decode("plane.PurgatoryRankType", content))
    local clt_id = ctx:get_rpc_clt_id()
    local user = user_mgr.get_user(clt_id)
    if user ~= nil then
        user:send_pur_rank_data(ctx, req.type)
    else
        log:debug('GetPurgatoryRank 找不到client id=%d的玩家', clt_id)
    end
end
function M.CheckAlreadyFollowed(ctx, content)
    local req = assert(pb.decode("plane.UidListMsg", content))
    local clt_id = ctx:get_rpc_clt_id()
    local user = user_mgr.get_user(clt_id)
    if user ~= nil then
        user:check_already_followed(ctx, req.uids)
    else
        log:debug('CheckAlreadyFollowed 找不到client id=%d的玩家', clt_id)
    end
end
function M.GetSeasonReward(ctx, content)
    local clt_id = ctx:get_rpc_clt_id()
    local user = user_mgr.get_user(clt_id)
    if user ~= nil then
        user:send_season_reward(ctx)
    else
        log:debug('GetShareData找不到client id=%d的玩家', clt_id)
    end
end
function M.GetLevelRank(ctx)
    local clt_id = ctx:get_rpc_clt_id()
    local user = user_mgr.get_user(clt_id)
    if user ~= nil then
        user:send_level_rank_data(ctx)
    else
        log:debug('GetShareData找不到client id=%d的玩家', clt_id)
    end
end

require("rpc_request_handler").register_service("plane.ReqSocialInfo", M)
return M
