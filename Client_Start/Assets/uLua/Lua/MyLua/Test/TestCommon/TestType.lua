require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "TestType";
GlobalNS[M.clsName] = M;

function M:run()
    self:test();
end

function M:testFunc()

end

function M.greet()
    print "hello world";
end

function M:test()
    print("number type = " .. type(2));
    print("string type = " .. type("asdf"));
    print("boolean type = " .. type(true));
    print("nil type = " .. type(nil));
    print("func type = " .. type(self.testFunc));
    print("talbe type = " .. type(self));
    local co = coroutine.create(self.greet);
    print("talbe type = " .. type(co));
end

return M;