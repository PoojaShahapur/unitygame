MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

local M = GlobalNS.StaticClass();
M.clsName = "UtilLogic";
GlobalNS[M.clsName] = M;

function M.convObjectType2PanelType(objectType)
	local ret = GlobalNS.UtilMath.MaxNum;

	if(GlobalNS.ObjectType.eModel == objectType or
	   GlobalNS.ObjectType.eBullet == objectType or
	   GlobalNS.ObjectType.eTrail == objectType) then
		ret = objectType;
	elseif(GlobalNS.ObjectType.eDazzleLight == objectType or
		   GlobalNS.ObjectType.ePiece == objectType) then
		ret = objectType;
	end

	return ret;
end

-- 0 + 1 开始是皮肤  1000 + 1 开始是  2000 + 1 开始是会员
function M.convTotal2ShopTopPageIndex(totalIndex)
	local ret = 0;
	ret = totalIndex / 1000;
	return ret;
end

function M.convTopIndex2ShopIndex(topType, subType)
	local ret = 0;
	
	if(0 == topType) then
		ret = subType;
	else
		ret = topType * 1000 + subType;
	end
	
	return ret;
end

function M.getSpriteName(imageName)
	local name = string.format("%04d", GlobalNS.UtilApi.tonumber(imageName));
	return name;
end

-- 获取图集路径
function M.getAtlasPath(typeValue, imageName)
	local path = "Atlas/Itemsicon/";
	local subPath = string.format("%04d", typeValue - 1);
	
	if(GlobalNS.ObjectType.eModel == typeValue) then
		subPath = subPath .. "Plane/";
	elseif(GlobalNS.ObjectType.eBullet == typeValue) then
		subPath = subPath .. "Bullet/";
	elseif(GlobalNS.ObjectType.eMoney == typeValue) then
		subPath = subPath .. "Money/";
	end

	local name = string.format("%04d", GlobalNS.UtilApi.tonumber(imageName));

	path = path .. subPath .. name .. ".asset";
	return path;
end

function M.convSkinId2StartId(skinId)
	return skinId * 10 + 1;
end

function M.convSkinId2EndId(skinId)
	return skinId * 10 + 10;
end

function M.getMoneySpriteNameByType(moneyType)
	local ret = "";
	
	if(GlobalNS.MoneyType.eMoney == moneyType) then
		ret = GlobalNS.MoneyType.MoneySpriteName;
	elseif(GlobalNS.MoneyType.eTicket == moneyType) then
		ret = GlobalNS.MoneyType.TicketSpriteName;
	elseif(GlobalNS.MoneyType.ePlastic == moneyType) then
		ret = GlobalNS.MoneyType.PlasticSpriteName;
	end
	
	return ret;
end

function M.getMoneyAtlasPathByType(moneyType)
	local ret = "";
	
	if(GlobalNS.MoneyType.eMoney == moneyType) then
		ret = GlobalNS.MoneyType.MoneyAtlasPath;
	elseif(GlobalNS.MoneyType.eTicket == moneyType) then
		ret = GlobalNS.MoneyType.TicketAtlasPath;
	elseif(GlobalNS.MoneyType.ePlastic == moneyType) then
		ret = GlobalNS.MoneyType.PlasticAtlasPath;
	end
	
	return ret;
end

function M.getObjectTypeByObjectBaseId(objId)
	local ret = 0;
	local item = GCtx.mTableSys:getItem(GlobalNS.TableId.TABLE_OBJECT, objId);
	
	if(nil ~= item) then
		ret = item.type;
	end
	
	return ret;
end

--转换 skin 表中的 baseId 到对应的道具的类型
function M.convSkinBaseId2ObjectType(skinBaseId)
	local ret = 0;
	local objectBaseId = M.convSkinBaseId2ObjectBaseId(skinBaseId);
	ret = M.getObjectTypeByObjectBaseId(objectBaseId);
	
	return ret;
end

--转换 skin 表中的 baseId 到对应的道具 id
function M.convSkinBaseId2ObjectBaseId(skinBaseId)
	local ret = 0;
	local item = GCtx.mTableSys:getItem(GlobalNS.TableId.TABLE_SKIN , skinBaseId);
	
	if(nil ~= item) then
		ret = M.convSkinTableSkinId2ObjectBaseId(item.skinid);
	end
	
	return ret;
end

-- 转换 skin 表中的 skinid 到对应的道具 id
function M.convSkinTableSkinId2ObjectBaseId(skinid)
	return 10000 + skinid;
end

--通过道具id获取对应道具图
function M.getAtlasAndImageByObjid(objid)
    local obj = LuaExcelManager.object_object[objid];
    local altasPath = GlobalNS.UtilLogic.getAtlasPath(obj.type, obj.image);
    local imageName = GlobalNS.UtilLogic.getSpriteName(obj.image);
    return altasPath, imageName;
end

function M.getAtlasAndImageBySkinBaseId(skinBaseId)
    local objectType = M.convSkinBaseId2ObjectType(skinBaseId);
	local tableItem = GCtx.mTableSys:getItem(GlobalNS.TableId.TABLE_SKIN, skinBaseId);
	local skinImage = 0;
	if(nil ~= tableItem) then
		skinImage = tableItem.image;
	end
    local altasPath = GlobalNS.UtilLogic.getAtlasPath(objectType, skinImage);
    local imageName = GlobalNS.UtilLogic.getSpriteName(skinImage);
    return altasPath, imageName;
end

--获取相对于当前日期偏移的年\月\日
function M.getDateByOffsetDay(deltaDay)
	local year = GlobalNS.UtilApi.getYear();
	local month = GlobalNS.UtilApi.getMonth();
	local day = GlobalNS.UtilApi.getDay();
	
	local sign = GlobalNS.UtilMath.sign(deltaDay);
	deltaDay = GlobalNS.UtilMath.abs(deltaDay);
	
	while(deltaDay > 0) do
		if(sign > 0) then
			year, month, day = M.getDateByAddOneDay(year, month, day);
		else
			year, month, day = M.getDateByMinusOneDay(year, month, day);
		end
		
		deltaDay = deltaDay - 1;
	end
	
	return year, month, day;
end

--在当前日期加一天
function M.getDateByAddOneDay(year, month, day)
	local fullDaysCurMonth = GlobalNS.UtilMath.getNumOfDaysByYearAndMonth(year, month);
	
	if(fullDaysCurMonth > day) then
		day = day + 1;
	else
		day = 1;
		
		if(month < 12) then
			month = month + 1;
		else
			month = 1;
			year = year + 1;
		end
	end
	
	return year, month, day;
end

--在当前日期减一天
function M.getDateByMinusOneDay(year, month, day)
	if(day > 1) then
		day = day - 1;
	else
		if(month > 1) then
			month = month - 1;
		else
			month = 12;
			year = year - 1;
		end
		
		day = GlobalNS.UtilMath.getNumOfDaysByYearAndMonth(year, month);
	end
	
	return year, month, day;
end

-- 获取道具名字
function M.getObjectNameByBaseId(objectBaseId)
	local name = "";
	local tableItem = GCtx.mTableSys:getItem(GlobalNS.TableId.TABLE_OBJECT, objectBaseId);
	
	if(nil ~= tableItem) then
		name = tableItem.name;
	end
	
	return name;
end

function M.conScrollRectIndex2TaskId(value)
	return value - 1;
end

return M;