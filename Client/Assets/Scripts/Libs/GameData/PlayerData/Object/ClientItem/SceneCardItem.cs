using Game.Msg;

namespace SDK.Lib
{
    /**
     * @brief 场景中的一个卡牌
     */
    public class SceneCardItem
    {
        protected t_Card m_svrCard;        // 服务器卡牌数据
        public t_CardPK m_pk = new t_CardPK();
        public TableCardItemBody m_cardTableItem;       // 卡牌表中的数据，类型从卡表中获取

        protected EnDZPlayer m_playerSide;                 // 卡牌属性哪个玩家
        protected CardArea m_preCardArea;               // 移动之前插槽位置
        protected CardArea m_cardArea;                  // 卡牌在什么位置

        public CardArea cardArea
        {
            get
            {
                return m_cardArea;
            }
            set
            {
                m_preCardArea = m_cardArea;
                m_cardArea = value;
            }
        }

        public EnDZPlayer playerSide
        {
            get
            {
                return m_playerSide;
            }
            set
            {
                m_playerSide = value;
            }
        }

        public t_Card svrCard
        {
            get
            {
                return m_svrCard;
            }
            set
            {
                m_svrCard = value;
                m_pk.attackTimes = m_svrCard.attackTimes;
            }
        }

        public bool bSelfSide()
        {
            return (EnDZPlayer.ePlayerSelf == m_playerSide);
        }

        public void copyFrom(SceneCardItem rhv)
        {
            if (m_svrCard == null)
            {
                m_svrCard = new t_Card();
            }
            m_svrCard.copyFrom(rhv.m_svrCard);
            m_cardTableItem = rhv.m_cardTableItem;
            m_playerSide = rhv.m_playerSide;
            m_cardArea = rhv.m_cardArea;
        }

        // 清除攻击次数
        public void clearAttTimes()
        {
            this.m_pk.attackTimes = 0;
        }

