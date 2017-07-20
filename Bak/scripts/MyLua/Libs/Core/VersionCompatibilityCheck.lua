-- 检查 lua 各个版本不兼容的情况

--[[
 @brief Lua 5.2 及新版本， table 没有 getn 这个函数了，用 # 操作符来取长度
]]
if(nil == table.getn) then
	table.getn = function(t)
		local ret = 0;
		
		if(nil ~= t) then
			for _, value in ipairs(t) do
				ret = ret + 1;
			end
		end
		
		return ret;
	end
end