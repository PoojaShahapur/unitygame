using System;

namespace SDK.Lib
{
    public class MBitConverter
    {
        public static bool ToBoolean(
            byte[] bytes, 
            int startIndex, 
            EEndian endian = EEndian.eLITTLE_ENDIAN
            )
        {
            return bytes[startIndex] != 0;
        }

        public static char ToChar(
            byte[] bytes, 
            int startIndex, 
            EEndian endian = EEndian.eLITTLE_ENDIAN
            )
        {
            return (char)bytes[startIndex];
        }

        public static short ToInt16(
            byte[] bytes, 
            int startIndex, 
            EEndian endian = EEndian.eLITTLE_ENDIAN
            )
        {
            short retValue = 0;
            if (endian == EEndian.eLITTLE_ENDIAN)
            {
                retValue = (short)(
                    (bytes[startIndex + 1] << 8) + 
                    bytes[startIndex]
                    );
            }
            else
            {
                retValue = (short)(
                    (bytes[startIndex] << 8) + 
                    bytes[startIndex + 1]
                    );
            }
            return retValue;
        }

        public static ushort ToUInt16(
            byte[] bytes, 
            int startIndex, 
            EEndian endian = EEndian.eLITTLE_ENDIAN
            )
        {
            ushort retValue = 0;
            if (endian == EEndian.eLITTLE_ENDIAN)
            {
                retValue = (ushort)(
                    (bytes[startIndex + 1] << 8) + 
                    bytes[startIndex]
                    );
            }
            else
            {
                retValue = (ushort)(
                    (bytes[startIndex] << 8) + 
                    bytes[startIndex + 1]
                    );
            }
            return retValue;
        }

        public static int ToInt32(
            byte[] bytes, 
            int startIndex, EEndian 
            endian = EEndian.eLITTLE_ENDIAN
            )
        {
            int retValue = 0;
            if (endian == EEndian.eLITTLE_ENDIAN)
            {
                retValue = (int)(
                    (bytes[startIndex + 3] << 24) +
                    (bytes[startIndex + 2] << 16) +
                    (bytes[startIndex + 1] << 8) + 
                    bytes[startIndex]
                    );
            }
            else
            {
                retValue = (int)(
                    (bytes[startIndex] << 24) +
                    (bytes[startIndex + 1] << 16) +
                    (bytes[startIndex + 2] << 8) + 
                    bytes[startIndex + 3]
                    );
            }
            return retValue;
        }

        public static uint ToUInt32(
            byte[] bytes, 
            int startIndex, 
            EEndian endian = EEndian.eLITTLE_ENDIAN
            )
        {
            uint retValue = 0;
            if (endian == EEndian.eLITTLE_ENDIAN)
            {
                retValue = (uint)(
                    (bytes[startIndex + 3] << 24) +
                    (bytes[startIndex + 2] << 16) +
                    (bytes[startIndex + 1] << 8) + 
                    bytes[startIndex]
                    );
            }
            else
            {
                retValue = (uint)(
                    (bytes[startIndex] << 24) +
                    (bytes[startIndex + 1] << 16) +
                    (bytes[startIndex + 2] << 8) + 
                    bytes[startIndex + 3]
                    );
            }
            return retValue;
        }

        public static long ToInt64(
            byte[] bytes, 
            int startIndex, 
            EEndian endian = EEndian.eLITTLE_ENDIAN
            )
        {
            long retValue = 0;
            if (endian == EEndian.eLITTLE_ENDIAN)
            {
                retValue = (long)(
                    (bytes[startIndex + 7] << 56) + 
                    (bytes[startIndex + 6] << 48) +
                    (bytes[startIndex + 5] << 40) +
                    (bytes[startIndex + 4] << 32) +
                    (bytes[startIndex + 3] << 24) +
                    (bytes[startIndex + 2] << 16) +
                    (bytes[startIndex + 1] << 8) + 
                    bytes[startIndex]
                    );
            }
            else
            {
                retValue = (long)(
                    (bytes[startIndex] << 56) +
                    (bytes[startIndex + 1] << 48) +
                    (bytes[startIndex + 2] << 40) +
                    (bytes[startIndex + 3] << 32) +
                    (bytes[startIndex + 4] << 24) +
                    (bytes[startIndex + 5] << 16) +
                    (bytes[startIndex + 6] << 8) + 
                    bytes[startIndex + 7]
                    );
            }
            return retValue;
        }

