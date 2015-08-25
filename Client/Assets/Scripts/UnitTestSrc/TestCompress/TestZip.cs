using Game.Msg;
using SDK.Lib;
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
            //uint outSize = 0;

            ByteBuffer ba = new ByteBuffer();
            stUserRequestLoginCmd cmd = new stUserRequestLoginCmd();
            cmd.serialize(ba);
            //Compress.CompressByteZip(ba.dynBuff.buff, 0, (uint)ba.length, ref outBytes, ref outSize);

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

            ByteBuffer pByteBuffer = new ByteBuffer();
            UnitTestStrCmd pUnitTestCmd = new UnitTestStrCmd();

            // 发送第一个数据包
            pUnitTestCmd.testStr = "测试数据";
            pByteBuffer.clear();
            pUnitTestCmd.serialize(pByteBuffer);

            Compress.CompressData(pByteBuffer.dynBuff.buff, 0, pByteBuffer.length, ref outBytes, ref outSize);
            Compress.DecompressData(outBytes, 0, outSize, ref inBytes, ref inSize);

            pByteBuffer.clear();
            pByteBuffer.writeBytes(inBytes, 0, inSize);
            pByteBuffer.position = 0;
            pUnitTestCmd.derialize(pByteBuffer);

            UAssert.DebugAssert(pUnitTestCmd.testStr != "测试数据");
        }
    }
}