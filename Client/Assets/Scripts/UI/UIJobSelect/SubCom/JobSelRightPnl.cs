using Game.Msg;
using SDK.Common;
using SDK.Lib;
using UnityEngine.UI;

namespace Game.UI
{
    public class JobSelRightPnl : JobSelPnlBase
    {
        public JobSelProg m_jobSelProg;
        protected Text m_jobLvl;
        protected Text m_skillName;
        protected Text m_skillDesc;
        protected AuxDynImageStaticGO m_skillImage;

        public JobSelRightPnl(JobSelectData data) :
            base(data)
        {
            m_jobSelProg = new JobSelProg(m_jobSelectData);

            m_skillImage = new AuxDynImageStaticGO();
        }

        public new void findWidget()
        {
            m_jobSelProg.findWidget();

            m_jobLvl = UtilApi.getComByP<Text>(m_jobSelectData.m_form.m_GUIWin.m_uiRoot, JobSelectPath.TextJobLvl);
            m_skillName = UtilApi.getComByP<Text>(m_jobSelectData.m_form.m_GUIWin.m_uiRoot, JobSelectPath.TextSkillName);
            m_skillDesc = UtilApi.getComByP<Text>(m_jobSelectData.m_form.m_GUIWin.m_uiRoot, JobSelectPath.TextSkillDesc);

            m_skillImage.pntGo = UtilApi.TransFindChildByPObjAndPath(m_jobSelectData.m_form.m_GUIWin.m_uiRoot, JobSelectPath.ImageSkillIcon);
            m_skillImage.findWidget();
        }

        public new void addEventHandle()
        {
            UtilApi.addEventHandle(m_jobSelectData.m_form.m_GUIWin.m_uiRoot, JobSelectPath.BtnSel, onSelBtnClk);
            m_jobSelProg.addEventHandle();
        }

        public new void init()
        {
            m_jobSelProg.init();
        }

        override public void dispose()
        {
            base.dispose();

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
                // test
#if DEBUG_NOTNET
                uiMS.startmatch();
#endif
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
            m_skillImage.updateImage();
        }
    }
}