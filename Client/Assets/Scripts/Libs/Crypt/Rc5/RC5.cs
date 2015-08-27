using System;

namespace SDK.Lib
{
    public class RC5_32_KEY : CryptKeyBase
    {
        public int rounds;
        public ulong[] data;       // 34

        public RC5_32_KEY()
        {
            data = new ulong[34];
        }
    }

    public class RC5
    {
        public const int RC5_ENCRYPT = 1;
        public const int RC5_DECRYPT = 0;

        public const int RC5_8_ROUNDS = 8;
        public const int RC5_12_ROUNDS = 12;
        public const int RC5_16_ROUNDS = 16;

        public const ulong RC5_32_MASK = 0xffffffffL;

        public const ulong RC5_16_P = 0xB7E1;
        public const ulong RC5_16_Q = 0x9E37;
        public const ulong RC5_32_P = 0xB7E15163L;
        public const ulong RC5_32_Q = 0x9E3779B9L;
        public const ulong RC5_64_P = 0xB7E151628AED2A6BL;
        public const ulong RC5_64_Q = 0x9E3779B97F4A7C15L;

        public static ulong ROTATE_l32(ulong a, ulong n)
        {
            return (((a) << (int)(n & 0x1f)) | (((a) & 0xffffffff) >> (int)((32 - (n & 0x1f)))));
        }

        public static ulong ROTATE_r32(ulong a, ulong n)
        {
            return (((a) << (int)((32 - (n & 0x1f)))) | (((a) & 0xffffffff) >> (int)(n & 0x1f)));
        }

        public static void E_RC5_32(ref ulong a, ref ulong b, ulong[] s, ulong n)
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

        public static void D_RC5_32(ref ulong a, ref ulong b, ulong[] s, ulong n)
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

        public static void RC5_32_encrypt(ref ulong[] d, RC5_32_KEY key)
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

        public static void RC5_32_decrypt(ref ulong[] d, RC5_32_KEY key)
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

        public static void RC5_32_cbc_encrypt(byte[] inBytes, byte[] outBytes,
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
                CryptUtil.c2l(iv, ref tout0, ref c2l_iv_startIdx);
                CryptUtil.c2l(iv, ref tout1, ref c2l_iv_startIdx);
                c2l_iv_startIdx -= 8;

                for (l -= 8; l >= 0; l -= 8) {
                    CryptUtil.c2l(inBytes, ref tin0, ref c2l_inBytes_startIdx);
                    CryptUtil.c2l(inBytes, ref tin1, ref c2l_inBytes_startIdx);
                    tin0 ^= tout0;
                    tin1 ^= tout1;
                    tin[0] = tin0;
                    tin[1] = tin1;
                    RC5_32_encrypt(ref tin, ks);
                    tout0 = tin[0];
                    CryptUtil.l2c(tout0, outBytes, ref l2c_outBytes_startIdx);
                    tout1 = tin[1];
                    CryptUtil.l2c(tout1, outBytes, ref l2c_outBytes_startIdx);
                }
                if (l != -8) {
                    CryptUtil.c2ln(inBytes, ref tin0, ref tin1, l + 8, ref c2l_inBytes_startIdx);
                    tin0 ^= tout0;
                    tin1 ^= tout1;
                    tin[0] = tin0;
                    tin[1] = tin1;
                    RC5_32_encrypt(ref tin, ks);
                    tout0 = tin[0];
                    CryptUtil.l2c(tout0, outBytes, ref l2c_outBytes_startIdx);
                    tout1 = tin[1];
                    CryptUtil.l2c(tout1, outBytes, ref l2c_outBytes_startIdx);
                }
                CryptUtil.l2c(tout0, iv, ref l2c_iv_startIdx);
                CryptUtil.l2c(tout1, iv, ref l2c_iv_startIdx);
            } else {
                CryptUtil.c2l(iv, ref xor0, ref c2l_iv_startIdx);
                CryptUtil.c2l(iv, ref xor1, ref c2l_iv_startIdx);
                c2l_iv_startIdx -= 8;

                for (l -= 8; l >= 0; l -= 8) {
                    CryptUtil.c2l(inBytes, ref tin0, ref c2l_inBytes_startIdx);
                    tin[0] = tin0;
                    CryptUtil.c2l(inBytes, ref tin1, ref c2l_inBytes_startIdx);
                    tin[1] = tin1;
                    RC5_32_decrypt(ref tin, ks);
                    tout0 = tin[0] ^ xor0;
                    tout1 = tin[1] ^ xor1;
                    CryptUtil.l2c(tout0, outBytes, ref l2c_outBytes_startIdx);
                    CryptUtil.l2c(tout1, outBytes, ref l2c_outBytes_startIdx);
                    xor0 = tin0;
                    xor1 = tin1;
                }
                if (l != -8) {
                    CryptUtil.c2l(inBytes, ref tin0, ref c2l_inBytes_startIdx);
                    tin[0] = tin0;
                    CryptUtil.c2l(inBytes, ref tin1, ref c2l_inBytes_startIdx);
                    tin[1] = tin1;
                    RC5_32_decrypt(ref tin, ks);
                    tout0 = tin[0] ^ xor0;
                    tout1 = tin[1] ^ xor1;
                    CryptUtil.l2cn(tout0, tout1, outBytes, l + 8, ref l2c_outBytes_startIdx);
                    xor0 = tin0;
                    xor1 = tin1;
                }
                CryptUtil.l2c(xor0, iv, ref l2c_iv_startIdx);
                CryptUtil.l2c(xor1, iv, ref l2c_iv_startIdx);
            }
            tin0 = tin1 = tout0 = tout1 = xor0 = xor1 = 0;
            tin[0] = tin[1] = 0;
        }

