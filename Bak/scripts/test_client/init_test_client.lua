--TestClient 的初始化脚本

local function add_package_path(sub_dir)
    package.path = G_LUA_ROOTPATH.."/"..sub_dir.."/?.lua;"..package.path
    package.path = G_LUA_ROOTPATH.."/"..sub_dir.."/?.luac;"..package.path
end

add_package_path("test_client")

require("plane.svc_plane_push").Init()
