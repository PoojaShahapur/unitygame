using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using SDK.Common;
using SDK.Lib;
using System;

namespace Game.UI
{
    /**
     * @brief 英雄卡
     */
    public class HeroCard : SceneCardEntityBase
    {
        protected Text m_classs, m_heroname;
        protected Text m_hpText;        // 血
        protected Action m_heroAniEndDisp;         // hero 动画结束后，分发一个消息

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

        public override void Start()
        {
            base.Start();
            m_classs = transform.FindChild("classs").GetComponent<Text>();
            m_heroname = transform.FindChild("name").GetComponent<Text>();
            m_hpText = transform.FindChild("healthdi/TextHp").GetComponent<Text>();
        }

        public void updateHp()
        {
            m_hpText.text = m_sceneCardItem.svrCard.hp.ToString();
        }

        public void setclasss(EnPlayerCareer c)
        {
            setpic(Ctx.m_instance.m_matMgr.getCardGroupMatByOccup((EnPlayerCareer)c).m_mat);

            //播放动画,
            animation.Play();
            // 启动定时器
            TimerItemBase timer = new TimerItemBase();
            timer.m_internal = 3;           // 3 秒动画播放完成
            timer.m_totalCount = 3;
            timer.m_timerDisp = hidevs;
            Ctx.m_instance.m_timerMgr.addObject(timer);
        }

        public void setpic(Material m)
        {
#if UNITY_5
            transform.FindChild("pic").GetComponent<Renderer>().material = m;
#elif UNITY_4_6
            transform.FindChild("pic").renderer.material = m;
#endif
        }

        public void banpick()
        {
            Ctx.m_instance.m_camSys.m_dzCam.banpick();
        }

        //第一次抽,向主摄像发送消息
        public void firstdarw()
        {
            Ctx.m_instance.m_camSys.m_dzCam.firstdarw();
        }

        // 隐藏 VS 图标
        public void hidevs(TimerItemBase timer)
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