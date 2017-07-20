MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

MLoader("MyLua.Libs.DataStruct.MList");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "LogSys";
GlobalNS[M.clsName] = M;

function M:ctor()
	self.mEnableLog = GlobalNS.new(GlobalNS.MList);
	self.mEnableLog:add(MacroDef.ENABLE_LOG);
	self.mEnableLog:add(MacroDef.ENABLE_WARN);
	self.mEnableLog:add(MacroDef.ENABLE_ERROR);
	
	self.mEnableLogTypeList = GlobalNS.new(GlobalNS.MList);
	self.mEnableLogTypeList:add(GlobalNS.new(GlobalNS.MList));     -- log
	self.mEnableLogTypeList:add(GlobalNS.new(GlobalNS.MList));     -- warn
	self.mEnableLogTypeList:add(GlobalNS.new(GlobalNS.MList));     -- error
	
	local filterList = self.mEnableLogTypeList:get(GlobalNS.LogColor.eLC_LOG);
	--filterList:add(GlobalNS.LogTypeId.eLogCommon);
	filterList:add(GlobalNS.LogTypeId.eLogTest);
	--filterList:add(GlobalNS.LogTypeId.eLogNoPriorityListCheck);
	
	filterList = self.mEnableLogTypeList:get(GlobalNS.LogColor.eLC_WARN);
	
	filterList = self.mEnableLogTypeList:get(GlobalNS.LogColor.eLC_ERROR);
end

function M:dtor()
    
end

function M:init()
    
end

function M:dispose()

end

function M:isInFilter(logTypeId, logColor)
	local ret = false;
	
	if(self.mEnableLog:get(logColor)) then
		if(self.mEnableLogTypeList:get(logColor):Contains(logTypeId)) then
            ret = true;
        end
	end
    
    return ret;
end

function M:log(message, logTypeId)
	if(nil == logTypeId) then
		logTypeId = GlobalNS.LogTypeId.eLogCommon;
	end
    -- 输出日志信息
    if(self:isInFilter(logTypeId, GlobalNS.LogColor.eLC_LOG)) then
        GlobalNS.CSSystem.log(message, logTypeId);    
    end
end

function M:warn(message, logTypeId)
	if(nil == logTypeId) then
		logTypeId = GlobalNS.LogTypeId.eWarnCommon;
	end
	
    -- 输出日志信息
    if(self:isInFilter(logTypeId, GlobalNS.LogColor.eLC_WARN)) then
        GlobalNS.CSSystem.warn(message, logTypeId);    
    end
end

function M:error(message, logTypeId)
	if(nil == logTypeId) then
		logTypeId = GlobalNS.LogTypeId.eErrorCommon;
	end
	
    -- 输出日志信息
    if(self:isInFilter(logTypeId, GlobalNS.LogColor.eLC_ERROR)) then
        GlobalNS.CSSystem.error(message, logTypeId);    
    end
end

return M;