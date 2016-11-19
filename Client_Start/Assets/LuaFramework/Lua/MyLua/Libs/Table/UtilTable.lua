MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

local M = GlobalNS.StaticClass();
local this = M;
M.clsName = "UtilTable";
GlobalNS[M.clsName] = M;

function M.ctor()
    this.m_prePos = 0;        -- 记录之前的位置
    this.m_sCnt = 0;
end

function M.readString(bytes, tmpStr)
    _, this.m_sCnt = bytes:readUnsignedInt16(this.m_sCnt);
    _, tmpStr = bytes:readMultiByte(tmpStr, this.m_sCnt, GlobalNS.GkEncode.UTF8);
	return tmpStr;
end

M.ctor();        -- 构造

return M;