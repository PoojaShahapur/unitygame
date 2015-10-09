using Game.Game;
using Game.Msg;
using SDK.Lib;
using System;

namespace UnitTestSrc
{
    public class UnitTestBuffer
    {
        public void run()
        {
            testMsgBuffer();
            //testBA();
            //testSend();
            //testReadMsgFormHex();
            //testReceiveMsg();
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

            pDataBuffer.getSocketSendData();
            ByteBuffer cryptLenBA = new ByteBuffer();
            cryptLenBA.writeUnsignedInt32(pDataBuffer.sendBuffer.length);
            pDataBuffer.rawBuffer.circularBuffer.pushBackBA(cryptLenBA);                     // 自己补上消息头
            pDataBuffer.rawBuffer.circularBuffer.pushBackBA(pDataBuffer.sendBuffer);         // 直接放到接收原始消息缓冲区
            pDataBuffer.moveRaw2Msg();

            ByteBuffer bu;
            bu = pDataBuffer.getMsg();
            UAssert.DebugAssert(bu != null);
            pUnitTestCmd.derialize(bu);
            UAssert.DebugAssert(pUnitTestCmd.testStr.Substring(0, 4) == "测试数据");

            pDataBuffer.getSocketSendData();
            if (pDataBuffer.sendBuffer.length > 0)
            {
                cryptLenBA.clear();
                cryptLenBA.writeUnsignedInt32(pDataBuffer.sendBuffer.length);
                pDataBuffer.rawBuffer.circularBuffer.pushBackBA(cryptLenBA);                     // 自己补上消息头
                pDataBuffer.rawBuffer.circularBuffer.pushBackBA(pDataBuffer.sendBuffer);         // 直接放到接收原始消息缓冲区
                pDataBuffer.moveRaw2Msg();
            }

            bu = pDataBuffer.getMsg();
            UAssert.DebugAssert(bu != null);
            pUnitTestCmd.derialize(bu);
            UAssert.DebugAssert(pUnitTestCmd.testStr.Substring(0, 4) == "成功返回");

            pDataBuffer.getSocketSendData();
            if (pDataBuffer.sendBuffer.length > 0)
            {
                cryptLenBA.clear();
                cryptLenBA.writeUnsignedInt32(pDataBuffer.sendBuffer.length);
                pDataBuffer.rawBuffer.circularBuffer.pushBackBA(cryptLenBA);                     // 自己补上消息头
                pDataBuffer.rawBuffer.circularBuffer.pushBackBA(pDataBuffer.sendBuffer);         // 直接放到接收原始消息缓冲区
                pDataBuffer.moveRaw2Msg();
            }

            bu = pDataBuffer.getMsg();
            UAssert.DebugAssert(bu != null);
            pUnitTesNumtCmd.derialize(bu);
            UAssert.DebugAssert(pUnitTesNumtCmd.num == 2001);

            Ctx.m_instance.m_netDispList.clearOneRevMsg();
            Ctx.m_instance.m_netDispList.clearOneHandleMsg();
        }

