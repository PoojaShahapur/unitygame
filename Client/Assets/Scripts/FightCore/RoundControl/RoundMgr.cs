using SDK.Lib;

namespace FightCore
{
    /**
     * @brief 回合管理
     */
    public class RoundMgr
    {
        protected SceneDZData m_sceneDZData;
        protected TimerItemBase m_timer;   // 回合开始的时候开始回合倒计时，进入对战，每一回合倒计时
        protected bool m_bStartRound = false;      // 起始牌都落下，才算开始回合
        protected DJSNum m_DJSNum;             // 定时器

        public RoundMgr(SceneDZData sceneDZData_)
        {
            m_sceneDZData = sceneDZData_;
        }

        public void dispose()
        {
            stopTimer();
        }

        public bool bStartRound
        {
            get
            {
                return m_bStartRound;
            }
            set
            {
                m_bStartRound = value;
            }
        }

        // 启动初始化定时器
        public void startInitCardTimer()
        {
            Ctx.m_instance.m_logSys.log(Ctx.m_instance.m_langMgr.getText(LangTypeId.eDZ4, LangItemID.eItem4));

            if (m_timer == null)
            {
                m_timer = new TimerItemBase();
            }
            else
            {
                m_timer.reset();        // 重置内部数据
            }

            m_timer.m_internal = m_sceneDZData.m_DZDaoJiShiXmlLimit.m_preparetime - m_sceneDZData.m_DZDaoJiShiXmlLimit.m_lastpreparetime;
            m_timer.m_totalTime = m_timer.m_internal;
            m_timer.m_timerDisp.setFuncObject(onTimerInitCardHandle);

            Ctx.m_instance.m_timerMgr.addObject(m_timer);
        }

        // 开始对战定时器
        public void startDZTimer()
        {
            Ctx.m_instance.m_logSys.log(Ctx.m_instance.m_langMgr.getText(LangTypeId.eDZ4, LangItemID.eItem5));

            if (m_timer == null)
            {
                m_timer = new TimerItemBase();
            }
            else
            {
                m_timer.reset();    // 重置参数
            }

            m_timer.m_internal = m_sceneDZData.m_DZDaoJiShiXmlLimit.m_roundtime - m_sceneDZData.m_DZDaoJiShiXmlLimit.m_lastroundtime;
            m_timer.m_totalTime = m_timer.m_internal;
            m_timer.m_timerDisp.setFuncObject(onTimerDZHandle);

            Ctx.m_instance.m_timerMgr.addObject(m_timer);
        }

        // 停止定时器
        public void stopTimer()
        {
            Ctx.m_instance.m_logSys.log(Ctx.m_instance.m_langMgr.getText(LangTypeId.eDZ4, LangItemID.eItem7));

            if (m_timer != null)
            {
                Ctx.m_instance.m_timerMgr.delObject(m_timer);
            }

            if (m_DJSNum != null)
            {
                m_DJSNum.stopTimer();
            }
        }

        // 开始卡牌倒计时
        public void onTimerInitCardHandle(TimerItemBase timer)
        {
            Ctx.m_instance.m_logSys.log(Ctx.m_instance.m_langMgr.getText(LangTypeId.eDZ4, LangItemID.eItem8));

            // 开始显示倒计时数据
            if (m_DJSNum == null)
            {
                m_DJSNum = new DJSNum(m_sceneDZData.m_placeHolderGo.m_timerGo);
            }

            m_DJSNum.startTimer();
        }

        // 每一回合倒计时
        public void onTimerDZHandle(TimerItemBase timer)
        {
            Ctx.m_instance.m_logSys.log(Ctx.m_instance.m_langMgr.getText(LangTypeId.eDZ4, LangItemID.eItem9));

            // 开始显示倒计时数据
            if (m_DJSNum == null)
            {
                m_DJSNum = new DJSNum(m_sceneDZData.m_placeHolderGo.m_timerGo);
            }

            m_DJSNum.startTimer();
        }
    }
}