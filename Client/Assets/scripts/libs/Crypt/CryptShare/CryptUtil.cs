using System;
namespace SDK.Lib
{
    public class CryptKeyBase
    {

    }

    public class CryptUtil
    {
        public static void c2l(byte[] c, ref ulong l, ref int startIdx)
        {
            l = ((ulong)(c[startIdx++]));
            l |= ((ulong)(c[startIdx++])) << 8;
            l |= ((ulong)(c[startIdx++])) << 16;
            l |= ((ulong)(c[startIdx++])) << 24;
        }

        public static void l2c(ulong l, byte[] c, ref int startIdx)
        {
            c[startIdx++] = (byte)(((l)) & 0xff);
            c[startIdx++] = (byte)(((l) >> 8) & 0xff);
            c[startIdx++] = (byte)(((l) >> 16) & 0xff);
            c[startIdx++] = (byte)(((l) >> 24) & 0xff);
        }

        public static void c2ln(byte[] c, ref ulong l1, ref ulong l2, long n, ref int startIdx)
        {
            startIdx += (int)n;
            l1 = l2 = 0;

            if (8 == n)
            {
                l2 = ((ulong)(c[--startIdx])) << 24;
            }
            if (8 == n || 7 == n)
            {
                l2 |= ((ulong)(c[--startIdx])) << 16;
            }
            if (8 == n || 7 == n || 6 == n)
            {
                l2 |= ((ulong)(c[--startIdx])) << 8;
            }
            if (8 == n || 7 == n || 6 == n || 5 == n)
            {
                l2 |= ((ulong)(c[--startIdx]));
            }
            if (8 == n || 7 == n || 6 == n || 5 == n || 4 == n)
            {
                l1 = ((ulong)(c[--startIdx])) << 24;
            }
            if (8 == n || 7 == n || 6 == n || 5 == n || 4 == n || 3 == n)
            {
                l1 |= ((ulong)(c[--startIdx])) << 16;
            }
            if (8 == n || 7 == n || 6 == n || 5 == n || 4 == n || 3 == n || 2 == n)
            {
                l1 |= ((ulong)(c[--startIdx])) << 8;
            }
            if (8 == n || 7 == n || 6 == n || 5 == n || 4 == n || 3 == n || 2 == n || 1 == n)
            {
                l1 |= ((ulong)(c[--startIdx]));
            }
        }

        public static void l2cn(ulong l1, ulong l2, byte[] c, long n, ref int startIdx)
        {
            startIdx += (int)n;

            if (8 == n)
            {
                c[--startIdx] = (byte)(((l2) >> 24) & 0xff);
            }
            if (8 == n || 7 == n)
            {
                c[--startIdx] = (byte)(((l2) >> 16) & 0xff);
            }
            if (8 == n || 7 == n || 6 == n)
            {
                c[--startIdx] = (byte)(((l2) >> 8) & 0xff);
            }
            if (8 == n || 7 == n || 6 == n || 5 == n)
            {
                c[--startIdx] = (byte)(((l2)) & 0xff);
            }
            if (8 == n || 7 == n || 6 == n || 5 == n || 4 == n)
            {
                c[--startIdx] = (byte)(((l1) >> 24) & 0xff);
            }
            if (8 == n || 7 == n || 6 == n || 5 == n || 4 == n || 3 == n)
            {
                c[--startIdx] = (byte)(((l1) >> 16) & 0xff);
            }
            if (8 == n || 7 == n || 6 == n || 5 == n || 4 == n || 3 == n || 2 == n)
            {
                c[--startIdx] = (byte)(((l1) >> 8) & 0xff);
            }
            if (8 == n || 7 == n || 6 == n || 5 == n || 4 == n || 3 == n || 2 == n || 1 == n)
            {
                c[--startIdx] = (byte)(((l1)) & 0xff);
            }
        }

        public static void PERM_OP(ref ulong a, ref ulong b, ref ulong t, long n, ulong m) 
        {
            (t) = ((((a) >> (int)(n)) ^ (b)) & (m));
		    (b)^=(t);
            (a) ^= ((t) << (int)(n));
        }

        public static void HPERM_OP(ref ulong a, ulong t, long n, ulong m)
        {
            (t) = ((((a) << (int)(16 - (n))) ^ (a)) & (m));
            (a) = (a) ^ (t) ^ (t >> (int)(16 - (n)));
        }

        public static ulong ROTATE(ulong a, int n)	
        {
			return (((a) >> (int)(n)) | (((uint)a) << (32 - (int)(n))));
		}

        //public static ulong ROTATE_LEFT(ulong x, int n) 
        public static uint ROTATE_LEFT(uint x, int n)
        {
            return (((x) << (int)(n)) | (((uint)x) >> (32 - (int)(n))));
        }

        public static void IP(ref ulong l, ref ulong r)
        {
	        ulong tt = 0;
		    PERM_OP(ref r, ref l, ref tt,4,0x0f0f0f0fL);
		    PERM_OP(ref l, ref r, ref tt,16,0x0000ffffL);
		    PERM_OP(ref r, ref l, ref tt,2,0x33333333L);
		    PERM_OP(ref l, ref r, ref tt,8,0x00ff00ffL);
		    PERM_OP(ref r, ref l, ref tt,1,0x55555555L);
        }

        public static void FP(ref ulong l, ref ulong r)
        {
	        ulong tt = 0;
            PERM_OP(ref l, ref r, ref tt, 1, 0x55555555L);
            PERM_OP(ref r, ref l, ref tt, 8, 0x00ff00ffL);
            PERM_OP(ref l, ref r, ref tt, 2, 0x33333333L);
            PERM_OP(ref r, ref l, ref tt, 16, 0x0000ffffL);
            PERM_OP(ref l, ref r, ref tt, 4, 0x0f0f0f0fL);
        }

        public static void get8Byte(byte[] inBytesTotal, int startIdx, byte[] inBytes)
        {
            Array.Clear(inBytes, 0, 8);
            if (startIdx + 8 <= inBytesTotal.Length)
            {
                Array.Copy(inBytesTotal, startIdx, inBytes, 0, 8);
            }
            else
            {
                Array.Copy(inBytesTotal, startIdx, inBytes, 0, inBytesTotal.Length - startIdx);
            }
        }
    }
}