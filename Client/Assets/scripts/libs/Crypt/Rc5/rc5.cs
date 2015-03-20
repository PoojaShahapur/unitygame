using SDK.Common;

namespace SDK.Lib
{
    public class RC5_32_KEY
    {
        public int rounds;
        public ulong[] data;       // 34
    }

    public class RC5
    {
        public const int RC5_ENCRYPT = 1;
        public const int RC5_DECRYPT = 0;

        public const ulong RC5_32_MASK = 0xffffffffL;

        ulong ROTATE_l32(ulong a, ulong n)
        {
            return (((a) << (int)(n & 0x1f)) | (((a) & 0xffffffff) >> (int)((32 - (n & 0x1f)))));
        }

        ulong ROTATE_r32(ulong a,ulong n)
        {
            return (((a) << (int)((32 - (n & 0x1f)))) | (((a) & 0xffffffff) >> (int)(n & 0x1f)));
        }

        void E_RC5_32(ref ulong a, ref ulong b, ulong[] s, ulong n)
        {
            a^=b;
            a=ROTATE_l32(a,b);
            a+=s[n];
            a&=RC5_32_MASK;
            b^=a;
            b=ROTATE_l32(b,a);
            b+=s[n+1];
            b&=RC5_32_MASK;
        }

        void D_RC5_32(ref ulong a, ref ulong b, ulong[] s, ulong n)
        {
            b -= s[n + 1];
            b &= RC5_32_MASK;
            b = ROTATE_r32(b, a);
            b ^= a;
            a -= s[n];
            a &= RC5_32_MASK;
            a = ROTATE_r32(a, b);
            a ^= b;
        }

        void c2l(byte[] c, ref ulong l, ref int startIdx)
        {
            l = ((ulong)(c[startIdx++]));
            l |= ((ulong)(c[startIdx++])) << 8;
            l |= ((ulong)(c[startIdx++])) << 16;
            l |= ((ulong)(c[startIdx++])) << 24;
        }

        void l2c(ulong l, byte[] c, ref int startIdx)
        {
            c[startIdx++] = (byte)(((l)     )&0xff);
            c[startIdx++] = (byte)(((l)>> 8)&0xff);
            c[startIdx++] = (byte)(((l)>>16)&0xff);
            c[startIdx++] = (byte)(((l) >> 24) & 0xff);
        }

        void c2ln(byte[] c, ref ulong l1, ref ulong l2, long n, ref int startIdx)
        {
            startIdx += (int)n;
            l1=l2=0;
            switch (n) 
            {
                case 8: l2 = ((ulong)(c[--startIdx])) << 24;
                case 7: l2 |= ((ulong)(c[--startIdx])) << 16;
                case 6: l2 |= ((ulong)(c[--startIdx])) << 8;
                case 5: l2 |= ((ulong)(c[--startIdx]));
                case 4: l1 = ((ulong)(c[--startIdx])) << 24;
                case 3: l1 |= ((ulong)(c[--startIdx])) << 16;
                case 2: l1 |= ((ulong)(c[--startIdx])) << 8;
                case 1: l1 |= ((ulong)(c[--startIdx]));
            }
        }

        void l2cn(ulong l1, ulong l2, byte[] c, long n, ref int startIdx)
        {
            startIdx += (int)n;
            switch (n) 
            {
                case 8: c[--startIdx] = (byte)(((l2) >> 24) & 0xff);
                case 7: c[--startIdx] = (byte)(((l2) >> 16) & 0xff);
                case 6: c[--startIdx] = (byte)(((l2) >> 8) & 0xff);
                case 5: c[--startIdx] = (byte)(((l2)) & 0xff);
                case 4: c[--startIdx] = (byte)(((l1) >> 24) & 0xff);
                case 3: c[--startIdx] = (byte)(((l1) >> 16) & 0xff);
                case 2: c[--startIdx] = (byte)(((l1) >> 8) & 0xff);
                case 1: c[--startIdx] = (byte)(((l1)) & 0xff);
            }
        }

        void RC5_32_encrypt(ref ulong[] d, RC5_32_KEY key)
        {
            ulong a, b;
            ulong[] s = key.data;

            a = d[0] + s[0];
            b = d[1] + s[1];
            E_RC5_32(ref a, ref b, s, 2);
            E_RC5_32(ref a, ref b, s, 4);
            E_RC5_32(ref a, ref b, s, 6);
            E_RC5_32(ref a, ref b, s, 8);
            E_RC5_32(ref a, ref b, s, 10);
            E_RC5_32(ref a, ref b, s, 12);
            E_RC5_32(ref a, ref b, s, 14);
            E_RC5_32(ref a, ref b, s, 16);
            if (key.rounds == 12) {
                E_RC5_32(ref a, ref b, s, 18);
                E_RC5_32(ref a, ref b, s, 20);
                E_RC5_32(ref a, ref b, s, 22);
                E_RC5_32(ref a, ref b, s, 24);
            } else if (key.rounds == 16) {
                /* Do a full expansion to avoid a jump */
                E_RC5_32(ref a, ref b, s, 18);
                E_RC5_32(ref a, ref b, s, 20);
                E_RC5_32(ref a, ref b, s, 22);
                E_RC5_32(ref a, ref b, s, 24);
                E_RC5_32(ref a, ref b, s, 26);
                E_RC5_32(ref a, ref b, s, 28);
                E_RC5_32(ref a, ref b, s, 30);
                E_RC5_32(ref a, ref b, s, 32);
            }
            d[0] = a;
            d[1] = b;
        }

