require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"

local M = GlobalNS.Class(GlobalNS.GObject:new())
M.clsName = "TableItemBase"
GlobalNS[M.clsName] = M

function M:ctor()
    self.m_itemHeader = nil
    self.m_itemBody = nil
end

function M:parseHeaderByteBuffer(bytes)
    if nil == self.m_itemHeader then
        self.m_itemHeader = GlobalNS.TableItemHeader:news();
    end
    self.m_itemHeader.parseHeaderByteBuffer(bytes);
end

function M:parseBodyByteBuffer(bytes, offset, cls)
    if nil == self.m_itemBody then
        self.m_itemBody = cls:new();
    end

    self.m_itemBody.parseBodyByteBuffer(bytes, offset);
end

function M:parseAllByteBuffer(bytes, cls)
    -- 解析头
    self.parseHeaderByteBuffer(bytes);
    -- 保存下一个 Item 的头位置
    GlobalNS["UtilTable"].m_prePos = bytes.position;
    -- 解析内容
    self.parseBodyByteBuffer(bytes, self.m_itemHeader.m_offset, cls);
    -- 移动到下一个 Item 头位置
    bytes.setPos(UtilTable.m_prePos);
end

return M