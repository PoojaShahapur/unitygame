using Game.Msg;
using SDK.Common;
using SDK.Lib;

namespace Game.UI
{
    /**
     * @brief 战斗消息都放在这里缓存，需要的时候再拿出来执行
     */
    public class FightMsgMgr
    {
        protected SceneDZData m_sceneDZData;
        protected stNotifyBattleCardPropertyUserCmd m_curFightData;     // 当前战斗数据
        protected MList<stNotifyBattleCardPropertyUserCmd> m_cacheList; // 缓存的战斗数据列表
        protected MList<HurtData> m_hurtList;       // 当前受伤对象列表

        public FightMsgMgr(SceneDZData data)
        {
            m_sceneDZData = data;
            m_curFightData = null;
            m_cacheList = new MList<stNotifyBattleCardPropertyUserCmd>();
            m_hurtList = new MList<HurtData>();
        }

        public SceneDZData sceneDZData
        {
            get
            {
                return m_sceneDZData;
            }
            set
            {
                m_sceneDZData = value;
            }
        }

        public void onOneAttackAndHurtEndHandle(IDispatchObject dispObj)
        {
            m_curFightData = null;
            m_hurtList.Remove(dispObj as HurtData);
            if (m_hurtList.Count() == 0)
            {
                nextOneAttact();
            }
        }

        public void psstNotifyBattleCardPropertyUserCmd(stNotifyBattleCardPropertyUserCmd msg)
        {
            if(m_curFightData == null)
            {
                processOneAttack(msg);
            }
            else
            {
                m_cacheList.Add(msg);
            }
        }

        protected void nextOneAttact()
        {
            if(m_cacheList.Count() > 0)
            {
                m_curFightData = m_cacheList[0];
                m_cacheList.Remove(m_curFightData);
                processOneAttack(m_curFightData);
            }
        }

        public void processOneAttack(stNotifyBattleCardPropertyUserCmd msg)
        {
            if (msg.dwMagicType == 0) // 普通攻击必然是单攻，单攻必然有攻击目标
            {
                commonAttack(msg);
            }
            else                        // 如果是法术群攻，可能有攻击目标
            {
                skillAttack(msg);
            }
        }

        // 普通攻击，必然造成伤害
        protected void commonAttack(stNotifyBattleCardPropertyUserCmd msg)
        {
            SceneCardBase att = null;
            SceneCardBase def = null;

            if (!attackCheck(msg.A_object.qwThisID, ref att) || !attackCheck(msg.defList[0].qwThisID, ref def))
            {
                Ctx.m_instance.m_logSys.log("攻击失败");
            }
            else
            {
                att.sceneCardItem.svrCard = msg.A_object;
                def.sceneCardItem.svrCard = msg.defList[0];
                attackTo(att, def, EAttackType.eCommon, msg);
            }
        }

        // 法术攻击有攻击木目标，如果不用选择攻击目标的法术攻击，服务器发送过来的攻击者是释放一边的主角，技能攻击可能给自己回血，也可能给对方伤血
        protected void skillAttack(stNotifyBattleCardPropertyUserCmd msg)
        {
            SceneCardBase att = null;
            SceneCardBase def = null;

            if (!attackCheck(msg.A_object.qwThisID, ref att))
            {
                Ctx.m_instance.m_logSys.log("攻击者无效");
            }

            att.sceneCardItem.svrCard = msg.A_object;

            foreach (var svrCard in msg.defList)
            {
                if (!attackCheck(svrCard.qwThisID, ref def))
                {
                    Ctx.m_instance.m_logSys.log("被击者无效");
                }
                else
                {
                    msg.m_bDamage = bDamageHp(def.sceneCardItem.svrCard, svrCard);      // 技能攻击计算是否是伤血值
                    def.sceneCardItem.svrCard = svrCard;
                    attackTo(att, def, EAttackType.eSkill, msg);
                }
            }
        }

        // 攻击检查
        protected bool attackCheck(uint thisId, ref SceneCardBase card)
        {
            // 更新动画
            EnDZPlayer side = EnDZPlayer.ePlayerTotal;
            CardArea slot = CardArea.CARDCELLTYPE_NONE;

            card = m_sceneDZData.getSceneCardByThisID(thisId, ref side, ref slot);

            if (side == EnDZPlayer.ePlayerTotal ||
                slot == CardArea.CARDCELLTYPE_NONE)
            {
                return false;
            }

            return true;
        }

        // 攻击者攻击被击者
        protected void attackTo(SceneCardBase att, SceneCardBase def, EAttackType attackType, stNotifyBattleCardPropertyUserCmd msg)
        {
            if (att != null && def != null)
            {
                // 攻击
                AttackItemBase attItem = null;
                attItem = att.fightData.attackData.createItem(attackType);
                attItem.initItemData(att, def, msg);

                // 受伤
                HurtItemBase hurtItem = null;
                hurtItem = def.fightData.hurtData.createItem((EHurtType)attackType);
                hurtItem.initItemData(att, def, msg);
                def.fightData.hurtData.allHurtExecEndDisp.addEventHandle(onOneAttackAndHurtEndHandle);

                m_hurtList.Add(def.fightData.hurtData);
            }
        }

        // 计算是否是伤血
        protected bool bDamageHp(t_Card src, t_Card dest)
        {
            if(src.hp > dest.hp)        // HP 减少
            {
                if(src.hp != src.maxhp)         // 不是由于技能导致的将这两个值减少并且设置成同样的值，就是伤血
                {
                    return true;
                }
            }

            return false;
        }
    }
}