MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

local M = GlobalNS.StaticClass();
M.clsName = "MoneyType";
GlobalNS[M.clsName] = M;

M.eMoney  = 0;
M.eTicket = 1;
M.ePlastic  = 2;

M.MoneySpriteName  = "suliaokuai";
M.TicketSpriteName = "youxiquan";
M.PlasticSpriteName  = "youxibi";

M.MoneyAtlasPath  = "Atlas/DefaultSkin/SkyWarSkin.asset";
M.TicketAtlasPath = "Atlas/DefaultSkin/SkyWarSkin.asset";
M.PlasticAtlasPath  = "Atlas/DefaultSkin/SkyWarSkin.asset";

return M;