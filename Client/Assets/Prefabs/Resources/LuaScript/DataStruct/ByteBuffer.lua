--[[字节缓冲区]]
require('LuaScript/DataStruct/Class')

--ByteBuffer = {}
ByteBuffer = class()    -- 定义一个类，必须从返回的类中添加成员

-- 只读属性，所有的类共享一份，所有这里定义的属性都放在类的 vtbl 表中，不是放在类自己表中
ByteBuffer.ENDIAN_LITTLE = 0    -- 小端字节序是 0
ByteBuffer.ENDIAN_BIG = 1       -- 大端字节序是 0
-- ByteBuffer.m_endian = ByteBuffer.ENDIAN_LITTLE -- 自己字节序
ByteBuffer.m_sysEndian = ByteBuffer.ENDIAN_LITTLE -- 系统字节序

-- ByteBuffer.m_buff = {}  -- 字节缓冲区
-- ByteBuffer.m_position = 1   -- 缓冲区当前位置，注意 Lua 下表是从 1 开始的，不是从 0 开始的。 self.m_buff[0] == nil ，太坑了

-- local self = ByteBuffer   -- 局部引用

-- ByteBuffer 表中的所有的成员都是添加到 vtbl 这个表中的，如果从外部通过压栈的方式访问，这个时候是不能访问到函数的，因为根本不在 ByteBuffer 这个表，而在 vtbl 这个表 
-- 设置系统字节序
function ByteBuffer:setSysEndian(endian_)
    self.m_sysEndian = endian_
end

function ByteBuffer:ctor()  -- 定义 ByteBuffer 的构造函数
    -- 一定要重新赋值不共享的数据成员，否则会直接从父类表中获取同名字的成员
    self.m_endian = self.ENDIAN_LITTLE -- 自己字节序
    self.m_buff = {}  -- 字节缓冲区
    self.m_position = 0   -- 缓冲区当前位置，注意 Lua 下标是从 1 开始的，不是从 0 开始的。 self.m_buff[0] == nil ，太坑了
    self.m_size = 0
end

-- 是否有足够的字节可以读取
function ByteBuffer:canRead(len)
    if self.m_position + len > self:length() then
        return false
    end
    
    return true
end

-- 设置读写位置
function ByteBuffer:setPos(pos_)
	  self.m_position = pos_
end

function ByteBuffer:setSize(size_)
    self.m_size = size_
end

function ByteBuffer:setEndian(endian)
    self.m_endian = endian
end

function ByteBuffer:advPos(num)
    self.m_position = self.m_position + num;
end

function ByteBuffer:advPosAndLen(num)
    self.m_position = self.m_position + num;
	self.m_size = self.m_size + num
end

