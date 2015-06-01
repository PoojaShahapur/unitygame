﻿using UnityEngine;
using System.Collections;
using SDK.Common;
using SDK.Lib;
using System;

namespace Game.UI
{
    /**
     * @brief 英雄卡
     */
    public class HeroCard : SceneCardBase
    {
        protected AuxLabel m_classs, m_heroname;
        protected AuxLabel m_hpText;        // 血
        protected Action m_heroAniEndDisp;         // hero 动画结束后，分发一个消息

        public HeroCard(SceneDZData sceneDZData) :
            base(sceneDZData)
        {
            m_clickControl = new HeroClickControl(this);
            m_aniControl = new HeroAniControl(this);
            m_behaviorControl = new HeroBehaviorControl(this);
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
            m_classs = new AuxLabel(this.transform.FindChild("classs").gameObject);
            m_heroname = new AuxLabel(this.transform.FindChild("name").gameObject);
            m_hpText = new AuxLabel(this.transform.FindChild("healthdi/TextHp").gameObject);
        }

        public void updateHp()
        {
            m_hpText.text = m_sceneCardItem.svrCard.hp.ToString();
        }

        public void setClasss(EnPlayerCareer c)
        {
            setPic(Ctx.m_instance.m_matMgr.getCardGroupMatByOccup((EnPlayerCareer)c).m_mat);

            //播放动画,
            //animation.Play();
            // 启动定时器
            TimerItemBase timer = new TimerItemBase();
            timer.m_internal = 3;           // 3 秒动画播放完成
            timer.m_totalCount = 3;
            timer.m_timerDisp = hideVS;
            Ctx.m_instance.m_timerMgr.addObject(timer);
        }

        public void setPic(Material m)
        {
#if UNITY_5
            transform.FindChild("pic").GetComponent<Renderer>().material = m;
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
    }
}