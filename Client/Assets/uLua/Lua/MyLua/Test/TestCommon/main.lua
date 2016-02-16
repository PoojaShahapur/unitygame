package.path = string.format("%s;%s/?.lua", package.path, "F:/File/opensource/unity-game-git/unitygame/unitygame/Client/Assets/uLua/Lua")
--package.path = string.format("%s;%s/?.lua", package.path, "E:/Self/Self/unity/unitygame/Client/Assets/uLua/Lua")

-- 核心导入
require "MyLua.Libs.Core.Prequisites"
require "MyLua.Libs.FrameWork.GCtx"

-- 测试代码导入
require "MyLua.Test.TestCommon.TestMisc"
require "MyLua.Test.TestCommon.TestLinkList"
require "MyLua.Test.TestCommon.TestCoroutine"
require "MyLua.Test.TestCommon.TestTable"
require "MyLua.Test.TestCommon.TestTimer"

require "MyLua.Test.TestCommon.TestString"
require "MyLua.Test.TestCommon.TestFunction"

local m_testMisc = GlobalNS.new(GlobalNS.TestMisc);
local m_testLinkList = GlobalNS.new(GlobalNS.TestLinkList);
local m_testCoroutine = GlobalNS.new(GlobalNS.TestCoroutine);
local m_testTable = GlobalNS.new(GlobalNS.TestTable);
local m_testTimer = GlobalNS.new(GlobalNS.TestTimer);

local m_testString = GlobalNS.new(GlobalNS.TestString);
local m_testFunction = GlobalNS.new(GlobalNS.TestFunction);

local function main()
    --m_testMisc:run();
    --m_testLinkList:run();
    --m_testCoroutine:run();
    --m_testTable:run();
    --m_testTimer:run();
    --m_testString:run();
    m_testFunction:run();
end

main();