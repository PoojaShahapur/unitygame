MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "TableItemBase";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.mItemHeader = nil;
    self.mItemBody = nil;
end

function M:parseHeaderByteBuffer(bytes)
    if(nil == self.mItemHeader) then
        self.mItemHeader = GlobalNS.new(GlobalNS.TableItemHeader);
    end
    self.mItemHeader:parseHeaderByteBuffer(bytes);
end

function M:parseBodyByteBuffer(bytes, offset, cls)
    if(nil == self.mItemBody) then
        self.mItemBody = GlobalNS.new(cls);
    end

    self.mItemBody:parseBodyByteBuffer(bytes, offset);
end

function M:parseAllByteBuffer(bytes, cls)
    -- 解析头
    self:parseHeaderByteBuffer(bytes);
    -- 保存下一个 Item 的头位置
    GlobalNS.UtilTable.mPrePos = bytes.position;
    -- 解析内容
    self:parseBodyByteBuffer(bytes, self.mItemHeader.mOffset, cls);
    -- 移动到下一个 Item 头位置
    bytes:setPos(GlobalNS.UtilTable.mPrePos);
end

return M;