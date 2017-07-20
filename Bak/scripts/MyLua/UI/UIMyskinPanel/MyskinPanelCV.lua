MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

MLoader("MyLua.UI.UIMyskinPanel.MyskinPanelNS");

--常量区
local M = GlobalNS.StaticClass();
local this = M;
M.clsName = "MyskinPanelPath";
GlobalNS.MyskinPanelNS[M.clsName] = M;

function M.init()
	M.ModelPanel = "BG/MainPanel/ScrollViewModel"; 	-- 机型(皮肤)
	M.BulletPanel = "BG/MainPanel/ScrollViewBullet";	-- 子弹
	M.TrailPanel = "BG/MainPanel/ScrollViewTrail"; 	-- 轨迹
	M.DazzleLightPanel = "BG/MainPanel/ScrollViewDazzleLight"; 	-- 炫光

	M.ModelPanelContent = "BG/MainPanel/ScrollViewModel/Viewport/Content"; 	-- 机型(皮肤)
	M.BulletPanelContent = "BG/MainPanel/ScrollViewBullet/Viewport/Content";	-- 子弹
	M.TrailPanelContent = "BG/MainPanel/ScrollViewTrail/Viewport/Content"; 	-- 轨迹
	M.DazzleLightPanelContent = "BG/MainPanel/ScrollViewDazzleLight/Viewport/Content"; 	-- 炫光

	M.ModelBtn = "BG/MainPanel/Title/BtnModel"; 	-- 机型(皮肤)
	M.BulletBtn = "BG/MainPanel/Title/BtnBullet";	-- 子弹
	M.TrailBtn = "BG/MainPanel/Title/BtnTrail"; 	-- 轨迹
	M.DazzleLightBtn = "BG/MainPanel/Title/BtnDizzleLight"; 	-- 炫光

	M.ModelRed = "BG/MainPanel/Title/BtnModel/biaoshi"; 	-- 机型(皮肤)
	M.BulletRed = "BG/MainPanel/Title/BtnBullet/biaoshi";	-- 子弹
	M.TrailRed = "BG/MainPanel/Title/BtnTrail/biaoshi"; 	-- 轨迹
	M.DazzleLightRed = "BG/MainPanel/Title/BtnDizzleLight/biaoshi"; 	-- 炫光

	M.ObjectItemTpl = "BG/MainPanel/ScrollViewModel/Viewport/Content/itempic_bagpack";     -- 模板项

	M.CloseBtn = "BG/TitlePanel/EscImage";  -- 关闭按钮
	M.ClosePanel = "";  -- 关闭面板
end

M.init();

return M;