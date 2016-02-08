package.path = string.format("%s;%s/?.lua", package.path, "F:/File/opensource/unity-game-git/unitygame/unitygame/Client/Assets/uLua/Lua")
--package.path = string.format("%s;%s/?.lua", package.path, "E:/Self/Self/unity/unitygame/Client/Assets/uLua/Lua")

require "MyLua.Libs.Core.Prequisites"
require "MyLua.Test.TestCommon.TestMisc"
require "MyLua.Test.TestCommon.TestLinkList"
require "MyLua.Test.TestCommon.TestCoroutine"
require "MyLua.Test.TestCommon.TestTable"

local m_testMisc = GlobalNS.new(GlobalNS.TestMisc);
local m_testLinkList = GlobalNS.new(GlobalNS.TestLinkList);
local m_testCoroutine = GlobalNS.new(GlobalNS.TestCoroutine);
local m_testTable = GlobalNS.new(GlobalNS.TestTable);

local function main()
    --m_testMisc:run();
    --m_testLinkList:run();
    -- m_testCoroutine:run();
    --m_testTable:run();
end

main();