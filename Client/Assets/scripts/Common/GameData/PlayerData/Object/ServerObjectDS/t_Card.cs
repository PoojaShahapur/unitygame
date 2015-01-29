namespace SDK.Common
{
    public class t_Card
    {
        public uint qwThisID;		    //物品唯一id
	    public uint dwObjectID;		    //物品表中的编号
	    public stObjectLocation pos;	// 位置
	
	    public uint dwNum;		//这个属性需要删除
	    public string strName; //名称
        public uint mpcost;		    //蓝耗 --
        public uint damage;		    //攻击力 -- 
        public uint hp;		        //血量 -- 
        public uint maxhp;		    //血量上限
        public uint dur;		    //耐久度
        public uint maxdur;		    //耐久度上限

        public byte taunt;		//嘲讽
        public byte charge;		//冲锋
        public byte windfury;		//风怒
        public byte sneak;		//潜行
        public byte shield;		//圣盾
        public byte magicDamAdd;	//法术伤害增加(X)
        public byte overload;		//过载(num)
        public byte awake;
        public uint magicID;
        public byte attackTimes;
        public uint armor;

        public void derialize(IByteArray ba)
        {
            qwThisID = ba.readUnsignedInt();
            dwObjectID = ba.readUnsignedInt();
	        pos = new stObjectLocation();
            pos.derialize(ba);

            dwNum = ba.readUnsignedInt();
            strName = ba.readMultiByte(CVMsg.MAX_NAMESIZE, GkEncode.UTF8);
            mpcost = ba.readUnsignedInt();
            damage = ba.readUnsignedInt();
            hp = ba.readUnsignedInt();
            maxhp = ba.readUnsignedInt();
            dur = ba.readUnsignedInt();
            maxdur = ba.readUnsignedInt();

            taunt = ba.readUnsignedByte();
            charge = ba.readUnsignedByte();
            windfury = ba.readUnsignedByte();
            sneak = ba.readUnsignedByte();
            shield = ba.readUnsignedByte();
            magicDamAdd = ba.readUnsignedByte();
            overload = ba.readUnsignedByte();
            awake = ba.readUnsignedByte();
            magicID = ba.readUnsignedInt();
            attackTimes = ba.readUnsignedByte();
            armor = ba.readUnsignedInt();
        }

        public void copyFrom(t_Card rhv)
        {
            qwThisID = rhv.qwThisID;
	        dwObjectID = rhv.dwObjectID;
	        pos.copyFrom(rhv.pos);
	
	        dwNum = rhv.dwNum;
	        strName = rhv.strName;
            mpcost = rhv.mpcost;
            damage = rhv.damage;
            hp = rhv.hp;
            maxhp = rhv.maxhp;
            dur = rhv.dur;
            maxdur = rhv.maxdur;

            taunt = rhv.taunt;
            charge = rhv.charge;
            windfury = rhv.windfury;
            sneak = rhv.sneak;
            shield = rhv.shield;
            magicDamAdd = rhv.magicDamAdd;
            overload = rhv.overload;
            magicID = rhv.magicID;
            attackTimes = rhv.attackTimes;
            armor = rhv.armor;
        }
    }

    //typedef struct t_Card
    //{
    //    DWORD qwThisID;		    //物品唯一id
    //    DWORD dwObjectID;		    //物品表中的编号
    //    stObjectLocation pos;	// 位置
    //    DWORD dwNum;		//这个属性需要删除
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
    //    BYTE sneak;		//潜行
    //    BYTE shield;		//圣盾
    //    BYTE magicDamAdd;	//法术伤害增加(X)
    //    BYTE overload;		//过载(num)
	
    //    BYTE awake;		//醒
    //    DWORD magicID;
    //};
}