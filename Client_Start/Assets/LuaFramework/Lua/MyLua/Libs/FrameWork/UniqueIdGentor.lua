-- 生成唯一 ID

MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "UniqueIdGentor";
GlobalNS[M.clsName] = M;

function M:ctor()
   self.mCurIdx = 0;
   self.mPreIdx = 0;
end

function M:next()
    self.mPreIdx = self.mCurIdx;
    self.mCurIdx = self.mCurIdx + 1;
    return self.mPreIdx;
end

return M;