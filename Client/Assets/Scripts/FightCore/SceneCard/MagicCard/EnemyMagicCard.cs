using SDK.Common;
using SDK.Lib;

namespace FightCore
{
    /**
     * @brief 法术卡，必然是自己的， Enemy 使用的是 BlackCard
     */
    public class EnemyMagicCard : MagicCard
    {
        public EnemyMagicCard(SceneDZData sceneDZData) :
            base(sceneDZData)
        {
            m_sceneCardBaseData.m_trackAniControl = new EnemyMagicAniControl(this);
            m_sceneCardBaseData.m_behaviorControl = new EnemyMagicBehaviorControl(this);
        }
    }
}