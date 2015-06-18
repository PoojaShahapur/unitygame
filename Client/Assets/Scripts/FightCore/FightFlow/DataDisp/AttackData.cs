using SDK.Common;

namespace FightCore
{
    /**
     * @brief 攻击数据
     */
    public class AttackData : FightListBase
    {
        protected MList<AttackItemBase> m_attackList;
        protected AttackItemBase m_curAttackItem;       // 当前攻击项，因为攻击不能打断，必须整个攻击流程结束才算是攻击结束

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
        public void getNextItem()
        {
            if (m_curAttackItem == null && m_attackList.Count() > 0)
            {
                m_curAttackItem = m_attackList[0];
            }
        }

        // 攻击一次只能有一个，因此攻击 Item 没有状态
        public void execCurItem(SceneCardBase card)
        {
            m_curAttackItem.execAttack(card);
        }

        // 执行队列中的一个 Item
        public void endCurItem()
        {
            m_curAttackItem.attackEndDisp.dispatchEvent(m_curAttackItem);
            removeItem(m_curAttackItem);
            m_curAttackItem = null;
        }

        public void removeItem(AttackItemBase item)
        {
            m_attackList.Remove(item);
            item.dispose();
        }

        public void onTime(float delta)
        {
            foreach (var item in m_attackList.list)
            {
                item.onTime(delta);
            }
        }

        public AttackItemBase createItem(EAttackType type)
        {
            AttackItemBase ret = null;
            if (EAttackType.eCommon == type)
            {
                ret = new ComAttackItem(type);
            }
            else if(EAttackType.eSkill== type)
            {
                ret = new SkillAttackItem(type);
            }

            m_attackList.Add(ret);
            return ret;
        }
    }
}