local Array = {}
local M = Array

function M:construct()
    self.m_data = {}
end

function M:add(value)
    len = #self.m_data
end

env = getfenv()

--env =  _ENV

return M