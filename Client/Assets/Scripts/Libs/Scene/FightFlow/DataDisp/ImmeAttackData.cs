using SDK.Lib;

namespace SDK.Lib
{
    /**
     * @brief 攻击数据
     */
    public class ImmeAttackData : ImmeFightListBase
    {
        protected MList<ImmeAttackItemBase> m_attackList;
        protected ImmeAttackItemBase m_curAttackItem;       // 当前攻击项，因为攻击不能打断，必须整个攻击流程结束才算是攻击结束

        public ImmeAttackData()
        {
            m_attackList = new MList<ImmeAttackItemBase>();
        }

        public MList<ImmeAttackItemBase> attackList
        {
            get
            {
                return m_attackList;
            }
        }

        public ImmeAttackItemBase curAttackItem
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

        public void addItem(ImmeAttackItemBase item)
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
        public void execCurItem(BeingEntity being)
        {
            m_curAttackItem.execAttack(being);
        }

        // 执行队列中的一个 Item
        public void endCurItem()
        {
            m_curAttackItem.attackEndDisp.dispatchEvent(m_curAttackItem);
            removeItem(m_curAttackItem);
            m_curAttackItem = null;
        }

        public void removeItem(ImmeAttackItemBase item)
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

        public ImmeAttackItemBase createItem(EImmeAttackType type)
        {
            ImmeAttackItemBase ret = null;
            if (EImmeAttackType.eCommon == type)
            {
                ret = new ImmeComAttackItem(type);
            }
            else if (EImmeAttackType.eSkill == type)
            {
                ret = new ImmeSkillAttackItem(type);
            }

            m_attackList.Add(ret);
            return ret;
        }
    }
}