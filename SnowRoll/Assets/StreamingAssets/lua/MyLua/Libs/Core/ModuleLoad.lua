-- 模块导入，这个添加到全局表 _G 里面，因为这样才能保证自己的代码使用肯定正确
function MLoader(path)
	path = (string.gsub(path, "%.", "/")); 	-- 替换点(.)成斜杠(/)，(string.gsub(path, '%.', '/')); 这样替换是替换字符，是错误的
	
    local M = require(path);
	
	if(false == M) then
		error(string.format("Error %s load failed", tostring(path)));
	end
	
	return M;
end

function MUnload(path)
    path = (string.gsub(path, "%.", "/"));
    package.loaded[path] = nil;
end

function MReload(path)
    MUnload(path);
    return MLoader(path);
end