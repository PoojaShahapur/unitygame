namespace SDK.Lib
{
    /**
     * @brief 技能基本表
     * // 添加一个表的步骤一
     */
    public class TableSkillItemBody : TableItemBodyBase
    {
        public string m_name;               // 名称
        public string m_effect;             // 效果
        public uint m_skillAttackEffect;    // 技能攻击特效
        public float m_effectMoveTime;      // 移动
        public int m_bNeedMove;             // 是否弹道特效, 0 不需要 1 需要

        override public void parseBodyByteBuffer(ByteBuffer bytes, uint offset)
        {
            bytes.setPos(offset);
            UtilTable.readString(bytes, ref m_name);
            UtilTable.readString(bytes, ref m_effect);
            bytes.readUnsignedInt32(ref m_skillAttackEffect);
            bytes.readInt32(ref m_bNeedMove);

            initDefaultValue();
        }

        protected void initDefaultValue()
        {
            if (m_skillAttackEffect == 0)
            {
                m_skillAttackEffect = 8;
            }

            m_effectMoveTime = 1;
            //m_bNeedMove = 1;
        }
    }
}