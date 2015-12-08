--[[  
      @brief 测试 require 实现
]]--



local M = {};
local modelName = ...;
_G[modelName] = M;
setfenv(1, M);
function play()
    print("那么，开始吧");
end
function quit()
    print("你走吧，我保证你不会出事的，呵，呵呵");
end
return M;