        protected void testBA()
        {
            string str = "测试数据";
            ByteBuffer bu = new ByteBuffer();
            bu.writeMultiByte(str, GkEncode.UTF8, 24);
            bu.position = 0;
            string ret = "";
            bu.readMultiByte(ref ret, 24, GkEncode.UTF8);
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
            byte[] key = new byte[8] { 100, 206, 174, 233, 122, 55, 242, 234 };

            byte[] hexMsg = new byte[1024] { 163, 8, 10, 98, 208, 112, 52, 233, 120, 50, 123, 78, 7, 32, 74, 41, 175, 80, 167, 210, 64, 138, 173, 151, 229, 83, 86, 134, 19, 14, 76, 25, 11, 219, 22, 62, 244, 195, 168, 132, 110, 135, 20, 97, 80, 79, 52, 173, 185, 178, 247, 164, 133, 215, 146, 139, 227, 118, 138, 110, 215, 162, 195, 71, 20, 103, 221, 238, 23, 90, 254, 152, 165, 244, 203, 51, 90, 37, 162, 104, 198, 103, 93, 125, 62, 121, 248, 163, 85, 106, 214, 66, 22, 123, 230, 108, 13, 137, 74, 165, 181, 25, 73, 133, 149, 215, 151, 214, 13, 134, 237, 160, 184, 241, 6, 199, 4, 183, 183, 165, 210, 166, 4, 222, 210, 56, 22, 86, 3, 140, 98, 71, 242, 249, 41, 168, 220, 188, 107, 11, 74, 58, 6, 145, 46, 179, 23, 136, 175, 86, 195, 231, 219, 93, 187, 155, 41, 200, 229, 74, 217, 117, 187, 253, 235, 183, 222, 119, 245, 78, 67, 154, 5, 114, 101, 162, 242, 93, 78, 141, 68, 27, 219, 210, 209, 11, 212, 132, 133, 70, 51, 208, 233, 200, 29, 162, 237, 32, 147, 40, 64, 30, 186, 135, 96, 82, 160, 12, 187, 123, 148, 233, 245, 92, 208, 181, 167, 66, 252, 48, 224, 17, 239, 145, 87, 2, 175, 185, 96, 177, 203, 7, 122, 40, 30, 124, 245, 74, 182, 6, 171, 148, 146, 36, 105, 154, 108, 246, 96, 112, 189, 24, 45, 11, 231, 214, 127, 119, 229, 209, 170, 248, 163, 164, 253, 71, 81, 79, 22, 167, 233, 148, 145, 59, 54, 100, 165, 216, 68, 187, 34, 51, 38, 131, 87, 252, 11, 216, 208, 171, 218, 25, 186, 22, 41, 26, 135, 133, 107, 132, 170, 208, 238, 116, 135, 123, 50, 77, 153, 27, 204, 61, 172, 43, 169, 204, 45, 171, 97, 113, 213, 63, 66, 41, 174, 126, 143, 155, 113, 50, 211, 169, 147, 38, 54, 83, 186, 103, 80, 190, 31, 150, 167, 17, 45, 177, 180, 84, 242, 81, 96, 159, 172, 169, 131, 187, 102, 141, 130, 51, 148, 125, 193, 74, 59, 36, 218, 167, 4, 61, 109, 250, 190, 24, 233, 166, 42, 100, 61, 204, 254, 186, 11, 144, 180, 84, 178, 44, 188, 228, 12, 68, 182, 142, 30, 85, 226, 30, 236, 190, 169, 116, 65, 37, 103, 233, 240, 103, 139, 126, 70, 79, 172, 16, 231, 118, 60, 191, 19, 93, 241, 254, 174, 87, 183, 182, 226, 40, 202, 252, 196, 169, 145, 135, 120, 248, 109, 241, 42, 40, 127, 119, 229, 209, 170, 248, 163, 164, 84, 252, 57, 201, 173, 113, 174, 145, 74, 185, 137, 165, 86, 21, 41, 150, 34, 51, 38, 131, 87, 252, 11, 216, 208, 171, 218, 25, 186, 22, 41, 26, 135, 133, 107, 132, 170, 208, 238, 116, 255, 5, 146, 248, 69, 247, 93, 246, 141, 49, 126, 191, 237, 144, 26, 147, 104, 97, 16, 63, 184, 194, 66, 67, 153, 105, 27, 15, 145, 243, 211, 189, 45, 239, 237, 197, 226, 190, 152, 249, 212, 179, 18, 162, 115, 157, 8, 225, 75, 47, 118, 107, 15, 244, 38, 35, 221, 26, 116, 82, 105, 202, 33, 254, 198, 64, 244, 141, 78, 54, 3, 165, 179, 67, 161, 165, 224, 31, 199, 145, 70, 175, 74, 96, 45, 31, 154, 190, 10, 123, 119, 253, 122, 178, 219, 176, 255, 65, 178, 137, 12, 227, 60, 26, 195, 219, 56, 190, 155, 120, 191, 213, 180, 42, 251, 118, 30, 196, 143, 165, 19, 30, 98, 234, 205, 43, 8, 125, 199, 221, 48, 122, 222, 201, 181, 250, 0, 23, 136, 245, 207, 68, 106, 223, 38, 254, 8, 209, 237, 94, 5, 118, 8, 238, 241, 82, 91, 173, 202, 1, 54, 192, 219, 74, 82, 54, 88, 159, 74, 89, 69, 112, 209, 34, 106, 176, 43, 18, 34, 81, 99, 252, 61, 100, 117, 86, 170, 10, 76, 79, 249, 120, 104, 97, 16, 63, 184, 194, 66, 67, 153, 105, 27, 15, 145, 243, 211, 189, 45, 239, 237, 197, 226, 190, 152, 249, 212, 179, 18, 162, 115, 157, 8, 225, 26, 54, 6, 94, 187, 145, 31, 44, 83, 44, 4, 112, 169, 162, 0, 75, 104, 232, 249, 81, 148, 46, 49, 109, 208, 144, 34, 60, 244, 183, 152, 100, 252, 204, 163, 242, 88, 170, 80, 129, 227, 118, 138, 110, 215, 162, 195, 71, 252, 236, 202, 102, 29, 212, 59, 65, 111, 0, 91, 220, 33, 116, 220, 73, 49, 197, 208, 179, 230, 6, 36, 64, 85, 106, 214, 66, 22, 123, 230, 108, 13, 137, 74, 165, 181, 25, 73, 133, 149, 215, 151, 214, 13, 134, 237, 160, 184, 241, 6, 199, 4, 183, 183, 165, 202, 96, 248, 139, 52, 161, 198, 183, 53, 110, 173, 134, 45, 230, 145, 107, 223, 143, 139, 236, 115, 107, 116, 126, 66, 73, 195, 36, 108, 3, 51, 191, 168, 28, 68, 192, 51, 88, 247, 103, 93, 222, 189, 83, 239, 228, 89, 112, 138, 223, 6, 49, 20, 211, 82, 88, 63, 104, 132, 123, 26, 2, 11, 117, 48, 27, 56, 32, 144, 120, 27, 240, 35, 16, 24, 122, 226, 196, 156, 185, 116, 35, 16, 150, 205, 13, 180, 44, 151, 114, 205, 153, 67, 75, 83, 246, 178, 223, 227, 203, 30, 196, 1, 51, 20, 161, 147, 179, 161, 34, 151, 85, 32, 77, 35, 144, 220, 25, 229, 1, 127, 119, 229, 209, 170, 248, 163, 164, 214, 11, 204, 77, 16, 149, 134, 204, 151, 51, 73, 58, 80, 254, 8, 211, 85, 224, 182, 90, 166, 189, 64, 134, 191, 9, 74, 237, 13, 160, 72, 71, 203, 240, 82, 20, 221, 190, 26, 65, 51, 71, 225, 235, 150, 218, 229, 109, 107, 20, 49, 147, 154, 100, 78, 14, 127, 119, 229, 209, 170, 248, 163, 164, 253, 71, 81, 79, 22, 167, 233, 148, 145, 59, 54, 100, 165, 216, 68, 187 };

#if NET_MULTHREAD
            pDataBuffer.setCryptKey(key);
#endif
            pDataBuffer.dynBuff.size = (uint)hexMsg.Length;
            Array.Copy(hexMsg, 0, pDataBuffer.dynBuff.buff, 0, hexMsg.Length);
            pDataBuffer.moveDyn2Raw();
            pDataBuffer.moveRaw2Msg();

            GameNetHandleCB gameNetHandleCB = new GameNetHandleCB();
            Ctx.m_instance.m_netDispList.addOneDisp(gameNetHandleCB);

            ByteBuffer buff;
            stNullUserCmd cmd = new stNullUserCmd();
            while((buff = pDataBuffer.getMsg()) != null)
            {
                if (null != Ctx.m_instance.m_netDispList)
                {
                    //Ctx.m_instance.m_netDispList.handleMsg(buff);
                    cmd.derialize(buff);
                }
            }

            Ctx.m_instance.m_netDispList.removeOneDisp(gameNetHandleCB);
            Ctx.m_instance.m_netDispList.clearOneRevMsg();
            Ctx.m_instance.m_netDispList.clearOneHandleMsg();   
        }

