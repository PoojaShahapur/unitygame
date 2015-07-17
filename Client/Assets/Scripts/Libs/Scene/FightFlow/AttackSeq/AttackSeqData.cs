using SDK.Common;

namespace SDK.Lib
{
    /**
     * @brief 攻击序列数据
     */
    public class AttackSeqData
    {
        protected MList<OneAttackFlowSeq> m_attackSeqList;          // 攻击序列的列表

        public AttackSeqData()
        {
            m_attackSeqList = new MList<OneAttackFlowSeq>();
        }

        public void onTime(float delta)
        {
            foreach (var item in m_attackSeqList.list)
            {
                item.onTime(delta);
            }
        }
    }
}