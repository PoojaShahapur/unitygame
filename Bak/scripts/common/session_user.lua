local SessionUser = {}

function SessionUser:new(svrid)
    local u = {
        state = 1,
        gateway_id = svrid,
        teamid = 0,
        friendm = {},
    }

    return u
end

return SessionUser
