using SDK.Common;

namespace Game.UI
{
    /**
     * @brief 攻击数据
     */
    public class AttackData
    {
        protected MList<AttackItemBase> m_attackList;
        protected AttackItemBase m_curAttackItem;       // 当前攻击项

        public AttackData()
        {
            m_attackList = new MList<AttackItemBase>();
        }

        public MList<AttackItemBase> attackList
        {
            get
            {
                return m_attackList;
            }
        }

        public AttackItemBase curAttackItem
        {
            get
            {
                return m_curAttackItem;
            }
            set
            {
                m_curAttackItem = value;
            }
        }

        public void addItem(AttackItemBase item)
        {
            m_attackList.Add(item);
        }

        // 执行队列中的一个 Item
        public void getFirstItem()
        {
            m_curAttackItem = m_attackList[0];
        }

        public void execCurItem()
        {
            m_curAttackItem.setEnableHurt();
        }

        // 执行队列中的一个 Item
        public void endCurItem()
        {
            removeItem(m_curAttackItem);
            m_curAttackItem = null;
        }

        public void removeItem(AttackItemBase item)
        {
            m_attackList.Remove(item);
        }

        public void onTime(float delta)
        {
            foreach (var item in m_attackList.list)
            {
                item.onTime(delta);
            }
        }
    }
}