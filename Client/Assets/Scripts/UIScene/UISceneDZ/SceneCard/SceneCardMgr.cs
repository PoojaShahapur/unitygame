using SDK.Common;
using SDK.Lib;

namespace Game.UI
{
    /**
     * @brief 场景中所有的卡牌
     */
    public class SceneCardMgr : EntityMgrBase
    {
        public SceneCardMgr()
        {

        }

        override protected void onTickExec(float delta)
        {
            foreach(ISceneEntity entity in m_sceneEntityList)
            {
                entity.onTick(delta);
            }
        }

        public SceneCardBase createCard(CardType cardType, SceneDZData sceneDZData)
        {
            if (CardType.CARDTYPE_ATTEND == cardType)
            {
                return new AttendCard(sceneDZData);
            }
            else if (CardType.CARDTYPE_SECRET == cardType)
            {
                return new SecretCard(sceneDZData);
            }
            else if (CardType.CARDTYPE_MAGIC == cardType)
            {
                return new MagicCard(sceneDZData);
            }
            else if (CardType.CARDTYPE_EQUIP == cardType)
            {
                return new EquipCard(sceneDZData);
            }
            else if (CardType.CARDTYPE_HERO == cardType)
            {
                return new HeroCard();
            }
            else if (CardType.CARDTYPE_SKILL == cardType)
            {
                return new SkillCard(sceneDZData);
            }
            //else if (CardType.CARDTYPE_LUCK_COINS == cardType)
            //{
            //    return new LuckCoinCard(sceneDZData);
            //}
            return null;
        }
    }
}