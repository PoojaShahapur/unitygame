namespace SDK.Common
{
    /**
     * @brief 卡牌表
     */
    public class TableCardItemBody : TableItemBodyBase
    {
        public string m_name;           // 名称
        public int m_type;           // 类型
        public int m_career;         // 职业
        public int m_race;           // 种族
        public int m_quality;        // 品质
        public int m_magicConsume;   // 魔法消耗
        public int m_attack;         // 攻击力
        public int m_hp;             // 血量
        public int m_Durable;        // 耐久

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
        }
    }
}