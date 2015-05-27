using SDK.Lib;

namespace SDK.Common
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
        protected string m_prefab;      // 预制

        public int m_chaoFeng;      // 嘲讽
        public int m_chongFeng;     // 冲锋
        public int m_fengNu;        // 风怒
        public int m_qianXing;      // 潜行
        public int m_shengDun;      // 圣盾

        public int m_mpAdded;       // 魔法伤害增加
        public int m_guoZai;        // 过载

        public int m_faShu;         // 法术
        public int m_zhanHou;       // 战吼
        public byte m_bNeedFaShuTarget;     // 是否需要法术目标
        public int m_bNeedZhanHouTarget;    // 战吼需要目标
        public string m_cardDesc;           // 卡牌描述
        public string m_cardHeader;         // 卡牌头像贴图路径

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
            UtilTable.readString(bytes, ref m_prefab);

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

            initDefaultValue();
        }

        public string path
        {
            get
            {
                return string.Format("{0}{1}{2}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathModel], m_prefab, ".prefab");
            }
        }

        protected void initDefaultValue()
        {
            if (string.IsNullOrEmpty(m_cardHeader))
            {
                m_cardHeader = "gaibangzhutu_kapai";
            }
        }
    }
}