        public bool hasEndDieFlag()
        {
            if (this.m_cardTableItem.m_type == (int)CardType.CARDTYPE_ATTEND)
            {
                if (this.m_pk.endDieFlag != 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool hasEnragedFlag()
        {
            if (this.m_cardTableItem.m_type == (int)CardType.CARDTYPE_ATTEND)
            {
                if (this.m_pk.enragedFlag != 0)
                {
                    return true;
                }
            }
            return false;
        }

        public void addAttackTimes()
        {
            this.m_pk.attackTimes++;
        }

        public byte getAttackTimes()
        {
            return this.m_pk.attackTimes;
        }

        public bool hasMagic()
        {
            if (this.m_pk.magicID != 0)
            {
                return true;
            }
            return false;
        }

        public bool addMagic(uint skillID)
        {
            this.m_pk.magicID = skillID;
            return true;
        }

        public bool clearMagic()
        {
            this.m_pk.magicID = 0;
            return true;
        }

        public bool hasShout()
        {
            if (this.m_pk.shoutID != 0)
            {
                return true;
            }
            return false;
        }

        public bool addShout(uint skillID)
        {
            this.m_pk.shoutID = skillID;
            return true;
        }

        public bool clearShout()
        {
            this.m_pk.magicID = 0;
            return true;
        }

        public bool hasDeadLanguage()
        {
            if (this.m_pk.deadID != 0)
            {
                return true;
            }
            return false;
        }

        public bool addDeadLanguage(uint skillID)
        {
            this.m_pk.deadID = skillID;
            return true;
        }

        public bool clearDeadLanguage()
        {
            this.m_pk.deadID = 0;
            return true;
        }

        public bool hasEnrage()
        {
            if (this.m_pk.enrageID != 0)
            {
                return true;
            }
            return false;
        }

        public bool addEnrage(uint skillID)
        {
            this.m_pk.enrageID = skillID;
            return true;
        }

        public bool clearEnrage()
        {
            this.m_pk.enrageID = 0;
            return true;
        }

        public bool isDie()
        {
            if (this.m_svrCard.hp == 0)
            {
                return true;
            }
            return false;
        }

        public bool isAwake()
        {
            if (this.m_cardTableItem.m_type == (int)CardType.CARDTYPE_ATTEND)
            {
                if(UtilMath.checkState(StateID.CARD_STATE_SLEEP, m_svrCard.state))
                {
                    return false;
                }
            }
            return true;
        }

        public bool isCharge()
        {
            if (this.m_cardTableItem.m_type == (int)CardType.CARDTYPE_ATTEND)
            {
                if (UtilMath.checkState(StateID.CARD_STATE_CHARGE, m_svrCard.state))
                {
                    return true;
                }
            }
            return false;
        }

        public bool hasTaunt()
        {
            if (this.m_cardTableItem.m_type == (int)CardType.CARDTYPE_ATTEND)
            {
                if (UtilMath.checkState(StateID.CARD_STATE_TAUNT, m_svrCard.state))
                {
                    return true;
                }
            }
            return false;
        }

        public bool hasWindfury()
        {
            if (this.m_cardTableItem.m_type == (int)CardType.CARDTYPE_ATTEND)
            {
                if (UtilMath.checkState(StateID.CARD_STATE_WINDFURY, m_svrCard.state))
                {
                    return true;
                }
            }
            return false;
        }

        public bool isSneak()
        {
            if (this.m_cardTableItem.m_type == (int)CardType.CARDTYPE_ATTEND)
            {
                if (UtilMath.checkState(StateID.CARD_STATE_SNEAK, m_svrCard.state))
                {
                    return true;
                }
            }
            return false;
        }

        public bool hasShield()
        {
            if (this.m_cardTableItem.m_type == (int)CardType.CARDTYPE_ATTEND)
            {
                if (UtilMath.checkState(StateID.CARD_STATE_SHIED, m_svrCard.state))
                {
                    return true;
                }
            }
            return false;
        }

        public bool isFreeze()
        {
            if (this.m_cardTableItem.m_type == (int)CardType.CARDTYPE_ATTEND || this.m_cardTableItem.m_type == (int)CardType.CARDTYPE_HERO)
            {
                if (UtilMath.checkState(StateID.CARD_STATE_FREEZE, m_svrCard.state))
                {
                    return true;
                }
            }
            return false;
        }

        // True 是可以还有攻击次数
        public bool checkAttackTimes()
        {
            byte limitTimes = 0;
            if (this.hasWindfury())
            {
                limitTimes = 2;
            }
            else
            {
                limitTimes = 1;
            }
            if (this.getAttackTimes() >= limitTimes)
            {
                return false;
            }
            return true;
        }

        public void resetAttackTimes()
        {
            this.m_pk.attackTimes = 0;
        }

        public bool isHero()
        {
            if (this.m_cardTableItem.m_type == (int)CardType.CARDTYPE_HERO)
            {
                return true;
            }
            return false;
        }

        public bool isAttend()
        {
            if (this.m_cardTableItem.m_type == (int)CardType.CARDTYPE_ATTEND)
            {
                return true;
            }
            return false;
        }

        public bool isMagicCard()
        {
            if (this.m_cardTableItem.m_type == (int)CardType.CARDTYPE_MAGIC)
            {
                return true;
            }
            return false;
        }

        public bool isHeroMagicCard()
        {
            if (this.m_cardTableItem.m_type == (int)CardType.CARDTYPE_SKILL)
            {
                return true;
            }
            return false;
        }

        public bool hasDamage()
        {
            if (this.m_cardTableItem.m_type == (int)CardType.CARDTYPE_ATTEND || this.m_cardTableItem.m_type == (int)CardType.CARDTYPE_HERO)
            {
                if (this.m_svrCard.damage != 0)
                {
                    return true;
                }
            }
            return false;
        }

        public void setDamage(uint dam)
        {
            this.m_svrCard.damage = dam;
        }

        public bool addDamage(uint dam)
        {
            if (this.m_cardTableItem.m_type == (int)CardType.CARDTYPE_ATTEND || this.m_cardTableItem.m_type == (int)CardType.CARDTYPE_HERO || this.m_cardTableItem.m_type == (int)CardType.CARDTYPE_EQUIP)
            {
                this.m_svrCard.damage += dam;
                return true;
            }
            return false;
        }

        public bool subDamage(uint dam)
        {
            if (this.m_cardTableItem.m_type == (int)CardType.CARDTYPE_ATTEND || this.m_cardTableItem.m_type == (int)CardType.CARDTYPE_HERO || this.m_cardTableItem.m_type == (int)CardType.CARDTYPE_EQUIP)
            {
                if (this.m_svrCard.damage >= dam)
                {
                    this.m_svrCard.damage -= dam;
                }
                else
                {
                    this.m_svrCard.damage = 0;
                }
                return true;
            }
            return false;
        }

        public bool hasArmor()
        {
            if (this.m_cardTableItem.m_type == (int)CardType.CARDTYPE_HERO)
            {
                if (this.m_svrCard.armor != 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool addArmor(uint value)
        {
            if (this.m_cardTableItem.m_type == (int)CardType.CARDTYPE_HERO)
            {
                this.m_svrCard.armor += value;
                return true;
            }
            return false;
        }

        public bool hasDur()
        {
            if (this.m_cardTableItem.m_type == (int)CardType.CARDTYPE_EQUIP)
            {
                if (this.m_svrCard.dur != 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool addDur(uint value)
        {
            if (this.m_cardTableItem.m_type == (int)CardType.CARDTYPE_EQUIP)
            {
                this.m_svrCard.dur += value;
                return true;
            }
            return false;
        }

        public bool subDur(uint value)
        {
            if (this.m_cardTableItem.m_type == (int)CardType.CARDTYPE_EQUIP)
            {
                if (this.m_svrCard.dur >= value)
                {
                    this.m_svrCard.dur -= value;
                }
                else
                {
                    this.m_svrCard.dur = 0;
                }
                return true;
            }
            return false;
        }

        public void restoreLife(uint hp)
        {
            if (this.m_cardTableItem.m_type == (int)CardType.CARDTYPE_ATTEND || this.m_cardTableItem.m_type == (int)CardType.CARDTYPE_HERO)
            {
                if (hp == 0)
                {
                    this.m_svrCard.hp = this.m_svrCard.maxhp;
                }
                else
                {
                    this.m_svrCard.hp += hp;
                    if (this.m_svrCard.hp >= this.m_svrCard.maxhp)
                    {
                        this.m_svrCard.hp = this.m_svrCard.maxhp;
                    }
                }
            }
        }

        public bool toDie()
        {
            if (this.m_cardTableItem.m_type == (int)CardType.CARDTYPE_ATTEND)
            {
                this.m_svrCard.hp = 0;
                return true;
            }
            else if (this.m_cardTableItem.m_type == (int)CardType.CARDTYPE_EQUIP)
            {
                this.m_svrCard.dur = 0;
                return true;
            }
            return false;
        }

        public bool addTaunt()
        {
            if (this.m_cardTableItem.m_type == (int)CardType.CARDTYPE_ATTEND)
            {
                this.m_pk.taunt = 1;
                UtilMath.clearState(StateID.CARD_STATE_TAUNT, m_svrCard.state);
                return true;
            }
            return false;
        }

        public bool clearTaunt()
        {
            if (this.m_cardTableItem.m_type == (int)CardType.CARDTYPE_ATTEND)
            {
                this.m_pk.taunt = 0;
                UtilMath.clearState(StateID.CARD_STATE_TAUNT, m_svrCard.state);
                return true;
            }
            return false;
        }

        public bool setHp(uint hp)
        {
            if (this.m_cardTableItem.m_type == (int)CardType.CARDTYPE_ATTEND)
            {
                this.m_svrCard.hp = hp;
                return true;
            }
            return false;
        }

        public bool addHpBuff(uint hp)
        {
            if (this.m_cardTableItem.m_type == (int)CardType.CARDTYPE_ATTEND)
            {
                this.m_svrCard.maxhp += hp;
                this.m_svrCard.hp += hp;
                return true;
            }
            return false;
        }

        public bool clearHpBuff(uint hp)
        {
            if (this.m_cardTableItem.m_type == (int)CardType.CARDTYPE_ATTEND)
            {
                if (this.m_svrCard.maxhp >= hp)
                {
                    this.m_svrCard.maxhp -= hp;
                }
                if (this.m_svrCard.hp >= hp)
                {
                    this.m_svrCard.hp -= hp;
                }
                return true;
            }
            return false;
        }

        public bool addCharge()
        {
            if (this.m_cardTableItem.m_type == (int)CardType.CARDTYPE_ATTEND)
            {
                this.m_pk.charge = 1;
                UtilMath.clearState(StateID.CARD_STATE_SLEEP, m_svrCard.state);
                return true;
            }
            return false;
        }

        public bool clearCharge()
        {
            if (this.m_cardTableItem.m_type == (int)CardType.CARDTYPE_ATTEND)
            {
                this.m_pk.charge = 0;
                return true;
            }
            return false;
        }

        public bool addShield()
        {
            if (this.m_cardTableItem.m_type == (int)CardType.CARDTYPE_ATTEND)
            {
                this.m_pk.shield = 1;
                UtilMath.clearState(StateID.CARD_STATE_SHIED, m_svrCard.state);
                return true;
            }
            return false;
        }

        public bool addWindfury()
        {
            if (this.m_cardTableItem.m_type == (int)CardType.CARDTYPE_ATTEND)
            {
                this.m_pk.windfury = 1;
                UtilMath.clearState(StateID.CARD_STATE_WINDFURY, m_svrCard.state);
                return true;
            }
            return false;
        }

        public bool clearWindfury()
        {
            if (this.m_cardTableItem.m_type == (int)CardType.CARDTYPE_ATTEND)
            {
                this.m_pk.windfury = 0;
                UtilMath.clearState(StateID.CARD_STATE_WINDFURY, m_svrCard.state);
                return true;
            }
            return false;
        }

        public bool addSneak()
        {
            if (this.m_cardTableItem.m_type == (int)CardType.CARDTYPE_ATTEND)
            {
                this.m_pk.sneak = 1;
                UtilMath.clearState(StateID.CARD_STATE_SNEAK, m_svrCard.state);
                return true;
            }
            return false;
        }

        public bool preAttackMe(SceneCardItem pAtt, stCardAttackMagicUserCmd rev)
        {
            if (pAtt.isDie())
            {
                return false;
            }
            if (!pAtt.isAwake())
            {
                return false;
            }
            if (!pAtt.checkAttackTimes())
            {
                return false;
            }

            return true;
        }

        public bool bInSleepState()
        {
            return (UtilMath.checkState(StateID.CARD_STATE_SLEEP, m_svrCard.state));
        }
    }
}