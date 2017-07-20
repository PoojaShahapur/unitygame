MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");
MLoader("MyLua.Libs.DataStruct.MLinkListNode");
MLoader("MyLua.Libs.Functor.CmpFuncObject");
MLoader("MyLua.Libs.DataStruct.MListBase");

--连接列表实现
local M = GlobalNS.Class(GlobalNS.MListBase);
M.clsName = "MLinkList";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.mHead = nil;  -- 头指针
    self.mTail = nil;  -- 尾指针
    self.mCount = 0    -- 总共数量
end

function M:dtor()

end

function M:isEmpty()
    return self.mCount == 0;
end

function M:get(index)
    if index < self:getLen() then
        local idx = 0;
        local ret = self.mHead;
        
        while(idx < index) do
            idx = idx + 1;
            ret = ret:getNext();
        end
        
        return ret;
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
    return self.mCount;
end

-- 在 Head 添加一个 Node
function M:addHead(value)
    self.mCount = self.mCount + 1;
    
    local node = GlobalNS.new(GlobalNS.MLinkListNode)
    
    node:setData(value)
    node:setNext(self.mHead);
    
    self.mHead = node;
    
    if(self.mTail == nil) then
        self.mTail = node
    end
end

function M:removeHead()
    if(self.mHead ~= nil) then
        self.mCount = self.mCount - 1;
        
        local ret;
        ret = self.mHead;
        self.mHead = self.mHead:getNext();
        
        if(self.mHead == nil) then
            self.mTail = nil;
        else
            self.mHead:setPrev(nil);
        end
        
        return ret;
    end
    
    return nil;
end

function M:addTail(value)
    self.mCount = self.mCount + 1;
    
    local node = GlobalNS.new(GlobalNS.MLinkListNode);
    
    node:setData(value);
    node:setPrev(self.mTail);

    if(self.mTail ~= nil) then
        self.mTail:setNext(node);
    else
        self.mHead = node;
    end
    
    self.mTail = node;
end

function M:removeTail()
    if(self.mTail ~= nil) then
        self.mCount = self.mCount + 1;
        
        local ret;
        ret = self.mTail;
        self.mTail = self.mTail:getPrev();
        
        if(self.mTail == nil) then
            self.mHead = nil;
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
    elseif(index < self.mCount) then
        -- 如果插入位置不是在结尾
        local elem = self:get(index);
        local node = GlobalNS.new(GlobalNS.MLinkListNode);
        node:setData(data);
        node:setNext(elem)
        node:setPrev(elem:getPrev())
        
        if(elem:getPrev() ~= nil) then
            elem:getPrev():setNext(node);
        end
        elem:setPrev(node);
        
        self.mCount = self.mCount + 1;
    else
        -- 如果插入位置在结尾
        self:addTail(data);
    end
end

function M:removeAt(index)
    if(index < self.mCount) then
        if(index == 0) then
            self:removeHead();
        elseif(index == self.mCount - 1) then
            self:removeTail();
        else
            -- LinkList 不使用 table ，而是使用的是 node 连接的
            local idx = 0;
            local ret = self.mHead;
            
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

function M:remove(value)
    local elem = self.mHead;
    local bFind = false
    
    while(elem ~= nil) do
        if self:cmpFunc(elem:getData(), value) == 0 then
            if(elem:getPrev() ~= nil) then
                elem:getPrev():setNext(elem:getNext());
            elseif(elem:getNext() ~= nil) then
                elem:getNext():setPrev(elem:getPrev());
            end
            
            if(self.mHead == elem) then
                self.mHead = elem:getNext();
            end
            if(self.mTail == elem) then
                self.mTail = elem:getPrev(); 
            end
            
            break;
        end
        elem = elem:getNext();
    end
    
    return bFind
end

function M:find(value, func, pThis)
    self:setFuncObject(pThis, func);

    local elem = self.mHead;
    local bFind = false;
    
    while(elem ~= nil) do
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

function M:tostring()
    local str = ''
    local elem = self.mHead;
    
    while(elem ~= nil) do
        str = str .. elem:getData();
        elem = elem:getNext();
        str = str .. ', ';
    end
    
    print(str)
end

return M;