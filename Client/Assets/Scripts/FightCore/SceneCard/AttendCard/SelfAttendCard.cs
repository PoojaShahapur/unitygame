using SDK.Common;
using SDK.Lib;

namespace FightCore
{
    /**
     * @brief 随从卡，必然是自己的， Enemy 随从卡是用的是 BlackCard
     */
    public class SelfAttendCard : AttendCard
    {
        public SelfAttendCard(SceneDZData sceneDZData) :
            base(sceneDZData)
        {
            m_sceneCardBaseData.m_trackAniControl = new SelfAttendAniControl(this);
        }
    }
}