        public static ulong ToUInt64(
            byte[] bytes, 
            int startIndex, 
            EEndian endian = EEndian.eLITTLE_ENDIAN
            )
        {
            ulong retValue = 0;
            if (endian == EEndian.eLITTLE_ENDIAN)
            {
                retValue = (ulong)(
                    (bytes[startIndex + 7] << 56) +
                    (bytes[startIndex + 6] << 48) +
                    (bytes[startIndex + 5] << 40) +
                    (bytes[startIndex + 4] << 32) +
                    (bytes[startIndex + 3] << 24) +
                    (bytes[startIndex + 2] << 16) +
                    (bytes[startIndex + 1] << 8) + 
                    bytes[startIndex]
                    );
            }
            else
            {
                retValue = (ulong)(
                    (bytes[startIndex] << 56) +
                    (bytes[startIndex + 1] << 48) +
                    (bytes[startIndex + 2] << 40) +
                    (bytes[startIndex + 3] << 32) +
                    (bytes[startIndex + 4] << 24) +
                    (bytes[startIndex + 5] << 16) +
                    (bytes[startIndex + 6] << 8) + 
                    bytes[startIndex + 7]
                    );
            }
            return retValue;
        }
        
        public static void GetBytes(
            bool data, 
            byte[] bytes, 
            int startIndex, 
            EEndian endian = EEndian.eLITTLE_ENDIAN
            )
        {
            bytes[startIndex] = (byte)(data ? 1 : 0);
        }

        public static void GetBytes(
            char data, 
            byte[] bytes, 
            int startIndex, 
            EEndian endian = EEndian.eLITTLE_ENDIAN
            )
        {
            bytes[startIndex] = (byte)data;
        }

        public static void GetBytes(
            short data, 
            byte[] bytes, 
            int startIndex, 
            EEndian endian = EEndian.eLITTLE_ENDIAN
            )
        {
            if (endian == EEndian.eLITTLE_ENDIAN)
            {
                //bytes[index] = (byte)(data & 0x00FF);
                //bytes[index + 1] = (byte)((data & 0xFF00) >> 8);
                bytes[startIndex] = (byte)(data << 8 >> 8);
                bytes[startIndex + 1] = (byte)(data >> 8);
            }
            else
            {
                //bytes[index + 1] = (byte)((data & 0xFF00) >> 8);
                //bytes[index] = (byte)(data & 0x00FF);
                bytes[startIndex] = (byte)(data >> 8);
                bytes[startIndex + 1] = (byte)(data << 8 >> 8);
            }
        }

        public static void GetBytes(
            ushort data, 
            byte[] bytes, 
            int startIndex, 
            EEndian endian = EEndian.eLITTLE_ENDIAN
            )
        {
            if (endian == EEndian.eLITTLE_ENDIAN)
            {
                bytes[startIndex] = (byte)(data << 8 >> 8);
                bytes[startIndex + 1] = (byte)(data >> 8);
            }
            else
            {
                bytes[startIndex] = (byte)(data >> 8);
                bytes[startIndex + 1] = (byte)(data << 8 >> 8);
            }
        }

        public static void GetBytes(
            int data, 
            byte[] bytes, 
            int startIndex, 
            EEndian endian = EEndian.eLITTLE_ENDIAN
            )
        {
            if (endian == EEndian.eLITTLE_ENDIAN)
            {
                bytes[startIndex] = (byte)(data << 24 >> 24);
                bytes[startIndex + 1] = (byte)(data << 16 >> 24);
                bytes[startIndex + 2] = (byte)(data << 8 >> 24);
                bytes[startIndex + 3] = (byte)(data >> 24);
            }
            else
            {
                bytes[startIndex] = (byte)(data >> 24);
                bytes[startIndex + 1] = (byte)(data << 8 >> 24);
                bytes[startIndex + 2] = (byte)(data << 16 >> 24);
                bytes[startIndex + 3] = (byte)(data << 24 >> 24);
            }
        }

