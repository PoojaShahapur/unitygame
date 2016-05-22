require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.StaticClass"

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

function M.getComp(go, path, comptName)
    local retgo = uiMgr:TransFindChildByPath(go, path);
    return retgo:GetComponent(comptName);
end

function M.setTextStr(go, str)
    go:GetComponent('Text').text = str;
end

function M.setTextColor(go, r, g, b)
    go:GetComponent('Text').color = Color.New(r, g, b);
end

function M.setTextColorInOne(go, color)
    go:GetComponent('Text').color = color;
end

function M.setImageColor(go, r, g, b)
    go:GetComponent('Image').color = Color.New(r, g, b);
end

function M.getText(textComp)
    return textComp.text;
end

function M.GoFindChildByName(name)
	return GlobalNS.CSSystem.UtilApi.GoFindChildByName(name);
end

function M.TransFindChildByPObjAndPath(pObject, path)
	return GlobalNS.CSSystem.UtilApi.TransFindChildByPObjAndPath(pObject, path);
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
	GlobalNS.CSSystem.UtilApi.SetParent(child, parent, worldPositionStays);
end

function M.SetRectTransformParent(child, parent, worldPositionStays)
	GlobalNS.CSSystem.UtilApi.SetRectTransParent(child, parent, worldPositionStays);
end

function M.SetActive(target, bShow)
    target:SetActive(bShow);
end

function M.IsActive(target)
    return target.activeSelf;
end

function M.AddComponent(target, name)
    target:AddComponent(name);
end

function M:setImageSprite(go, path)
    go:GetComponent('Image').sprite = uiMgr:LoadSprite(path);
end

function M.setSpriteRenderSprite(go, path)
    local sprite = uiMgr:LoadSprite(path);
    go:GetComponent('SpriteRenderer').sprite = sprite;
end

function M.setSpriteRenderSpriteByGo(go, goPath, spritePath)
    local sprite = uiMgr:LoadSprite(spritePath);
    local spriteGo = uiMgr:TransFindChildByPath(goPath);
    spriteGo:GetComponent('SpriteRenderer').sprite = sprite;
end

function M.setLayoutElementPreferredHeight(go, preferredHeight)
    local layoutElem = go:GetDComponent('LayoutElement');
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

function M.getChildCount(trans)
    return trans.childCount;
end

function M.setRectRotate(go, rotateX, rotateY, rotateZ)
    local rectTransform = go:GetComponent('RectTransform');
    local rot = rectTransform.localEulerAngles;
    rot.x = rotateX;
    rot.y = rotateY;
    rot.z = rotateZ;
    rectTransform.localEulerAngles = rot;
end

function M.getComponent(go, name)
	return go:getComponent(name);
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
    local rectTransform = go:GetComponent('RectTransform');
    local sizeDelta = rectTransform.sizeDela;
    sizeDelta.x = width;
    sizeDelta.y = height;
end

function M.enableBtn(go)
    local btn = go:GetComponent('Button');
    if(btn ~= nil) then
        btn.interactable = true;
    end
end

function M.disableBtn()
    local btn = go:GetComponent('Button');
    if(btn ~= nil) then
        btn.interactable = false;
    end
end

function M.setSliderPos(go, value)
    local slider = go:GetComponent('Slider');
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

function LuaGC()
  local c = collectgarbage("count")
  --Debugger.Log("Begin gc count = {0} kb", c)
  collectgarbage("collect")
  c = collectgarbage("count")
  --Debugger.Log("End gc count = {0} kb", c)
end

M.ctor()        -- 构造

return M;