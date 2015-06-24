using SDK.Common;
using SDK.Lib;

namespace FightCore
{
    public class SelfDZArea : SceneDZArea
    {
        public SelfDZArea(SceneDZData sceneDZData, EnDZPlayer playerSide)
            : base(sceneDZData, playerSide)
        {
            m_outSceneCardList = new SelfOutSceneCardList(m_sceneDZData, m_playerSide);
            m_inSceneCardList = new SelfInSceneCardList(m_sceneDZData, m_playerSide);
        }

        // 除了 card 禁止所有手牌区域卡牌拖动，目前只有自己区域才能做这个功能
        override public void disableAllInCardDragExceptOne(SceneCardBase card)
        {
            m_inSceneCardList.disableAllCardDragExceptOne(card);
        }

        override public void enableAllInCardDragExceptOne(SceneCardBase card)
        {
            m_inSceneCardList.enableAllCardDragExceptOne(card);
        }

        override public void clearAttTimes()
        {
            // 出牌列表
            m_outSceneCardList.clearAttTimes();
            // 技能区
            if (m_sceneSkillCard != null)
            {
                m_sceneSkillCard.clearAttTimes();
            }

            // 装备区
            if (m_sceneEquipCard != null)
            {
                m_sceneEquipCard.clearAttTimes();
            }

            // 检查英雄
            if (m_centerHero != null)
            {
                m_centerHero.clearAttTimes();
            }
        }

        override public void updateCanLaunchAttState(bool bEnable)
        {
            // 出牌列表
            m_outSceneCardList.updateCanLaunchAttState(bEnable);
            // 技能区
            if (m_sceneSkillCard != null)
            {
                m_sceneSkillCard.effectControl.updateCanLaunchAttState(bEnable);
            }

            // 装备区
            if (m_sceneEquipCard != null)
            {
                m_sceneEquipCard.effectControl.updateCanLaunchAttState(bEnable);
            }

            // 检查英雄
            if (m_centerHero != null)
            {
                m_centerHero.effectControl.updateCanLaunchAttState(bEnable);
            }
        }

        // 自己移动卡牌位置，需要更新能发起攻击状态特效
        override public void changeSceneCardArea(uint qwThisID, CardArea dwLocation, ushort yPos)
        {
            base.changeSceneCardArea(qwThisID, dwLocation, yPos);

            if (Ctx.m_instance.m_dataPlayer.m_dzData.bSelfSide())
            {
                //updateCanLaunchAttState(true);
                SceneCardBase srcCard = null;
                srcCard = m_outSceneCardList.getSceneCardByThisID(qwThisID);
                srcCard.effectControl.updateCanLaunchAttState(true);
            }
        }
    }
}