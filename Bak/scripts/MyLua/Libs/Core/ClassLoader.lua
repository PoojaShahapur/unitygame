-- 类加载器，加载类使用，不要在使用这个，使用 ModuleImport.lua 这个

MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

local M = GlobalNS.StaticClass();
M.clsName = "ClassLoader";     -- 记录类的名字，方便调试
GlobalNS[M.clsName] = M;

--local this = M; 		-- this 访问变量， M 访问类

function M.ctor()

end

function M.loadClass(path)
	path = (string.gsub(path, "%.", "/")); 	-- 替换点(.)成斜杠(/)
	
    -- require path -- 竟然会报错
    local M = require(path); -- 需要这么写才行，一定要返回加载的内容， 宿主语言中需要返回值
    -- require "aaa" -- 直接跟字符串就可以这么写
	
	if(false == M) then
		error(string.format("Error %s load failed", tostring(path)));
	end
	
	return M;
end

function M.unloadClass(path)
    package.loaded[path] = nil;
end

M.ctor();        -- 调用构造函数

return M;