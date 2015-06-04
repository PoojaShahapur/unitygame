namespace SDK.Common
{
    /**
     * @brief 技能基本表
     * // 添加一个表的步骤一
     */
    public class TableSkillItemBody : TableItemBodyBase
    {
        public string m_name;               // 名称
        public string m_effect;             // 效果
        public uint m_skillAttackEffect;      // 技能攻击特效

        override public void parseBodyByteBuffer(ByteBuffer bytes, uint offset)
        {
            bytes.position = offset;
            UtilTable.readString(bytes, ref m_name);
            UtilTable.readString(bytes, ref m_effect);
            bytes.readUnsignedInt32(ref m_skillAttackEffect);

            initDefaultValue();
        }

        protected void initDefaultValue()
        {
            if (m_skillAttackEffect == 0)
            {
                m_skillAttackEffect = 4;
            }
        }
    }
}