﻿using Game.Msg;
using SDK.Lib;

namespace Game.UI
{
    public class JobSelRightPnl : JobSelPnlBase
    {
        public JobSelProg m_jobSelProg;
        protected AuxLabel m_jobLvl;
        protected AuxLabel m_skillName;
        protected AuxLabel m_skillDesc;
        protected AuxDynImageStaticGOImage m_skillImage;

        public JobSelRightPnl(JobSelectData data) :
            base(data)
        {
            m_jobSelProg = new JobSelProg(m_jobSelectData);

            m_skillImage = new AuxDynImageStaticGOImage();
        }

        public new void findWidget()
        {
            m_jobSelProg.findWidget();

            m_jobLvl = new AuxLabel(m_jobSelectData.m_form.m_guiWin.m_uiRoot, JobSelectPath.TextJobLvl);
            m_skillName = new AuxLabel(m_jobSelectData.m_form.m_guiWin.m_uiRoot, JobSelectPath.TextSkillName);
            m_skillDesc = new AuxLabel(m_jobSelectData.m_form.m_guiWin.m_uiRoot, JobSelectPath.TextSkillDesc);

            m_skillImage.selfGo = UtilApi.TransFindChildByPObjAndPath(m_jobSelectData.m_form.m_guiWin.m_uiRoot, JobSelectPath.ImageSkillIcon);
        }

        public new void addEventHandle()
        {
            UtilApi.addEventHandle(m_jobSelectData.m_form.m_guiWin.m_uiRoot, JobSelectPath.BtnSel, onSelBtnClk);
            m_jobSelProg.addEventHandle();
        }

        public new void init()
        {
            m_jobSelProg.init();
        }

        override public void dispose()
        {
            base.dispose();
            Ctx.m_instance.m_dataPlayer.m_dzData.m_canReqDZ = true;
            if(m_skillImage != null)
            {
                m_skillImage.dispose();
            }
        }

        public void onSelBtnClk()
        {
            if (JobSelectMode.eNewCardSet == Ctx.m_instance.m_auxUIHelp.m_auxJobSelectData.jobSelectMode)
            {
                if (m_jobSelectData.m_midPnl.curSelJobCard != null)       // 如果有选择
                {
                    stReqCreateOneCardGroupUserCmd cmd = new stReqCreateOneCardGroupUserCmd();
                    cmd.occupation = (uint)m_jobSelectData.m_midPnl.curSelJobCard.career;
                    UtilMsg.sendMsg(cmd);
                }

                m_jobSelectData.m_form.exit();
            }
            if (JobSelectMode.eDz == Ctx.m_instance.m_auxUIHelp.m_auxJobSelectData.jobSelectMode)
            {
                if (m_jobSelectData.m_midPnl.curSelJobCard != null)      // 如果有选中
                {
                    if (Ctx.m_instance.m_dataPlayer.m_dzData.m_canReqDZ)
                    {
                        Ctx.m_instance.m_dataPlayer.m_dzData.m_canReqDZ = false;
                        Ctx.m_instance.m_dataPlayer.m_dzData.setSelfHeroInfo(m_jobSelectData.m_midPnl.curSelJobCard.cardGroupItem);

                        stReqHeroFightMatchUserCmd cmd = new stReqHeroFightMatchUserCmd();
                        cmd.index = m_jobSelectData.m_midPnl.curSelJobCard.cardGroupItem.m_cardGroup.index;
                        UtilMsg.sendMsg(cmd);

                        //开始查找
                        m_jobSelectData.m_midPnl.startmatch();
                    }
                }
                if (MacroDef.DEBUG_NOTNET)
                {
                    // test
                    m_jobSelectData.m_midPnl.startmatch();
                }
            }
        }

        public void toggleJob(int id, TableJobItemBody tableJobItemBody)
        {
            HeroItem jobInfo = Ctx.m_instance.m_dataPlayer.m_dataHero.getJobInfo(id);
            if (jobInfo != null)
            {
                m_jobLvl.text = string.Format("{0}等级:{1}", tableJobItemBody.m_jobName, jobInfo.m_svrHero.level);
            }
            else
            {
                m_jobLvl.text = string.Format("{0}等级:{1}", tableJobItemBody.m_jobName, 0);
            }

            m_skillName.text = tableJobItemBody.m_skillName;
            m_skillDesc.text = tableJobItemBody.m_skillDesc;

            m_skillImage.setImageInfo(CVAtlasName.JobSelectDyn, tableJobItemBody.m_skillRes);
            m_skillImage.syncUpdateCom();
        }
    }
}