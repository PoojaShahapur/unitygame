MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

local M = GlobalNS.StaticClass();
local this = M;
M.clsName = "UtilApi";
GlobalNS[M.clsName] = M;

function M.ctor()

end

function M.getInputFieldText(inputField)
	local ret = nil;
	
	if(nil ~= inputField) then
		ret = inputField.m_Text;
	end
	
	return ret;
end

function M.getInputFieldComp(go, path)
	local ret = nil;
	
	if(nil ~= go) then
		ret = this.getCompByPath(go, path, 'InputField');
	end
	
	return ret;
end

function M.getTextCompByPath(go, path)
	local ret = nil;
	
	if(nil ~= go) then
		ret = this.getCompByPath(go, path, 'Text');
	end
	
	return ret;
end

function M.getTextCompNoPath(go, path)
	local ret = nil;
	
	if(nil ~= go) then
		ret = M.GetComponent(go, 'Text');
	end

	return ret;
end

function M.getImageCompByPath(go, path)
	local ret = nil;
	
	if(nil ~= go) then
		ret = this.getCompByPath(go, path, 'Image');
	end
	
	return ret;
end

function M.getImageCompNoPath(go)
	local ret = nil;
	
	if(nil ~= go) then
		ret = M.GetComponent(go, 'Image');
	end
	
	return ret;
end

function M.getCompByPath(go, path, comptName)
	local ret = nil;
	
	if(nil ~= go) then
		local retgo = GlobalNS.CSSystem.TransFindChildByPObjAndPath(go, path);
		ret = M.GetComponent(retgo, comptName);
	end
	
	return ret;
end

function M.getSliderCompNoPath(go)
	local ret = nil;
	
	if(nil ~= go) then
		ret = M.GetComponent(go, 'Slider');
	end
	
	return ret;
end

function M.setTextStr(go, str)
	if(nil ~= go) then
		local text = nil;
		text = M.GetComponent(go, 'Text');
		
		if(nil ~= text) then
			text.text = str;
		end
	
		text = nil;
	end
end

function M.setTextColor(go, r, g, b)
	if(nil ~= go) then
		local text = nil;
		text = M.GetComponent(go, 'Text');
		
		if(nil ~= text) then
			text.color = Color.New(r, g, b);
		end
		
		text = nil;
	end
end

function M.setTextColorInOne(go, color)
	if(nil ~= go) then
		local text = nil;
		text = M.GetComponent(go, 'Text');
		
		if(nil ~= text) then
			text.color = color;
		end
		
		text = nil;
	end
end

function M.setImageColor(go, r, g, b, a)
	if(nil ~= go) then
		local image = nil;
		image = M.GetComponent(go, 'Image');
		
		if(nil ~= image) then
			image.color = Color.New(r, g, b, a);
		end
		
		image = nil;
	end
end

function M.getText(textComp)
	local ret = nil;
	
	if(nil ~= textComp) then
		ret = textComp.text;
	end

	return ret;
end

function M.GoFindChildByName(name)
	return GlobalNS.CSSystem.GoFindChildByName(name);
end

function M.TransFindChildByPObjAndPath(pObject, path)
	return GlobalNS.CSSystem.TransFindChildByPObjAndPath(pObject, path);
end

function M.Destroy(Obj)
	if(nil ~= Obj) then
		UnityEngine.Object.Destroy(Obj);
	end
end

function M.DestroyImmediate(Obj)
	if(nil ~= Obj) then
		UnityEngine.Object.DestroyImmediate(Obj);
	end
end

function M.Instantiate(orig)
	local ret = nil;
	
	if(nil ~= orig) then
		ret = UnityEngine.Object.Instantiate(orig);
	end

	return ret;
end

function M.SetParent(child, parent, worldPositionStays)
	GlobalNS.CSSystem.SetParent(child, parent, worldPositionStays);
end

function M.SetRectTransformParent(child, parent, worldPositionStays)
	GlobalNS.CSSystem.SetRectTransParent(child, parent, worldPositionStays);
end

function M.SetActive(target, isShow)
	if(nil ~= target and (GlobalNS.UtilApi.IsActive(target) ~= isShow)) then
		target:SetActive(isShow);
	end
end

function M.IsActive(target)
	local ret = false;
	
	if(nil ~= target) then
		ret = target.activeSelf;
	end
	
	return ret;
end

