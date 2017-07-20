-- 所有的类的基类

MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");

local M = GlobalNS.Class();
M.clsName = "GObject";
GlobalNS[M.clsName] = M;

function M:ctor()
    --self:addCallMeta();
end

function M:dtor()
	
end

--[[
function M:addCallMeta()
    self.__call = function(self, ...)
        self:call(...);
    end
end
]]

function M.callMeta(...)
    
end

M.__call = M.callMeta;

--[[
-- 表访问
M.__index = M
M.__newindex = M
]]

-- 函数访问
function M.get(tbl, k)
    -- 方法一:自己手工设置 __index 值，防止递归访问
    --[[
    M.__index = nil;
    local ret = tbl[k];
    M.__index = M.get;
    return ret;
    ]]
    
    -- 方法二:通过 rawget ，不查找 __index ，进行访问
    local ret = rawget(M, k);
    return ret
end

M.__index = M.get;

function M.set(tbl, key, value)
    -- 方法一:自己手工设置 __newindex 值，防止递归访问
    --[[
    M.__newindex = nil;  -- 删除 __newindex ，否则调用 tbl[key] = value 的时候会递归调用 set 函数
    tbl[key] = value;
    M.__newindex = M.set;
    
    M.checkAttrRedef(tbl, key, value);
    ]]
    
    -- 方法二:通过 rawget ，不查找 __index ，进行访问
    rawset(tbl, key, value);
	
    M.checkAttrRedef(tbl, key, value);
end

M.__newindex = M.set;

function M.checkAttrRedef(tbl, key, value)
    --测试相同属性定义导致的属性覆盖
    if(nil ~= tbl.clsName and 'Form' == tbl.clsName) then
        
    end
end

return M;