using Game.Msg;
using SDK.Common;
using SDK.Lib;
using System;

namespace UnitTestSrc
{
    public class UnitTestBuffer
    {
        public void run()
        {
            //testMsgBuffer();
            //testBA();
            //testSend();
            testReadMsgFormHex();
        }

        protected void testMsgBuffer()
        {
            ClientBuffer pDataBuffer = new ClientBuffer();
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

        protected void testSend()
        {
            ClientBuffer pDataBuffer = new ClientBuffer();
            stUseObjectPropertyUserCmd pCmd = new stUseObjectPropertyUserCmd();

            for (int idx = 0; idx < 500; ++idx)
            {
                Ctx.m_instance.m_logSys.log(string.Format("发送缓冲区测试索引 {0}", idx));
                pDataBuffer.sendData.clear();
                pCmd.serialize(pDataBuffer.sendData);
                pDataBuffer.send();
            }
        }

        // 从十六进制读取数据，看消息内容
        protected void testReadMsgFormHex()
        {
            ClientBuffer pDataBuffer = new ClientBuffer();
            byte[] key = new byte[8] {52, 121, 220, 22, 25, 97, 76, 167};

            byte[] hexMsg = new byte[128] { 0x46, 0x44, 0xb6, 0x4e, 0xda, 0xf6, 0x42, 0xbb, 0xd3, 0x91, 0xa8, 0xad, 0x80, 0x0b, 0x79, 0x3c, 0x91, 0x8c, 0xed, 0xb3, 0x0d, 0xb0, 0xa1, 0x38, 0x22, 0x6b, 0xa3, 0x41, 0xf7, 0x3d, 0xbe, 0xb3, 0xf2, 0x45, 0x71, 0xd2, 0x01, 0x96, 0x77, 0xa4, 0x1c, 0x95, 0xc2, 0x56, 0x95, 0xfd, 0x8e, 0xd6, 0x51, 0xe0, 0x9a, 0xb8, 0xb9, 0x1c, 0x9b, 0xe8, 0x02, 0x74, 0xd8, 0x3a, 0x3c, 0xbc, 0x4d, 0x9b, 0xba, 0x9e, 0x1e, 0x64, 0xf7, 0xe2, 0x55, 0x6d, 0x8b, 0x5c, 0x69, 0x71, 0x30, 0x7a, 0xe4, 0xd8, 0xe4, 0xef, 0xc2, 0xf8, 0x1a, 0xa5, 0x2f, 0x9a, 0x70, 0xd4, 0xce, 0x9b, 0x9b, 0x3b, 0xa1, 0xa1, 0x5a, 0x72, 0x4a, 0xc2, 0xb1, 0x6f, 0xa4, 0xbc, 0xa3, 0xb3, 0x0d, 0x54, 0x42, 0x45, 0xa7, 0x8f, 0x98, 0x39, 0xde, 0x43, 0x73, 0xb3, 0x13, 0xcd, 0x74, 0xca, 0xed, 0xf2, 0xfb, 0x91, 0x64, 0x49 };

            pDataBuffer.setCryptKey(key);
            pDataBuffer.dynBuff.size = (uint)hexMsg.Length;
            Array.Copy(hexMsg, 0, pDataBuffer.dynBuff.buff, 0, hexMsg.Length);
            pDataBuffer.moveDyn2Raw();
            pDataBuffer.moveRaw2Msg();

            ByteBuffer buff;
            while((buff = pDataBuffer.getMsg()) != null)
            {
                if (null != Ctx.m_instance.m_netDispList && false == Ctx.m_instance.m_bStopNetHandle)
                {
                    Ctx.m_instance.m_netDispList.handleMsg(buff);
                }
            }
        }
    }
}