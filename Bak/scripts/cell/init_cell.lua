--cellapp的初始化脚本
local function add_package_path(sub_dir)
    package.path = package.path..";"..G_LUA_ROOTPATH.."/"..sub_dir.."/?.lua"
    package.path = package.path..";"..G_LUA_ROOTPATH.."/"..sub_dir.."/?.luac"
end

add_package_path("cell")

-- After package.path.
--require("luaprofiler_helper").init()
require("init_cell_services")
require("event_handler.register").register_all()

require("plane.init").init()
--require("online_count").init()
