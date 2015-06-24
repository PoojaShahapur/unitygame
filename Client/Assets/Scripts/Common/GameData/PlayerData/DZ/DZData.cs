using Game.Msg;
using Game.UI;

namespace SDK.Common
{
    public enum EnDZPlayer
    {
        ePlayerSelf = 0,            // 自己
        ePlayerEnemy = 1,           // 敌人
        ePlayerTotal            // 总共数量
    }

    /**
     * @brief 对战基本数据
     */
    public class DZData
    {
        public bool m_canReqDZ = true;     // 是否可以请求对战，如果已经请求了需要等待服务器返回
        public DZPlayer[] m_playerArr = new DZPlayer[(int)EnDZPlayer.ePlayerTotal];
        public byte m_priv;                 // 当前拥有权力的一方
        public byte m_state;                // 当前游戏所处的阶段，每一个阶段允许的操作是不同的
        public ushort m_enemyCardCount = 0;       // enemy 卡牌数量

        public bool m_bLastEnd = true;          // 下线前最后一次战斗是否结束，如果没有结束，需要继续进入战斗
        protected uint m_curPlayCardCount = 0;          // 当前总共的出牌次数

        public DZData()
        {
            m_playerArr[(int)EnDZPlayer.ePlayerSelf] = new DZPlayer(EnDZPlayer.ePlayerSelf);
            m_playerArr[(int)EnDZPlayer.ePlayerEnemy] = new DZPlayer(EnDZPlayer.ePlayerEnemy);
        }

        public void setSelfHeroInfo(CardGroupItem cardGroup)
        {
            m_playerArr[(int)EnDZPlayer.ePlayerSelf].m_heroName = Ctx.m_instance.m_dataPlayer.m_dataMain.m_name;
            m_playerArr[(int)EnDZPlayer.ePlayerSelf].m_heroOccupation = cardGroup.m_cardGroup.occupation;
        }

        public void clear()
        {
            m_playerArr[(int)EnDZPlayer.ePlayerSelf].clear();
            m_playerArr[(int)EnDZPlayer.ePlayerEnemy].clear();
        }

        // 移动后，更新数据
        public int updateCardInfo(stRetMoveGameCardUserCmd cmd)
        {
            // 查找后两边更新
            if (m_playerArr[(int)EnDZPlayer.ePlayerSelf].updateCardInfo(cmd))
            {
                return 1;
            }
            else
            {
                m_playerArr[(int)EnDZPlayer.ePlayerEnemy].updateCardInfo(cmd);
                return 2;
            }
        }

        // 是否是自己回合控制
        public bool bSelfSide()
        {
            return Ctx.m_instance.m_dataPlayer.m_dzData.m_priv == 1;
        }

        // 计算卡牌属性哪一方出的
        public int getCardSideByThisID(uint thisID)
        {
            int idx = 0;
            while (idx < 2)
            {
                foreach (SceneCardItem item in m_playerArr[idx].sceneCardList)
                {
                    if (item.svrCard.qwThisID == thisID)
                    {
                        return idx;
                    }
                }

                ++idx;
            }

            return idx;
        }

        public void getCardSideAndItemByThisID(uint thisID, ref int side, ref SceneCardItem retItem)
        {
            int idx = 0;
            while (idx < 2)
            {
                foreach (SceneCardItem item in m_playerArr[idx].sceneCardList)
                {
                    if (item.svrCard.qwThisID == thisID)
                    {
                        retItem = item;
                        side = idx;
                        return;
                    }
                }

                ++idx;
            }

            side = 2;       // 说明没有查找到
        }

        public SceneCardItem getCardItemByThisID(uint thisID)
        {
            int idx = 0;
            while (idx < 2)
            {
                foreach (SceneCardItem item in m_playerArr[idx].sceneCardList)
                {
                    if (item.svrCard.qwThisID == thisID)
                    {
                        return item;
                    }
                }

                ++idx;
            }

            return null;
        }

        public SceneCardItem getCardItemByThisIDAndSide(uint thisID, byte side)
        {
            foreach (SceneCardItem item in m_playerArr[side].sceneCardList)
            {
                if (item.svrCard.qwThisID == thisID)
                {
                    return item;
                }
            }

            return null;
        }

        public uint curPlayCardCount
        {
            get
            {
                return m_curPlayCardCount;
            }
            set
            {
                m_curPlayCardCount = value;
            }
        }

        // 获取战局数量，两面都出算一次战局
        public uint getWarCount()
        {
            return (m_curPlayCardCount + 1 / 2);
        }

        public bool isHavePrivilege(EnDZPlayer side)
        {
            return ((byte)side == m_priv - 1);
        }

