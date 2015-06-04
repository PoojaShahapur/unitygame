using SDK.Common;

namespace Game.UI
{
    /**
     * @brief 攻击数据
     */
    public class AttackData : FightListBase
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
        public void getNextItem()
        {
            if (m_curAttackItem == null && m_attackList.Count() > 0)
            {
                m_curAttackItem = m_attackList[0];
            }
        }

        public void execCurItem(SceneCardBase card)
        {
            m_curAttackItem.execAttack(card);
        }

        // 执行队列中的一个 Item
        public void endCurItem()
        {
            m_curAttackItem.attackEndPlayDisp.dispatchEvent(m_curAttackItem);
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

        public AttackItemBase createItem(EAttackType type)
        {
            AttackItemBase ret = null;
            if (EAttackType.eCommon == type)
            {
                ret = new ComAttackItem();
            }
            else if(EAttackType.eSkill== type)
            {
                ret = new SkillAttackItem();
            }

            m_attackList.Add(ret);
            return ret;
        }
    }
}