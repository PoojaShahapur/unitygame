-- 类加载器，加载类使用

require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.StaticClass"

local M = GlobalNS.StaticClass()
M.clsName = "ClassLoader"     -- 记录类的名字，方便调试
GlobalNS[M.clsName] = M

local this = M 		-- this 访问变量， M 访问类

function M.ctor()

end

function M.loadClass(path)
    -- require path -- 竟然会报错
    return require(path) -- 需要这么写才行，一定要返回加载的内容， 宿主语言中需要返回值
    -- require "aaa" -- 直接跟字符串就可以这么写
end

function M.unloadClass(path)
    package.loaded[path] = nil
end

M.ctor()        -- 调用构造函数

return M