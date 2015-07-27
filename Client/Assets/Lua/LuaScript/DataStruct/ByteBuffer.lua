--[[字节缓冲区]]
require('LuaScript/DataStruct/Class')

--ByteBuffer = {}
ByteBuffer = class()    -- 定义一个类，必须从返回的类中添加成员

-- 只读属性，所有的类共享一份
ByteBuffer.ENDIAN_LITTLE = 0    -- 小端字节序是 0
ByteBuffer.ENDIAN_BIG = 1       -- 大端字节序是 0
-- ByteBuffer.m_endian = ByteBuffer.ENDIAN_LITTLE -- 自己字节序
ByteBuffer.m_sysEndian = ByteBuffer.ENDIAN_LITTLE -- 系统字节序

-- ByteBuffer.m_buff = {}  -- 字节缓冲区
-- ByteBuffer.m_position = 1   -- 缓冲区当前位置，注意 Lua 下表是从 1 开始的，不是从 0 开始的。 self.m_buff[0] == nil ，太坑了

-- local self = ByteBuffer   -- 局部引用

function ByteBuffer:ctor()  -- 定义 ByteBuffer 的构造函数
    -- 一定要重新赋值不共享的数据成员，否则会直接从父类表中获取同名字的成员
    self.m_endian = ByteBuffer.ENDIAN_LITTLE -- 自己字节序
    self.m_buff = {}  -- 字节缓冲区
    self.m_position = 0   -- 缓冲区当前位置，注意 Lua 下标是从 1 开始的，不是从 0 开始的。 self.m_buff[0] == nil ，太坑了，现在使用 Table ，因此不存在这个问题了
end

function ByteBuffer:setEndian(endian)
    self.m_endian = endian
end

-- 读取一个字节
function ByteBuffer:readInt8()
    local elem = self.m_buff[self.m_position]
    local retData = string.byte(elem)
    self:advPos(1);
    return retData
end

-- 读取和写入的时候只看存储时候的字节序就行了，不用管系统字节序，因为是自组合成本地数据的
-- 读取两个字节
function ByteBuffer:readInt16()
    local retData = 0
    if self:canRead(2) then
        if self.m_endian == ByteBuffer.ENDIAN_LITTLE then-- 如果是小端字节序
            retData = string.byte(self.m_buff[self.m_position]) * 256 + string.byte(self.m_buff[self.m_position + 1])
        else
            retData = string.byte(self.m_buff[self.m_position + 1]) * 256 + string.byte(self.m_buff[self.m_position])
        end
        self:advPos(2);
    end
    
    return retData
end

function ByteBuffer:readInt32()
    local retData = 0
    if self:canRead(4) then
        if self.m_endian == ByteBuffer.ENDIAN_LITTLE then-- 如果是小端字节序
            retData = string.byte(self.m_buff[self.m_position]) * 256 * 256 * 256 + string.byte(self.m_buff[self.m_position + 1]) * 256 * 256 + string.byte(self.m_buff[self.m_position + 2]) * 256 + string.byte(self.m_buff[self.m_position + 3])
        else
            retData = string.byte(self.m_buff[self.m_position + 3]) * 256 * 256 * 256 + string.byte(self.m_buff[self.m_position + 2]) * 256 * 256 + string.byte(self.m_buff[self.m_position + 1]) * 256 + string.byte(self.m_buff[self.m_position])
        end
        self:advPos(4);
    end
    
    return retData
end

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

-- 读取 utf-8 字符串
function ByteBuffer:readMultiByte(len)
    if self:canRead(len) then
        local utf8Str
        idx = 1
        while(idx <= len)
        do
            if utf8Str == nil then
                utf8Str = string.char(self.m_buff[self.m_position + idx - 1])
            else
                utf8Str = utf8Str .. string.char(self.m_buff[self.m_position + idx - 1])
            end 
        end
        
        self:advPos(len);
    end
    
    return utf8Str
end

function ByteBuffer:writeInt8(retData)
	local aaa = retData + 0
	-- self:log("writeInt8" .. retData)
	self:log("writeInt8" .. aaa)
    self.m_buff[self.m_position] = retData
    self:advPosAndLen(1);
	
	return retData
end

function ByteBuffer.writeInt16(retData)
    local oneByte = retData % 256
    local twoByte = retData / 256

    if self.m_endian == ByteBuffer.ENDIAN_LITTLE then-- 如果是小端字节序
        self.m_buff[self.m_position] = twoByte
        self.m_buff[self.m_position + 1] = oneByte
    else
        self.m_buff[self.m_position] = oneByte
        self.m_buff[self.m_position + 1] = twoByte
    end
    
    self:advPosAndLen(2);
end

function ByteBuffer:writeInt32(retData)
    local oneByte = retData % 256
    local twoByte = retData / 256 % 256
    local threeByte = retData / (256 * 256) % 256
    local fourByte = retData / (256 * 256 * 256)

    if self.m_endian == ByteBuffer.ENDIAN_LITTLE then-- 如果是小端字节序
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

function ByteBuffer:writeNumber(retData)
    str = tostrng(retData)
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

-- 写 utf-8 字节字符串，必须是 utf-8 的
function ByteBuffer:writeMultiByte(value)
    if value ~= nil then
        idx = 1
        while(idx <= string.len(value))
        do
            buffIdx = self.m_position + idx - 1
            subStr = string.sub(value, idx, idx)
            byte = string.byte(subStr)
            self.m_buff[buffIdx] = byte
            idx = idx + 1
        end
    end
    
    self:advPosAndLen(string.len(value));
end

-- 是否有足够的字节可以读取
function ByteBuffer:canRead(len)
    if self.m_position + len > self:length() then
        return false
    end
    
    return true
end

function ByteBuffer:advPos(num)
    self.m_position = self.m_position + num;
end

function ByteBuffer:advPosAndLen(num)
    self.m_position = self.m_position + num;
end

-- 判断字节序和系统字节序是否相同
function ByteBuffer:isEqualEndian()
    return self.m_endian == ByteBuffer.m_sysEndian
end

-- 获取长度
function ByteBuffer:length()
    if self == nil then
        self:log("self nil")
    end
    if self.m_buff == nil then
        self:log("buff nil")
    end
    self:log("buff len" .. #self.m_buff)
    return #self.m_buff
end

-- 清理数据
function ByteBuffer:clear()
	self:log("clear ByteBuffer")
    self.m_buff = {}
    self.m_position = 0
end

-- 输出缓冲区所有的字节
function ByteBuffer:dumpAllBytes()
	self:log("dumpAllBytes" .. self:length())
    for idx = 0, #(self.m_buff) do
        self:log(tostring(self.m_buff[idx]))
    end
end

function ByteBuffer:log(msg)
    SDK.Lib.TestStaticHandle.log(msg)
end

function ByteBuffer.tableFunc()

end