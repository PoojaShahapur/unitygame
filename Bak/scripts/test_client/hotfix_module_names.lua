--[[
热更新模块名列表。
例如：
hotfix_module_names = {
	"svc_login",
	"cats_and_dogs.svc_cats_and_dogs",
}

common/hotfix_helper.lua中定时 check() 将重新加载本模块，获取热更新模块名列表。
可以任意添加或删除热更新模块名。
--]]

local hotfix_module_names = {
	"test2",
	"plane.test_plane",
	"plane.svc_plane_push",
}

return hotfix_module_names