-- 获取长度
function ByteBuffer:length()
    if self == nil then
        self:log("self nil")
    end
    if self.m_buff == nil then
        self:log("buff nil")
    end
    self:log("buff len " .. #self.m_buff)
	  self:log("buff len size " .. self.m_size)
    --return #self.m_buff + 1 	-- 这个返回的从 0 开始的索引，需要加 1 才行
	  return self.m_size
end

-- 清理数据
function ByteBuffer:clear()
	self:log("clear ByteBuffer")
    self.m_buff = {}
    self.m_position = 0
end

-- 判断字节序和系统字节序是否相同
function ByteBuffer:isEqualEndian()
    return self.m_endian == self.m_sysEndian
end

--[[
(retData > 2^(bitsLen-1) -1) and (retData - 2^bitsLen) or retData 解释
(retData > 2^(bitsLen-1) -1) 判断最高位是不是 1 ，如果是 1 ，就是负数
(retData - 2^bitsLen) 负数的补码，就是原码除符号位外按位取反 + 1 注意是除符号位
or retData 就是保证如果 (retData > 2^(bitsLen-1) -1) 判断后是整数，就返回 or retData 中的 retData

]]
-- 读取一个字节
function ByteBuffer:readInt8()
    local retData = self:readUnsignedInt8()
	local bitsLen = 8
	retData = (retData > 2^(bitsLen-1) -1) and (retData - 2^bitsLen) or retData
    return retData
end

function ByteBuffer:readUnsignedInt8()
    local elem = self.m_buff[self.m_position]
    local retData = elem
    self:advPos(1);
    return retData
end

-- 读取和写入的时候只看存储时候的字节序就行了，不用管系统字节序，因为是自组合成本地数据的
-- 读取两个字节
function ByteBuffer:readInt16()
    local retData = self:readUnsignedInt16()
    local bitsLen = 16
    retData = (retData > 2^(bitsLen-1) -1) and (retData - 2^bitsLen) or retData
    return retData
end

function ByteBuffer:readUnsignedInt16()
    local retData = 0
    local bitsLen = 16
	
    self:log("self.m_endian " .. self.m_endian)
    self:log("self.ENDIAN_BIG " .. self.ENDIAN_BIG)
    self:log("self.m_position " .. self.m_position)
	
    self:log("string.byte(self.m_buff[self.m_position]) " .. self.m_buff[self.m_position])
    self:log("string.byte(self.m_buff[self.m_position + 1]) " .. self.m_buff[self.m_position + 1])
	
    if self:canRead(2) then
        if self.m_endian == self.ENDIAN_BIG then-- 如果是小端字节序
            retData = self.m_buff[self.m_position] * 256 + self.m_buff[self.m_position + 1]
        else
            retData = self.m_buff[self.m_position + 1] * 256 + self.m_buff[self.m_position]
        end
        self:advPos(2);
    end
    
    retData = (retData > 2^(bitsLen-1) -1) and (retData - 2^bitsLen) or retData
    
    return retData
end

function ByteBuffer:readInt32()
    local retData = self:readUnsignedInt32()
	local bitsLen = 32
    retData = (retData > 2^(bitsLen-1) -1) and (retData - 2^bitsLen) or retData
    return retData
end

function ByteBuffer:readUnsignedInt32()
    local retData = 0
    if self:canRead(4) then
		self:log("2222 ")
        if self.m_endian == self.ENDIAN_BIG then-- 如果是小端字节序
            retData = self.m_buff[self.m_position] * 256 * 256 * 256 + self.m_buff[self.m_position + 1] * 256 * 256 + self.m_buff[self.m_position + 2] * 256 + self.m_buff[self.m_position + 3]
			self:log("3333 ")
        else
            retData = self.m_buff[self.m_position + 3] * 256 * 256 * 256 + self.m_buff[self.m_position + 2] * 256 * 256 + self.m_buff[self.m_position + 1] * 256 + self.m_buff[self.m_position]
			self:log("4444 ")
        end
        self:advPos(4);
    end
    
    return retData
end

function ByteBuffer:readDouble()
    local retData = self:readUnsignedDouble()
	local bitsLen = 64
    retData = (retData > 2^(bitsLen-1) -1) and (retData - 2^bitsLen) or retData
    return retData
end

function ByteBuffer:readUnsignedDouble()
    local retData = 0
	
    if self:canRead(8) then
		local low = 0
		local heigh = 0
	
		if self.m_endian == self.ENDIAN_BIG then
			heigh = self:readInt32()
			low = self:readInt32()
		else
			low = self:readInt32()
			heigh = self:readInt32()
		end
		
		retData = (heigh * 4294967296 + low) / 100
		
		self:advPos(8);
	
	end
    
    return retData
end

--[[
-- 读取双精度浮点数
function ByteBuffer:readNumber()
    local retData = 0
    if self:canRead(8) then
        if self.m_endian == ByteBuffer.ENDIAN_LITTLE then-- 如果是小端字节序
            local str = self.m_buff[self.m_position] .. self.m_buff[self.m_position + 1] .. self.m_buff[self.m_position + 2] .. self.m_buff[self.m_position + 3] .. self.m_buff[self.m_position + 4] .. self.m_buff[self.m_position + 5] .. self.m_buff[self.m_position + 6] .. self.m_buff[self.m_position + 7]
        else
            local str = self.m_buff[self.m_position + 7] .. self.m_buff[self.m_position + 6] .. self.m_buff[self.m_position + 5] .. self.m_buff[self.m_position + 4] .. self.m_buff[self.m_position + 3] .. self.m_buff[self.m_position + 2] .. self.m_buff[self.m_position + 1] .. self.m_buff[self.m_position]
        end

        retData = tonumber(str)        
        self:advPos(8);
    end
    
    return retData
end
]]

-- 读取 utf-8 字符串
function ByteBuffer:readMultiByte(len_)
    self:log("len_ " .. len_)
    self:log("m_position " .. self.m_position)
    self:log("m_size " .. self.m_size)

    local utf8Str
    if self:canRead(len_) then
        idx = 0
		
		self:log("aaaaaaaaa")
		
        while(idx < len_)
        do
            if utf8Str == nil then
                utf8Str = string.char(self.m_buff[self.m_position + idx])
				self:log("bbbbbbbbbbbb")
            else
                utf8Str = utf8Str .. string.char(self.m_buff[self.m_position + idx])
				self:log("fffffffffff")
            end
            
            idx = idx + 1
        end
        
        self:advPos(len_);
    end
    
    return utf8Str
end

function ByteBuffer:writeInt8(retData)
    self:writeUnsignedInt8(retData)
end

function ByteBuffer:writeUnsignedInt8(retData)
    self:log("writeInt8 " .. retData)
	
    self.m_buff[self.m_position] = retData
	
    self:log("self.m_buff[self.m_position] " .. self.m_buff[self.m_position])
	
    self:advPosAndLen(1);
end

function ByteBuffer.writeInt16(retData)
	self:writeUnsignedInt16(retData)
end

function ByteBuffer.writeUnsignedInt16(retData)
	self:log("writeInt16 " .. retData)
    local oneByte = retData % 256
    local twoByte = retData / 256

    if self.m_endian == self.ENDIAN_BIG then-- 如果是小端字节序
        self.m_buff[self.m_position] = twoByte
        self.m_buff[self.m_position + 1] = oneByte
    else
        self.m_buff[self.m_position] = oneByte
        self.m_buff[self.m_position + 1] = twoByte
    end
    
    self:advPosAndLen(2);
end

function ByteBuffer:writeInt32(retData)
    self:writeUnsignedInt32(retData)
end

function ByteBuffer:writeUnsignedInt32(retData)
    local oneByte = retData % 256
    local twoByte = math.floor((retData / 256) % 256)
    local threeByte = math.floor((retData / (256 * 256)) % 256)
    local fourByte = math.floor(retData / (256 * 256 * 256))

    if self.m_endian == self.ENDIAN_BIG then-- 如果是小端字节序
        self.m_buff[self.m_position] = fourByte
        self.m_buff[self.m_position + 1] = threeByte
        self.m_buff[self.m_position + 2] = twoByte
        self.m_buff[self.m_position + 3] = oneByte
    else
        self.m_buff[self.m_position] = oneByte
        self.m_buff[self.m_position + 1] = twoByte
        self.m_buff[self.m_position + 2] = threeByte
        self.m_buff[self.m_position + 3] = fourByte
    end
    
    self:advPosAndLen(4);
end

-- 保存双精度浮点数，精度两位小数
function ByteBuffer:writeDouble(retData)
    self:writeUnsignedDouble(retData)
end

function ByteBuffer:writeUnsignedDouble(retData)
    local low = (retData * 100) % 4294967296
    local heigh = math.floor((retData * 100) / 4294967296)
	
	  if self.m_endian == self.ENDIAN_BIG then
		    self:writeInt32(heigh)
		    self:writeInt32(low)
	  else
		    self:writeInt32(low)
		    self:writeInt32(heigh)
	  end
    
    self:advPosAndLen(8);
end

--[[
-- 保存双精度浮点数
function ByteBuffer:writeNumber(retData)
    str = tostring(retData)
    len = string.len(str)
    idx = 1
    if self.m_endian == ByteBuffer.ENDIAN_LITTLE then-- 如果是小端字节序
        while( idx <= 8 )
        do
            self.m_buff[self.m_position + idx - 1] = string.byte(str, idx)
            idx = idx + 1
        end
        
        while( idx <= 8 )
        do
            self.m_buff[self.m_position + idx - 1] = 0
            idx = idx + 1
        end
    else
        while( idx < 8 )
        do
            self.m_buff[self.m_position + idx - 1] = string.byte(str, len - idx)
            idx = idx + 1
        end
        
        while( idx < 8 )
        do
            self.m_buff[self.m_position + idx - 1] = 0
            idx = idx + 1
        end
    end
    
    self:advPosAndLen(8);
end
]]

-- 写 utf-8 字节字符串，必须是 utf-8 的
function ByteBuffer:writeMultiByte(value)
    if value ~= nil then
        idx = 1
        while(idx <= string.len(value))
        do
            buffIdx = self.m_position + idx - 1
            subStr = string.sub(value, idx, idx)
            oneByte = string.byte(subStr)
            self.m_buff[buffIdx] = oneByte
            idx = idx + 1
        end
    end
    
    self:advPosAndLen(string.len(value));
end

-- 输出缓冲区所有的字节
function ByteBuffer:dumpAllBytes()
    self:log("dumpAllBytes " .. self:length())
    for idx = 0, #(self.m_buff) do
        self:log(tostring(self.m_buff[idx]))
    end
    
    self:log("self.m_sysEndian " .. self.m_sysEndian)
end

function ByteBuffer:log(msg)
    --SDK.Lib.TestStaticHandle.log(msg)
end

-- 测试通过 . 获取表中的函数
function ByteBuffer.tableFunc()
	
end

-- 类似实现如下功能
--[[
--------------------------------------------------------------------------------------------
require 'struct'
-- convert character codes to a Lua string - this may come from your source
local str = string.char(0x00, 0x1d, 0xff, 0x23, 0x44, 0x32)
-- format string: < = little endian, In = unsigned int (n bytes)
local u16, u32 = struct.unpack('<I2I4', str)
print(u16, u32) --> 7424    843326463
---------------------------------------------------------------------------------------------
]]
function ByteBuffer:bytes_to_int(str,endian,signed) -- use length of string to determine 8,16,32,64 bits
    local t={str:byte(1,-1)}
    if endian=="big" then --reverse bytes
        local tt={}
        for k=1,#t do
            tt[#t-k+1]=t[k]
        end
        t=tt
    end
    local n=0
    for k=1,#t do
        n=n+t[k]*2^((k-1)*8)
    end
    if signed then
        n = (n > 2^(#t-1) -1) and (n - 2^#t) or n -- if last bit set, negative. (n > 2^(#t-1) -1) 说明有符号位是 1 ，(n - 2^#t) 求取补码
    end
    return n
end

function ByteBuffer:int_to_bytes(num,endian,signed)
    if num<0 and not signed then num=-num print"warning, dropping sign from number converting to unsigned" end
    local res={}
    local n = math.ceil(select(2,math.frexp(num))/8) -- number of bytes to be used.
    if signed and num < 0 then
        num = num + 2^n
    end
    for k=n,1,-1 do -- 256 = 2^8 bits per char.
        local mul=2^(8*(k-1))
        res[k]=math.floor(num/mul)
        num=num-res[k]*mul
    end
    assert(num==0)
    if endian == "big" then
        local t={}
        for k=1,n do
            t[k]=res[n-k+1]
        end
        res=t
    end
    return string.char(unpack(res))
end

-- 字节转换成 Number
function ByteBuffer:bytes2Int(str, endian, signed) -- use length of string to determine 8,16,32,64 bits
    local t={str:byte(1,-1)}
    if endian == self.ENDIAN_BIG then --reverse bytes
        local tt = {}
        for k=1,#t do
            tt[#t-k+1] = t[k]
        end
        t=tt
    end
    local n=0
    for k=1,#t do
        n = n+t[k]*2^((k-1)*8)
    end
    if signed then
        n = (n > 2^(#t-1) -1) and (n - 2^#t) or n -- if last bit set, negative. (n > 2^(#t-1) -1) 说明有符号位是 1 ，(n - 2^#t) 求取补码
    end
    return n
end

-- Number 换换成 Bytes
function ByteBuffer:int2Bytes(num, endian, signed)
    if num < 0 and not signed then
		num=-num print"warning, dropping sign from number converting to unsigned"
	end
	
    local res = {}
    local n = math.ceil(select(2, math.frexp(num)) / 8) -- number of bytes to be used.
    if signed and num < 0 then
        num = num + 2^n
    end
    for k = n,1,-1 do -- 256 = 2^8 bits per char.
        local mul = 2^(8*(k-1))
        res[k] = math.floor(num/mul)
        num = num-res[k]*mul
    end
    assert(num == 0)
    if endian == self.ENDIAN_BIG then
        local t = {}
        for k = 1,n do
            t[k] = res[n-k+1]
        end
        res = t
    end
    return string.char(unpack(res))
end