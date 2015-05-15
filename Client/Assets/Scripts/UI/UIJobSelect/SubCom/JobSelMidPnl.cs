using SDK.Common;
using SDK.Lib;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class JobSelMidPnl : JobSelPnlBase
    {
        protected List<JobCard> m_jobCardList;
        protected JobSelProg m_jobSelProg;
        protected EnPlayerCareer m_curSelCareer;        // 当前选择职业索引
        protected AuxLayoutH m_auxLayoutH;              // 中间滚动条
        protected Text m_jobText;       // 职业描述

        public JobSelMidPnl(JobSelectData data) :
            base(data)
        {
            m_curSelCareer = EnPlayerCareer.HERO_OCCUPATION_NONE;

            m_jobCardList = new List<JobCard>();
            for (int idx = 0; idx < (int)EnPlayerCareer.ePCTotal - 1; ++idx)
            {
                m_jobCardList.Add(new JobCard(m_jobSelectData, idx, (EnPlayerCareer)(idx + 1)));
            }

            m_jobSelProg = new JobSelProg(m_jobSelectData);
            m_auxLayoutH = new AuxLayoutH();
            m_auxLayoutH.elemWidth = 445;
            m_auxLayoutH.elemHeight = 500;
        }

        public EnPlayerCareer curSelCareer
        {
            get
            {
                return m_curSelCareer;
            }
            set
            {
                m_curSelCareer = value;
            }
        }

        public new void findWidget()
        {
            // 获取 GO ， initJobRes 里面要用到
            m_auxLayoutH.pntGo = UtilApi.TransFindChildByPObjAndPath(m_jobSelectData.m_form.m_GUIWin.m_uiRoot, JobSelectPath.ScrollCont);
            // 将职业选择的卡牌添加进去
            initJobRes();

            for (int idx = 0; idx < (int)EnPlayerCareer.ePCTotal - 1; ++idx)
            {
                m_jobCardList[idx].findWidget();
            }

            m_jobSelProg.findWidget();
            m_jobText = UtilApi.getComByP<Text>(m_jobSelectData.m_form.m_GUIWin.m_uiRoot, JobSelectPath.TextJobDesc);
        }

        public new void addEventHandle()
        {
            for (int idx = 0; idx < (int)EnPlayerCareer.ePCTotal - 1; ++idx)
            {
                m_jobCardList[idx].addEventHandle();
            }
        }

        public new void init()
        {
            for (int idx = 0; idx < (int)EnPlayerCareer.ePCTotal - 1; ++idx)
            {
                m_jobCardList[idx].init();
            }
        }

        protected void initJobRes()
        {
            TableItemBase tableItem = null;
            TableJobItemBody tableJobItemBody = null;
            string jobPath = "";
            GameObject _go = null;
            // 添加职业，从 1 开始
            for (int idx = 0; idx < (int)EnPlayerCareer.ePCTotal - 1; ++idx)
            {
                tableItem = Ctx.m_instance.m_tableSys.getItem(TableID.TABLE_JOB, (uint)m_jobCardList[idx].career);
                if (tableItem != null)
                {
                    tableJobItemBody = tableItem.m_itemBody as TableJobItemBody;
                    jobPath = string.Format("{0}UIJobSelect/JobSelCard_{1}.prefab", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI], 3);
                    _go = Ctx.m_instance.m_uiPrefabMgr.syncGet<UIPrefabRes>(jobPath).InstantiateObject(jobPath);
                    _go.name = string.Format("JobSelCard_{0}", (uint)m_jobCardList[idx].career);
                    m_auxLayoutH.addElem(_go);
                }
            }
        }

        public void toggleJob(int idx, bool bShow)
        {
            m_jobCardList[idx].toggleCardNameImage(bShow);

            TableItemBase tableItem;
            TableJobItemBody tableJobItemBody = null;
            if(bShow)
            {
                tableItem = Ctx.m_instance.m_tableSys.getItem(TableID.TABLE_JOB, (uint)m_jobCardList[idx].career);
                if (tableItem != null)
                {
                    tableJobItemBody = tableItem.m_itemBody as TableJobItemBody;
                    m_jobText.text = tableJobItemBody.m_jobDesc;

                    m_jobSelectData.m_rightPnl.toggleJob((int)(m_jobCardList[idx].career), tableJobItemBody);
                }
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
    }
}