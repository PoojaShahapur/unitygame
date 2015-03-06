using SDK.Common;
using SDK.Lib;

namespace UnitTestSrc
{
    public class UnitTestBuffer
    {
        public void run()
        {
            DataBuffer pDataBuffer = new DataBuffer();
            UnitTestCmd pUnitTestCmd = new UnitTestCmd();

            // ���͵�һ�����ݰ�
            pUnitTestCmd.testStr = "��������";
            pDataBuffer.sendData.clear();
            pUnitTestCmd.serialize(pDataBuffer.sendData);
            pDataBuffer.send();

            // ���͵ڶ������ݰ�
            pUnitTestCmd.testStr = "�ɹ�����";
            pDataBuffer.sendData.clear();
            pUnitTestCmd.serialize(pDataBuffer.sendData);
            pDataBuffer.send();

            pDataBuffer.getSendData();

            pDataBuffer.rawBuffer.pushBackBA(pDataBuffer.sendBuffer);         // ֱ�ӷŵ�����ԭʼ��Ϣ������
            pDataBuffer.moveRaw2Msg();

            ByteArray ba;
            ba = pDataBuffer.getMsg();
            UAssert.DebugAssert(ba != null);
            pUnitTestCmd.derialize(ba);
            UAssert.DebugAssert(pUnitTestCmd.testStr != "��������");

            ba = pDataBuffer.getMsg();
            UAssert.DebugAssert(ba != null);
            pUnitTestCmd.derialize(ba);
            UAssert.DebugAssert(pUnitTestCmd.testStr != "�ɹ�����");
        }
    }
}