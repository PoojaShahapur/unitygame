﻿namespace SDK.Common
{
    public class ItemHeader
    {
        public uint m_uID;              // 唯一 ID
        public uint m_offset;           // 这一项在文件中的偏移

        // 解析头部
        virtual public void parseHeaderByteArray(IByteArray bytes)
        {
            m_uID = bytes.readUnsignedInt();
            m_offset = bytes.readUnsignedInt();
        }
    }
}