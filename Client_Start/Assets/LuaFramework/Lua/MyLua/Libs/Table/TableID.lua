MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");

local M = GlobalNS.StaticClass();
M.clsName = "TableID";
GlobalNS[M.clsName] = M;

M.TABLE_OBJECT = 0;           -- 道具基本表
M.TABLE_CARD = 1;             -- 卡牌基本表
M.TABLE_SKILL = 2;            -- 技能基本表 -- 添加一个表的步骤二
M.TABLE_JOB = 3;              -- 职业基本表
M.TABLE_SPRITEANI = 4;        -- 精灵动画基本表
M.TABLE_RACE = 5;             -- 种族表
M.TABLE_STATE = 6;            -- 状态表

M.eTableTotal = 7;             -- 表的总数

return M;