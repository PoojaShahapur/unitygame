MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

--[[
    @brief 字节缓冲区
]]

local M = GlobalNS.Class(GlobalNS.GObject);    -- 定义一个类，必须从返回的类中添加成员
M.clsName = "MByteBuffer";
GlobalNS[M.clsName] = M;

-- 只读属性，所有的类共享一份，所有这里定义的属性都放在类的 vtbl 表中，不是放在类自己表中
M.ENDIAN_LITTLE = 0;    -- 小端字节序是 0
M.ENDIAN_BIG = 1;       -- 大端字节序是 0

M.m_sysEndian = M.ENDIAN_LITTLE; -- 系统字节序

function M:setSysEndian(endian_)
    self.m_sysEndian = endian_;
end

function M:ctor()  -- 定义 MByteBuffer 的构造函数
    -- 一定要重新赋值不共享的数据成员，否则会直接从父类表中获取同名字的成员
    self.mEndian = self.ENDIAN_LITTLE; -- 自己字节序
    self.mBuffer = {};  -- 字节缓冲区
    self.mPosition = 0;   -- 缓冲区当前位置，注意 Lua 下标是从 1 开始的，不是从 0 开始的。 self.mBuffer[0] == nil ，太坑了
    self.mSize = 0;
end

-- 是否有足够的字节可以读取
function M:canRead(len)
    if self.mPosition + len > self:length() then
        return false;
    end
    
    return true;
end

-- 设置读写位置
function M:setPos(pos_)
    self.mPosition = pos_;
end

function M:setSize(size_)
    self.mSize = size_;
end

function M:setEndian(endian)
    self.mEndian = endian;
end

function M:advPos(num)
    self.mPosition = self.mPosition + num;
end

function M:advPosAndLen(num)
    self.mPosition = self.mPosition + num;
	self.mSize = self.mSize + num;
end

-- 获取长度
function M:length()
    --return #self.mBuffer + 1 	-- 这个返回的从 0 开始的索引，需要加 1 才行
	return self.mSize;
end

-- 清理数据
function M:clear()
    self.mBuffer = {};
    self.mPosition = 0;
end

-- 判断字节序和系统字节序是否相同
function M:isEqualEndian()
    return self.mEndian == self.m_sysEndian;
end

--[[
(retData > 2^(bitsLen-1) -1) and (retData - 2^bitsLen) or retData 解释
(retData > 2^(bitsLen-1) -1) 判断最高位是不是 1 ，如果是 1 ，就是负数
(retData - 2^bitsLen) 负数的补码，就是原码除符号位外按位取反 + 1 注意是除符号位
or retData 就是保证如果 (retData > 2^(bitsLen-1) -1) 判断后是整数，就返回 or retData 中的 retData
]]
-- 读取一个字节
function M:readInt8()
    local retData = self:readUnsignedInt8();
	local bitsLen = 8;
	retData = (retData > 2^(bitsLen-1) -1) and (retData - 2^bitsLen) or retData;
    return retData;
end

function M:readUnsignedInt8()
    local elem = self.mBuffer[self.mPosition];
    local retData = elem;
    self:advPos(1);
    return retData;
end

-- 读取和写入的时候只看存储时候的字节序就行了，不用管系统字节序，因为是自组合成本地数据的
-- 读取两个字节
function M:readInt16()
    local retData = self:readUnsignedInt16();
    local bitsLen = 16;
    retData = (retData > 2^(bitsLen-1) -1) and (retData - 2^bitsLen) or retData;
    return retData;
end

function M:readUnsignedInt16()
    local retData = 0;
    local bitsLen = 16;

    if self:canRead(2) then
        if self.mEndian == self.ENDIAN_BIG then-- 如果是小端字节序
            retData = self.mBuffer[self.mPosition] * 256 + self.mBuffer[self.mPosition + 1];
        else
            retData = self.mBuffer[self.mPosition + 1] * 256 + self.mBuffer[self.mPosition];
        end
        self:advPos(2);
    end
    
    retData = (retData > 2^(bitsLen-1) -1) and (retData - 2^bitsLen) or retData;
    
    return retData;
end

function M:readInt32()
    local retData = self:readUnsignedInt32();
	local bitsLen = 32;
    retData = (retData > 2^(bitsLen-1) -1) and (retData - 2^bitsLen) or retData;
    return retData;
end

function M:readUnsignedInt32()
    local retData = 0;
    if self:canRead(4) then
        if self.mEndian == self.ENDIAN_BIG then    -- 如果是小端字节序
            retData = self.mBuffer[self.mPosition] * 256 * 256 * 256 + self.mBuffer[self.mPosition + 1] * 256 * 256 + self.mBuffer[self.mPosition + 2] * 256 + self.mBuffer[self.mPosition + 3];
        else
            retData = self.mBuffer[self.mPosition + 3] * 256 * 256 * 256 + self.mBuffer[self.mPosition + 2] * 256 * 256 + self.mBuffer[self.mPosition + 1] * 256 + self.mBuffer[self.mPosition];
        end
        self:advPos(4);
    end
    
    return retData;
