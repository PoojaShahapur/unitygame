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

function ByteBuffer:ctor()  -- 定义 ByteBuffer 的构造函数
    -- 一定要重新赋值不共享的数据成员，否则会直接从父类表中获取同名字的成员
    self.m_endian = self.ENDIAN_LITTLE -- 自己字节序
    self.m_buff = {}  -- 字节缓冲区
    self.m_position = 0   -- 缓冲区当前位置，注意 Lua 下标是从 1 开始的，不是从 0 开始的。 self.m_buff[0] == nil ，太坑了
	self.m_size = 0
end

function ByteBuffer:setEndian(endian)
    self.m_endian = endian
end

-- 读取一个字节
function ByteBuffer:readInt8()
    local elem = self.m_buff[self.m_position]
    local retData = elem
    self:advPos(1);
    return retData
end

-- 读取和写入的时候只看存储时候的字节序就行了，不用管系统字节序，因为是自组合成本地数据的
-- 读取两个字节
function ByteBuffer:readInt16()
    local retData = 0
	
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
    
    return retData
end

function ByteBuffer:readInt32()
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
    local retData = 0
	
	if self:canRead(4) then
		local low = 0
		local heigh = 0
		
		if self.m_endian == self.ENDIAN_BIG then
			heigh = self:readInt32(heigh)
			low = self:readInt32(low)
		else
			low = self:readInt32(low)
			heigh = self:readInt32(heigh)
		end
		
		retData = (heigh * 4294967296 + low) / 100
		
		self:advPos(8);
	
	end
    
    return retData
end

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
	self:log("writeInt8 " .. retData)
	
    self.m_buff[self.m_position] = retData
	
	self:log("self.m_buff[self.m_position] " .. self.m_buff[self.m_position])
	
    self:advPosAndLen(1);
	
	return retData
end

function ByteBuffer.writeInt16(retData)
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
    local oneByte = retData % 256
    local twoByte = retData / 256 % 256
    local threeByte = retData / (256 * 256) % 256
    local fourByte = retData / (256 * 256 * 256)

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
    local low = (retData * 100) % 4294967296
	local heigh = (retData * 100) / 4294967296
	
	if self.m_endian == self.ENDIAN_BIG then
		self:writeInt32(heigh)
		self:writeInt32(low)
	else
		self:writeInt32(low)
		self:writeInt32(heigh)
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
            oneByte = string.byte(subStr)
            self.m_buff[buffIdx] = oneByte
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
	self.m_size = self.m_size + num
end

-- 判断字节序和系统字节序是否相同
function ByteBuffer:isEqualEndian()
    return self.m_endian == self.m_sysEndian
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

-- 设置读写位置
function ByteBuffer:setPos(pos_)
	self.m_position = pos_
end

function ByteBuffer:setSize(size_)
	self.m_size = size_
end

-- 输出缓冲区所有的字节
function ByteBuffer:dumpAllBytes()
	self:log("dumpAllBytes " .. self:length())
    for idx = 0, #(self.m_buff) do
        self:log(tostring(self.m_buff[idx]))
    end
end

function ByteBuffer:log(msg)
    --SDK.Lib.TestStaticHandle.log(msg)
end

-- 测试通过 . 获取表中的函数
function ByteBuffer.tableFunc()
	
end