        public static void GetBytes(
            uint data, 
            byte[] bytes, 
            int startIndex, 
            EEndian endian = EEndian.eLITTLE_ENDIAN
            )
        {
            if (endian == EEndian.eLITTLE_ENDIAN)
            {
                bytes[startIndex] = (byte)(data << 24 >> 24);
                bytes[startIndex + 1] = (byte)(data << 16 >> 24);
                bytes[startIndex + 2] = (byte)(data << 8 >> 24);
                bytes[startIndex + 3] = (byte)(data >> 24);
            }
            else
            {
                bytes[startIndex] = (byte)(data >> 24);
                bytes[startIndex + 1] = (byte)(data << 8 >> 24);
                bytes[startIndex + 2] = (byte)(data << 16 >> 24);
                bytes[startIndex + 3] = (byte)(data << 24 >> 24);
            }
        }

        public static void GetBytes(
            long data, 
            byte[] bytes, 
            int startIndex, 
            EEndian endian = EEndian.eLITTLE_ENDIAN
            )
        {
            if (endian == EEndian.eLITTLE_ENDIAN)
            {
                bytes[startIndex] = (byte)(data << 56 >> 56);
                bytes[startIndex + 1] = (byte)(data << 48 >> 56);
                bytes[startIndex + 2] = (byte)(data << 40 >> 56);
                bytes[startIndex + 3] = (byte)(data << 32 >> 56);

                bytes[startIndex + 4] = (byte)(data << 24 >> 56);
                bytes[startIndex + 5] = (byte)(data << 16 >> 56);
                bytes[startIndex + 6] = (byte)(data << 8 >> 56);
                bytes[startIndex + 7] = (byte)(data >> 56);
            }
            else
            {
                bytes[startIndex] = (byte)(data >> 56);
                bytes[startIndex + 1] = (byte)(data << 8 >> 56);
                bytes[startIndex + 2] = (byte)(data << 16 >> 56);
                bytes[startIndex + 3] = (byte)(data << 24 >> 56);

                bytes[startIndex + 4] = (byte)(data << 32 >> 56);
                bytes[startIndex + 5] = (byte)(data << 40 >> 56);
                bytes[startIndex + 6] = (byte)(data << 48 >> 56);
                bytes[startIndex + 7] = (byte)(data << 56 >> 56);
            }
        }

        public static void GetBytes(
            ulong data, 
            byte[] bytes, 
            int startIndex, 
            EEndian endian = EEndian.eLITTLE_ENDIAN
            )
        {
            if (endian == EEndian.eLITTLE_ENDIAN)
            {
                bytes[startIndex] = (byte)(data << 56 >> 56);
                bytes[startIndex + 1] = (byte)(data << 48 >> 56);
                bytes[startIndex + 2] = (byte)(data << 40 >> 56);
                bytes[startIndex + 3] = (byte)(data << 32 >> 56);

                bytes[startIndex + 4] = (byte)(data << 24 >> 56);
                bytes[startIndex + 5] = (byte)(data << 16 >> 56);
                bytes[startIndex + 6] = (byte)(data << 8 >> 56);
                bytes[startIndex + 7] = (byte)(data >> 56);
            }
            else
            {
                bytes[startIndex] = (byte)(data >> 56);
                bytes[startIndex + 1] = (byte)(data << 8 >> 56);
                bytes[startIndex + 2] = (byte)(data << 16 >> 56);
                bytes[startIndex + 3] = (byte)(data << 24 >> 56);

                bytes[startIndex + 4] = (byte)(data << 32 >> 56);
                bytes[startIndex + 5] = (byte)(data << 40 >> 56);
                bytes[startIndex + 6] = (byte)(data << 48 >> 56);
                bytes[startIndex + 7] = (byte)(data << 56 >> 56);
            }
        }

        static public int ToInt32(string value)
        {
            return Convert.ToInt32(value);
        }
    }
}