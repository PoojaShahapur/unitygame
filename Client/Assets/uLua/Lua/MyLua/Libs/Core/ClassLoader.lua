-- 类加载器，加载类使用

require "MyLua.Libs.Core.GlobalNS"

local M = {}
M.clsName = "ClassLoader"     -- 记录类的名字，方便调试
GlobalNS[M.clsName] = M

function M:loadClass(path)
    -- require path -- 竟然会报错
    require(path) -- 需要这么写才行
    -- require "aaa" -- 直接跟字符串就可以这么写
end

function M:unloadClass(path)
    package.loaded[path] = nil
end

return M