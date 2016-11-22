namespace SDK.Lib
{
    // 加密数据上下文
    public class CryptContext
    {
        public CryptAlgorithm m_cryptAlgorithm = CryptAlgorithm.RC5;      // 当前是否需要加密解密
        public byte[] m_cryptKey = null;            // 秘钥
        public CryptKeyBase[] m_cryptKeyArr = new CryptKeyBase[(int)CryptAlgorithm.eTotal];

        public CryptContext()
        {
            m_cryptKeyArr[(int)CryptAlgorithm.RC5] = new RC5_32_KEY();
            m_cryptKeyArr[(int)CryptAlgorithm.DES] = new DES_key_schedule();
            RC5.RC5_32_set_key(m_cryptKeyArr[(int)CryptAlgorithm.RC5] as RC5_32_KEY, 16, Crypt.RC5_KEY, RC5.RC5_12_ROUNDS);     // 生成秘钥
        }

        public void setCryptKey(byte[] encrypt)
        {
            m_cryptAlgorithm = CryptAlgorithm.DES;
            m_cryptKey = encrypt;
            Dec.DES_set_key_unchecked(m_cryptKey, m_cryptKeyArr[(int)CryptAlgorithm.DES] as DES_key_schedule);
        }
    }

    public class Crypt
    {
        //默认密钥向量
        public static byte[] rgbIV = { 0x3f, 0x79, 0xd5, 0xe2, 0x4a, 0x8c, 0xb6, 0xc1, 0xaf, 0x31, 0x5e, 0xc7, 0xeb, 0x9d, 0x6e, 0xcb };
        public static byte[] RC5_KEY = { 0x3f, 0x79, 0xd5, 0xe2, 0x4a, 0x8c, 0xb6, 0xc1, 0xaf, 0x31, 0x5e, 0xc7, 0xeb, 0x9d, 0x6e, 0xcb };

        public static void encryptData(byte[] encryptByte, uint startPos, uint inLen, ref byte[] outBytes, CryptContext cryptContext)
        {
            if (CryptAlgorithm.RC5 == cryptContext.m_cryptAlgorithm)
            {
                Crypt.RC5_ECB_Symmetry_Encode_Byte(encryptByte, startPos, inLen, ref outBytes, cryptContext.m_cryptKeyArr[(int)cryptContext.m_cryptAlgorithm] as RC5_32_KEY, cryptContext);
            }
            else
            {
                Crypt.DES_ECB_Symmetry_Encode_OpenSSL(encryptByte, startPos, inLen, ref outBytes, cryptContext.m_cryptKeyArr[(int)cryptContext.m_cryptAlgorithm] as DES_key_schedule, cryptContext);
            }
        }

        public static void decryptData(byte[] decryptByte, uint startPos, uint inLen, ref byte[] outBytes, CryptContext cryptContext)
        {
            if (CryptAlgorithm.RC5 == cryptContext.m_cryptAlgorithm)
            {
                Crypt.RC5_ECB_Symmetry_Decode_Byte(decryptByte, startPos, inLen, ref outBytes, cryptContext.m_cryptKeyArr[(int)cryptContext.m_cryptAlgorithm] as RC5_32_KEY, cryptContext);
            }
            else
            {
                Crypt.DES_ECB_Symmetry_Decode_OpenSSL(decryptByte, startPos, inLen, ref outBytes, cryptContext.m_cryptKeyArr[(int)cryptContext.m_cryptAlgorithm] as DES_key_schedule, cryptContext);
            }
        }


        // rc5 加密后大小不会变化的
        static public bool RC5_ECB_Symmetry_Encode_Byte(byte[] encryptByte, uint startPos, uint inLen, ref byte[] outBytes, RC5_32_KEY rgbKey, CryptContext cryptContext)
        {
            try
            {
                RC5.RC5_32_ecb_encrypt_one(encryptByte, startPos, inLen, ref outBytes, rgbKey, RC5.RC5_ENCRYPT, cryptContext);
                return true;
            }
            catch
            {
                Ctx.mInstance.mLogSys.error("RC5_ECB_Symmetry_Encode_Byte error");
                return false;
            }
        }

        static public bool RC5_ECB_Symmetry_Decode_Byte(byte[] decryptByte, uint startPos, uint inLen, ref byte[] outBytes, RC5_32_KEY rgbKey, CryptContext cryptContext)
        {
            try
            {
                RC5.RC5_32_ecb_encrypt_one(decryptByte, startPos, inLen, ref outBytes, rgbKey, RC5.RC5_DECRYPT, cryptContext);

                return true;
            }
            catch
            {
                Ctx.mInstance.mLogSys.error("RC5_ECB_Symmetry_Decode_Byte error");
                return false;
            }
        }

        // rc5 加密后大小不会变化的
        static public bool DES_ECB_Symmetry_Encode_OpenSSL(byte[] encryptByte, uint startPos, uint inLen, ref byte[] outBytes, DES_key_schedule rgbKey, CryptContext cryptContext)
        {
            try
            {
                Dec.DES_ecb_encrypt_one(encryptByte, startPos, inLen, ref outBytes, rgbKey, RC5.RC5_ENCRYPT, cryptContext);
                return true;
            }
            catch
            {
                Ctx.mInstance.mLogSys.error("DES_ECB_Symmetry_Encode_OpenSSL error");
                return false;
            }
        }

        static public bool DES_ECB_Symmetry_Decode_OpenSSL(byte[] decryptByte, uint startPos, uint inLen, ref byte[] outBytes, DES_key_schedule rgbKey, CryptContext cryptContext)
        {
            try
            {
                Dec.DES_ecb_encrypt_one(decryptByte, startPos, inLen, ref outBytes, rgbKey, RC5.RC5_DECRYPT, cryptContext);

                return true;
            }
            catch
            {
                Ctx.mInstance.mLogSys.error("DES_ecb_encrypt_one error");
                return false;
            }
        }

#if DEPRECATE_CODE
        /// <summary>
        /// 对称加密法加密函数
        /// </summary>
        /// <param name="encryptString">待加密的字符串</param>
        /// <param name="encryptKey">加密密钥,要求为8位</param>
        /// <returns>加密成功返回加密后的字符串，失败返回源串</returns>
        static public string DES_CBC_Symmetry_Encode(string encryptString, string encryptKey)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
                //byte[] rgbIV = Keys;
                byte[] inputByteBuffer = Encoding.UTF8.GetBytes(encryptString);
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteBuffer, 0, inputByteBuffer.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
            catch
            {
                return encryptString;
            }
        }

        /// <summary>
        /// 对称加密法解密函数
        /// </summary>
        /// <param name="decryptString">待解密的字符串</param>
        /// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同</param>
        /// <returns>解密成功返回解密后的字符串，失败返源串</returns>
        static public string DES_CBC_Symmetry_Decode(string decryptString, uint inLen, string decryptKey)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey);
                //byte[] rgbIV = Keys;
                byte[] inputByteBuffer = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteBuffer, 0, inputByteBuffer.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                return decryptString;
            }
        }

        // DES 加密后大小不会变化的
        static public bool DES_ECB_Symmetry_Encode_Byte(byte[] encryptByte, uint startPos, uint inLen, ref byte[] outBytes, byte[] rgbKey)
        {
            try
            {
                //byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
                //byte[] rgbIV = Keys;
                //byte[] inputByteBuffer = encryptByte;
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                dCSP.Mode = CipherMode.ECB;
                dCSP.Padding = PaddingMode.Zeros;
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(encryptByte, (int)startPos, (int)inLen);
                cStream.FlushFinalBlock();
                outBytes = mStream.ToArray();

                mStream.Close();
                cStream.Close();

                return true;
            }
            catch
            {
                return false;
            }
        }

        static public bool DES_ECB_Symmetry_Decode_Byte(byte[] decryptByte, uint startPos, uint inLen, ref byte[] outBytes, byte[] rgbKey)
        {
            try
            {
                //byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey);
                //byte[] rgbIV = Keys;
                //byte[] inputByteBuffer = decryptByte;
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                DCSP.Mode = CipherMode.ECB;
                DCSP.Padding = PaddingMode.Zeros;
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(decryptByte, (int)startPos, (int)inLen);
                cStream.FlushFinalBlock();
                outBytes = mStream.ToArray();

                mStream.Close();
                cStream.Close();

                return true;
            }
            catch
            {
                return false;
            }
        }

        // rc2 加密后大小不会变化的
        static public bool RC2_CBC_Symmetry_Encode_Byte(byte[] encryptByte, uint startPos, uint inLen, ref byte[] outBytes, byte[] rgbKey)
        {
            try
            {
                RC2CryptoServiceProvider m_RC2Provider = new RC2CryptoServiceProvider();
                MemoryStream m_stream = new MemoryStream();
                CryptoStream m_cstream = new CryptoStream(m_stream, m_RC2Provider.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);

                m_cstream.Write(encryptByte, (int)startPos, (int)inLen);
                m_cstream.FlushFinalBlock();
                outBytes = m_stream.ToArray();
                m_stream.Close(); 
                m_stream.Dispose();

                m_cstream.Close(); 
                m_cstream.Dispose();
                return true;
            }
            catch
            {
                return false;
            }
        }

        static public bool RC2_CBC_Symmetry_Decode_Byte(byte[] decryptByte, uint startPos, uint inLen, ref byte[] outBytes, byte[] rgbKey)
        {
            try
            {
                RC2CryptoServiceProvider m_RC2Provider = new RC2CryptoServiceProvider();
                MemoryStream m_stream = new MemoryStream();
                CryptoStream m_cstream = new CryptoStream(m_stream, m_RC2Provider.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                m_cstream.Write(decryptByte, (int)startPos, (int)inLen);
                m_cstream.FlushFinalBlock();
                outBytes = m_stream.ToArray();

                m_stream.Close(); 
                m_stream.Dispose();

                m_cstream.Close(); 
                m_cstream.Dispose();

                return true;
            }
            catch
            {
                return false;
            }
        }
#endif
    }
}