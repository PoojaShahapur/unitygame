require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "TableItemBodyBase";
GlobalNS[M.clsName] = M;

-- 解析主要数据部分
function M:parseBodyByteBuffer(bytes, offset)
    
end

return M;