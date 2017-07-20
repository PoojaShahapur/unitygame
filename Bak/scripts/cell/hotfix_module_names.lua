--[[
热更新模块名列表。
例如：
hotfix_module_names = {
	"user",
	"cats_and_dogs.svc_cats_and_dogs",
}

common/hotfix_helper.lua中定时 check() 将重新加载本模块，获取热更新模块名列表。
可以任意添加或删除热更新模块名。
--]]

local hotfix_module_names = {
	"plane.player",
	"plane.room",
	"plane.bagm",
	"plane.shopm",
	"plane.skinm",
	"plane.room_mgr",
	"plane.svc_plane",
	"plane.svc_object",
	"plane.balancer",
	"plane.aiplayer",
	"plane.object",
	"plane.svc_object",
	"login.svc_login",
	"plane.svc_team",

	"remote_run.svc_run_lua",
	"remote_run.remote_runner",
	"svc_test_cmd",
	"test.redis_test",

	"user_mgr",
	"team_mgr",
	"session_user_state_mgr",
	"user",
}

return hotfix_module_names
