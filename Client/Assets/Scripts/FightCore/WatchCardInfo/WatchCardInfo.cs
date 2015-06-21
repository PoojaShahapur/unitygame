using SDK.Common;
using SDK.Lib;
using UnityEngine;

namespace FightCore
{
    public enum WatchStage
    {
        eNone,              // 无状态
        eStartTimer,        // 启动定时器
        eWatching,          // 观察中
        eEnd,               // 结束观察
    }

    /**
     * @brief 查看卡牌详细信息
     */
    public class WatchCardInfo
    {
        protected WatchStage m_watchStage;
        protected TimerItemBase m_timer;

        protected SceneCardBase m_watchCard;
        protected SceneCardBase m_showCard;

        public WatchCardInfo()
        {
            m_watchStage = WatchStage.eNone;
        }

        public void dispose()
        {
            if(m_showCard != null)
            {
                m_showCard.dispose();
            }
        }

        protected void createCard(SceneCardItem sceneItem, SceneDZData sceneDZData)
        {
            if (m_showCard == null)
            {
                m_showCard = Ctx.m_instance.m_sceneCardMgr.createCard(sceneItem, sceneDZData);
                Ctx.m_instance.m_sceneCardMgr.remove(m_showCard);
            }
        }

        // 开始观察卡牌
        public void startWatch(SceneCardBase card_)
        {
            m_watchCard = card_;
            startTimer();
        }

        // 启动初始化定时器
        protected void startTimer()
        {
            m_watchStage = WatchStage.eStartTimer;

            if (m_timer == null)
            {
                m_timer = new TimerItemBase();
            }
            else
            {
                m_timer.reset();        // 重置内部数据
            }

            m_timer.m_internal = 2;
            m_timer.m_totalTime = 2;
            m_timer.m_timerDisp = onTimerEndHandle;

            Ctx.m_instance.m_timerMgr.addObject(m_timer);
        }

        protected void stopTimer()
        {
            Ctx.m_instance.m_timerMgr.delObject(m_timer);
        }

        // 开始卡牌倒计时
        public void onTimerEndHandle(TimerItemBase timer)
        {
            m_watchStage = WatchStage.eWatching;

            if (m_showCard == null)
            {
                createCard(m_watchCard.sceneCardItem, m_watchCard.m_sceneDZData);
            }
            else
            {
                m_showCard.setIdAndPnt(m_watchCard.sceneCardItem.svrCard.dwObjectID, m_watchCard.m_sceneDZData.m_placeHolderGo.m_centerGO);
                m_showCard.sceneCardItem = m_watchCard.sceneCardItem;
            }

            UtilApi.setPos(m_showCard.transform(), m_watchCard.transform().localPosition + new Vector3(SceneDZCV.COMMON_CARD_WIDTH, SceneDZCV.DRAG_YDELTA, 0));
            m_showCard.show();
        }

        // 将当前卡牌结束观察
        public void endWatch()
        {
            if (m_watchStage == WatchStage.eStartTimer)
            {
                stopTimer();
            }

            m_watchCard = null;
            if (m_showCard != null)
            {
                m_showCard.hide();
            }
        }

        // 尝试结束，可能当前观察的卡牌不是要结束的卡牌
        public void tryEndWatch(SceneCardBase card_)
        {
            if (UtilApi.isAddressEqual(m_watchCard, card_))
            {
                endWatch();
            }
        }
    }
}