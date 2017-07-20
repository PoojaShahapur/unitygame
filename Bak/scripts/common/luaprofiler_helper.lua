local M = { }

enable_luatrace = 0
time_interval =(60 * 1000)
file_name = "lua.prof"
inited1 = false
inited2 = false

local ProFi = nil
local timer_queue = nil
if enable_luatrace ~= 0 and inited1 == false then
    inited1 = true
    ProFi = require('ProFi'):new()
    ProFi:start()
    timer_queue = c_timer_queue.CTimerQueue()
end

function M.init()
    if ProFi ~= nil and inited2 == false then
        inited2 = true
        timer_queue:insert_repeat_from_now(0, time_interval, function() ProFi:writeReport(file_name) end)
    end
end

return M