        public static void RC5_32_ecb_encrypt(byte[] inBytes, byte[] outBytes,
                        RC5_32_KEY ks, int encrypt, CryptContext cryptContext)
        {
            ulong l = 0;
            ulong[] d = new ulong[2];

            int c2l_inBytes_startIdx = 0;
            int l2c_outBytes_startIdx = 0;

            CryptUtil.c2l(inBytes, ref l, ref c2l_inBytes_startIdx);
            d[0] = l;
            CryptUtil.c2l(inBytes, ref l, ref c2l_inBytes_startIdx);
            d[1] = l;
            if (encrypt > 0)
                RC5_32_encrypt(ref d, ks);
            else
                RC5_32_decrypt(ref d, ks);
            l = d[0];
            CryptUtil.l2c(l, outBytes, ref l2c_outBytes_startIdx);
            l = d[1];
            CryptUtil.l2c(l, outBytes, ref l2c_outBytes_startIdx);
            l = d[0] = d[1] = 0;
        }

        public static void RC5_32_set_key(RC5_32_KEY key, int len, byte[] data,
                            int rounds)
        {
            int c2l_data_startIdx = 0;

            ulong[] L = new ulong[64];
            ulong l = 0, ll = 0, A = 0, B = 0, k = 0;
            ulong[] S = null;

            int i = 0, j = 0, m = 0, c = 0, t = 0, ii = 0, jj = 0;

            if ((rounds != RC5_16_ROUNDS) &&
                (rounds != RC5_12_ROUNDS) && (rounds != RC5_8_ROUNDS))
                rounds = RC5_16_ROUNDS;

            key.rounds = rounds;
            S = key.data;
            j = 0;
            for (i = 0; i <= (len - 8); i += 8) {
                CryptUtil.c2l(data, ref l, ref c2l_data_startIdx);
                L[j++] = l;
                CryptUtil.c2l(data, ref l, ref c2l_data_startIdx);
                L[j++] = l;
            }
            ii = len - i;
            if (ii != 0) {
                k = (ulong)(len & 0x07);
                CryptUtil.c2ln(data, ref l, ref ll, (long)k, ref c2l_data_startIdx);
                L[j + 0] = l;
                L[j + 1] = ll;
            }

            c = (len + 3) / 4;
            t = (rounds + 1) * 2;
            S[0] = RC5_32_P;
            for (i = 1; i < t; i++)
                S[i] = (S[i - 1] + RC5_32_Q) & RC5_32_MASK;

            j = (t > c) ? t : c;
            j *= 3;
            ii = jj = 0;
            A = B = 0;
            for (i = 0; i < j; i++) {
                k = (S[ii] + A + B) & RC5_32_MASK;
                A = S[ii] = ROTATE_l32(k, 3);
                m = (int)(A + B);
                k = (L[jj] + A + B) & RC5_32_MASK;
                B = L[jj] = ROTATE_l32(k, (ulong)m);
                if (++ii >= t)
                    ii = 0;
                if (++jj >= c)
                    jj = 0;
            }
        }

        public static void RC5_32_ecb_encrypt_one(byte[] inBytesTotal, uint startPos, uint inLen, ref byte[] outBytesTotal,
                RC5_32_KEY ks, int encrypt, CryptContext cryptContext)
        {
            int len_ = (((int)inLen + 7) / 8) * 8;
            outBytesTotal = new byte[len_];

            byte[] inBytes = new byte[8];
            byte[] outBytes = new byte[8];

            int idx = 0;
            for (idx = 0; idx <= inLen - 8; idx += 8)
            {
                // 保证 8 个字节
                CryptUtil.get8Byte(inBytesTotal, (int)startPos + idx, inBytes);
                RC5_32_ecb_encrypt(inBytes, outBytes, ks, encrypt, cryptContext);
                Array.Copy(outBytes, 0, outBytesTotal, idx, 8);
            }
        }
    }
}