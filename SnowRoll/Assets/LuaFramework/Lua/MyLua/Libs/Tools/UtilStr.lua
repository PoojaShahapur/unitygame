MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

local M = GlobalNS.StaticClass();
local this = M;
M.clsName = "UtilStr";
GlobalNS[M.clsName] = M;

-- 字符串是否为空值
function M.IsNullOrEmpty(str)
	return (nil == str or "" == str);
end

--[[
模式串
.   任意字符 
%a   字母 
%c   控制字符 
%d   数字 
%l   小写字母 
%p   标点字符 
%s   空白符 
%u   大写字母 
%w   字母和数字 
%x   十六进制数字 
%z   代表 0的字符 

特殊字符如下：
(). % + - * ? [ ^ $ 
% 也作为以上特殊字符的转义字符。

如果要替换点(.)，使用
s = (string.gsub("Lua.is.good", "%.", "/"));
]]
-- 替换字符串
function M.Replace(srcStr, pat, repl)
	-- return (string.gsub(...))， 注意 string.gsub 外面还有一层括号。 事实上， gsub 调用之后返回 两个值， 一个是替换后的字符串， 第二个是替换的次数。 而一旦外面加了括号， 就只返回第一个值， 即替换后的字符串。
	return (string.gsub(srcStr, pat, repl));
	--[[
	local retStr, retNum;
	retStr, retNum = string.gsub(srcStr, pat, repl);
	return retStr;
	]]
end

--数字转换成字符串
function M.tostring(num)
	return tostring(num);
end

return M;