using SDK.Common;
using SDK.Lib;

namespace UnitTestSrc
{
    public class UnitTestBuffer
    {
        public void run()
        {
            testMsgBuffer();
            //testBA();
        }

        protected void testMsgBuffer()
        {
            DataBuffer pDataBuffer = new DataBuffer();
            UnitTestCmd pUnitTestCmd = new UnitTestCmd();

            // 发送第一个数据包
            pUnitTestCmd.testStr = "测试数据";
            pDataBuffer.sendData.clear();
            pUnitTestCmd.serialize(pDataBuffer.sendData);
            //pDataBuffer.sendData.position = 0;
            //pUnitTestCmd.derialize(pDataBuffer.sendData);
            pDataBuffer.send();

            // 发送第二个数据包
            pUnitTestCmd.testStr = "成功返回";
            pDataBuffer.sendData.clear();
            pUnitTestCmd.serialize(pDataBuffer.sendData);
            pDataBuffer.send();

            pDataBuffer.getSendData();

            pDataBuffer.rawBuffer.pushBackBA(pDataBuffer.sendBuffer);         // 直接放到接收原始消息缓冲区
            pDataBuffer.moveRaw2Msg();

            ByteArray ba;
            ba = pDataBuffer.getMsg();
            UAssert.DebugAssert(ba != null);
            pUnitTestCmd.derialize(ba);
            UAssert.DebugAssert(pUnitTestCmd.testStr.Substring(0, 4) == "测试数据");

            ba = pDataBuffer.getMsg();
            UAssert.DebugAssert(ba != null);
            pUnitTestCmd.derialize(ba);
            UAssert.DebugAssert(pUnitTestCmd.testStr.Substring(0, 4) == "成功返回");
        }

        protected void testBA()
        {
            string str = "测试数据";
            ByteArray ba = new ByteArray();
            ba.writeMultiByte(str, GkEncode.UTF8, 24);
            ba.position = 0;
            string ret = ba.readMultiByte(24, GkEncode.UTF8);
        }
    }
}