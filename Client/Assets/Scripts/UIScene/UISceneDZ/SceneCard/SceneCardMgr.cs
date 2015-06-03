using SDK.Common;
using SDK.Lib;
using UnityEngine;

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
            foreach (SceneEntityBase entity in m_sceneEntityList)
            {
                entity.onTick(delta);
            }
        }

        public SceneCardBase createCard(uint objid, EnDZPlayer m_playerFlag, CardArea area, CardType cardType, SceneDZData sceneDZData)
        {
            SceneCardBase ret = null;

            if (SceneCardBase.WHITECARDID == objid)       // 白色占位卡牌
            {
                ret = new WhiteCard(sceneDZData);
            }
            else if (SceneCardBase.BLACK_CARD_ID == objid)       // 背面牌
            {
                ret = new BlackCard(sceneDZData);
            }
            else if (CardType.CARDTYPE_ATTEND == cardType)
            {
                ret = new AttendCard(sceneDZData);
            }
            else if (CardType.CARDTYPE_SECRET == cardType)
            {
                ret = new SecretCard(sceneDZData);
            }
            else if (CardType.CARDTYPE_MAGIC == cardType)
            {
                ret = new MagicCard(sceneDZData);
            }
            else if (CardType.CARDTYPE_EQUIP == cardType)
            {
                ret = new EquipCard(sceneDZData);
            }
            else if (CardType.CARDTYPE_HERO == cardType)
            {
                ret = new HeroCard(sceneDZData);
            }
            else if (CardType.CARDTYPE_SKILL == cardType)
            {
                ret = new SkillCard(sceneDZData);
            }
            //else if (CardType.CARDTYPE_LUCK_COINS == cardType)
            //{
            //    return new LuckCoinCard(sceneDZData);
            //}

            ret.setIdAndPnt(objid, sceneDZData.m_centerGO);
            ret.init();
            ret.setBaseInfo(m_playerFlag, area, cardType);
            
            this.add2List(ret);
            if (SceneCardBase.WHITECARDID != objid &&
                SceneCardBase.BLACK_CARD_ID != objid)       // 这两个没有 AI 
            {
                Ctx.m_instance.m_aiSystem.aiControllerMgr.add2List(ret.aiController);       // 添加到控制器中
            }

            return ret;
        }

        public SceneCardBase getCard(uint thidId)
        {
            foreach(var card in m_sceneEntityList)
            {
                if(((card as SceneCardBase)).sceneCardItem.svrCard.qwThisID == thidId)
                {
                    return card as SceneCardBase;
                }
            }

            return null;
        }
    }
}