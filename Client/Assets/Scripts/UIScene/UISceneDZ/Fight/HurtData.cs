using SDK.Common;
using SDK.Lib;
using System.Collections.Generic;

namespace Game.UI
{
    /**
     * @brief 受伤数据
     */
    public class HurtData : FightListBase
    {
        protected MList<HurtItemBase> m_hurtList;
        protected HurtItemBase m_curHurtItem;           // 当前被击项
        protected EventDispatch m_allHurtExecEndDisp;   // 所有 Hurt Item 执行结束事件分发

        public HurtData()
        {
            m_hurtList = new MList<HurtItemBase>();
            m_allHurtExecEndDisp = new AddOnceEventDispatch();
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

        public EventDispatch allHurtExecEndDisp
        {
            get
            {
                return m_allHurtExecEndDisp;
            }
            set
            {
                m_allHurtExecEndDisp = value;
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
                    if (item.delayTime <= 0 && item.execState== EHurtExecState.eNone)
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
                if (item.delayTime <= 0 && item.execState == EHurtExecState.eNone)
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

        public HurtItemBase createItem(EHurtType type)
        {
            HurtItemBase ret = null;
            if(EHurtType.eCommon == type)
            {
                ret = new ComHurtItem();
            }
            else if (EHurtType.eSkill == type)
            {
                ret = new SkillHurtItem();
            }

            m_hurtList.Add(ret);
            ret.hurtExecEndDisp.addEventHandle(onOneHurtExecEnd);

            return ret;
        }

        // 一个受伤播放结束
        public void onOneHurtExecEnd(IDispatchObject dispObj)
        {
            // 直接移除，不会在循环中删除的情况
            removeItem(dispObj as HurtItemBase);
            // 检查是否所有的受伤都播放结束
            if (m_hurtList.Count() == 0)
            {
                m_allHurtExecEndDisp.dispatchEvent(this);
            }
        }
    }
}