using UnityEngine;
using System.Collections;
using SDK.Common;
using SDK.Lib;
using System;

namespace Game.UI
{
    /**
     * @brief 英雄卡
     */
    public class HeroCard : SceneCard
    {
        protected AuxLabel m_classs, m_heroname;
        protected AuxLabel m_hpText;        // 血
        protected Action m_heroAniEndDisp;         // hero 动画结束后，分发一个消息

        public HeroCard(SceneDZData sceneDZData) :
            base(sceneDZData)
        {
            m_sceneCardBaseData.m_clickControl = new HeroClickControl(this);
            m_sceneCardBaseData.m_aniControl = new HeroAniControl(this);
            m_sceneCardBaseData.m_behaviorControl = new HeroBehaviorControl(this);

            m_render = new HeroRender(this);
            m_sceneCardBaseData.m_effectControl = new EffectControl(this);
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

        public void setClasss(EnPlayerCareer c)
        {
            //setPic(Ctx.m_instance.m_matMgr.getCardGroupMatByOccup((EnPlayerCareer)c).m_mat);

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
#elif UNITY_4_6
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

        override public void updateCardDataChange(t_Card svrCard_ = null)
        {
            base.updateCardDataChange();
            updateHp();
        }

        override public void setIdAndPnt(uint objId, GameObject pntGo_)
        {
            (m_render as HeroRender).setIdAndPnt(objId, pntGo_);
        }

        override public void setBaseInfo(EnDZPlayer m_playerFlag, CardArea area, CardType cardType)
        {
            this.transform().localPosition = m_sceneDZData.m_cardCenterGOArr[(int)m_playerFlag, (int)area].transform.localPosition;
        }
    }
}