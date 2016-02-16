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

-- 格式化时间，显示格式为 00年00天00时00分00秒
function M.formatTime(second)
    local ret = "";

    local left = 0;
    local year = math.floor(second / (356 * 24 * 60 * 60));
    left = math.floor(second % (356 * 24 * 60 * 60));
    local day = math.floor(left / (24 * 60 * 60));
    left = math.floor(left % (24 * 60 * 60));
    local hour = math.floor(left / (60 * 60));
    left = math.floor(left % (60 * 60));
    local min = math.floor(left / 60);
    left = math.floor(left % 60);
    local sec = left;

    if(year ~= 0) then
        ret = string.format("%s%d年", ret, year);
    end
    if (day ~= 0) then
        ret = string.format("%s%d天", ret, day);
    end
    if (hour ~= 0) then
        ret = string.format("%s%d时", ret, hour);
    end
    if (min ~= 0) then
        ret = string.format("%s%d分", ret, min);
    end
    if (sec ~= 0) then
        ret = string.format("%s%d秒", ret, sec);
    end

    return ret;
end

M.ctor()        -- 构造

return M;