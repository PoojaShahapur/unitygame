local M = {}

-- 是否2个时间戳在同一天
-- 比如 23:59:55秒 和 00:00:00
-- 00:00:00 和 00:00:05
-- 23:59:57 和 00:00:03
function M.is_two_time_in_different_day(time1, time2)
    assert(time1 <= time2)
    -- 28800是东八区
    local module = (time1 + 28800) % 86400
    local val2 = module + (time2 - time1)
    if val2 >=  86400 then
        return true
    else
        return false
    end
end

-- 1496246400 是6月1号00:00:00; 1496246399 是5月31号 23:59:59
function M.is_two_time_in_different_month(time1, time2)
    local month1 = os.date("%m", time1)
    local month2 = os.date("%m", time2)
    return month1 ~= month2
end
return M
