using SDK.Common;
using SDK.Lib;
using System;
using System.Text;

namespace UnitTestSrc
{
    public class TestEncrypt
    {
        public void run()
        {
            //testCrypt();
            //testRc5();
            testDes();
        }

        protected void testCrypt()
        {
            string testStr = "asdfasdf";
            byte[] inBytes = System.Text.Encoding.UTF8.GetBytes(testStr);
            byte[] outBytes = null;
            uint inSize = (uint)inBytes.Length;
            //uint outSize = 0;

            byte[] encryptKey = Encoding.UTF8.GetBytes("aaaaaaaa");

            Crypt.DES_ECB_Symmetry_Encode_Byte(inBytes, 0, inSize, ref outBytes, encryptKey);
        }

        protected void testRc5()
        {
            string testStr = "asdfasdf5";
            //byte[] inBytes = System.Text.Encoding.UTF8.GetBytes(testStr);
            byte[] inBytes = { 0x65, 0xC1, 0x78, 0xB2, 0x84, 0xD1, 0x97, 0xCC };
            byte[] outBytes = new byte[8];
            uint inSize = (uint)inBytes.Length;

            RC5_32_KEY rc5Key = new RC5_32_KEY();     // RC5 key
            RC5.RC5_32_set_key(rc5Key, 16, Crypt.RC5_KEY, RC5.RC5_16_ROUNDS);     // 生成秘钥

            Crypt.encryptData(inBytes, 0, 8, ref outBytes, rc5Key, CryptAlgorithm.RC5);
            Crypt.decryptData(outBytes, 0, 8, ref inBytes, rc5Key, CryptAlgorithm.RC5);
            testStr = System.Text.Encoding.Default.GetString(inBytes);
        }

        protected void testDes()
        {
            string testStr = "asdfasdf5";
            //byte[] inBytes = System.Text.Encoding.UTF8.GetBytes(testStr);
            byte[] key = { 0x65, 0xC1, 0x78, 0xB2, 0x84, 0xD1, 0x97, 0xCC };
            byte[] inBytes = { 0x3f, 0x79, 0xd5, 0xe2, 0x4a, 0x8c, 0xb6, 0xc1 };
            byte[] outBytes = new byte[8];
            uint inSize = (uint)inBytes.Length;

            DES_key_schedule des5Key = new DES_key_schedule();     // RC5 key
            Dec.DES_set_key_unchecked(key, des5Key);

            Crypt.encryptData(inBytes, 0, 8, ref outBytes, des5Key, CryptAlgorithm.DES);
            Crypt.decryptData(outBytes, 0, 8, ref inBytes, des5Key, CryptAlgorithm.DES);
            testStr = System.Text.Encoding.Default.GetString(inBytes);
        }
    }
}