        protected void testReceiveMsg()
        {
            ClientBuffer pDataBuffer = new ClientBuffer();
            stAddBattleCardPropertyUserCmd pCmd = new stAddBattleCardPropertyUserCmd();

            for (int idx = 0; idx < 6; ++idx)
            {
                Ctx.m_instance.m_logSys.log(string.Format("接收缓冲区测试索引 {0}", idx));

                pDataBuffer.sendData.clear();
                pCmd.serialize(pDataBuffer.sendData);
                pDataBuffer.send();
                pDataBuffer.getSocketSendData();
                pDataBuffer.dynBuff.size = pDataBuffer.sendBuffer.length;
                Array.Copy(pDataBuffer.sendBuffer.dynBuff.buff, 0, pDataBuffer.dynBuff.buff, 0, pDataBuffer.sendBuffer.length);
                pDataBuffer.moveDyn2Raw();
                pDataBuffer.moveRaw2Msg();
            }

            Ctx.m_instance.m_logSys.log(string.Format("接收缓冲区测试索引 {0}", 6));

            stRetRemoveBattleCardUserCmd pCmd_1 = new stRetRemoveBattleCardUserCmd();

            pDataBuffer.sendData.clear();
            pCmd_1.serialize(pDataBuffer.sendData);
            pDataBuffer.send();
            pDataBuffer.getSocketSendData();
            pDataBuffer.dynBuff.size = pDataBuffer.sendBuffer.length;
            Array.Copy(pDataBuffer.sendBuffer.dynBuff.buff, 0, pDataBuffer.dynBuff.buff, 0, pDataBuffer.sendBuffer.length);
            pDataBuffer.moveDyn2Raw();
            pDataBuffer.moveRaw2Msg();

            for (int idx = 7; idx < 13; ++idx)
            {
                Ctx.m_instance.m_logSys.log(string.Format("接收缓冲区测试索引 {0}", idx));

                pDataBuffer.sendData.clear();
                pCmd.serialize(pDataBuffer.sendData);
                pDataBuffer.send();
                pDataBuffer.getSocketSendData();
                pDataBuffer.dynBuff.size = pDataBuffer.sendBuffer.length;
                Array.Copy(pDataBuffer.sendBuffer.dynBuff.buff, 0, pDataBuffer.dynBuff.buff, 0, pDataBuffer.sendBuffer.length);
                pDataBuffer.moveDyn2Raw();
                pDataBuffer.moveRaw2Msg();
            }

            Ctx.m_instance.m_netDispList.clearOneRevMsg();
            Ctx.m_instance.m_netDispList.clearOneHandleMsg();
        }
    }
}