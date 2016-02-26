using Game.Msg;
using SDK.Lib;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class JobSelMidPnl : JobSelPnlBase
    {
        protected List<JobCard> m_jobCardList;
        protected JobSelProg m_jobSelProg;
        protected AuxLayoutH m_auxLayoutH;              // 中间滚动条
        protected AuxLabel m_jobText;       // 职业描述
        protected JobCard m_curSelJobCard;        // 点击的卡牌

        protected int m_cardCount;              // 中间选择职业的数量
        protected AuxDynImageStaticGOImage m_jobNameImage;
        //protected AuxLabel m_dzStartDescText;
        protected SpriteAni m_spriteAni;        //匹配中动画
        protected SpriteAni m_spriteAni2;        //匹配中动画
        protected GameObject m_imageGo;
        protected GameObject m_imageGo2;
        protected GameObject m_imageMatchBg;
        protected GameObject m_bgPanel;
        protected stRetHeroIntoBattleSceneUserCmd m_cmd;

        public JobSelMidPnl(JobSelectData data) :
            base(data)
        {
            m_jobCardList = new List<JobCard>();
            if (JobSelectMode.eNewCardSet == Ctx.m_instance.m_auxUIHelp.m_auxJobSelectData.jobSelectMode)
            {
                m_cardCount = (int)EnPlayerCareer.ePCTotal - 1;     // 没有中立
            }
            else if (JobSelectMode.eDz == Ctx.m_instance.m_auxUIHelp.m_auxJobSelectData.jobSelectMode)
            {
                m_cardCount = Ctx.m_instance.m_dataPlayer.m_dataCard.m_cardGroupListArr.Count;
            }

            for (int idx = 0; idx < m_cardCount; ++idx)
            {
                if (JobSelectMode.eNewCardSet == Ctx.m_instance.m_auxUIHelp.m_auxJobSelectData.jobSelectMode)
                {
                    m_jobCardList.Add(new JobCard(m_jobSelectData, idx, (EnPlayerCareer)(idx + 1)));    // 职业从 1 开始
                }
                else if (JobSelectMode.eDz == Ctx.m_instance.m_auxUIHelp.m_auxJobSelectData.jobSelectMode)
                {
                    m_jobCardList.Add(new JobCard(m_jobSelectData, idx, (EnPlayerCareer)Ctx.m_instance.m_dataPlayer.m_dataCard.m_cardGroupListArr[idx].m_cardGroup.occupation));
                    m_jobCardList[idx].cardGroupItem = Ctx.m_instance.m_dataPlayer.m_dataCard.m_cardGroupListArr[idx];
                }
            }

            m_jobSelProg = new JobSelProg(m_jobSelectData);
            m_auxLayoutH = new AuxLayoutH();
            m_auxLayoutH.elemWidth = 445;
            m_auxLayoutH.elemHeight = 500;

            m_jobNameImage = new AuxDynImageStaticGOImage();
        }

        public JobCard curSelJobCard
        {
            get
            {
                return m_curSelJobCard;
            }
            set
            {
                m_curSelJobCard = value;
            }
        }

        public new void findWidget()
        {
            // 获取 GO ， initJobRes 里面要用到
            m_auxLayoutH.pntGo = UtilApi.TransFindChildByPObjAndPath(m_jobSelectData.m_form.m_guiWin.m_uiRoot, JobSelectPath.ScrollContParent);
            m_auxLayoutH.selfGo = UtilApi.TransFindChildByPObjAndPath(m_jobSelectData.m_form.m_guiWin.m_uiRoot, JobSelectPath.ScrollCont);

            Ctx.m_instance.m_logSys.log(string.Format("添加 {0} 卡组数据", m_cardCount));

            for (int idx = 0; idx < m_cardCount; ++idx)
            {
                m_jobCardList[idx].initJobCard();
                m_jobCardList[idx].add2LayoutH(m_auxLayoutH);
                m_jobCardList[idx].findWidget();
            }

            m_jobSelProg.findWidget();
            m_jobText = new AuxLabel(m_jobSelectData.m_form.m_guiWin.m_uiRoot, JobSelectPath.TextJobDesc);

            m_jobNameImage.selfGo = UtilApi.TransFindChildByPObjAndPath(m_jobSelectData.m_form.m_guiWin.m_uiRoot, JobSelectPath.ImageJobName);

            //m_dzStartDescText = new AuxLabel(m_jobSelectData.m_form.m_guiWin.m_uiRoot, JobSelectPath.DzStartDescText);
            //m_dzStartDescText.hide();     // 默认隐藏
            m_imageGo = UtilApi.TransFindChildByPObjAndPath(m_jobSelectData.m_form.m_guiWin.m_uiRoot, JobSelectPath.DzStartMatch);
            UtilApi.SetActive(m_imageGo, false);
            m_imageGo2 = UtilApi.TransFindChildByPObjAndPath(m_jobSelectData.m_form.m_guiWin.m_uiRoot, JobSelectPath.DzStartMatch2);
            UtilApi.SetActive(m_imageGo2, false);
            m_imageMatchBg = UtilApi.TransFindChildByPObjAndPath(m_jobSelectData.m_form.m_guiWin.m_uiRoot, JobSelectPath.DzStartMatchBg);
            UtilApi.SetActive(m_imageMatchBg, false);
            m_bgPanel = UtilApi.TransFindChildByPObjAndPath(m_jobSelectData.m_form.m_guiWin.m_uiRoot, JobSelectPath.bgPanel);
            UtilApi.SetActive(m_bgPanel, false);
        }

        public new void addEventHandle()
        {
            for (int idx = 0; idx < m_cardCount; ++idx)
            {
                m_jobCardList[idx].addEventHandle();
            }
        }

        public new void init()
        {
            for (int idx = 0; idx < m_cardCount; ++idx)
            {
                m_jobCardList[idx].init();
            }

            setDefaultCard();
        }

        override public void dispose()
        {
            base.dispose();

            if (m_jobNameImage != null)
            {
                m_jobNameImage.dispose();
            }

            if(m_spriteAni != null)
            {
                m_spriteAni.dispose();
                m_spriteAni = null;
            }

            if (m_spriteAni2 != null)
            {
                m_spriteAni2.dispose();
                m_spriteAni2 = null;
            }

            foreach(var ele in m_jobCardList)
            {
                ele.dispose();
            }
        }

        public void toggleJob(int idx)
        {
            TableItemBase tableItem;
            TableJobItemBody tableJobItemBody = null;

            tableItem = Ctx.m_instance.m_tableSys.getItem(TableID.TABLE_JOB, (uint)m_jobCardList[idx].career);
            if (tableItem != null)
            {
                tableJobItemBody = tableItem.m_itemBody as TableJobItemBody;
                m_jobText.text = tableJobItemBody.m_jobDesc;

                m_jobSelectData.m_rightPnl.toggleJob((int)(m_jobCardList[idx].career), tableJobItemBody);
                m_jobNameImage.setImageInfo(CVAtlasName.JobSelectDyn, tableJobItemBody.m_jobNameRes);
                m_jobNameImage.syncUpdateCom();
            }
        }

        public int getIdxByCareerID(EnPlayerCareer careerID)
        {
            for(int idx = 0; idx < m_jobCardList.Count; ++idx)
            {
                if(m_jobCardList[idx].career == careerID)
                {
                    return idx;
                }
            }

            return 0;
        }

        public void startmatch()
        {
            //m_dzStartDescText.text = "开始匹配中";
            //m_dzStartDescText.show();     // 默认隐藏
            m_bgPanel = UtilApi.TransFindChildByPObjAndPath(m_jobSelectData.m_form.m_guiWin.m_uiRoot, JobSelectPath.bgPanel);
            UtilApi.SetActive(m_bgPanel, true);
            m_imageMatchBg = UtilApi.TransFindChildByPObjAndPath(m_jobSelectData.m_form.m_guiWin.m_uiRoot, JobSelectPath.DzStartMatchBg);
            UtilApi.SetActive(m_imageMatchBg, true);

            UtilApi.SetActive(m_imageGo, true);
            m_spriteAni = Ctx.m_instance.m_spriteAniMgr.createAndAdd();
            m_spriteAni.selfGo = m_imageGo;
            m_spriteAni.tableID = 8;
            m_spriteAni.bLoop = true;
            m_spriteAni.play();

            UtilApi.SetActive(m_imageGo2, true);
            m_spriteAni2 = Ctx.m_instance.m_spriteAniMgr.createAndAdd();
            m_spriteAni2.selfGo = m_imageGo2;
            m_spriteAni2.tableID = 9;
            m_spriteAni2.bLoop = true;
            m_spriteAni2.play();
            // test 进入战场
            if (!Config.DEBUG_NOTNET)
            {
                Ctx.m_instance.m_gameSys.loadDZScene(1);
            }
        }

        public void matchSuccess()
        {
            m_spriteAni.stop();
            m_spriteAni2.stop();
            UtilApi.SetActive(m_imageGo, true);
            m_spriteAni.selfGo = m_imageGo;
            m_spriteAni.tableID = 10;
            m_spriteAni.bLoop = false;
            m_spriteAni.play();
            m_spriteAni.playEndEventDispatch.addEventHandle(gotoBattle);
        }

        public void gotoBattle(IDispatchObject dispObj)
        {
            Ctx.m_instance.m_gameSys.loadDZScene(m_cmd.sceneNumber);
        }

        public void updateHeroList()
        {
            if (JobSelectMode.eDz == Ctx.m_instance.m_auxUIHelp.m_auxJobSelectData.jobSelectMode)
            {
                m_cardCount = Ctx.m_instance.m_dataPlayer.m_dataCard.m_cardGroupListArr.Count;
            }

            for (int idx = 0; idx < m_cardCount; ++idx)
            {
                m_jobCardList.Add(new JobCard(m_jobSelectData, idx, (EnPlayerCareer)Ctx.m_instance.m_dataPlayer.m_dataCard.m_cardGroupListArr[idx].m_cardGroup.occupation));
                if (JobSelectMode.eDz == Ctx.m_instance.m_auxUIHelp.m_auxJobSelectData.jobSelectMode)
                {
                    m_jobCardList[idx].cardGroupItem = Ctx.m_instance.m_dataPlayer.m_dataCard.m_cardGroupListArr[idx];
                }

                m_jobCardList[idx].initJobCard();
                m_jobCardList[idx].add2LayoutH(m_auxLayoutH);
                m_jobCardList[idx].findWidget();

                m_jobCardList[idx].addEventHandle();
            }

            setDefaultCard();
        }

        protected void setDefaultCard()
        {
            // 默认选择一个
            if (m_jobCardList.Count > 0)
            {
                m_jobCardList[0].onJobSelBtnClk();
            }
        }

        // 返回进入战斗场景消息
        public void psstRetHeroIntoBattleSceneUserCmd(ByteBuffer msg)
        {
            m_cmd = new stRetHeroIntoBattleSceneUserCmd();
            m_cmd.derialize(msg);

            matchSuccess();
        }
    }
}