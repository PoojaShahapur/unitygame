MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

-- 非优先级或者优先级列表
local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "INoOrPriorityList";
GlobalNS[M.clsName] = M;

function M:ctor()
	
end

function M:dtor()
	
end

function M:setIsSpeedUpFind(value)

end

function M:setIsOpKeepSort(value)

end

function M:Clear()

end

function M:Count()

end

function M:get(index)

end

function M:Contains(item)

end

function M:RemoveAt(index)

end

function M:getIndexByNoOrPriorityObject(priorityObject)

end

function M:addNoOrPriorityObject(noPriorityObject, priority)

end

function M:removeNoOrPriorityObject(noPriorityObject)

end

return M;