namespace SDK.Lib
{
    public class t_group_list
    {
        public uint index;              // 卡牌唯一 ID
        public uint occupation;         // 职业
        public uint cardNum;             // 套牌中卡牌的数量
        public string name;             // 卡牌名字

        public void derialize(ByteBuffer bu)
        {
            bu.readUnsignedInt32(ref index);
            bu.readUnsignedInt32(ref occupation);
            bu.readUnsignedInt32(ref cardNum);
            bu.readMultiByte(ref name, ProtoCV.MAX_NAMESIZE + 1, GkEncode.UTF8);
        }

        public void copyFrom(t_group_list rhv)
        {
            index = rhv.index;
            occupation = rhv.occupation;
            cardNum = rhv.cardNum;
            name = rhv.name;
        }
    }

    //struct t_group_list
    //{
    //DWORD index;
    //DWORD occupation;
    //char name[MAX_NAMESIZE+1];
    //t_group_list()
    //{
    //    bzero(name, sizeof(name));
    //    index = 0;
    //}
    //};
}