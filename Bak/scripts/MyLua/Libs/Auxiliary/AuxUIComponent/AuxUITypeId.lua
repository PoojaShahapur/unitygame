MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

local M = GlobalNS.StaticClass();
M.clsName = "AuxUITypeId";
GlobalNS[M.clsName] = M;

function M.ctor()
    M.Button = "Button";
    M.InputField = "InputField";
    M.Label = "Text";
	M.Image = "Image";
end

M.ctor();

return M;