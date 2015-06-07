using SDK.Common;
using SDK.Lib;
using UnityEngine;

namespace FightCore
{
    /**
     * @brief 倒计时定时器
     */
    public class DJSTimer
    {
        protected DaoJiShiTimer m_timer;        // 定时器
        protected NumResItem m_numObj;          // 数字对象
        protected GameObject m_timerGo;         // 定时器对象 parent

        public DJSTimer(GameObject go)
        {
            m_timerGo = go;
        }

        public void dispose()
        {
            if(m_numObj != null)
            {
                m_numObj.dispose();
                m_numObj = null;
            }

            stopTimer();
            m_timerGo = null;
        }

        // 启动定时器
        public void startTimer()
        {
            if(m_timer == null)
            {
                m_timer = new DaoJiShiTimer();
                m_timer.m_curCount = 15;
                m_timer.m_totalCount = 15;
                m_timer.m_timerDisp = onTimerHandle;

                Ctx.m_instance.m_timerMgr.addObject(m_timer);
            }
            else
            {
                m_timer.reset();
            }

            updateDJSNum();
        }

        // 停止定时器
        public void stopTimer()
        {
            if (m_numObj != null)
            {
                m_numObj.disposeNum();
            }

            if (m_timer != null)
            {
                Ctx.m_instance.m_timerMgr.delObject(m_timer);
            }
        }

        public void onTimerHandle(TimerItemBase timer)
        {
            updateDJSNum();
        }

        // 更新倒计时数字
        protected void updateDJSNum()
        {
            if (m_numObj == null)
            {
                m_numObj = new NumResItem();
                m_numObj.setParent(m_timerGo);
            }

            if(m_timer != null)
            {
                if (!m_timer.m_disposed)
                {
                    m_numObj.setNum((int)m_timer.m_curCount);
                }
                else
                {
                    //dispose();
                    m_numObj.disposeNum();
                }
            }
        }
    }
}