function M.AddComponent(target, name)
	if(nil ~= target and not UtilStr.isNullOrEmpty(name)) then
		target:AddComponent(name);
	end
end

function M.addCanvasGroupComponent(target)
	if(nil ~= target) then
		target:AddComponent("CanvasGroup");
	end
end

function M.getCanvasGroupComponent(target)
	local ret = nil;
	
	if(nil ~= target) then
		ret = target:GetComponent("CanvasGroup");
	end
	
	return ret;
end

function M.setImageSprite(go, path, spriteName)
	local auxSpriteAtlasLoader = GlobalNS.new(GlobalNS.AuxSpriteAtlasLoader);
	auxSpriteAtlasLoader:syncLoad(path, nil, nil, nil);
	local sprite = auxSpriteAtlasLoader:getSprite(spriteName);
	M.GetComponent(go, 'Image').sprite = sprite;
	sprite = nil;
end

function M.setImageSpriteBySprite(image, sprite)
	if(not GlobalNS.UtilApi.IsUObjNil(image)) then
		image.sprite = sprite;
	end
end

function M.setSpriteRenderSprite(go, path, spriteName)
    local auxSpriteAtlasLoader = GlobalNS.new(GlobalNS.AuxSpriteAtlasLoader);
	auxSpriteAtlasLoader:syncLoad(path, nil, nil, nil);
	local sprite = auxSpriteAtlasLoader:getSprite(spriteName);
    M.GetComponent(go, 'SpriteRenderer').sprite = sprite;
end

function M.setSpriteRenderSpriteByGo(go, goPath, spritePath)
	local auxSpriteAtlasLoader = GlobalNS.new(GlobalNS.AuxSpriteAtlasLoader);
	auxSpriteAtlasLoader:syncLoad(path, nil, nil, nil);
	local sprite = auxSpriteAtlasLoader:getSprite(spritePath);
	
    local spriteGo = GlobalNS.CSSystem.TransFindChildByPObjAndPath(go, goPath);
    M.GetComponent(spriteGo, 'SpriteRenderer').sprite = sprite;
end

function M.setLayoutElementPreferredHeight(go, preferredHeight)
	if(nil ~= go) then
		local layoutElem = M.GetComponent(go, 'LayoutElement');
		
		if(layoutElem ~= nil) then
			layoutElem.preferredHeight = preferredHeight;
		end
	end
end

function M.SetSiblingIndex(trans, index)
	if(nil ~= trans) then
		trans:SetSiblingIndex(index);
	end
end

function M.SetSiblingIndexByGo(go, index)
	if(nil ~= go) then
		go.transform:SetSiblingIndex(index);
	end
end

function M.SetSiblingIndexToLastTwoByGo(go)
	if(nil ~= go) then
		go.transform:SetSiblingIndex(go.transform.parent.childCount - 1);
	end
end

-- 设置索引到最低端，显示在最后面
function M.SetSiblingIndexToZero(go)
	if(nil ~= go) then
		go.transform:SetSiblingIndex(0);
	end
end

function M.addToggleHandle(go, table, method)
    GlobalNS.CSSystem.addToggleHandle(go, table, method);
end

function M.addInputEndHandle(go, table, method)
    GlobalNS.CSSystem.addInputEndHandle(go, table, method);
end

function M.addDropdownHandle(go, table, method)
    GlobalNS.CSSystem.addDropdownHandle(go, table, method);
end

function M.getChildCount(trans)
	local ret = 0;
	
	if(nil ~= trans) then
		ret = trans.childCount;
	end
	
	return ret;
end

function M.setRectRotate(go, rotateX, rotateY, rotateZ)
    local rectTransform = M.GetComponent(go, 'RectTransform');
    local rot = rectTransform.localEulerAngles;
    rot.x = rotateX;
    rot.y = rotateY;
    rot.z = rotateZ;
    rectTransform.localEulerAngles = rot;
end

-- GetComponent 不是 getComponent ，大小写是有区别的
function M.GetComponent(go, name)
	local ret = "";
	
	if(nil ~= go) then
		ret = go:GetComponent(name);
	end
	
	return ret;
end

function M.notBool(value)
	local ret = value;
	ret = not value;
	return ret;
end

function M.modifyListByList(srcList, destList, cls)
    local index = 0;
    local srcItem = nil;
	
    if srcList:Count() > destList:Count() then
        index = srcList:Count() - 1;
        while(index >= destList:Count()) do
            srcItem = srcList:getAndRemoveByIndex(index);
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

