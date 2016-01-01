require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.StaticClass"

local M = GlobalNS.StaticClass()
local this = M
M.clsName = "UtilApi"
GlobalNS[M.clsName] = M

function M.ctor()

end

function M.getComponent(go, name)
	return go:getComponent(name)
end

function M.notBool(value)
	local ret = value
	ret = not value
	return ret
end

function M:setTextStr(go, str)
    go:getComponent('Text').text = str
end

function M:setImageSprite(go, path)
    go:getComponent('Image').sprite = loadSprite(path)
end

M.ctor()        -- 构造

return M