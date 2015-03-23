using SDK.Common;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SDK.Lib
{
    public class Crypt
    {
        //默认密钥向量
        public static byte[] rgbIV = { 0x3f, 0x79, 0xd5, 0xe2, 0x4a, 0x8c, 0xb6, 0xc1, 0xaf, 0x31, 0x5e, 0xc7, 0xeb, 0x9d, 0x6e, 0xcb };
        public static byte[] RC5_KEY = { 0x3f, 0x79, 0xd5, 0xe2, 0x4a, 0x8c, 0xb6, 0xc1, 0xaf, 0x31, 0x5e, 0xc7, 0xeb, 0x9d, 0x6e, 0xcb };

        public static void encryptData(byte[] encryptByte, uint startPos, uint inLen, ref byte[] outBytes, CryptKeyBase rgbKey, CryptAlgorithm algorithm = CryptAlgorithm.RC5)
        {
            if(CryptAlgorithm.RC5 == algorithm)
            {
                Crypt.RC5_ECB_Symmetry_Encode_Byte(encryptByte, startPos, inLen, ref outBytes, rgbKey as RC5_32_KEY);
            }
            else
            {
                Crypt.DES_ECB_Symmetry_Encode_OpenSSL(encryptByte, startPos, inLen, ref outBytes, rgbKey as DES_key_schedule);
            }
        }

        public static void decryptData(byte[] decryptByte, uint startPos, uint inLen, ref byte[] outBytes, CryptKeyBase rgbKey, CryptAlgorithm algorithm = CryptAlgorithm.RC5)
        {
            if (CryptAlgorithm.RC5 == algorithm)
            {
                Crypt.RC5_ECB_Symmetry_Decode_Byte(decryptByte, startPos, inLen, ref outBytes, rgbKey as RC5_32_KEY);
            }
            else
            {
                Crypt.DES_ECB_Symmetry_Decode_OpenSSL(decryptByte, startPos, inLen, ref outBytes, rgbKey as DES_key_schedule);
            }
        }

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
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
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
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
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
                //byte[] inputByteArray = encryptByte;
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
                //byte[] inputByteArray = decryptByte;
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

        // rc5 加密后大小不会变化的
        static public bool DES_ECB_Symmetry_Encode_OpenSSL(byte[] encryptByte, uint startPos, uint inLen, ref byte[] outBytes, DES_key_schedule rgbKey)
        {
            try
            {
                Dec.DES_ecb_encrypt_one(encryptByte, startPos, inLen, ref outBytes, rgbKey, RC5.RC5_ENCRYPT);
                return true;
            }
            catch
            {
                return false;
            }
        }

        static public bool DES_ECB_Symmetry_Decode_OpenSSL(byte[] decryptByte, uint startPos, uint inLen, ref byte[] outBytes, DES_key_schedule rgbKey)
        {
            try
            {
                Dec.DES_ecb_encrypt_one(decryptByte, startPos, inLen, ref outBytes, rgbKey, RC5.RC5_DECRYPT);

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

        // rc5 加密后大小不会变化的
        static public bool RC5_ECB_Symmetry_Encode_Byte(byte[] encryptByte, uint startPos, uint inLen, ref byte[] outBytes, RC5_32_KEY rgbKey)
        {
            try
            {
                RC5.RC5_32_ecb_encrypt_one(encryptByte, startPos, inLen, ref outBytes, rgbKey, RC5.RC5_ENCRYPT);
                return true;
            }
            catch
            {
                return false;
            }
        }

        static public bool RC5_ECB_Symmetry_Decode_Byte(byte[] decryptByte, uint startPos, uint inLen, ref byte[] outBytes, RC5_32_KEY rgbKey)
        {
            try
            {
                RC5.RC5_32_ecb_encrypt_one(decryptByte, startPos, inLen, ref outBytes, rgbKey, RC5.RC5_DECRYPT);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}