        void RC5_32_decrypt(ref ulong[] d, RC5_32_KEY key)
        {
            ulong a, b;
            ulong[] s;

            s = key.data;

            a = d[0];
            b = d[1];
            if (key.rounds == 16) {
                D_RC5_32(ref a, ref b, s, 32);
                D_RC5_32(ref a, ref b, s, 30);
                D_RC5_32(ref a, ref b, s, 28);
                D_RC5_32(ref a, ref b, s, 26);
                /* Do a full expansion to avoid a jump */
                D_RC5_32(ref a, ref b, s, 24);
                D_RC5_32(ref a, ref b, s, 22);
                D_RC5_32(ref a, ref b, s, 20);
                D_RC5_32(ref a, ref b, s, 18);
            } else if (key.rounds == 12) {
                D_RC5_32(ref a, ref b, s, 24);
                D_RC5_32(ref a, ref b, s, 22);
                D_RC5_32(ref a, ref b, s, 20);
                D_RC5_32(ref a, ref b, s, 18);
            }
            D_RC5_32(ref a, ref b, s, 16);
            D_RC5_32(ref a, ref b, s, 14);
            D_RC5_32(ref a, ref b, s, 12);
            D_RC5_32(ref a, ref b, s, 10);
            D_RC5_32(ref a, ref b, s, 8);
            D_RC5_32(ref a, ref b, s, 6);
            D_RC5_32(ref a, ref b, s, 4);
            D_RC5_32(ref a, ref b, s, 2);
            d[0] = a - s[0];
            d[1] = b - s[1];
        }

        void RC5_32_cbc_encrypt(byte[] inBytes, byte[] outBytes,
                                long length, RC5_32_KEY ks, byte[] iv,
                                int encrypt)
        {
            ulong tin0 = 0, tin1 = 0;
            ulong tout0 = 0, tout1 = 0, xor0 = 0, xor1 = 0;
            long l = length;
            ulong[] tin = new ulong[2];

            int c2l_iv_startIdx = 0;
            int c2l_inBytes_startIdx = 0;
            int l2c_outBytes_startIdx = 0;
            int l2c_iv_startIdx = 0;

            if (encrypt > 0) {
                c2l(iv, ref tout0, ref c2l_iv_startIdx);
                c2l(iv, ref tout1, ref c2l_iv_startIdx);
                c2l_iv_startIdx -= 8;

                for (l -= 8; l >= 0; l -= 8) {
                    c2l(inBytes, ref tin0, ref c2l_inBytes_startIdx);
                    c2l(inBytes, ref tin1, ref c2l_inBytes_startIdx);
                    tin0 ^= tout0;
                    tin1 ^= tout1;
                    tin[0] = tin0;
                    tin[1] = tin1;
                    RC5_32_encrypt(ref tin, ks);
                    tout0 = tin[0];
                    l2c(tout0, outBytes, ref l2c_outBytes_startIdx);
                    tout1 = tin[1];
                    l2c(tout1, outBytes, ref l2c_outBytes_startIdx);
                }
                if (l != -8) {
                    c2ln(inBytes, ref tin0, ref tin1, l + 8, ref c2l_inBytes_startIdx);
                    tin0 ^= tout0;
                    tin1 ^= tout1;
                    tin[0] = tin0;
                    tin[1] = tin1;
                    RC5_32_encrypt(ref tin, ks);
                    tout0 = tin[0];
                    l2c(tout0, outBytes, ref l2c_outBytes_startIdx);
                    tout1 = tin[1];
                    l2c(tout1, outBytes, ref l2c_outBytes_startIdx);
                }
                l2c(tout0, iv, ref l2c_iv_startIdx);
                l2c(tout1, iv, ref l2c_iv_startIdx);
            } else {
                c2l(iv, ref xor0, ref c2l_iv_startIdx);
                c2l(iv, ref xor1, ref c2l_iv_startIdx);
                c2l_iv_startIdx -= 8;

                for (l -= 8; l >= 0; l -= 8) {
                    c2l(inBytes, ref tin0, ref c2l_inBytes_startIdx);
                    tin[0] = tin0;
                    c2l(inBytes, ref tin1, ref c2l_inBytes_startIdx);
                    tin[1] = tin1;
                    RC5_32_decrypt(ref tin, ks);
                    tout0 = tin[0] ^ xor0;
                    tout1 = tin[1] ^ xor1;
                    l2c(tout0, outBytes, ref l2c_outBytes_startIdx);
                    l2c(tout1, outBytes, ref l2c_outBytes_startIdx);
                    xor0 = tin0;
                    xor1 = tin1;
                }
                if (l != -8) {
                    c2l(inBytes, ref tin0, ref c2l_inBytes_startIdx);
                    tin[0] = tin0;
                    c2l(inBytes, ref tin1, ref c2l_inBytes_startIdx);
                    tin[1] = tin1;
                    RC5_32_decrypt(ref tin, ks);
                    tout0 = tin[0] ^ xor0;
                    tout1 = tin[1] ^ xor1;
                    l2cn(tout0, tout1, outBytes, l + 8, ref l2c_outBytes_startIdx);
                    xor0 = tin0;
                    xor1 = tin1;
                }
                l2c(xor0, iv, ref l2c_iv_startIdx);
                l2c(xor1, iv, ref l2c_iv_startIdx);
            }
            tin0 = tin1 = tout0 = tout1 = xor0 = xor1 = 0;
            tin[0] = tin[1] = 0;
        }
    }
}