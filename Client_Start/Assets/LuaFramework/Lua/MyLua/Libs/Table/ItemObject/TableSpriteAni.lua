--[[
    @brief 精灵动画配置
]]

require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"

local M = GlobalNS.Class(GlobalNS.TableItemBodyBase);
M.clsName = "TableSpriteAniItemBody";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.m_frameRate = 0;     -- 帧率 FPS，每秒帧数
    self.m_frameCount = 0;    -- 帧数，总共多少帧
    self.m_aniResNameNoExt = ""; -- 动画资源的名字，没有扩展名
    
    self.m_invFrameRate = 0;    -- 一帧需要的时间
    self.m_aniResName = "";     -- 动画资源的名字，有扩展名
    self.m_aniPrefabName = "";  -- 动画预制资源
end

function M:parseBodyByteBuffer(bytes, offset)
    local UtilTable = nil;
    bytes.position = offset;
    bytes:readInt32(self.m_frameRate);
    bytes:readInt32(self.m_frameCount);
    UtilTable.readString(bytes, self.m_aniResNameNoExt);

    self.m_invFrameRate = 1 / self.m_frameRate;
    self.m_aniResName = string.format("%s.asset", self.m_aniResNameNoExt);

    self.m_aniPrefabName = string.format("%sprefab.prefab", self.m_aniResNameNoExt);
end

return M;