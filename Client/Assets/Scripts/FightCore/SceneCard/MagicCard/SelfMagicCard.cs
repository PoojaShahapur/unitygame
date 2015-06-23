using SDK.Common;
using SDK.Lib;

namespace FightCore
{
    /**
     * @brief 法术卡，必然是自己的， Enemy 使用的是 BlackCard
     */
    public class SelfMagicCard : MagicCard
    {
        public SelfMagicCard(SceneDZData sceneDZData) :
            base(sceneDZData)
        {
            m_sceneCardBaseData.m_trackAniControl = new SelfMagicAniControl(this);
        }
    }
}