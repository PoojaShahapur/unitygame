using System;
using System.Collections.Generic;

namespace SDK.Lib
{
    public enum Endian
    {
        BIG_ENDIAN,         // 大端
        LITTLE_ENDIAN,      // 小端
    }

    public class SystemEndian
    {
        static public Endian m_sEndian = Endian.LITTLE_ENDIAN;     // 当前机器的编码
    }
}