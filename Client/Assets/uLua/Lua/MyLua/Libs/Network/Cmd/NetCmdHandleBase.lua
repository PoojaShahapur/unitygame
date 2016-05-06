local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "NetDispList";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.m_id2HandleDic = GlobalNS.new(GlobalNS.Dictionary);
end

function M:handleMsg(bu, byCmd, byParam)
    if(self.m_id2HandleDic:ContainsKey(byParam)) then
        self.m_id2HandleDic:key(byParam)(bu);
    else
        
    end
end