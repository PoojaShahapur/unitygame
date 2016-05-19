using System;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @beirf 一项数字，可能有动画
     */
    public class FlyNumItem
    {
        protected NumResItem m_num;         // 数字
        protected NumAniParallel m_numAni;  // 数字动画
        protected Action<FlyNumItem> m_aniEndDisp;

        public void setNum(int num)
        {
            m_num = new NumResItem();
            m_num.setNum(num);
        }

        public void setPos(Vector3 pos)
        {
            m_num.setPos(pos);
        }

        public void setDisp(Action<FlyNumItem> disp)
        {
            m_aniEndDisp = disp;
        }

        public void setParent(GameObject parentGo)
        {
            m_num.setParent(parentGo);
        }

        public void play()
        {
            // 动画
            //PosAni posAni = new PosAni();
            //m_numAni = new NumAniParallel();
            //m_numAni.addOneNumAni(posAni);
            //posAni.setTime(5);
            //posAni.setGO(m_num.getParentGo());
            //posAni.destPos = m_num.getPos() + new Vector3(0, 3, 0);
            //m_numAni.play();

            // 启动定时器
            TimerItemBase timer = new TimerItemBase();
            timer.m_internal = 1;
            timer.m_totalTime = 1;
            timer.m_timerDisp.setFuncObject(endTimer);
            Ctx.m_instance.m_timerMgr.addTimer(timer);
        }

        protected void endTimer(TimerItemBase timer)
        {
            m_num.dispose();

            if(m_aniEndDisp != null)
            {
                m_aniEndDisp(this);
            }
        }
    }
}