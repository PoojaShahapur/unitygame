require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.StaticClass"

local M = GlobalNS.StaticClass();
local this = M;
M.clsName = "UtilApi";
GlobalNS[M.clsName] = M;

function M.ctor()

end

function M.getComponent(go, name)
	return go:getComponent(name);
end

function M.notBool(value)
	local ret = value;
	ret = not value;
	return ret;
end

function M.setTextStr(go, str)
    go:getComponent('Text').text = str;
end

function M.setImageSprite(go, path)
    go:getComponent('Image').sprite = loadSprite(path);
end

function M.modifyListByList(srcList, destList, cls)
    local index;
    local srcItem;
    if srcList:Count() > destList:Count() then
        index = srcList:Count() - 1;
        while(index >= destList:Count()) do
            srcItem = srcList:removeAtAndRet(index);
            srcItem:dtor();
            index = index - 1;
        end
    else
        index = srcList:Count();
        while(index < destList:Count()) do
            srcItem = GlobalNS.new(cls);
            srcList:add(srcItem);
            index = index + 1;
        end
    end
end

function M.enableWidget()

end

M.ctor()        -- 构造

return M;