local sinTable = {}
for i = 0,359 do
    sinTable[i] = math.sin(math.rad(i))
    print('{' .. i .. ',' .. math.sin(math.rad(i)) .. '},')
end

return sinTable
