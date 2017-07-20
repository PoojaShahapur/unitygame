MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

MLoader("MyLua.UI.UIPack.PackNS");

-- Path 区域
local M = GlobalNS.StaticClass();
M.clsName = "PackPath";
GlobalNS.PackNS[M.clsName] = M;

M.ModelPanel = "BG/MainPanel/ScrollViewModel"; 	-- 机型(皮肤)
M.BulletPanel = "BG/MainPanel/ScrollViewBullet";	-- 子弹
M.TrailPanel = "BG/MainPanel/ScrollViewTrail"; 	-- 轨迹
M.MoneyPanel = "BG/MainPanel/ScrollViewPiece"; 		-- 货币
M.GiftPanel = "BG/MainPanel/ScrollViewPackage"; 		-- 礼包
M.PiecePanel = "BG/MainPanel/ScrollViewFragment"; 		-- 碎片
M.DazzleLightPanel = "BG/MainPanel/ScrollViewDazzleLight"; 	-- 炫光

M.ModelPanelContent = "BG/MainPanel/ScrollViewModel/Viewport/Content"; 	-- 机型(皮肤)
M.BulletPanelContent = "BG/MainPanel/ScrollViewBullet/Viewport/Content";	-- 子弹
M.TrailPanelContent = "BG/MainPanel/ScrollViewTrail/Viewport/Content"; 	-- 轨迹
M.MoneyPanelContent = "BG/MainPanel/ScrollViewPiece/Viewport/Content"; 		-- 货币
M.GiftPanelContent = "BG/MainPanel/ScrollViewPackage/Viewport/Content"; 		-- 礼包
M.PiecePanelContent = "BG/MainPanel/ScrollViewFragment/Viewport/Content"; 		-- 碎片
M.DazzleLightPanelContent = "BG/MainPanel/ScrollViewDazzleLight/Viewport/Content"; 	-- 炫光

M.ModelBtn = "BG/MainPanel/Title/BtnModel"; 	-- 机型(皮肤)
M.BulletBtn = "BG/MainPanel/Title/BtnBullet";	-- 子弹
M.TrailBtn = "BG/MainPanel/Title/BtnTrail"; 	-- 轨迹
M.MoneyBtn = "BG/MainPanel/Title/BtnMoney"; 	-- 货币
M.GiftBtn = "BG/MainPanel/Title/BtnPackage"; 	-- 礼包
M.PieceBtn = "BG/MainPanel/Title/BtnFragment"; 	-- 碎片
M.DazzleLightBtn = "BG/MainPanel/Title/BtnDizzleLight"; 	-- 炫光

M.ModelRed = "BG/MainPanel/Title/BtnModel/biaoshi"; 	-- 机型(皮肤)
M.BulletRed = "BG/MainPanel/Title/BtnBullet/biaoshi";	-- 子弹
M.TrailRed = "BG/MainPanel/Title/BtnTrail/biaoshi"; 	-- 轨迹
M.MoneyRed = "BG/MainPanel/Title/BtnPiece/biaoshi"; 		-- 货币
M.GiftRed = "BG/MainPanel/Title/BtnPackage/biaoshi"; 		-- 礼包
M.PieceRed = "BG/MainPanel/Title/BtnFragment/biaoshi"; 		-- 碎片
M.DazzleLightRed = "BG/MainPanel/Title/BtnDizzleLight/biaoshi"; 	-- 炫光

M.ObjectItemTpl = "BG/MainPanel/ScrollViewModel/Viewport/Content/itempic_bagpack";     -- 模板项

M.CloseBtn = "BG/TitlePanel/EscImage";  -- 关闭按钮

M.GiftPrefabPath = "UI/UIPack/PackagetItem";
M.PiecePrefabPath = "UI/UIPack/FragmentItem";

return M;