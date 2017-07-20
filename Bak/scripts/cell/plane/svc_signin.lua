local M = {}

local pb = require("protobuf")
local log = require("log"):new("plane.svc_object")
local user_mgr = require("user_mgr")

-- 请求每日签到
function M.ReqDaySignin(ctx, content)
    local clt_id = ctx:get_rpc_clt_id();
    local user = user_mgr.get_user(clt_id);
	
    if(user ~= nil) then
        user.mSigninSys:reqDaySignin();
    else
        log:debug('ReqDaySignin, 找不到 client id = %d 的玩家', clt_id);
    end
end

-- 请求领取累积奖励
function M.ReqReceiveCumulaReward(ctx, content)
	local req = assert(pb.decode("plane.ReceiveCumulaRewardIdMsg", content))
    local clt_id = ctx:get_rpc_clt_id();
    local user = user_mgr.get_user(clt_id);
	
	if(nil ~= user) then
        user.mSigninSys:reqReceiveCumulaReward(req);
    else
        log:debug('ReqReceiveCumulaReward, 找不到 client id = %d 的玩家', clt_id);
    end
end

require("rpc_request_handler").register_service("plane.SignInC2S", M); 	-- proto 文件中 service SignInC2S 这个字符串

return M;