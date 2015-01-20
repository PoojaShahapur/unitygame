namespace SDK.Common
{
    public class t_Card
    {
        public uint qwThisID;		    //物品唯一id
	    public uint dwObjectID;		    //物品表中的编号
	    public stObjectLocation pos;	// 位置
	
	    public uint dwNum;		//这个属性需要删除
	    public uint kind;		//这个属性应该没用
	    public uint type;		    //物品类别
	    public string strName; //名称
        public uint mpcost;		    //蓝耗
        public uint damage;		    //攻击力
        public uint hp;		    //血量
        public uint maxhp;		    //血量上限
        public uint dur;		    //耐久度
        public uint maxdur;		    //耐久度上限

        public byte taunt;		//嘲讽
        public byte charge;		//冲锋
        public byte windfury;		//风怒
	    public byte freeze;		//冻结
        public byte sneak;		//潜行
        public byte overload;		//过载(num)

        public void derialize(IByteArray ba)
        {
            qwThisID = ba.readUnsignedInt();
            dwObjectID = ba.readUnsignedInt();
	        pos = new stObjectLocation();
            pos.derialize(ba);

            dwNum = ba.readUnsignedInt();
            kind = ba.readUnsignedInt();
            type = ba.readUnsignedInt();
            strName = ba.readMultiByte(CVMsg.MAX_NAMESIZE + 1, GkEncode.UTF8);
            mpcost = ba.readUnsignedInt();
            damage = ba.readUnsignedInt();
            hp = ba.readUnsignedInt();
            maxhp = ba.readUnsignedInt();
            dur = ba.readUnsignedInt();
            maxdur = ba.readUnsignedInt();

            taunt = ba.readUnsignedByte();
            charge = ba.readUnsignedByte();
            windfury = ba.readUnsignedByte();
            freeze = ba.readUnsignedByte();
            sneak = ba.readUnsignedByte();
            overload = ba.readUnsignedByte();
        }
    }

    //typedef struct t_Card
    //{
    //    DWORD qwThisID;		    //物品唯一id
    //    DWORD dwObjectID;		    //物品表中的编号
    //    stObjectLocation pos;	// 位置
	
    //    DWORD dwNum;		//这个属性需要删除
    //    DWORD kind;		//这个属性应该没用
    //    DWORD type;		    //物品类别
    //    char strName[MAX_NAMESIZE]; //名称
    //    DWORD mpcost;		    //蓝耗
    //    DWORD damage;		    //攻击力
    //    DWORD hp;		    //血量
    //    DWORD maxhp;		    //血量上限
    //    DWORD dur;		    //耐久度
    //    DWORD maxdur;		    //耐久度上限
	
    //    BYTE taunt;		//嘲讽
    //    BYTE charge;		//冲锋
    //    BYTE windfury;		//风怒
    //    BYTE freeze;		//冻结
    //    BYTE sneak;		//潜行
    //    BYTE overload;		//过载(num)

    //};
}