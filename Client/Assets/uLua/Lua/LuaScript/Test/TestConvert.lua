-- 测试转换
local _num = 123.786
print(type(_num))
print(_num)
local _int = 2323
print(type(_int))
print(_int)
local _strNum = "5689.6587"
print(type(_strNum))
print(_strNum)
local _strInt = "9898"
print(type(_strInt))
print(_strInt)

local _convStrNum = tostring(_num)
print(type(_convStrNum))
print(_convStrNum)
local _convStrInt = tostring(_int)
print(type(_convStrInt))
print(_convStrInt)
local _convStrNum = tonumber(_strNum)
print(type(_convStrNum))
print(_convStrNum)
local _convIntStr = tonumber(_strInt)
print(type(_convIntStr))
print(_convIntStr)

local _end = 10