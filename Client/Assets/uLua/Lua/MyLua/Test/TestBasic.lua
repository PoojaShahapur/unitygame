--[[
for i = start, limit, step do
    -- do stuff here
end
其中, start是起始值, limit是结束值, step是步进(可省, 默认是1).
i是for循环的local变量, for循环之后i不存在.
]]

for i=1,10 do
    print(i)
end

max=i
print(max)
--结果为：1 2 3 4 5 6 7 8 9 10 nil

--table 的使用方式

s = "ok"
mytable = {
    k = "aa",
    i = 12
}

print(mytable[k]) --输出nil
print(mytable["k"]) --输出 aa ,因为键值的索引是个字符串,所以必须有双引号

print(mytable.i) -- 输出12 在Lua中 mytable.k 等价于 mytable["i"] 


mytable[s] = 10

print(mytable[s]) --输出 10

--总结, 在大括号中 是键值对, 而给s赋值,s相当于一个索引,我们这里相当于给索引赋值.这里要特别注意区别
--注意容易混淆的 table.x 和 table[x]
--[[
table.x 等价于 table["x"]
table[x] 以变量x的值来索引table
]]

-- table的遍历
-- 第一种遍历方式for循环
mytable = {
    1,
    2,
    3,
    k = 12,
    4,
    8,
}

--第一种遍历方式
for i = 1, #mytable do
    print(mytable[i])
end

-- 这里输出 1,2,3,4,8  注意,这里并没有输出k,因为当for循环i+1的时候 第四个索引并不是连续的数字引,虽然mytable中的索引元素有6个,但是索引只到5 ,所以索引是4的值是4,索引是5的值是8

--第2中遍历方式 for ... ipairs...
--ipairs 迭代器使用的方式跟我们第一种普通for的方式获取的值是一样的,都是按照当前的隐式的缩印来去迭代并显示值
for i,v in ipairs(mytable) do
    print(i,v)
end
-- 输出:
-- 1 1
-- 2 2
-- 3 3
-- 4 4
-- 5 8

--第3种遍历方式 for ... pairs....
mytable = {
    k = 12,
    4,
    config = {},
    8,
}
--paris 迭代器是完全将所有的值显示出来,并且table中的索引并不完全按照书写顺序来的

for k,v in pairs(mytable) do
    print(k,v)
end

--输出
--1 4
--2 8
--k 12
--config table: 007CC738

-- lua中table如何安全移除元素

local test = { 2, 3, 4, 8, 9, 100, 20, 13, 15, 7, 11}
for i, v in ipairs( test ) do
    if v % 2 == 0 then
        table.remove(test, i)
    end
end

for i, v in ipairs( test ) do
    print(i .. "====" .. v)
end

--[[
打印结果：

 1====3
2====8
3====9
4====20
5====13
6====15
7====7
8====11
[Finished in 0.0s]
有问题吧，20怎么还在？这就是在遍历中删除导致的。
]]

local test = { 'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p' }
local remove = { a = true, b = true, c = true, e = true, f = true, p = true }

local function dump(table)
    for k, v in pairs( table ) do
        print(k)
        print(v)
        print("*********")
    end
end

-- 方法1 从后往前删除

 for i = #test, 1, -1 do
    if remove[test[i]] then
        table.remove(test, i)
    end
end

dump(test)

--方法2 while删除

 local i = 1
while i <= #test do
    if remove[test[i]] then
        table.remove(test, i)
    else
        i = i + 1
    end
end

dump(test)
--方法3 quick中提供的removeItem

 function table.removeItem(list, item, removeAll)
    local rmCount = 0
    for i = 1, #list do
        if list[i - rmCount] == item then
            table.remove(list, i - rmCount)
            if removeAll then
                rmCount = rmCount + 1
            else
                break
            end
        end
    end
end

for k, v in pairs( remove ) do
    table.removeItem(test, k)
end

dump(test)
