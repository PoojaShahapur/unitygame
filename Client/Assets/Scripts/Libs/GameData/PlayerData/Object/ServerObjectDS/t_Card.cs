using System;

namespace SDK.Lib
{
    public class t_Card
    {
        public uint qwThisID;		    //物品唯一id
	    public uint dwObjectID;		    //物品表中的编号
	    public stObjectLocation pos;	// 位置

        public uint mpcost;		    //蓝耗
        public uint damage;		    //攻击力
        public uint hp;		        //血量
        public uint maxhp;		    //血量上限
        public uint dur;		    //耐久度

        public byte magicDamAdd;	//法术伤害增加(X)
        public byte overload;		//过载(num)
        public uint armor;          //护甲值
        public byte attackTimes;    // 每一局已经攻击次数，判断每一局是否能继续攻击，如果有 CARD_STATE_WINDFURY 这个状态就是每一局能攻击 2 次，其它的都只能攻击一次
        public byte equipOpen;      //武器状态(1开启,0关闭)

        public byte side;
        public uint popHpValue;               //冒出的数字(回血)
        public uint popDamValue;              //冒出的数字(受伤)
        public byte[] state;

        public void derialize(ByteBuffer bu)
        {
            bu.readUnsignedInt32(ref qwThisID);
            bu.readUnsignedInt32(ref dwObjectID);
	        pos = new stObjectLocation();
            pos.derialize(bu);

            bu.readUnsignedInt32(ref mpcost);
            bu.readUnsignedInt32(ref damage);
            bu.readUnsignedInt32(ref hp);
            bu.readUnsignedInt32(ref maxhp);
            bu.readUnsignedInt32(ref dur);

            bu.readUnsignedInt8(ref magicDamAdd);
            bu.readUnsignedInt8(ref overload);
            bu.readUnsignedInt32(ref armor);
            bu.readUnsignedInt8(ref attackTimes);
            bu.readUnsignedInt8(ref equipOpen);

            bu.readUnsignedInt8(ref side);
            bu.readUnsignedInt32(ref popHpValue);
            bu.readUnsignedInt32(ref popDamValue);

            uint len = ((int)StateID.CARD_STATE_MAX + 7) / 8;
            state = new byte[len];
            bu.readBytes(ref state, len);
        }

        public void serialize(ByteBuffer bu)
        {
            bu.writeUnsignedInt32(qwThisID);
            bu.writeUnsignedInt32(dwObjectID);
            pos = new stObjectLocation();
            pos.serialize(bu);

            bu.writeUnsignedInt32(mpcost);
            bu.writeUnsignedInt32(damage);
            bu.writeUnsignedInt32(hp);
            bu.writeUnsignedInt32(maxhp);
            bu.writeUnsignedInt32(dur);

            bu.writeUnsignedInt8(magicDamAdd);
            bu.writeUnsignedInt8(overload);
            bu.writeUnsignedInt32(armor);
            bu.writeUnsignedInt8(attackTimes);
            bu.writeUnsignedInt8(equipOpen);

            bu.writeUnsignedInt8(side);
            bu.writeUnsignedInt32(popHpValue);
            bu.writeUnsignedInt32(popDamValue);

            uint len = ((int)StateID.CARD_STATE_MAX + 7) / 8;
            state = new byte[len];
            bu.writeBytes(state, 0, len);
        }

        public void copyFrom(t_Card rhv)
        {
            qwThisID = rhv.qwThisID;
	        dwObjectID = rhv.dwObjectID;
            if (pos == null)
            {
                pos = new stObjectLocation();
            }
	        pos.copyFrom(rhv.pos);

            mpcost = rhv.mpcost;
            damage = rhv.damage;
            hp = rhv.hp;
            maxhp = rhv.maxhp;
            dur = rhv.dur;

            magicDamAdd = rhv.magicDamAdd;
            overload = rhv.overload;
            armor = rhv.armor;
            attackTimes = rhv.attackTimes;
            equipOpen = rhv.equipOpen;

            if(state == null || rhv.state.Length != state.Length)
            {
                state = new byte[((int)StateID.CARD_STATE_MAX + 7) / 8];
            }
            Array.Copy(rhv.state, 0, state, 0, rhv.state.Length);

            side = rhv.side;

            popHpValue = rhv.popHpValue;
            popDamValue = rhv.popDamValue;
        }

        public string log()
        {
            return string.Format("[Fight] Side = {0}, Area = {1}, Pos = {2}, qwThisID = {3}, HP = {4}, MaxHP = {5} popHpValue={6} popDamValue={7}", side, pos.dwLocation, pos.y, qwThisID, hp, maxhp, popHpValue, popDamValue);
        }
    }

    //typedef struct t_Card
    //{
        //DWORD qwThisID;		    //物品唯一id
        //DWORD dwObjectID;		    //物品表中的编号
        //stObjectLocation pos;	// 位置
        //DWORD dwNum;		//这个属性需要删除
        //char strName[MAX_NAMESIZE]; //名称
        //DWORD mpcost;		    //蓝耗
        //DWORD damage;		    //攻击力
        //DWORD hp;		    //血量
        //DWORD maxhp;		    //血量上限
        //DWORD dur;		    //耐久度
        //DWORD maxdur;		    //耐久度上限

        //BYTE magicDamAdd;	//法术伤害增加(X)
        //BYTE overload;		//过载(num)
	
        //DWORD armor;		//护甲值
    //};
}