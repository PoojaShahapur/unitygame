﻿namespace SDK.Common
{
    /**
     * @brief 服务器 hero
     */
    public class t_hero
    {
        public ushort occupation;
        public ushort level;
        public ulong exp;
        public byte isActive;
        public byte isGold;

        public void derialize(ByteBuffer ba)
        {
            occupation = ba.readUnsignedInt16();
            level = ba.readUnsignedInt16();
            exp = ba.readUnsignedLong();
            isActive = ba.readUnsignedInt8();
            isGold = ba.readUnsignedInt8();
        }

        public void copyFrom(t_hero rhv)
        {
            this.occupation = rhv.occupation;
            this.level = rhv.level;
            this.exp = rhv.exp;
            this.isActive = rhv.isActive;
            this.isGold = rhv.isGold;
        }
    }

    //struct t_hero
    //{   
    //    WORD occupation;
    //    WORD level;             //英雄等级
    //    QWORD exp;              //英雄经验
    //    BYTE isActive;          //英雄是否已激活
    //    BYTE isGold;            //英雄是否是金色
    //    t_hero()
    //    {   
    //        bzero(this, sizeof(*this));
    //    }   
    //};  

}