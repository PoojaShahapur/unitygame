using SDK.Common;
using System.Collections.Generic;

namespace Game.UI
{
    /**
     * @brief 受伤数据
     */
    public class HurtData
    {
        protected MList<HurtItemBase> m_hurtList;
        protected HurtItemBase m_curHurtItem;           // 当前被击项

        public HurtData()
        {
            m_hurtList = new MList<HurtItemBase>();
        }

        public MList<HurtItemBase> hurtList
        {
            get
            {
                return m_hurtList;
            }
        }

        public HurtItemBase curHurtItem
        {
            get
            {
                return m_curHurtItem;
            }
            set
            {
                m_curHurtItem = value;
            }
        }

        public void addItem(HurtItemBase item)
        {
            m_hurtList.Add(item);
        }

        // 执行队列中的一个 Item，这个必须是有效的
        public void getNextItem()
        {
            if (m_curHurtItem == null && m_hurtList.Count() > 0)
            {
                foreach (var item in m_hurtList.list)
                {
                    if (item.delayTime <= 0 && item.state == EHurtItemState.eEnable && item.execState== EHurtExecState.eNone)
                    {
                        m_curHurtItem = item;
                        m_curHurtItem.execState = EHurtExecState.eExec;
                        return;
                    }
                }
            }
        }

        public void execCurItem(SceneCardBase card)
        {
            m_curHurtItem.execHurt(card);
        }

        // 执行队列中的一个 Item
        public void endCurItem()
        {
            //removeItem(m_curHurtItem);
            m_curHurtItem = null;
        }

        public void removeItem(HurtItemBase item)
        {
            m_hurtList.Remove(item);
        }

        // 获取是否有有效的被击 Item
        public bool hasEnableItem()
        {
            foreach (var item in m_hurtList.list)
            {
                if (item.delayTime <= 0 && item.state == EHurtItemState.eEnable && item.execState == EHurtExecState.eNone)
                {
                    return true;
                }
            }

            return false;
        }

        public void onTime(float delta)
        {
            List<HurtItemBase> list = new List<HurtItemBase>();
            foreach(var item in m_hurtList.list)
            {
                item.onTime(delta);
                if(item.execState == EHurtExecState.eEnd)
                {
                    list.Add(item);
                }
            }

            foreach (var item in list)
            {
                m_hurtList.Remove(item);
            }

            list.Clear();
        }
    }
}