function M.setRectTransformSizeDelta(go, width, height)
    local rectTransform = M.GetComponent(go, 'RectTransform');
    local sizeDelta = rectTransform.sizeDela;
    sizeDelta.x = width;
    sizeDelta.y = height;
end

function M.setRectScale(rectTrans, scale)
	if (nil ~= rectTrans) then
		rectTrans.localScale = scale;
	end
end

function M.setGoRectScale(go, scale)
	if(nil ~= go) then
		local rectTransform = M.GetComponent(go, 'RectTransform');
		
		if(nil ~= rectTransform) then
			if(rectTransform.localScale.x ~= scale.x or 
				rectTransform.localScale.y ~= scale.y or 
				rectTransform.localScale.z ~= scale.z) then
				rectTransform.localScale = scale;
			end
		end
	end
end

function M.getGoRectScaleX(go)
	local scaleX = 0;
	
	if (nil ~= go) then
		local rectTransform = M.GetComponent(go, 'RectTransform');
		
		if(nil ~= rectTransform) then
			scaleX = rectTransform.localScale.x;
		end
	end
	
	return scaleX;
end

function M.setUGuiRectScale(uguiElement, scale)
	if (nil ~= uguiElement) then
		uguiElement.rectTransform.localScale = scale;
	end
end

function M.enableBtn(go)
    local btn = M.GetComponent(go, 'Button');
    if(btn ~= nil) then
        btn.interactable = true;
    end
end

function M.disableBtn(go)
    local btn = M.GetComponent(go, 'Button');
    if(btn ~= nil) then
        btn.interactable = false;
    end
end

function M.setSliderPos(go, value)
    local slider = M.GetComponent(go, 'Slider');
    if(slider ~= nil) then
        slider.Value = value;
    end
end

function M.createGameObject(name)
    local ret = UnityEngine.GameObject.New(name);
    return ret;
end

function M.isTableEmpty(tbl)
    for _, _ in paors(tbl) do
        return false;
    end
    
    return true;
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

function M.isTypeEqual(a, b)
    return type(a) == type(b);
end

function M.isType(a, TypeName)
    return type(a) == TypeName;
end

function M.isNumber(a)
    return type(a) == "number";
end

function M.isBoolean(a)
    return type(a) == "boolean";
end

function M.isString(a)
    return type(a) == "string";
end

function M.isFunction(a)
    return type(a) == "function";
end

function M.isTable(a)
    return type(a) == "table";
end

function M.isUserData(a)
    return type(a) == "userdata";
end

function M.isThread(a)
    return type(a) == "thread";
end

function M.isNil(a)
    return type(a) == "nil";
end

function M.isNullOrEmpty(str)
    return str == nil or str == '';
end

function M.toString(...)
    local arg = {...};
    local t = {};
    
    for i, k in ipairs(arg) do
        table.insert(t, tostring(k));
    end
    
    local str = table.concat(t);
    return str;
end

function M.formatStr(format, ...)
    return string.format(format, ...);
end

function M.error(str)
    error(str);
end

function M.assert(condition)
	assert(condition);
end

function M.LuaGC()
  local c = collectgarbage("count")
  --Debugger.Log("Begin gc count = {0} kb", c)
  collectgarbage("collect")
  c = collectgarbage("count")
  --Debugger.Log("End gc count = {0} kb", c)
end

function M.getComByPath(go, path, typeName)
	local ret = nil;
	
	if(nil ~= go) then
		local trans = go.transform:Find(path);
		
		if(nil ~= trans) then
			ret = trans:GetComponent(typeName);
		end
	end

	return ret;
end

-- 从 Parent 获取一个组件
function M.getComFromSelf(go, typeName)
    return M.GetComponent(go, typeName);
end

function M.addEventHandleByPath(go, path, luaTable, luaFunction)
    GlobalNS.CSSystem.addEventHandleByPath(go, path, luaTable, luaFunction);
end

function M.addEventHandleSelf(go, luaTable, luaFunction)
    GlobalNS.CSSystem.addEventHandleSelf(go, luaTable, luaFunction);
end

function M.addButtonDownEventHandle(go, luaTable, luaFunction)
    GlobalNS.CSSystem.addButtonDownEventHandle(go, luaTable, luaFunction, false);
end

