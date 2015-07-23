--[[字节缓冲区]]
ByteBuffer = {}

ByteBuffer.ENDIAN_LITTLE = 0    -- 小端字节序是 0
ByteBuffer.ENDIAN_BIG = 1       -- 大端字节序是 0
ByteBuffer.m_endian = ByteBuffer.ENDIAN_LITTLE -- 自己字节序
ByteBuffer.m_sysEndian = ByteBuffer.ENDIAN_LITTLE -- 系统字节序

ByteBuffer.m_buff = {}  -- 字节缓冲区
ByteBuffer.m_position = 1   -- 缓冲区当前位置，注意 Lua 下表是从 1 开始的，不是从 0 开始的。 this.m_buff[0] == nil ，太坑了

local this = ByteBuffer   -- 局部引用

-- 读取一个字节
function ByteBuffer.readInt8()
    local elem = this.m_buff[this.m_position]
    local retData = string.byte(elem)
    this.advPos(1);
    return retData
end

-- 读取和写入的时候只看存储时候的字节序就行了，不用管系统字节序，因为是自组合成本地数据的
-- 读取两个字节
function ByteBuffer.readInt16()
    local retData = 0
    if this.canRead(2) then
        if ByteBuffer.m_endian == ByteBuffer.ENDIAN_LITTLE then-- 如果是小端字节序
            retData = string.byte(this.m_buff[this.m_position]) * 256 + string.byte(this.m_buff[this.m_position + 1])
        else
            retData = string.byte(this.m_buff[this.m_position + 1]) * 256 + string.byte(this.m_buff[this.m_position])
        end
        this.advPos(2);
    end
    
    return retData
end

function ByteBuffer.readInt32()
    local retData = 0
    if this.canRead(4) then
        if ByteBuffer.m_endian == ByteBuffer.ENDIAN_LITTLE then-- 如果是小端字节序
            retData = string.byte(this.m_buff[this.m_position]) * 256 * 256 * 256 + string.byte(this.m_buff[this.m_position + 1]) * 256 * 256 + string.byte(this.m_buff[this.m_position + 2]) * 256 + string.byte(this.m_buff[this.m_position + 3])
        else
            retData = string.byte(this.m_buff[this.m_position + 3]) * 256 * 256 * 256 + string.byte(this.m_buff[this.m_position + 2]) * 256 * 256 + string.byte(this.m_buff[this.m_position + 1]) * 256 + string.byte(this.m_buff[this.m_position])
        end
        this.advPos(4);
    end
    
    return retData
end

function ByteBuffer.readNumber()
    local retData = 0
    if this.canRead(8) then
        if ByteBuffer.m_endian == ByteBuffer.ENDIAN_LITTLE then-- 如果是小端字节序
            local str = this.m_position[this.m_position] .. this.m_position[this.m_position + 1] .. this.m_position[this.m_position + 2] .. this.m_position[this.m_position + 3] .. this.m_position[this.m_position + 4] .. this.m_position[this.m_position + 5] .. this.m_position[this.m_position + 6] .. this.m_position[this.m_position + 7]
        else
            local str = this.m_position[this.m_position + 7] .. this.m_position[this.m_position + 6] .. this.m_position[this.m_position + 5] .. this.m_position[this.m_position + 4] .. this.m_position[this.m_position + 3] .. this.m_position[this.m_position + 2] .. this.m_position[this.m_position + 1] .. this.m_position[this.m_position]
        end

        retData = tonumber(str)        
        this.advPos(8);
    end
    
    return retData
end

function ByteBuffer.writeInt8(retData)
    this.m_buff[this.m_position] = retData
    this.advPosAndLen(1);
end

function ByteBuffer.writeInt16(retData)
    local oneByte = retData % 256
    local twoByte = retData / 256

    if ByteBuffer.m_endian == ByteBuffer.ENDIAN_LITTLE then-- 如果是小端字节序
        this.m_buff[this.m_position] = twoByte
        this.m_buff[this.m_position + 1] = oneByte
    else
        this.m_buff[this.m_position] = oneByte
        this.m_buff[this.m_position + 1] = twoByte
    end
    
    this.advPosAndLen(2);
end

function ByteBuffer.writeInt32(retData)
    local oneByte = retData % 256
    local twoByte = retData / 256 % 256
    local threeByte = retData / (256 * 256) % 256
    local fourByte = retData / (256 * 256 * 256)

    if ByteBuffer.m_endian == ByteBuffer.ENDIAN_LITTLE then-- 如果是小端字节序
        this.m_buff[this.m_position] = fourByte
        this.m_buff[this.m_position + 1] = threeByte
        this.m_buff[this.m_position + 2] = twoByte
        this.m_buff[this.m_position + 3] = oneByte
    else
        this.m_buff[this.m_position] = oneByte
        this.m_buff[this.m_position + 1] = twoByte
        this.m_buff[this.m_position + 2] = threeByte
        this.m_buff[this.m_position + 3] = fourByte
    end
    
    this.advPosAndLen(4);
end

function ByteBuffer.writeNumber(retData)
    str = tostrng(retData)
    len = string.len(str)
    idx = 0
    if ByteBuffer.m_endian == ByteBuffer.ENDIAN_LITTLE then-- 如果是小端字节序
        while( idx < 8 )
        do
            this.m_buff[this.m_position + idx] = string.byte(str, idx)
            idx = idx + 1
        end
        
        while( idx < 8 )
        do
            this.m_buff[this.m_position + idx] = 0
            idx = idx + 1
        end
    else
        while( idx < 8 )
        do
            this.m_buff[this.m_position + idx] = string.byte(str, len - idx)
            idx = idx + 1
        end
        
        while( idx < 8 )
        do
            this.m_buff[this.m_position + idx] = 0
            idx = idx + 1
        end
    end
    
    this.advPosAndLen(8);
end

-- 是否有足够的字节可以读取
function ByteBuffer.canRead(len)
    if this.m_position + len > this.length() then
        return false
    end
    
    return true
end

function ByteBuffer.advPos(num)
    this.m_position = this.m_position + num;
end

function ByteBuffer.advPosAndLen(num)
    this.m_position = this.m_position + num;
end

-- 判断字节序和系统字节序是否相同
function ByteBuffer.isEqualEndian()
    return ByteBuffer.m_endian == ByteBuffer.m_sysEndian
end

-- 获取长度
function ByteBuffer.length()
    return #this.m_buff
end