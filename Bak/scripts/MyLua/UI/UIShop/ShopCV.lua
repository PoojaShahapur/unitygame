MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

MLoader("MyLua.UI.UIShop.ShopNS");

--常量区
local M = GlobalNS.StaticClass();
local this = M;
M.clsName = "ShopPath";
GlobalNS.ShopNS[M.clsName] = M;

function M.init()
	M.CloseBtn = "BG/BottomBGImage/Back_BtnTouch";		-- 返回按钮
	M.PackBtn = "BG/Mybag_BTN"; 		-- 背包按钮
	M.PiFuBtn = "BG/Pifu_BTN";		-- 皮肤按钮
	
	M.ModelTopPanel = "BG/PiFuMiddlePanel";	-- 皮肤
	M.NiuDanTopPanel = "BG/NiuDanMiddlePanel";	-- 扭蛋
	M.HuiYuanTopPanel = "BG/HuiYuanMiddlePanel";	-- 会员
	
	M.ModelTopBtn = "BG/TitleBGImage/Text_pifu";	-- 皮肤
	M.NiuDanTopBtn = "BG/TitleBGImage/Text_niudan";	-- 扭蛋
	M.HuiYuanTopBtn = "BG/TitleBGImage/Text_huiyuan";	-- 会员	
	
	-- 皮肤页面
	M.ModelPanel = "BG/PiFuMiddlePanel/ScrollRectPiFu";
	M.BulletPanel = "BG/PiFuMiddlePanel/ScrollRectXuanGuang_0";
	M.GhostPanel = "BG/PiFuMiddlePanel/ScrollRectXuanGuang_1";
	M.LightPanel = "BG/PiFuMiddlePanel/ScrollRectXuanGuang_2";
	
	M.ModelPanelContent = "BG/PiFuMiddlePanel/ScrollRectPiFu/Viewport/Content";
	M.BulletPanelContent = "BG/PiFuMiddlePanel/ScrollRectXuanGuang_0/Viewport/Content";
	M.GhostPanelContent = "BG/PiFuMiddlePanel/ScrollRectXuanGuang_1/Viewport/Content";
	M.LightPanelContent = "BG/PiFuMiddlePanel/ScrollRectXuanGuang_2/Viewport/Content";
	
	M.ModelBtn = "BG/PiFuMiddlePanel/TopPanel/Skin_BtnTouch";
	M.BulletBtn = "BG/PiFuMiddlePanel/TopPanel/Bullet_BtnTouch";
	M.GhostBtn = "BG/PiFuMiddlePanel/TopPanel/Ghost_BtnTouch";
	M.LightBtn = "BG/PiFuMiddlePanel/TopPanel/Light_BtnTouch";
	
	
	M.MoneyText = "BG/TitleBGImage/SuLiaoKuaiBG/Num";
	M.TicketText = "BG/TitleBGImage/YouXiQuanBG/Num";
	M.PlasticText = "BG/TitleBGImage/YouXiBiBG/Num";
	
	M.PackRed = "BG/Bag_BTN/yuandian";
	M.SkinRed = "BG/Pifu_BTN/yuandian";
	
	M.TopSkinRed = "BG/TitleBGImage/Text_pifu/yuandian";
	M.TopSkinUnderline = "BG/TitleBGImage/Text_pifu/Image";
	M.TopNiuDanRed = "BG/TitleBGImage/Text_niudan/yuandian";
	M.TopNiuDanUnderline = "BG/TitleBGImage/Text_niudan/Image";
	M.TopHuiYuanRed = "BG/TitleBGImage/Text_huiyuan/yuandian";
	M.TopHuiYuanUnderline = "BG/TitleBGImage/Text_huiyuan/Image";
	
	M.MoneyBtn = "BG/TitleBGImage/SuLiaoKuaiBG/YouXiQuan_BtnTouch"; 	--游戏币
	M.TicketBtn = "BG/TitleBGImage/YouXiQuanBG/YouXiQuan_BtnTouch";	--糖果
	M.PlasticBtn = "BG/TitleBGImage/YouXiBiBG/YouXiBi_BtnTouch";	--人民币
    M.GetTicketBtn = "BG/TitleBGImage/YouXiQuanBG/zengYXQ_BtnTouch";	--获取糖果
    M.GetPlasticBtn = "BG/TitleBGImage/YouXiBiBG/zengYXB_BtnTouch";	--获取人民币
end

M.init();

M = GlobalNS.StaticClass();
M.clsName = "ShopPanelTag";
GlobalNS.ShopNS[M.clsName] = M;

function M.init()
	M.eTopModel  	= 0;
	M.eTopNiuDan 	= 1;
	M.eTopHuiYuan 	= 2;
	M.eTopCount 	= 3;
	
	-- 皮肤
	M.eModel 		= 0;
	M.eBullet 		= 1;
	M.eGhost 		= 2;
	M.eLight 		= 3;
	M.eModelCount 	= 4;
end

M.init();

return M;