function M.addButtonUpEventHandle(go, luaTable, luaFunction)
    GlobalNS.CSSystem.addButtonUpEventHandle(go, luaTable, luaFunction, false);
end

function M.addButtonExitEventHandle(go, luaTable, luaFunction)
    GlobalNS.CSSystem.addButtonExitEventHandle(go, luaTable, luaFunction, false);
end

function M.removeEventHandleSelf(go, luaTable, luaFunction)
	GlobalNS.CSSystem.removeEventHandleSelf(go, luaTable, luaFunction);
end

function M.removeButtonDownEventHandle(go, luaTable, luaFunction)
	GlobalNS.CSSystem.removeButtonDownEventHandle(go, luaTable, luaFunction);
end

function M.removeButtonUpEventHandle(go, luaTable, luaFunction)
	GlobalNS.CSSystem.removeButtonUpEventHandle(go, luaTable, luaFunction);
end

function M.removeButtonExitEventHandle(go, luaTable, luaFunction)
	GlobalNS.CSSystem.removeButtonExitEventHandle(go, luaTable, luaFunction);
end

function getStrLen(str)
	return string.len(str);
end

function M.isTrue(value)
	return not M.isFalse(value);
end

function M.isFalse(value)
	return (nil == value or 0 == value or false == value);
end

--判断字符串是否是 nil 或者 ""
function M.IsNullOrEmpty(value)
	return (nil == value or "" == value);
end

function M.tonumber(e)
	local ret = nil;
	ret = tonumber(e);
	return ret;
end

function M.setGoName(go, name)
	if(nil ~= go) then
		go.name = name;
	end
end

--[[
unity 对象判断为空, 如果你有些对象是在c#删掉了，lua 不知道
判断这种对象为空时可以用下面这个函数。
引用变量存储的是对象的内存地址，对象销毁时内存还没有回收，也就是说go存储原来的内存地址，那他就不会等于nil了；
而Equals是比较两个对象的状态，所以go == nil 是false，go:Equals(nil)的结果是true。
]]--
function M.IsUObjNil(uobj)
    return uobj == nil or uobj:Equals(nil);
end

--将当前go的tranfrom从form转到to
function M.convPtFromLocal2Local(from, to)
    return GlobalNS.CSSystem.convPtFromLocal2Local(from, to);
end

function M.getTableLen(table_)
	local listLen = 0;
	
	if(nil ~= table_) then
		listLen = table.getn(table_);
	end
	
	return listLen;
end

-- index 从 0 开始
function M.getTableElementByIndex(table_, index)
	local element = nil;
	
	if(nil ~= table_ and index < M.getTableLen(table_)) then
		element = table_[index + 1];
	end
	
	return element;
end

function M.GetTextWordNum(str)
    local lenInByte = #str;
    local count = 0;
    local i = 1;
    while i <= lenInByte do
        local curByte = string.byte(str, i);
        local byteCount = 1;
        if curByte > 0 and curByte < 128 then
            byteCount = 1;
        elseif curByte>=128 and curByte<224 then
            byteCount = 2;
        elseif curByte>=224 and curByte<240 then
            byteCount = 3;
        elseif curByte>=240 and curByte<=247 then
            byteCount = 4;
        else
            break;
        end

        i = i + byteCount;
        count = count + 1;
    end
    return count;
end

function M.GetIsContainKeyword(str)
    return ((string.find(str, " ") ~= nil) or (string.find(str, "%%") ~= nil));
end

function M.setCanvasGroupAlphaByGo(go, value)
	GlobalNS.CSSystem.UtilApi.setCanvasGroupAlphaByGo(go, value);
end

function M.setCanvasGroupAlpha(canvasGroup, value)
	GlobalNS.CSSystem.UtilApi.setCanvasGroupAlpha(canvasGroup, value);
end

--[[
  @ref http://blog.csdn.net/ningyuanhuo/article/details/43069969
]]

function M.getTimeDesc()
	local date = os.date("%Y-%m-%d %H:%M:%S");
	return date;
end

function M.getYear()
	local date = os.date("%Y");
	date = M.tonumber(date);
	return date;
end

function M.getMonth()
	local date = os.date("%m");
	date = M.tonumber(date);
	return date;
end

function M.getDay()
	local date = os.date("%d");
	date = M.tonumber(date);
	return date;
end

function M.insertTable(t, item)
	if(nil ~= t and nil ~= item) then
		table.insert(t, item);
	end
end

M.ctor()        -- 构造

return M;