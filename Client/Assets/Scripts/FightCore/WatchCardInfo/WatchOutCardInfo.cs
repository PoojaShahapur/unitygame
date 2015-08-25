using SDK.Lib;
using UnityEngine;

namespace FightCore
{
    public enum WatchOutStage
    {
        eNone,              // 无状态
        eStartTimer,        // 启动定时器
        eEnd,               // 结束观察
    }

    /**
     * @brief 查看卡牌详细信息
     */
    public class WatchOutCardInfo
    {
        protected SceneDZData m_sceneDZData;
        protected uint m_cardId;                // 卡表中的 Id
        protected WatchStage m_watchStage;
        protected TimerItemBase m_timer;
        protected SceneCardBase m_outCard;

        public WatchOutCardInfo(SceneDZData sceneDZData_)
        {
            m_sceneDZData = sceneDZData_;
            m_watchStage = WatchStage.eNone;
        }

        public void dispose()
        {
            if (m_outCard != null)
            {
                m_outCard.dispose();
            }

            if (m_watchStage == WatchStage.eStartTimer)
            {
                stopTimer();
                m_timer = null;
            }
        }

        protected void createCard()
        {
            if (m_outCard == null)
            {
                m_outCard = Ctx.m_instance.m_sceneCardMgr.createCardById(m_cardId, EnDZPlayer.ePlayerSelf, CardArea.CARDCELLTYPE_COMMON, CardType.CARDTYPE_ATTEND, m_sceneDZData);
                Ctx.m_instance.m_sceneCardMgr.remove(m_outCard);
            }
        }

        // 开始观察卡牌
        public void startWatch(uint id)
        {
            if (m_cardId != id)
            {
                m_cardId = id;
                if (m_watchStage == WatchStage.eStartTimer)     // 如果当前有在观察出牌
                {
                    m_timer.reset();    // 重新计时
                }
                else
                {
                    startTimer();
                }

                startWatchCard();
            }
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

            m_timer.m_internal = 3;
            m_timer.m_totalTime = 3;
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
            m_watchStage = WatchStage.eEnd;
            stopWatch();
        }

        public void startWatchCard()
        {
            if (m_outCard == null)
            {
                createCard();
            }
            else
            {
                m_outCard.setIdAndPnt(m_cardId, m_sceneDZData.m_placeHolderGo.m_centerGO);
            }

            UtilApi.setPos(m_outCard.transform(), m_sceneDZData.m_placeHolderGo.m_centerGO.transform.localPosition + new Vector3(-2.6f, 1, 1));
            m_outCard.show();
        }

        // 将当前卡牌结束观察
        public void stopWatch()
        {
            if (m_watchStage == WatchStage.eStartTimer)
            {
                stopTimer();
            }

            if (m_outCard != null)
            {
                m_outCard.hide();
            }
        }
    }
}