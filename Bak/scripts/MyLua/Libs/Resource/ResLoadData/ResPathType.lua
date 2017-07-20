local M = GlobalNS.StaticClass();
M.clsName = "ResPathType";
GlobalNS[M.clsName] = M;

--[[
	@brief 资源路径类型
]]
M.ePathScene = 1;         -- 场景路径
M.ePathSceneXml = 2;      -- 场景路径
M.ePathModule = 3;        -- Module 路径
M.ePathComUI = 4;         -- UI 控件路径
M.ePathComUIScene = 5;    -- Scene UI 控件路径
M.ePathCodePath = 6;      -- UI 代码目录
M.ePathBeingPath = 7;     -- Being 目录
M.ePathAIPath = 8;        -- AI 目录
M.ePathTablePath = 9;     -- Table 目录
M.ePathLangXml = 10;       -- 语言配置文件
M.ePathXmlCfg = 11;        -- Xml 配置文件
M.ePathModel = 12;         -- 各种 Prefab
M.ePathMaterial = 13;      -- 材质目录
M.ePathBuildImage = 14;    -- 图片目录
M.ePathCardImage = 15;     -- 图片目录
M.ePathWord = 16;          -- 过滤字、随机名字
M.ePathAudio = 17;         -- 音频
M.ePathAtlas = 18;         -- 地图集
M.ePathSpriteAni = 19;     -- 精灵动画
M.ePathSceneAnimatorController = 20;   -- Animator Controller 控制器
M.ePathULua = 21;                      -- ULua 脚本目录
M.ePathLuaScript = 22;                 -- Lua 脚本
M.ePathSkillAction = 23;               -- 技能动作配置文件

M.eTotal = 24;          -- 总数

return M;