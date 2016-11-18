require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.StaticClass"

local M = GlobalNS.StaticClass();
local this = M;
M.clsName = "AuxUITypeId";
GlobalNS[M.clsName] = M;

function M.ctor()
    M.Button = 'Button';
    M.InputField = 'InputField';
    M.Label = 'Text';
end

M.ctor();

return M;