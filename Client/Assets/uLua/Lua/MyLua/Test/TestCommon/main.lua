--package.path = string.format("%s;%s/?.lua", package.path, "D:/File/OpenSource/Unity/Tools/LuaLibs/src")
package.path = string.format("%s;%s/?.lua", package.path, "E:/Self/Self/Tools/Tools/LuaLibs/src")

require "MyLua.Libs.Core.Prequisites"
require "MyLua.Libs.Test.TestCommon.TestMisc"

local m_testMisc = GlobalNS.new(GlobalNS.TestMisc);
local m_testLinkList = GlobalNS.new(GlobalNS.TestLinkList);

local function main()
    m_testMisc:run();
    m_testLinkList:run();
end

main();