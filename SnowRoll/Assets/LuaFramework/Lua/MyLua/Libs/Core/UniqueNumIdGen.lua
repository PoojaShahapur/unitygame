-- 生成唯一 ID

MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "UniqueNumIdGen";
GlobalNS[M.clsName] = M;

function M:ctor(uniqueId)
   self.mCurIdx = uniqueId;
   self.mPreIdx = 0;
end

function M:genNewId()
    self.mPreIdx = self.mCurIdx;
    self.mCurIdx = self.mCurIdx + 1;
    return self.mPreIdx;
end

return M;