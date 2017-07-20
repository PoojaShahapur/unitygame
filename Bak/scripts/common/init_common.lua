local M = {}

-- return true if os is windows
local function get_is_windows()
    return "\\" == package.config:sub(1,1)
end

local is_windows = get_is_windows()

local function add_package_path(sub_dir)
    package.path = package.path..";"..G_LUA_ROOTPATH.."/"..sub_dir.."/?.lua"
    package.path = package.path..";"..G_LUA_ROOTPATH.."/"..sub_dir.."/?.luac"
end

local function add_package_cpath(sub_dir)
    if is_windows then
        package.cpath = package.cpath..";"..G_LUA_ROOTPATH.."/"..sub_dir.."/?.dll"
    else
        package.cpath = package.cpath..";"..G_LUA_ROOTPATH.."/"..sub_dir.."/?.so"
    end
end

local function add_paths_and_cpaths()
    add_package_path(".")
    add_package_path("common")
    add_package_path("lualibs")

    add_package_cpath("lualibs")
end

function M.init()
    add_paths_and_cpaths()

    -- Must after path setting.
    local log = require("log"):new("init_common")
    log:debug("G_LUA_ROOTPATH = " .. G_LUA_ROOTPATH)
    log:debug("package.path: " .. package.path)
    log:debug("package.cpath: " .. package.cpath)

    local pb = require("protobuf")
    pb.register_file(G_LUA_ROOTPATH.."/pb/descriptors_rpc.pb")
    pb.register_file(G_LUA_ROOTPATH.."/pb/descriptors_svr.pb")

    require("hotfix_helper").init()
    require("config"):loadconfig()
    local excelm = require("excelm")
    excelm:load_objectbm()
    excelm:load_skinbm()
    excelm:load_levelbm()
    require("init_common_services")
    require("login_mgr").init()
	
	require("MyLua.Module.Entry.MainEntry");
end

M.init()
return M