        // 判断能否进行攻击
        public bool cardAttackMagic(DZPlayer user, stCardAttackMagicUserCmd rev)
        {
            if (!isHavePrivilege(user.m_side))
            {
                (Ctx.m_instance.m_uiMgr.getForm(UIFormID.eUIChat) as IUIChat).outMsg("Tips: [PK] 不是你的回合 你点个毛线");
                return false;
            }

            bool ret = false;
            if (rev.dwMagicType > 0)
            {
                if (rev.bZhanHou)
                {
                    ret = true;
                }
                else
                {
                    ret = cardSkillAttack(user, rev);
                }
            }
            else
            {
                ret = cardNormalAttack(user, rev);
            }

            return ret;
        }

        // 判断能否进行正常攻击
        public bool cardNormalAttack(DZPlayer user, stCardAttackMagicUserCmd rev)
        {
            SceneCardItem pAtt = getCardItemByThisID(rev.dwAttThisID);
            SceneCardItem pDef = getCardItemByThisID(rev.dwDefThisID);
            if (pDef == null || pAtt == null)
            {
                return false;
            }
            if (pAtt == pDef)
            {
                (Ctx.m_instance.m_uiMgr.getForm(UIFormID.eUIChat) as IUIChat).outMsg("普通攻击失败  怎么能自己打自己");
                return false;
            }
            if (pAtt.playerSide == pDef.playerSide)
            {
                (Ctx.m_instance.m_uiMgr.getForm(UIFormID.eUIChat) as IUIChat).outMsg("普通攻击失败  普通攻击不能攻击自己的牌");
                return false;
            }
            if (pAtt.isFreeze())
            {
                (Ctx.m_instance.m_uiMgr.getForm(UIFormID.eUIChat) as IUIChat).outMsg("普通攻击失败  你自己处于冻结中无法攻击");
                return false;
            }
            if (!pDef.isHero() && !pDef.isAttend())
            {
                (Ctx.m_instance.m_uiMgr.getForm(UIFormID.eUIChat) as IUIChat).outMsg("普通攻击失败  你所攻击的对象既不是英雄也不是随从");
                return false;
            }
            if (!pDef.preAttackMe(pAtt, rev))
            {
                (Ctx.m_instance.m_uiMgr.getForm(UIFormID.eUIChat) as IUIChat).outMsg("普通攻击失败  攻击验证失败");
                return false;
            }
            if (!pAtt.hasDamage())
            {
                (Ctx.m_instance.m_uiMgr.getForm(UIFormID.eUIChat) as IUIChat).outMsg("普通攻击失败  你都没有攻击力 不能发起攻击");
                return false;
            }
            if (pDef.isSneak())
            {
                (Ctx.m_instance.m_uiMgr.getForm(UIFormID.eUIChat) as IUIChat).outMsg("普通攻击失败  普通攻击不能打到潜行单位");
                return false;
            }
            if (!pDef.isSneak()) //被攻击者隐形情况下,嘲讽无效
            {
                if (Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)EnDZPlayer.ePlayerEnemy].checkTaunt() && !pDef.hasTaunt())
                {
                    (Ctx.m_instance.m_uiMgr.getForm(UIFormID.eUIChat) as IUIChat).outMsg("普通攻击失败  你得攻击一个具有嘲讽的随从才行");
                    return false;
                }
            }

            return true;
        }

        // 判断能否进行法术攻击
        public bool cardSkillAttack(DZPlayer user, stCardAttackMagicUserCmd rev)
        {
            SceneCardItem pAtt = getCardItemByThisID(rev.dwAttThisID);
            if (pAtt == null)
            {
                return false;
            }
            if (!pAtt.isMagicCard() && !pAtt.isHeroMagicCard())
            {
                (Ctx.m_instance.m_uiMgr.getForm(UIFormID.eUIChat) as IUIChat).outMsg("释放技能失败  技能拥有者既不是法术 也不是英雄能力");
                return false;
            }
            if (!user.checkMp(pAtt.m_cardTableItem.m_magicConsume))
            {
                (Ctx.m_instance.m_uiMgr.getForm(UIFormID.eUIChat) as IUIChat).outMsg(string.Format("释放技能失败  这个技能需要你有{0}个法力水晶才可以", pAtt.m_cardTableItem.m_magicConsume));
                return false;
            }
            if (pAtt.m_cardTableItem.m_bNeedFaShuTarget > 0 && (rev.dwDefThisID == 0))
            {
                (Ctx.m_instance.m_uiMgr.getForm(UIFormID.eUIChat) as IUIChat).outMsg("释放技能失败  这个技能需要你手动选择一个目标");
                return false;
            }
            if (pAtt.isHeroMagicCard() && !pAtt.checkAttackTimes())
            {
                (Ctx.m_instance.m_uiMgr.getForm(UIFormID.eUIChat) as IUIChat).outMsg("释放技能失败  英雄能力每回合只能使用一次");
                return false;
            }

            return true;
        }
    }
}