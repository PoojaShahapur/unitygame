--连接列表实现

require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"
require "MyLua.Libs.DataStruct.MLinkListNode"
require "MyLua.Libs.Common.CmpFuncObject"
require "MyLua.Libs.DataStruct.MListBase"

local M = GlobalNS.Class(GlobalNS.MListBase);
M.clsName = "MLinkList";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.m_head = nil;  -- 头指针
    self.m_tail = nil;  -- 尾指针
    self.m_count = 0    -- 总共数量
end

function M:dtor()

end

function M:isEmpty()
    return self.m_count == 0;
end

function M:at(index)
    if index < self:getLen() then
        local idx = 0;
        local ret = self.m_head;
        
        while(idx < index) do
            idx = idx + 1;
            ret = ret:getNext();
        end
    end
    
    return nil;
end

function M:Count()
    return self:getLen();
end

function M:count()
    return self:getLen();
end

function M:getLen()
    return self.m_count;
end

-- 在 Head 添加一个 Node
function M:addHead(value)
    self.m_count = self.m_count + 1;
    
    local node = GlobalNS.new(GlobalNS.MLinkListNode)
    
    node:setData(value)
    node:setNext(self.m_head);
    
    self.m_head = node;
    
    if(self.m_tail == nil) then
        self.m_tail = node
    end
end

function M:removeHead()
    if(self.m_head ~= nil) then
        self.m_count = self.m_count - 1;
        
        local ret;
        ret = self.m_head;
        self.m_head = self.m_head:getNext();
        
        if(self.m_head == nil) then
            self.m_tail = nil;
        else
            self.m_head:setPrev(nil);
        end
        
        return ret;
    end
    
    return nil;
end

function M:addTail(value)
    self.m_count = self.m_count + 1;
    
    local node = GlobalNS.new(GlobalNS.MLinkListNode);
    
    node.setData(value);
    node.setPrev(self.m_tail);

    if(self.m_tail ~= nil) then
        self.m_tail:setNext(node);
    else
        self.m_head = node;
    end
    
    self.m_tail = node;
end

function M:removeTail()
    if(self.m_tail ~= nil) then
        self.m_count = self.m_count + 1;
        
        local ret;
        ret = self.m_tail;
        self.m_tail = self.m_tail:getPrev();
        
        if(self.m_tail == nil) then
            self.m_head = nil;
        end
        
        return ret;
    end
    
    return nil;
end

-- index 从 0 开始
function M:insert(index, data)
    if(index == 0) then
        -- 插入在开头
        self:addHead(data)
    elseif(index < self.m_count) then
        -- 如果插入位置不是在结尾
        local elem = self:at(index);
        local node = GlobalNS.new(GlobalNS.MLinkListNode);
        node.setData(data);
        node.setNext(elem)
        node.setPrev(elem:getPrev())
        elem:setPrev(node);
        
        self.m_count = self.m_count + 1;
    else
        -- 如果插入位置在结尾
        self:addTail(data)
    end
end

function M:removeAt(index)
    if(index < self.m_count) then
        if(index == 0) then
            self:removeHead();
        elseif(index == self.m_count - 1) then
            self:removeTail();
        else
            -- LinkList 不使用 table ，而是使用的是 node 连接的
            local idx = 0;
            local ret = self.m_head;
            
            while(idx < index) do
                idx = idx + 1;
                ret = ret:getNext();
            end
        
            ret:getPrev():setNext(ret:getNext());
            ret:getNext():setPrev(ret:getPrev());
        end
        
        return true;
    end
    
    return false;
end

function M:find(value, func, pThis)
    self:setFuncObject(pThis, func);

    local elem = self.m_head;
    local bFind = false;
    
    while(elem ~= self.m_tail) do
        if self:cmpFunc(elem:getData(), value) == 0 then
            bFind = true;
            break;
        end
        elem = elem:getNext();
    end
    
    if bFind then
        return elem:getData();
    else
        return nil;
    end
end

return M;