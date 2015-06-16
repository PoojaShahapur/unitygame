using Game.Msg;
using SDK.Common;
using SDK.Lib;

namespace FightCore
{
    /**
     * @brief 攻击战斗项
     */
    public class FightRoundAttItem : FightRoundItemBase
    {
        protected stNotifyBattleCardPropertyUserCmd m_attCmd;       // 攻击数据
        protected MList<HurtData> m_hurtList;       // 当前受伤对象列表

        public FightRoundAttItem(SceneDZData data) :
            base(data)
        {
            m_hurtList = new MList<HurtData>();
        }

        override public void psstNotifyBattleCardPropertyUserCmd(stNotifyBattleCardPropertyUserCmd msg)
        {
            m_attCmd = msg;
        }

        override public void processOneAttack()
        {
            if (m_attCmd.dwMagicType == 0) // 普通攻击必然是单攻，单攻必然有攻击目标
            {
                commonAttack(m_attCmd);
            }
            else                        // 如果是法术群攻，可能有攻击目标
            {
                skillAttack(m_attCmd);
            }
        }

        // 普通攻击，必然造成伤害
        protected void commonAttack(stNotifyBattleCardPropertyUserCmd msg)
        {
            Ctx.m_instance.m_logSys.log("[Fight] 开始一次普通攻击");

            SceneCardBase att = null;
            SceneCardBase def = null;

            if (!attackCheck(msg.A_object.qwThisID, ref att) || !attackCheck(msg.defList[0].qwThisID, ref def))
            {
                Ctx.m_instance.m_logSys.log("[Fight] 普通攻击攻击失败");
            }
            else
            {
                msg.m_origAttObject = att.sceneCardItem.svrCard;
                msg.m_origDefObject = def.sceneCardItem.svrCard;

                att.sceneCardItem.svrCard = msg.A_object;
                def.sceneCardItem.svrCard = msg.defList[0];

                attackTo(att, def, EAttackType.eCommon, msg);
            }
        }

        // 法术攻击有攻击木目标，如果不用选择攻击目标的法术攻击，服务器发送过来的攻击者是释放一边的英雄，技能攻击可能给自己回血，也可能给对方伤血
        protected void skillAttack(stNotifyBattleCardPropertyUserCmd msg)
        {
            Ctx.m_instance.m_logSys.log("[Fight] 开始一次技能攻击");

            SceneCardBase att = null;
            SceneCardBase def = null;

            if (!attackCheck(msg.A_object.qwThisID, ref att))
            {
                Ctx.m_instance.m_logSys.log("[Fight] 技能攻击攻击者无效");
            }

            msg.m_origAttObject = att.sceneCardItem.svrCard;
            att.sceneCardItem.svrCard = msg.A_object;

            foreach (var svrCard in msg.defList)
            {
                if (!attackCheck(svrCard.qwThisID, ref def))
                {
                    Ctx.m_instance.m_logSys.log("[Fight] 技能攻击被击者无效");
                }
                else
                {
                    msg.m_origDefObject = def.sceneCardItem.svrCard;
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

        protected void onOneAttackAndHurtEndHandle(IDispatchObject dispObj)
        {
            Ctx.m_instance.m_logSys.log("[Fight] 结束一场攻击，将要开始下一场攻击");

            m_hurtList.Remove(dispObj as HurtData);
            if (m_hurtList.Count() == 0)
            {
                m_OneAttackAndHurtEndDisp.dispatchEvent(this);
            }
        }
    }
}