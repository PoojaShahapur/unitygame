using Game.Msg;
using SDK.Common;
using UnityEngine.UI;

namespace Game.UI
{
    public class JobSelRightPnl : JobSelPnlBase
    {
        public JobSelProg m_jobSelProg;
        protected Text m_jobLvl;
        protected Text m_skillName;
        protected Text m_skillDesc;

        public JobSelRightPnl(JobSelectData data) :
            base(data)
        {
            m_jobSelProg = new JobSelProg(m_jobSelectData);
        }

        public new void findWidget()
        {
            m_jobSelProg.findWidget();

            m_jobLvl = UtilApi.getComByP<Text>(m_jobSelectData.m_form.m_GUIWin.m_uiRoot, JobSelectPath.TextJobLvl);
            m_skillName = UtilApi.getComByP<Text>(m_jobSelectData.m_form.m_GUIWin.m_uiRoot, JobSelectPath.TextSkillName);
            m_skillDesc = UtilApi.getComByP<Text>(m_jobSelectData.m_form.m_GUIWin.m_uiRoot, JobSelectPath.TextSkillDesc);
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

        public void onSelBtnClk()
        {
            if (m_jobSelectData.m_midPnl.curSelCareer != EnPlayerCareer.HERO_OCCUPATION_NONE)       // 如果有选择
            {
                stReqCreateOneCardGroupUserCmd cmd = new stReqCreateOneCardGroupUserCmd();
                cmd.occupation = (uint)m_jobSelectData.m_midPnl.curSelCareer;
                UtilMsg.sendMsg(cmd);
            }

            m_jobSelectData.m_form.exit();
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
        }
    }
}