using SDK.Common;
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
        protected AuxLabel m_dzStartDescText;

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
            m_auxLayoutH.pntGo = UtilApi.TransFindChildByPObjAndPath(m_jobSelectData.m_form.m_GUIWin.m_uiRoot, JobSelectPath.ScrollContParent);
            m_auxLayoutH.selfGo = UtilApi.TransFindChildByPObjAndPath(m_jobSelectData.m_form.m_GUIWin.m_uiRoot, JobSelectPath.ScrollCont);

            Ctx.m_instance.m_logSys.log(string.Format("添加 {0} 卡组数据", m_cardCount));

            for (int idx = 0; idx < m_cardCount; ++idx)
            {
                m_jobCardList[idx].initJobCard();
                m_jobCardList[idx].add2LayoutH(m_auxLayoutH);
                m_jobCardList[idx].findWidget();
            }

            m_jobSelProg.findWidget();
            m_jobText = new AuxLabel(m_jobSelectData.m_form.m_GUIWin.m_uiRoot, JobSelectPath.TextJobDesc);

            m_jobNameImage.selfGo = UtilApi.TransFindChildByPObjAndPath(m_jobSelectData.m_form.m_GUIWin.m_uiRoot, JobSelectPath.ImageJobName);

            m_dzStartDescText = new AuxLabel(m_jobSelectData.m_form.m_GUIWin.m_uiRoot, JobSelectPath.DzStartDescText);
            m_dzStartDescText.hide();     // 默认隐藏
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

            if (m_jobNameImage != null)
            {
                m_jobNameImage.dispose();
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
                    m_jobNameImage.setImageInfo(CVAtlasName.JobSelectDyn, tableJobItemBody.m_jobNameRes);
                    m_jobNameImage.syncUpdateCom();
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
            m_dzStartDescText.text = "开始匹配中";
            m_dzStartDescText.show();     // 默认隐藏
            // test 进入战场
#if DEBUG_NOTNET
            Ctx.m_instance.m_gameSys.loadDZScene(1000);
#endif
        }

        public void matchSuccess()
        {
            m_dzStartDescText.text = "开始成功";
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
        }
    }
}