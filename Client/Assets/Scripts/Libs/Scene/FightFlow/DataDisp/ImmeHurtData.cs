using SDK.Common;
using System.Collections.Generic;

namespace SDK.Lib
{
    /**
     * @brief 受伤数据
     */
    public class ImmeHurtData : ImmeFightListBase
    {
        protected MList<ImmeHurtItemBase> m_hurtList;
        protected ImmeHurtItemBase m_curHurtItem;           // 当前被击项，由于受伤流程可以同时并存，因此不存在当前被击 Item，不能获取这个作为当期被击 Item
        protected EventDispatch m_allHurtExecEndDisp;   // 所有 Hurt Item 执行结束事件分发

        public ImmeHurtData()
        {
            m_hurtList = new MList<ImmeHurtItemBase>();
            m_allHurtExecEndDisp = new AddOnceEventDispatch();
        }

        public MList<ImmeHurtItemBase> hurtList
        {
            get
            {
                return m_hurtList;
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

        public void addItem(ImmeHurtItemBase item)
        {
            m_hurtList.Add(item);
        }

        // 执行队列中的一个 Item，这个必须是有效的，只有执行完后，才会删除者 Item
        public void getNextItem()
        {
            if (m_hurtList.Count() > 0)
            {
                foreach (var item in m_hurtList.list)
                {
                    if (item.delayTime <= 0 && item.execState == EImmeHurtExecState.eNone)
                    {
                        m_curHurtItem = item;
                        m_curHurtItem.startHurt();
                        return;
                    }
                }
            }
        }

        public void execCurItem(BeingEntity being)
        {
            m_curHurtItem.execHurt(being);
        }

        // 执行队列中的一个 Item
        public void endCurItem()
        {
            //removeItem(m_curHurtItem);
            //m_curHurtItem = null;
        }

        public void removeItem(ImmeHurtItemBase item)
        {
            m_hurtList.Remove(item);
            item.dispose();
        }

        // 获取是否有被击 Item
        public bool hasHurtItem()
        {
            foreach (var item in m_hurtList.list)
            {
                if (item.delayTime <= 0 && item.execState == EImmeHurtExecState.eNone)
                {
                    return true;
                }
            }

            return false;
        }

        // 获取是否有执行中的被击 Item
        public bool hasExecHurtItem()
        {
            foreach (var item in m_hurtList.list)
            {
                if (item.execState == EImmeHurtExecState.eExecing || item.execState == EImmeHurtExecState.eStartExec)
                {
                    return true;
                }
            }

            return false;
        }

        public void onTime(float delta)
        {
            List<ImmeHurtItemBase> list = new List<ImmeHurtItemBase>();
            foreach(var item in m_hurtList.list)
            {
                item.onTime(delta);
                if (item.execState == EImmeHurtExecState.eEnd)
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

        public ImmeHurtItemBase createItem(EImmeHurtType type)
        {
            ImmeHurtItemBase ret = null;
            if (EImmeHurtType.eCommon == type)
            {
                ret = new ImmeComHurtItem(type);
                ret.delayTime = ImmeAttackItemBase.ComAttMoveTime;
            }
            else if (EImmeHurtType.eSkill == type)
            {
                ret = new ImmeSkillHurtItem(type);
                ret.delayTime = 1;  // 技能攻击延迟时间有技能攻击飞行特效的时间决定，这里赋值一个默认的值
            }
            else if (EImmeHurtType.eDie == type)
            {
                ret = new ImmeDieItem(type);
            }

            m_hurtList.Add(ret);
            ret.hurtExecEndDisp.addEventHandle(onOneHurtExecEnd);

            return ret;
        }

        // 一个受伤播放结束
        public void onOneHurtExecEnd(IDispatchObject dispObj)
        {
            Ctx.m_instance.m_logSys.log("删除当前被击项");

            // 直接移除，不会在循环中删除的情况
            removeItem(dispObj as ImmeHurtItemBase);
            // 检查是否所有的受伤都播放结束
            if (m_hurtList.Count() == 0)
            {
                m_allHurtExecEndDisp.dispatchEvent(this);
            }
        }
    }
}