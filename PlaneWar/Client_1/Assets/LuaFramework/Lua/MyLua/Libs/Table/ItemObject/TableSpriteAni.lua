--[[
    @brief 精灵动画配置
]]

MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");

local M = GlobalNS.Class(GlobalNS.TableItemBodyBase);
M.clsName = "TableSpriteAniItemBody";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.mFrameRate = 0;     -- 帧率 FPS，每秒帧数
    self.mFrameCount = 0;    -- 帧数，总共多少帧
    self.mAniResNameNoExt = ""; -- 动画资源的名字，没有扩展名
    
    self.mInvFrameRate = 0;    -- 一帧需要的时间
    self.mAniResName = "";     -- 动画资源的名字，有扩展名
    self.mAniPrefabName = "";  -- 动画预制资源
end

function M:parseBodyByteBuffer(bytes, offset)
    bytes.position = offset;
    _, self.mFrameRate = bytes:readInt32(self.mFrameRate);
    _, self.mFrameCount = bytes:readInt32(self.mFrameCount);
    self.mAniResNameNoExt = GlobalNS.UtilTable.readString(bytes, self.mAniResNameNoExt);

    self.mInvFrameRate = 1 / self.mFrameRate;
    self.mAniResName = string.format("%s.asset", self.mAniResNameNoExt);

    self.mAniPrefabName = string.format("%sprefab.prefab", self.mAniResNameNoExt);
end

return M;