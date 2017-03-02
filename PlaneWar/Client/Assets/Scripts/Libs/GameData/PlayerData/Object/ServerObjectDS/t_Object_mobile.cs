namespace SDK.Lib
{
    public class t_Object_mobile
    {
        public uint dwThisID;
        public uint dwObjectID;
        public stObjectLocation pos;
        public uint dwNum;

        public void derialize(ByteBuffer bu)
        {
            bu.readUnsignedInt32(ref dwThisID);
            bu.readUnsignedInt32(ref dwObjectID);
            pos = new stObjectLocation();
            pos.derialize(bu);
            bu.readUnsignedInt32(ref dwNum);
        }
    }
}


/**
 * \brief   手游 服务器和客户端通信道具数据
*/
//typedef struct t_Object_mobile
//{
//    DWORD dwThisID;   //物品唯一id
//    DWORD dwObjectID;  //物品表中的编号
//    stObjectLocation pos;	// 位置
//    DWORD dwNum;	// 数量
//};