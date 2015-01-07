namespace SDK.Common
{
    public struct t_group_list
    {
        public uint index;
        public uint occupation;
        public string name;

        public void derialize(IByteArray ba)
        {
            index = ba.readUnsignedInt();
            occupation = ba.readUnsignedInt();
            name = ba.readMultiByte(CVMsg.MAX_NAMESIZE + 1, GkEncode.UTF8);
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