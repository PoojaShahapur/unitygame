using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using SDK.Common;
using SDK.Lib;

namespace Game.UI
{
    /**
     * @brief self 和 enemy 的 hero ，就是中间的 hero 
     */
    public class hero : SceneCardEntityBase
    {
        dzminion m_dzminion = new dzminion();
        //public Material zs, dz, dly, lr, ms, sq, sm, fs, ss;
        Text classs, heroname;

        public override void Start()
        {
            base.Start();
            m_dzminion.setGameObject(gameObject);

            classs = transform.FindChild("classs").GetComponent<Text>();
            heroname = transform.FindChild("name").GetComponent<Text>();
        }

        public void setclasss(EnPlayerCareer c)
        {
            setpic(Ctx.m_instance.m_matMgr.getCardGroupMatByOccup((EnPlayerCareer)c).m_mat);

            //switch (c)
            //{
            //    case EnPlayerCareer.HERO_OCCUPATION_1:
            //        classs.text = "德鲁伊";
            //        heroname.text = "玛法里奥";
            //        setpic(dly);
            //        break;

            //    case EnPlayerCareer.HERO_OCCUPATION_2:
            //        classs.text = "猎人";
            //        heroname.text = "雷克萨";
            //        setpic(lr);
            //        break;

            //    case EnPlayerCareer.HERO_OCCUPATION_3:
            //        classs.text = "法师";
            //        heroname.text = "吉安娜";
            //        setpic(fs);
            //        break;

            //    case CardClass.kpaladin:
            //        classs.text = "圣骑士";
            //        heroname.text = "乌瑟尔";
            //        setpic(sq);
            //        break;

            //    case CardClass.kpriest:
            //        classs.text = "牧师";
            //        heroname.text = "安度因";
            //        setpic(ms);
            //        break;

            //    case CardClass.krogue:
            //        classs.text = "潜行者";
            //        heroname.text = "瓦莉拉";
            //        setpic(dz);
            //        break;

            //    case CardClass.kshama:
            //        classs.text = "萨满祭司";
            //        heroname.text = "萨尔";
            //        setpic(sm);
            //        break;

            //    case CardClass.kwarlock:
            //        classs.text = "术士";
            //        heroname.text = "古尔丹";
            //        setpic(ss);
            //        break;

            //    case CardClass.kwarrior:
            //        classs.text = "战士";
            //        heroname.text = "加尔鲁什";
            //        setpic(zs);
            //        break;
            //}
            //播放动画,
            animation.Play();
            // 启动定时器
            TimerItemBase timer = new TimerItemBase();
            timer.m_internal = 1;
            timer.m_timerDisp = hidevs;
            Ctx.m_instance.m_timerMgr.addObject(timer);
        }

        public void setpic(Material m)
        {
            transform.FindChild("pic").renderer.material = m;
        }

        public void banpick()
        {
            //Camera.main.SendMessage("banpick");
            Ctx.m_instance.m_camSys.m_dzcam.banpick();
        }

        //第一次抽,向主摄像发送消息
        public void firstdarw()
        {
            //transform.SendMessage("setMine");
            //Camera.main.SendMessage("firstdarw");
            m_dzminion.setMine();
            Ctx.m_instance.m_camSys.m_dzcam.firstdarw();
        }

        public void hidevs(TimerItemBase timer)
        {
            Camera.main.transform.FindChild("vs").gameObject.SetActive(false);
        }
    }
}