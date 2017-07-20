MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

MLoader("MyLua.UI.UIFindFriend.FindFriendNS");

--常量区
local M = GlobalNS.StaticClass();
local this = M;
M.clsName = "FindFriendPath";
GlobalNS.FindFriendNS[M.clsName] = M;

function M.init()

end

M.init();

return M;