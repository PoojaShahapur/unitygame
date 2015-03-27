using Game.Msg;
using SDK.Common;
using SDK.Lib;
using System.IO;

namespace UnitTestSrc
{
    public class TestZip
    {
        public void run()
        {
            //testCompress();
            //testCompressMsg();
            testZipMsg();
        }

        protected void testCompress()
        {
            //string testStr = "利用进行字符串的压缩和解压缩利用进行字符串的压缩和解压缩";
            string testStr = "asdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdasfasdfasdfasdf";
            byte[] inBytes = System.Text.Encoding.UTF8.GetBytes(testStr);
            byte[] outBytes = null;
            uint inSize = 0;
            uint outSize = 0;

            Compress.CompressData(inBytes, 0, (uint)inBytes.Length, ref outBytes, ref outSize);

            writeFile("e:\\log.zip", outBytes);

            Compress.DecompressData(outBytes, 0, outSize, ref inBytes, ref inSize);

            string str = System.Text.Encoding.UTF8.GetString(inBytes);

            UAssert.DebugAssert(str == testStr);
        }

        protected void testCompressMsg()
        {
            byte[] outBytes = null;
            uint outSize = 0;

            ByteArray ba = new ByteArray();
            stUserRequestLoginCmd cmd = new stUserRequestLoginCmd();
            cmd.serialize(ba);
            Compress.CompressByteZip(ba.dynBuff.buff, 0, (uint)ba.length, ref outBytes, ref outSize);

            writeFile("e:\\log.zip", outBytes);
        }

        protected void writeFile(string fileName, byte[] bytes)
        {
            FileStream fileStream = new FileStream(fileName, FileMode.Create);
            fileStream.Write(bytes, 0, (int)bytes.Length);
            fileStream.Close();
        }

        public void testZipMsg()
        {
            byte[] outBytes = null;
            uint outSize = 0;

            byte[] inBytes = null;
            uint inSize = 0;

            ByteArray pByteArray = new ByteArray();
            UnitTestStrCmd pUnitTestCmd = new UnitTestStrCmd();

            // 发送第一个数据包
            pUnitTestCmd.testStr = "测试数据";
            pByteArray.clear();
            pUnitTestCmd.serialize(pByteArray);

            Compress.CompressData(pByteArray.dynBuff.buff, 0, pByteArray.length, ref outBytes, ref outSize);
            Compress.DecompressData(outBytes, 0, outSize, ref inBytes, ref inSize);

            pByteArray.clear();
            pByteArray.writeBytes(inBytes, 0, inSize);
            pByteArray.position = 0;
            pUnitTestCmd.derialize(pByteArray);

            UAssert.DebugAssert(pUnitTestCmd.testStr != "测试数据");
        }
    }
}