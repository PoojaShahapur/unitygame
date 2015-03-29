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
            UnitTestStrCmd pUnitTestCmd = new UnitTestStrCmd();
            UnitTestNumCmd pUnitTesNumtCmd = new UnitTestNumCmd();

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

            // 发送第三个数据包
            pUnitTesNumtCmd = new UnitTestNumCmd();
            pUnitTesNumtCmd.num = 2001;
            pDataBuffer.sendData.clear();
            pUnitTesNumtCmd.serialize(pDataBuffer.sendData);
            pDataBuffer.send();

            pDataBuffer.getSendData();
            ByteBuffer cryptLenBA = new ByteBuffer();
            cryptLenBA.writeUnsignedInt32(pDataBuffer.sendBuffer.length);
            pDataBuffer.rawBuffer.circuleBuffer.pushBackBA(cryptLenBA);                     // 自己补上消息头
            pDataBuffer.rawBuffer.circuleBuffer.pushBackBA(pDataBuffer.sendBuffer);         // 直接放到接收原始消息缓冲区
            pDataBuffer.moveRaw2Msg();

            ByteBuffer ba;
            ba = pDataBuffer.getMsg();
            UAssert.DebugAssert(ba != null);
            pUnitTestCmd.derialize(ba);
            UAssert.DebugAssert(pUnitTestCmd.testStr.Substring(0, 4) == "测试数据");

            pDataBuffer.getSendData();
            if (pDataBuffer.sendBuffer.length > 0)
            {
                cryptLenBA.clear();
                cryptLenBA.writeUnsignedInt32(pDataBuffer.sendBuffer.length);
                pDataBuffer.rawBuffer.circuleBuffer.pushBackBA(cryptLenBA);                     // 自己补上消息头
                pDataBuffer.rawBuffer.circuleBuffer.pushBackBA(pDataBuffer.sendBuffer);         // 直接放到接收原始消息缓冲区
                pDataBuffer.moveRaw2Msg();
            }

            ba = pDataBuffer.getMsg();
            UAssert.DebugAssert(ba != null);
            pUnitTestCmd.derialize(ba);
            UAssert.DebugAssert(pUnitTestCmd.testStr.Substring(0, 4) == "成功返回");

            pDataBuffer.getSendData();
            if (pDataBuffer.sendBuffer.length > 0)
            {
                cryptLenBA.clear();
                cryptLenBA.writeUnsignedInt32(pDataBuffer.sendBuffer.length);
                pDataBuffer.rawBuffer.circuleBuffer.pushBackBA(cryptLenBA);                     // 自己补上消息头
                pDataBuffer.rawBuffer.circuleBuffer.pushBackBA(pDataBuffer.sendBuffer);         // 直接放到接收原始消息缓冲区
                pDataBuffer.moveRaw2Msg();
            }

            ba = pDataBuffer.getMsg();
            UAssert.DebugAssert(ba != null);
            pUnitTesNumtCmd.derialize(ba);
            UAssert.DebugAssert(pUnitTesNumtCmd.num == 2001);
        }

        protected void testBA()
        {
            string str = "测试数据";
            ByteBuffer ba = new ByteBuffer();
            ba.writeMultiByte(str, GkEncode.UTF8, 24);
            ba.position = 0;
            string ret = "";
            ba.readMultiByte(ref ret, 24, GkEncode.UTF8);
        }
    }
}