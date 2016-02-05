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
    self:testAddTail();
end

function M:testAddTail()
    self.m_linkList:addHead(1);
    self.m_linkList:addHead(2);
    self.m_linkList:addHead(3);
    self.m_linkList:addHead(4);
    self.m_linkList:addHead(5);
end

return M;