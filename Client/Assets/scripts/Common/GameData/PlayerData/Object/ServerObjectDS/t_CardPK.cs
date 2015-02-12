namespace SDK.Common
{
    public class t_CardPK
    {
        public byte taunt;		    //嘲讽
        public byte charge;	    //冲锋
        public byte windfury;	    //风怒
        public byte sneak;		    //潜行
        public byte shield;	    //圣盾
        public byte freeze;	    //冻结
        public uint freeze_round;	    //冻结发生的回合数
        public byte awake;		    //醒
        public byte endDieFlag;	    //回合结束死亡标识
        public byte enragedFlag;	    //被激怒了
        public byte attackTimes;	    //回合攻击次数
        public uint magicID;	    //卡牌携带的法术ID
        public uint shoutID;	    //战吼ID(skill)
        public uint deadID;	    //亡语ID(skill)
        public uint enrageID;	    //激怒ID(skill)
        public uint select1ID;	    //抉择1(卡牌ID)
        public uint select2ID;	    //抉择2(卡牌ID)
    }
}