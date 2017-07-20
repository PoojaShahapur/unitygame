C++ 调lua脚本：
	luaL_dostring
		控制台输入
		Debug输入
	lua_pcall()
	EntityMethodCall() -> lua_pcall()
	CLuaCallback?

C++导出到lua的模块名带 c_ 前缀， 如 c_logger。

可搜索 LuaBinding, 如：
	LuaIntf::LuaBinding(L).beginModule("c_logger")
		.addFunction("debug", &debug)

按功能分目录，例如:
	cell/cats_and_dogs 猫狗大战
这样删除功能基本上删除这个目录即可。
package.path仅加入cell, 不加入cell/cats_and_dogs，
这样必须按子模块加载，如:
	require("cats_and_dogs.matcher")

服务带svc_前缀，如svc_login.lua, svc_cats_and_dogs.lua
