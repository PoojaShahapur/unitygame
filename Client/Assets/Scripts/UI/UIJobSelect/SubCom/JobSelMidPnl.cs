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
        protected AuxLayoutH m_auxLayoutH;              // 中间滚动条
        protected Text m_jobText;       // 职业描述
        protected JobCard m_curSelJobCard;        // 点击的卡牌

        protected int m_cardCount;              // 中间选择职业的数量
        protected AuxDynImageStaticGO m_jobImage;

        public JobSelMidPnl(JobSelectData data) :
            base(data)
        {
            m_jobCardList = new List<JobCard>();
            if (JobSelectMode.eNewCardSet == Ctx.m_instance.m_auxUIHelp.m_auxJobSelectData.jobSelectMode)
            {
                m_cardCount = (int)EnPlayerCareer.ePCTotal - 1;
            }
            else if (JobSelectMode.eDz == Ctx.m_instance.m_auxUIHelp.m_auxJobSelectData.jobSelectMode)
            {
                m_cardCount = Ctx.m_instance.m_dataPlayer.m_dataCard.m_cardGroupListArr.Count;
            }

            for (int idx = 0; idx < m_cardCount; ++idx)
            {
                m_jobCardList.Add(new JobCard(m_jobSelectData, idx, (EnPlayerCareer)(idx + 1)));
                if (JobSelectMode.eDz == Ctx.m_instance.m_auxUIHelp.m_auxJobSelectData.jobSelectMode)
                {
                    m_jobCardList[idx].cardGroupItem = Ctx.m_instance.m_dataPlayer.m_dataCard.m_cardGroupListArr[idx];
                }
            }

            m_jobSelProg = new JobSelProg(m_jobSelectData);
            m_auxLayoutH = new AuxLayoutH();
            m_auxLayoutH.elemWidth = 445;
            m_auxLayoutH.elemHeight = 500;

            m_jobImage = new AuxDynImageStaticGO();
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
            m_auxLayoutH.pntGo = UtilApi.TransFindChildByPObjAndPath(m_jobSelectData.m_form.m_GUIWin.m_uiRoot, JobSelectPath.ScrollContParent);
            m_auxLayoutH.contentGo = UtilApi.TransFindChildByPObjAndPath(m_jobSelectData.m_form.m_GUIWin.m_uiRoot, JobSelectPath.ScrollCont);
            if (JobSelectMode.eNewCardSet == Ctx.m_instance.m_auxUIHelp.m_auxJobSelectData.jobSelectMode)
            {
                // 将职业选择的卡牌添加进去
                initJobRes();
            }
            else if (JobSelectMode.eDz == Ctx.m_instance.m_auxUIHelp.m_auxJobSelectData.jobSelectMode)
            {
                // 将职业选择的卡牌添加进去
                initCardSet();
            }

            for (int idx = 0; idx < m_cardCount; ++idx)
            {
                m_jobCardList[idx].findWidget();
            }

            m_jobSelProg.findWidget();
            m_jobText = UtilApi.getComByP<Text>(m_jobSelectData.m_form.m_GUIWin.m_uiRoot, JobSelectPath.TextJobDesc);

            m_jobImage.pntGo = UtilApi.TransFindChildByPObjAndPath(m_jobSelectData.m_form.m_GUIWin.m_uiRoot, JobSelectPath.ImageJobName);
            m_jobImage.findWidget();
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
        }

        override public void dispose()
        {
            base.dispose();

            if (m_jobImage != null)
            {
                m_jobImage.dispose();
            }
        }

        protected void initJobRes()
        {
            // 添加职业，从 1 开始
            for (int idx = 0; idx < m_cardCount; ++idx)
            {
                addOndCard((uint)m_jobCardList[idx].career);
            }
        }

        protected void initCardSet()
        {
            foreach(CardGroupItem item in Ctx.m_instance.m_dataPlayer.m_dataCard.m_cardGroupListArr)
            {
                addOndCard(item.m_cardGroup.occupation);
            }
        }

        protected void addOndCard(uint career)
        {
            TableItemBase tableItem = null;
            TableJobItemBody tableJobItemBody = null;
            string jobPath = "";
            GameObject _go = null;

            tableItem = Ctx.m_instance.m_tableSys.getItem(TableID.TABLE_JOB, career);
            if (tableItem != null)
            {
                tableJobItemBody = tableItem.m_itemBody as TableJobItemBody;
                jobPath = string.Format("{0}UIJobSelect/JobSelCard_{1}.prefab", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI], 3);
                _go = Ctx.m_instance.m_uiPrefabMgr.syncGet<UIPrefabRes>(jobPath).InstantiateObject(jobPath);
                _go.name = string.Format("JobSelCard_{0}", career);
                m_auxLayoutH.addElem(_go);
            }
        }

        public void toggleJob(int idx, bool bShow)
        {
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
                    m_jobImage.setImageInfo("Atlas/JobSelectDyn.asset", tableJobItemBody.m_jobRes);
                    m_jobImage.updateImage();
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

        public void startmatch()
        {
            //m_lblTip.text = "现在开始匹配了，注意了";
            // test 进入战场
#if DEBUG_NOTNET
            Ctx.m_instance.m_gameSys.loadDZScene(1000);
#endif
        }
    }
}