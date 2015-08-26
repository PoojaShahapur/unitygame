using SDK.Lib;

namespace SDK.Lib
{
    /**
     * @brief 卡表中的属性名字
     */
    public class TableCardAttrName
    {
        public const string ChaoFeng = "嘲讽";
        public const string ChongFeng = "冲锋";
        public const string FengNu = "风怒";
        public const string QianXing = "潜行";
        public const string ShengDun = "圣盾";

        public const string MoFaXiaoHao = "魔法消耗";
        public const string GongJiLi = "攻击力";
        public const string Xueliang = "血量";
        public const string NaiJiu = "耐久";
        public const string FaShuShangHai = "法术伤害增加";
        public const string GuoZai = "过载";
    }

    /**
     * @brief 卡牌基本表
     */
    public class TableCardItemBody : TableItemBodyBase
    {
        public string m_name;        // 名称
        public int m_type;           // 类型
        public int m_career;         // 职业
        public int m_race;           // 种族
        public int m_quality;        // 品质

        public int m_magicConsume;   // 魔法消耗
        public int m_attack;         // 攻击力
        public int m_hp;             // 血量
        public int m_Durable;        // 耐久

        public int m_chaoFeng;      // 嘲讽
        public int m_chongFeng;     // 冲锋
        public int m_fengNu;        // 风怒
        public int m_qianXing;      // 潜行
        public int m_shengDun;      // 圣盾

        public int m_mpAdded;       // 魔法伤害增加
        public int m_guoZai;        // 过载

        public int m_faShu;         // 法术
        public int m_zhanHou;       // 战吼
        public byte m_bNeedFaShuTarget;     // 是否需要法术目标，这个是出牌后是否需要选择目录，这个技能是否需要在目标位置释放，需要看技能表
        public int m_bNeedZhanHouTarget;    // 战吼需要目标
        public string m_cardDesc;           // 卡牌描述
        public string m_cardHeader;         // 卡牌头像贴图路径，卡牌模型中头像

        public string m_cardSetCardHeader;    // 卡牌头像贴图路径，卡组中卡牌资源
        public string m_dzCardHeader;         // 卡牌头像贴图路径，对战中卡牌图像
        public uint m_skillPrepareEffect;     // 技能攻击准备特效

        override public void parseBodyByteBuffer(ByteBuffer bytes, uint offset)
        {
            bytes.position = offset;
            UtilTable.readString(bytes, ref m_name);

            bytes.readInt32(ref m_type);
            bytes.readInt32(ref m_career);
            bytes.readInt32(ref m_race);
            bytes.readInt32(ref m_quality);
            bytes.readInt32(ref m_magicConsume);

            bytes.readInt32(ref m_attack);
            bytes.readInt32(ref m_hp);
            bytes.readInt32(ref m_Durable);

            bytes.readInt32(ref m_chaoFeng);
            bytes.readInt32(ref m_chongFeng);
            bytes.readInt32(ref m_fengNu);
            bytes.readInt32(ref m_qianXing);
            bytes.readInt32(ref m_shengDun);
            bytes.readInt32(ref m_mpAdded);
            bytes.readInt32(ref m_guoZai);
            bytes.readInt32(ref m_faShu);
            bytes.readInt32(ref m_zhanHou);
            bytes.readUnsignedInt8(ref m_bNeedFaShuTarget);
            bytes.readInt32(ref m_bNeedZhanHouTarget);
            UtilTable.readString(bytes, ref m_cardDesc);
            UtilTable.readString(bytes, ref m_cardHeader);
            bytes.readUnsignedInt32(ref m_skillPrepareEffect);

            initDefaultValue();
        }

        protected void initDefaultValue()
        {
            if (string.IsNullOrEmpty(m_cardHeader))
            {
                m_cardHeader = "gaibangzhutu_kapai";
            }

            m_cardSetCardHeader = string.Format("{0}_2", m_cardHeader);
            m_dzCardHeader = string.Format("{0}_3", m_cardHeader);
            m_cardHeader = string.Format("{0}_1", m_cardHeader);

            if (m_skillPrepareEffect == 0)
            {
                m_skillPrepareEffect = 4;
            }
        }
    }
}