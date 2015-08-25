using SDK.Lib;
using UnityEngine;

namespace FightCore
{
    /**
     * @brief 场景中所有的卡牌，白色卡牌是不加入管理器的
     */
    public class SceneCardMgr : EntityMgrBase
    {
        public SceneCardMgr()
        {

        }

        override protected void onTickExec(float delta)
        {
            base.onTickExec(delta);
        }

        public SceneCardBase createCardById(uint objid, EnDZPlayer m_playerSide, CardArea area, CardType cardType, SceneDZData sceneDZData)
        {
            SceneCardBase ret = null;

            if (SceneDZCV.WHITE_CARDID == objid)       // 白色占位卡牌
            {
                ret = new WhiteCard(sceneDZData);
            }
            else if (SceneDZCV.BLACK_CARD_ID == objid)       // 背面牌
            {
                ret = new BlackCard(sceneDZData);
            }
            else if (CardType.CARDTYPE_ATTEND == cardType)
            {
                if (EnDZPlayer.ePlayerSelf == m_playerSide)
                {
                    ret = new SelfAttendCard(sceneDZData);
                }
                else
                {
                    ret = new EnemyAttendCard(sceneDZData);
                }
            }
            else if (CardType.CARDTYPE_SECRET == cardType)
            {
                ret = new SecretCard(sceneDZData);
            }
            else if (CardType.CARDTYPE_MAGIC == cardType)
            {
                if (EnDZPlayer.ePlayerSelf == m_playerSide)
                {
                    ret = new SelfMagicCard(sceneDZData);
                }
                else
                {
                    ret = new EnemyMagicCard(sceneDZData);
                }
            }
            else if (CardType.CARDTYPE_EQUIP == cardType)
            {
                if (EnDZPlayer.ePlayerSelf == m_playerSide)
                {
                    ret = new SelfEquipCard(sceneDZData);
                }
                else
                {
                    ret = new EnemyEquipCard(sceneDZData);
                }
            }
            else if (CardType.CARDTYPE_HERO == cardType)
            {
                if (EnDZPlayer.ePlayerSelf == m_playerSide)
                {
                    ret = new SelfHeroCard(sceneDZData);
                }
                else
                {
                    ret = new EnemyHeroCard(sceneDZData);
                }
            }
            else if (CardType.CARDTYPE_SKILL == cardType)
            {
                if (EnDZPlayer.ePlayerSelf == m_playerSide)
                {
                    ret = new SelfSkillCard(sceneDZData);
                }
                else
                {
                    ret = new EnemySkillCard(sceneDZData);
                }
            }
            //else if (CardType.CARDTYPE_LUCK_COINS == cardType)
            //{
            //    return new LuckCoinCard(sceneDZData);
            //}

            ret.setIdAndPnt(objid, sceneDZData.m_placeHolderGo.m_centerGO);
            ret.init();
            ret.setBaseInfo(m_playerSide, area, cardType);

            this.addObject(ret);
            if (SceneDZCV.WHITE_CARDID != objid &&
                SceneDZCV.BLACK_CARD_ID != objid)       // 这两个没有 AI 
            {
                Ctx.m_instance.m_aiSystem.aiControllerMgr.addController(ret.aiController);       // 添加到控制器中
            }

            return ret;
        }

        // 通过服务器数据创建
        public SceneCardBase createCard(SceneCardItem sceneItem, SceneDZData sceneDZData)
        {
            SceneCardBase ret = null;
            ret = createCardById(sceneItem.svrCard.dwObjectID, sceneItem.playerSide, sceneItem.cardArea, (CardType)sceneItem.m_cardTableItem.m_type, sceneDZData);
            ret.sceneCardItem = sceneItem;
            return ret;
        }

        // 这个查找不包括敌人手里的黑色卡牌，敌人手牌是没有 m_sceneCardItem 这个字段的
        public SceneCardBase getCardByThisId(uint thidId)
        {
            foreach(var card in m_sceneEntityList)
            {
                if (((card as SceneCardBase)).sceneCardItem != null)    // 敌人手里的黑色卡牌是没有这个字段的
                {
                    if (((card as SceneCardBase)).sceneCardItem.svrCard.qwThisID == thidId)
                    {
                        return card as SceneCardBase;
                    }
                }
            }

            return null;
        }

        // 不在提供这个接口，需要释放对象，直接调用对象的 dispose 接口
        //public void removeAndDestroy(SceneCardBase card)
        //{
        //    this.delObject(card);
        //    card.dispose();
        //}

        public void remove(SceneCardBase card)
        {
            this.delObject(card);
        }

        // 提供全部释放的接口
        public void removeAndDestroyAll(SceneCardBase card)
        {
            int idx = m_sceneEntityList.Count;
            for(; idx >= 0; --idx)
            {
                m_sceneEntityList[idx].dispose();
            }
        }
    }
}