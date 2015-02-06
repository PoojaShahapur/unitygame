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
        public string m_prefab;      // 预制

        public int m_chaoFeng;      // 嘲讽
        public int m_chongFeng;     // 冲锋
        public int m_fengNu;        // 风怒
        public int m_qianXing;      // 潜行
        public int m_shengDun;      // 圣盾

        public int m_mpAdded;       // 魔法伤害增加
        public int m_guoZai;        // 过载

        public int m_faShu;         // 法术
        public int m_zhanHou;       // 战吼
        public int m_wangYu;        // 亡语
        public int m_jiNu;          // 激怒
        public byte m_bNeedFaShuTarget;     // 是否需要法术目标

        override public void parseBodyByteArray(IByteArray bytes, uint offset)
        {
            (bytes as ByteArray).position = offset;
            m_name = UtilTable.readString(bytes as ByteArray);

            m_type = bytes.readInt();
            m_career = bytes.readInt();
            m_race = bytes.readInt();
            m_quality = bytes.readInt();
            m_magicConsume = bytes.readInt();

            m_attack = bytes.readInt();
            m_hp = bytes.readInt();
            m_Durable = bytes.readInt();
            m_prefab = UtilTable.readString(bytes as ByteArray);

            m_chaoFeng = bytes.readInt();
            m_chongFeng = bytes.readInt();
            m_fengNu = bytes.readInt();
            m_qianXing = bytes.readInt();
            m_shengDun = bytes.readInt();
            m_mpAdded = bytes.readInt();
            m_guoZai = bytes.readInt();
            m_faShu = bytes.readInt();
            m_zhanHou = bytes.readInt();
            m_wangYu = bytes.readInt();
            m_jiNu = bytes.readInt();
            m_bNeedFaShuTarget = bytes.readByte();
        }

        public string path
        {
            get
            {
                return Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathModel] + m_prefab;
            }
        }
    }
}