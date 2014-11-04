using System;
using System.Collections.Generic;

namespace SDK.Lib
{
    /**
     *@brief 字节编码解码，大端小端
     */
    public class ByteUtil
    {
        /**
         *@brief 检查大端小端
         */
        static public void checkEndian()
        {
            // 检测默认编码
            // 方法一
            //if (ByteArray.m_sEndian == Endian.NONE_ENDIAN)
            //{
            //    byte[] bt = System.BitConverter.GetBytes(1);
            //    if (bt[0] == 1)  // 数据的低位保存在内存的低地址中
            //    {
            //        ByteArray.m_sEndian = Endian.LITTLE_ENDIAN;
            //    }
            //    else
            //   {
            //        ByteArray.m_sEndian = Endian.BIG_ENDIAN;
            //    }
            //}
            // 方法二
            if(System.BitConverter.IsLittleEndian)
            {
                ByteArray.m_sEndian = Endian.LITTLE_ENDIAN;
            }
            else
            {
                ByteArray.m_sEndian = Endian.BIG_ENDIAN;
            }
        }
    }
}