local cosTable = {}
for i = 0,359 do
    cosTable[i] = math.cos(math.rad(i))
    print('{' .. i .. ',' .. math.cos(math.rad(i)) .. '},')
end

return cosTable
