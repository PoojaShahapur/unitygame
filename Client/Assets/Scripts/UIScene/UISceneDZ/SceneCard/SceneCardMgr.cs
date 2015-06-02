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

        public SceneCardBase createCard(uint objid, CardType cardType, SceneDZData sceneDZData, GameObject pntGO_)
        {
            SceneCardBase ret = null;
            if (CardType.CARDTYPE_ATTEND == cardType)
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

            ret.setIdAndPnt(objid, pntGO_);

            this.add2List(ret);
            Ctx.m_instance.m_aiSystem.aiControllerMgr.add2List(ret.aiController);       // 添加到控制器中

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