using SDK.Lib;
namespace SDK.Common
{
    public class t_group_list
    {
        public uint index;              // 卡牌唯一 ID
        public uint occupation;         // 职业
        public string name;             // 卡牌名字

        public void derialize(ByteBuffer ba)
        {
            ba.readUnsignedInt32(ref index);
            ba.readUnsignedInt32(ref occupation);
            ba.readMultiByte(ref name, CVMsg.MAX_NAMESIZE + 1, GkEncode.UTF8);
        }

        public void copyFrom(t_group_list rhv)
        {
            index = rhv.index;
            occupation = rhv.occupation;
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