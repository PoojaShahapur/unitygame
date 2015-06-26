using SDK.Common;
using SDK.Lib;

namespace FightCore
{
    /**
     * @brief 随从卡，必然是自己的， Enemy 随从卡是用的是 BlackCard
     */
    public class SelfAttendCard : AttendCard
    {
        protected int m_clientIdx;          // 客户端拖动战吼进入场牌区域，此时几率客户端的 Idx，因为有攻击目标的战吼需要填这个位置

        public SelfAttendCard(SceneDZData sceneDZData) :
            base(sceneDZData)
        {
            m_sceneCardBaseData.m_trackAniControl = new SelfAttendAniControl(this);
            m_sceneCardBaseData.m_behaviorControl = new SelfAttendBehaviorControl(this);
        }

        override public void setZhanHouCommonClientIdx(int idx)
        {
            m_clientIdx = idx;
        }

        override public int getZhanHouCommonClientIdx()
        {
            return m_clientIdx;
        }
    }
}