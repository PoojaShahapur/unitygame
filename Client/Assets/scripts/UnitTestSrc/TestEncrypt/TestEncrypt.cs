using SDK.Lib;

namespace UnitTestSrc
{
    public class TestEncrypt
    {
        public void run()
        {
            testEncrypt();
        }

        protected void testEncrypt()
        {
            string testStr = "asdfasdf";
            byte[] inBytes = System.Text.Encoding.UTF8.GetBytes(testStr);
            byte[] outBytes = null;
            uint inSize = (uint)inBytes.Length;
            uint outSize = 0;

            string encryptKey = "aaaaaaaa";

            EncryptDecrypt.symmetry_Encode_Byte(inBytes, inSize, ref outBytes, encryptKey);
        }
    }
}