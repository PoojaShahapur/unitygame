MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");

local M = GlobalNS.StaticClass();
M.clsName = "TableId";
GlobalNS[M.clsName] = M;

M.TABLE_OBJECT = 0;           -- 道具基本表
M.TABLE_SKILL = 1;            -- 技能基本表 -- 添加一个表的步骤二
M.TABLE_STATE = 2;            -- 状态表
M.TABLE_SKIN = 3;             -- 皮肤表
M.TABLE_ATLAS_AND_IMAGE = 4;  -- SimpleAtlasAndImage表

M.eTableTotal = 5;             -- 表的总数

return M;