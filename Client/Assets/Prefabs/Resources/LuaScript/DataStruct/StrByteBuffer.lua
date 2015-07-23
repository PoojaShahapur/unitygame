--[[字节缓冲区]]
StrByteBuffer = {}

StrByteBuffer.ENDIAN_LITTLE = 0    -- 小端字节序是 0
StrByteBuffer.ENDIAN_BIG = 1       -- 大端字节序是 0
StrByteBuffer.m_endian = StrByteBuffer.ENDIAN_LITTLE -- 自己字节序
StrByteBuffer.m_sysEndian = StrByteBuffer.ENDIAN_LITTLE -- 系统字节序

--ByteBuffer.m_buff = ""  -- 缓冲区
StrByteBuffer.m_position = 0   -- 缓冲区当前位置

local this = StrByteBuffer   -- 局部引用

-- 读取一个 bool 类型
function StrByteBuffer.readBoolean(retData)
    retData = string.byte(this.m_buff, this.m_position) > 0
end

-- 读取一个字节
function StrByteBuffer.readInt8(retData)
    retData = string.byte(this.m_buff, this.m_position)
end

-- 读取只看存储时候的字节序就行了，保存的时候，要对比系统字节序和自己要保存的字节序
-- 读取两个字节
function StrByteBuffer.readInt16(retData)
    if canRead(2) then
        if ByteBuffer.m_endian == StrByteBuffer.ENDIAN_LITTLE then-- 如果是小端字节序
            retData = string.byte(this.m_buff, this.m_position) * 256 + string.byte(this.m_buff, this.m_position + 1)
        else
            retData = string.byte(this.m_buff, this.m_position + 1) * 256 + string.byte(this.m_buff, this.m_position)
        end
    end
end

function StrByteBuffer.readInt32(retData)
    if canRead(4) then
        if StrByteBuffer.m_endian == StrByteBuffer.ENDIAN_LITTLE then-- 如果是小端字节序
            retData = string.byte(this.m_buff, this.m_position) * 256 * 256 * 256 + string.byte(this.m_buff, this.m_position + 1) * 256 * 256 + string.byte(this.m_buff, this.m_position + 2) * 256 + string.byte(this.m_buff, this.m_position + 3)
        else
            retData = string.byte(this.m_buff, this.m_position + 3) * 256 * 256 * 256 + string.byte(this.m_buff, this.m_position + 2) * 256 * 256 + string.byte(this.m_buff, this.m_position + 1) * 256 + string.byte(this.m_buff, this.m_position)
        end
    end
end

function StrByteBuffer.writeInt16(retData)
    if canRead(2) then
        if StrByteBuffer.m_endian == StrByteBuffer.ENDIAN_LITTLE then-- 如果是小端字节序
            retData = string.byte(this.m_buff, this.m_position) * 10 + string.byte(this.m_buff, this.m_position + 1)
        else
            retData = string.byte(this.m_buff, this.m_position + 1) * 10 + string.byte(this.m_buff, this.m_position)
        end
    end
end

-- 是否有足够的字节可以读取
function StrByteBuffer.canRead(len)
    if this.m_position + len > this.length() then
        return false
    end
    
    return true
end

function StrByteBuffer.advPos(num)
    this.m_position = this.m_position + num;
end

-- 判断字节序和系统字节序是否相同
function StrByteBuffer.isEqualEndian()
    return StrByteBuffer.m_endian == StrByteBuffer.m_sysEndian
end

-- 获取长度
function StrByteBuffer.length()
    return string.len(this.m_buff)
end