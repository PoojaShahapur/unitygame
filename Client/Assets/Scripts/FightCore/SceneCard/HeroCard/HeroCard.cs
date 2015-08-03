using UnityEngine;
using System.Collections;
using SDK.Common;
using SDK.Lib;
using System;

namespace FightCore
{
    /**
     * @brief 英雄卡，即可以作为攻击者，也可以作为被击者
     */
    public class HeroCard : NotOutCard
    {
        protected AuxLabel m_classs, m_heroname;
        protected AuxLabel m_hpText;        // 血
        protected Action m_heroAniEndDisp;         // hero 动画结束后，分发一个消息

        public HeroCard(SceneDZData sceneDZData) :
            base(sceneDZData)
        {
            m_sceneCardBaseData.m_trackAniControl = new HeroAniControl(this);
            m_render = new HeroRender(this);
            m_sceneCardBaseData.m_ioControl = new HeroIOControl(this);
        }

        public Action heroAniEndDisp
        {
            get
            {
                return m_heroAniEndDisp;
            }
            set
            {
                m_heroAniEndDisp = value;
            }
        }

        public override void init()
        {
            base.init();
            //m_classs = new AuxLabel(m_render.transform().FindChild("classs").gameObject);
            //m_heroname = new AuxLabel(m_render.transform().FindChild("name").gameObject);
            //m_hpText = new AuxLabel(m_render.transform().FindChild("healthdi/TextHp").gameObject);
        }

        public void updateHp()
        {
            //m_hpText.text = m_sceneCardItem.svrCard.hp.ToString();
        }

        public void setPlayerCareer(EnPlayerCareer career)
        {
            //setPic(Ctx.m_instance.m_matMgr.getCardGroupMatByOccup((EnPlayerCareer)career).m_mat);

            ////播放动画,
            ////animation.Play();
            //// 启动定时器
            //TimerItemBase timer = new TimerItemBase();
            //timer.m_internal = 3;           // 3 秒动画播放完成
            //timer.m_totalCount = 3;
            //timer.m_timerDisp = hideVS;
            //Ctx.m_instance.m_timerMgr.addObject(timer);

            // 这个之后才开始显示播放自己第一次牌
            if (m_heroAniEndDisp != null)
            {
                m_heroAniEndDisp();
                m_heroAniEndDisp = null;
            }
        }

        public void setPic(Material m)
        {
#if UNITY_5
            m_render.transform().FindChild("pic").GetComponent<Renderer>().material = m;
#elif UNITY_4_6 || UNITY_4_5
            transform.FindChild("pic").renderer.material = m;
#endif
        }

        // 隐藏 VS 图标
        public void hideVS(TimerItemBase timer)
        {
            Camera.main.transform.FindChild("vs").gameObject.SetActive(false);
            // 这个之后才开始显示播放自己第一次牌
            if(m_heroAniEndDisp != null)
            {
                m_heroAniEndDisp();
                m_heroAniEndDisp = null;
            }
        }

        override public void updateCardDataChangeBySvr(t_Card svrCard_ = null)
        {
            base.updateCardDataChangeBySvr(svrCard_);

            if (svrCard_ == null)
            {
                svrCard_ = m_sceneCardItem.svrCard;
            }

            AuxLabel text = new AuxLabel();

            text.setSelfGo(m_render.gameObject(), "UIRoot/AttText");        // 攻击
            text.text = svrCard_.damage.ToString();
            text.setSelfGo(m_render.gameObject(), "UIRoot/HpText");         // HP
            text.text = svrCard_.hp.ToString();

            text.setSelfGo(m_render.gameObject(), "UIRoot/ArmorText");         // Armor 护甲
            text.text = svrCard_.armor.ToString();

            updateHp();
        }

        override public void setIdAndPnt(uint objId, GameObject pntGo_)
        {
            (m_render as HeroRender).setIdAndPnt(objId, pntGo_);
        }

        public override string getDesc()
        {
            return string.Format("CardType = HeroCard, CardSide = {0}, CardArea = {1}, CardPos = {2}, CardClientId = {3}, ThisId = {4}", getSideStr(), getAreaStr(), getPos(), m_ClientId, getThisId());
        }
    }
}