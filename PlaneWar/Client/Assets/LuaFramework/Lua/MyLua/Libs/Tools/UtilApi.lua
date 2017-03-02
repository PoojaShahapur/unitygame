MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

local M = GlobalNS.StaticClass();
local this = M;
M.clsName = "UtilApi";
GlobalNS[M.clsName] = M;

function M.ctor()

end

function M.getInputFieldText(inputField)
    return inputField.m_Text;
end

function M.getInputFieldComp(go, path)
    return this.getComp(go, path, 'InputField');
end

function M.getTextComp(go, path)
    return this.getComp(go, path, 'Text');
end

function M.getImageCompByPath(go, path)
	return this.getComp(go, path, 'Image');
end

function M.getImageCompNoPath(go)
	return M.GetComponent(go, 'Image');
end

function M.getComp(go, path, comptName)
    local retgo = GlobalNS.CSSystem.TransFindChildByPObjAndPath(go, path);
    return M.GetComponent(retgo, comptName);
end

function M.setTextStr(go, str)
    M.GetComponent(go, 'Text').text = str;
end

function M.setTextColor(go, r, g, b)
    M.GetComponent(go, 'Text').color = Color.New(r, g, b);
end

function M.setTextColorInOne(go, color)
	M.GetComponent(go, 'Text').color = color;
end

function M.setImageColor(go, r, g, b, a)
    M.GetComponent(go, 'Image').color = Color.New(r, g, b, a);
end

function M.getText(textComp)
    return textComp.text;
end

function M.GoFindChildByName(name)
	return GlobalNS.CSSystem.GoFindChildByName(name);
end

function M.TransFindChildByPObjAndPath(pObject, path)
	return GlobalNS.CSSystem.TransFindChildByPObjAndPath(pObject, path);
end

function M.Destroy(Obj)
    UnityEngine.Object.Destroy(Obj);
end

function M.DestroyImmediate(Obj)
    UnityEngine.Object.DestroyImmediate(Obj);
end

function M.Instantiate(orig)
    return UnityEngine.Object.Instantiate(orig);
end

function M.SetParent(child, parent, worldPositionStays)
	GlobalNS.CSSystem.SetParent(child, parent, worldPositionStays);
end

function M.SetRectTransformParent(child, parent, worldPositionStays)
	GlobalNS.CSSystem.SetRectTransParent(child, parent, worldPositionStays);
end

function M.SetActive(target, bShow)
	if(nil ~= target and (GlobalNS.UtilApi.IsActive(target) ~= bShow)) then
		target:SetActive(bShow);
	end
end

function M.IsActive(target)
	if(nil ~= target) then
		return target.activeSelf;
	end
	
	return false;
end

function M.AddComponent(target, name)
    target:AddComponent(name);
end

function M.setImageSprite(go, path)
	local auxSpriteAtlasLoader = GlobalNS.new(GlobalNS.AuxSpriteAtlasLoader);
	auxSpriteAtlasLoader:syncLoad(path, nil, nil, nil);
	local sprite = auxSpriteAtlasLoader:getSprite(path);
	M.GetComponent(go, 'Image').sprite = sprite;
	sprite = nil;
end

function M.setImageSpriteBySprite(image, sprite)
	image.sprite = sprite;
end

function M.setSpriteRenderSprite(go, path)
    local auxSpriteAtlasLoader = GlobalNS.new(GlobalNS.AuxSpriteAtlasLoader);
	auxSpriteAtlasLoader:syncLoad(path, nil, nil, nil);
	local sprite = auxSpriteAtlasLoader:getSprite(path);
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
    local layoutElem = M.GetComponent(go, 'LayoutElement');
    if(layoutElem ~= nil) then
        layoutElem.preferredHeight = preferredHeight;
    end
end

function M.SetSiblingIndex(trans, index)
    trans:SetSiblingIndex(index);
end

function M.SetSiblingIndexByGo(go, index)
    go.transform:SetSiblingIndex(index);
end

function M.SetSiblingIndexToLastTwoByGo(go, index)
    go.transform:SetSiblingIndex(go.transform.parent.childCount - 1);
end

function M.addToggleHandle(go, table, method)
    GlobalNS.CSSystem.addToggleHandle(go, table, method);
end

function M.getChildCount(trans)
    return trans.childCount;
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
	return go:GetComponent(name);
end

function M.notBool(value)
	local ret = value;
	ret = not value;
	return ret;
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
	if (nil ~= go) then
		local rectTransform = M.GetComponent(go, 'RectTransform');
		if(nil ~= rectTransform) then
			rectTransform.localScale = scale;
		end
	end
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
    return go.transform:Find(path):GetComponent(typeName);
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

function M.RemoveListener(go, luaTable, luaFunction)
    
end

function M.removeButtonDownEventHandle(go, luaTable, luaFunction)
	
end

function M.removeButtonUpEventHandle(go, luaTable, luaFunction)
	
end

function M.removeButtonExitEventHandle(go, luaTable, luaFunction)
	
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

M.ctor()        -- 构造

return M;