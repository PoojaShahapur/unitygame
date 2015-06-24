using SDK.Common;
using SDK.Lib;

namespace FightCore
{
    /**
     * @brief 随从卡，必然是自己的， Enemy 随从卡是用的是 BlackCard
     */
    public class EnemyAttendCard : AttendCard
    {
        public EnemyAttendCard(SceneDZData sceneDZData) :
            base(sceneDZData)
        {
            m_sceneCardBaseData.m_trackAniControl = new EnemyAttendAniControl(this);
            m_sceneCardBaseData.m_behaviorControl = new EnemyAttendBehaviorControl(this);
        }
    }
}