end

function M:readDouble()
    local retData = self:readUnsignedDouble();
	local bitsLen = 64;
    retData = (retData > 2^(bitsLen-1) -1) and (retData - 2^bitsLen) or retData;
    return retData;
end

function M:readUnsignedDouble()
    local retData = 0;
	
    if self:canRead(8) then
		local low = 0;
		local heigh = 0;
	
		if self.mEndian == self.ENDIAN_BIG then
			heigh = self:readInt32();
			low = self:readInt32();
		else
			low = self:readInt32();
			heigh = self:readInt32();
		end
		
		retData = (heigh * 4294967296 + low) / 100;
		
		self:advPos(8);
	end
    
    return retData;
end

-- 读取 utf-8 字符串
function M:readMultiByte(len_)
    local utf8Str;
    if self:canRead(len_) then
        local idx = 0;
        while(idx < len_) do
            if utf8Str == nil then
                utf8Str = string.char(self.mBuffer[self.mPosition + idx]);
            else
                utf8Str = utf8Str .. string.char(self.mBuffer[self.mPosition + idx]);
            end
            
            idx = idx + 1;
        end
        
        self:advPos(len_);
    end
    
    return utf8Str;
end

function M:writeInt8(retData)
    self:writeUnsignedInt8(retData);
end

function M:writeUnsignedInt8(retData)
    self.mBuffer[self.mPosition] = retData;
    self:advPosAndLen(1);
end

function M.writeInt16(retData)
	self:writeUnsignedInt16(retData);
end

function M.writeUnsignedInt16(retData)
    local oneByte = retData % 256;
    local twoByte = retData / 256;

    if self.mEndian == self.ENDIAN_BIG then-- 如果是小端字节序
        self.mBuffer[self.mPosition] = twoByte;
        self.mBuffer[self.mPosition + 1] = oneByte;
    else
        self.mBuffer[self.mPosition] = oneByte;
        self.mBuffer[self.mPosition + 1] = twoByte;
    end
    
    self:advPosAndLen(2);
end

function M:writeInt32(retData)
    self:writeUnsignedInt32(retData);
end

function M:writeUnsignedInt32(retData)
    local oneByte = retData % 256;
    local twoByte = math.floor((retData / 256) % 256);
    local threeByte = math.floor((retData / (256 * 256)) % 256);
    local fourByte = math.floor(retData / (256 * 256 * 256));

    if self.mEndian == self.ENDIAN_BIG then-- 如果是小端字节序
        self.mBuffer[self.mPosition] = fourByte;
        self.mBuffer[self.mPosition + 1] = threeByte;
        self.mBuffer[self.mPosition + 2] = twoByte;
        self.mBuffer[self.mPosition + 3] = oneByte;
    else
        self.mBuffer[self.mPosition] = oneByte;
        self.mBuffer[self.mPosition + 1] = twoByte;
        self.mBuffer[self.mPosition + 2] = threeByte;
        self.mBuffer[self.mPosition + 3] = fourByte;
    end
    
    self:advPosAndLen(4);
end

-- 保存双精度浮点数，精度两位小数
function MByteBuffer:writeDouble(retData)
    self:writeUnsignedDouble(retData);
end

function MByteBuffer:writeUnsignedDouble(retData)
    local low = (retData * 100) % 4294967296;
    local heigh = math.floor((retData * 100) / 4294967296);
	
        if self.mEndian == self.ENDIAN_BIG then
            self:writeInt32(heigh);
		    self:writeInt32(low);
        else
		    self:writeInt32(low);
		    self:writeInt32(heigh);
        end
    
    self:advPosAndLen(8);
end

-- 写 utf-8 字节字符串，必须是 utf-8 的
function M:writeMultiByte(value)
    if value ~= nil then
        local idx = 1;
        local buffIdx = 0
        local subStr = ''
        local oneByte = ''
        while(idx <= string.len(value)) do
            buffIdx = self.mPosition + idx - 1;
            subStr = string.sub(value, idx, idx);
            oneByte = string.byte(subStr);
            self.mBuffer[buffIdx] = oneByte;
            idx = idx + 1;
        end
    end
    
    self:advPosAndLen(string.len(value));
end

-- 输出缓冲区所有的字节
function M:dumpAllBytes()
    for idx = 0, #(self.mBuffer) do
        self:log(tostring(self.mBuffer[idx]));
    end
end

function MByteBuffer:log(msg)
    SDK.Lib.TestStaticHandle.log(msg);
end

-- 测试通过 . 获取表中的函数
function M.tableFunc()
	
end

return M;