using SDK.Lib;
using System.Text;

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
            //uint outSize = 0;

            byte[] encryptKey = Encoding.UTF8.GetBytes("aaaaaaaa");

            EncryptDecrypt.symmetry_Encode_Byte(inBytes, 0, inSize, ref outBytes, encryptKey);
        }
    }
}