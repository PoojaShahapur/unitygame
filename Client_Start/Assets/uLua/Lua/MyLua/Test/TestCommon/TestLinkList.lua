require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"

require "MyLua.Libs.DataStruct.MLinkList"

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "TestLinkList";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.m_linkList = GlobalNS.new(GlobalNS.MLinkList);
end

function M:dtor()
    
end

function M:run()
    --self:testAddHead();
    self:testAddTail();
end

function M:testAddHead()
    self.m_linkList:addHead(1);
    self.m_linkList:addHead(2);
    self.m_linkList:addHead(3);
    self.m_linkList:addHead(4);
    self.m_linkList:addHead(5);
    self.m_linkList:tostring();
end

function M:testAddTail()
    self.m_linkList:addTail(1);
    self.m_linkList:addTail(2);
    self.m_linkList:addTail(3);
    self.m_linkList:addTail(4);
    self.m_linkList:addTail(5);
    self.m_linkList:tostring();
    
    self.m_linkList:removeAt(2);
    self.m_linkList:tostring();
    
    self.m_linkList:insert(2, 3);
    self.m_linkList:tostring();
end

return M;