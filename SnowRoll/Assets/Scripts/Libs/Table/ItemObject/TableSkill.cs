namespace SDK.Lib
{
    /**
     * @brief 技能基本表
     * // 添加一个表的步骤一
     */
    public class TableSkillItemBody : TableItemBodyBase
    {
        public string mName;               // 名称
        public string mEffect;             // 效果
        public uint mSkillAttackEffect;    // 技能攻击特效
        public float mEffectMoveTime;      // 移动
        public int mIsNeedMove;             // 是否弹道特效, 0 不需要 1 需要

        override public void parseBodyByteBuffer(ByteBuffer bytes, uint offset)
        {
            bytes.setPos(offset);
            UtilTable.readString(bytes, ref mName);
            UtilTable.readString(bytes, ref mEffect);
            bytes.readUnsignedInt32(ref mSkillAttackEffect);
            bytes.readInt32(ref mIsNeedMove);

            initDefaultValue();
        }

        protected void initDefaultValue()
        {
            if (mSkillAttackEffect == 0)
            {
                mSkillAttackEffect = 8;
            }

            mEffectMoveTime = 1;
            //mIsNeedMove = 1;
        }
    }
}