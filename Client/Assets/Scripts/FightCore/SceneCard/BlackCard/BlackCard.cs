using SDK.Common;
using SDK.Lib;

namespace FightCore
{
    public class BlackCard : SceneCardBase
    {
        public BlackCard(SceneDZData data) : 
            base(data)
        {
            m_render = new BlackCardRender(this);
        }

        override public void setBaseInfo(EnDZPlayer m_playerFlag, CardArea area, CardType cardType)
        {

        }
    }
}