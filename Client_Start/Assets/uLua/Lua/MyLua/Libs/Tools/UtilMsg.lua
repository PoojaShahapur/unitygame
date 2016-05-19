local M = GlobalNS.StaticClass();
local this = M;
M.clsName = "UtilMsg";
GlobalNS[M.clsName] = M;

function sendMsg(msg, bnet)
    if(bnet == nil or bnet == true) then
        -- 从网络发送
    else
        -- 直接放在本地数据缓存
    end
end