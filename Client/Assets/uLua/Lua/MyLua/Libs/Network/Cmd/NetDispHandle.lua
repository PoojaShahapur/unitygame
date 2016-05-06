local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "NetDispHandle";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.m_id2DispDic = GlobalNS.new(GlobalNS.Dictionary);
end

function M:handleMsg(msg)
    local byCmd = 0;
    byCmd = msg.readUnsignedInt8(byCmd);
    local byParam = 0;
    byParam = msg.readUnsignedInt8(byParam);
    msg.setPos(0);

    if(self.m_id2DispDic.ContainsKey(byCmd)) then
        self.m_id2DispDic[byCmd].handleMsg(msg, byCmd, byParam);
    else
        
    end
end