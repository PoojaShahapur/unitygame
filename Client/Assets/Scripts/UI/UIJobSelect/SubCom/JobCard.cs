using SDK.Common;
using SDK.Lib;
using UnityEngine.EventSystems;

namespace Game.UI
{
    /**
     * @brief 中间的一个职业卡牌
     */
    public class JobCard : JobSelPnlBase
    {
        protected int m_tag;        // 索引
        protected EnPlayerCareer m_career;  // 职业
        
        protected CardGroupItem m_cardGroupItem;
        protected AuxDynImageDynGOImage m_jobCardBtn;
        protected AuxLabel m_cardSetCardNumText;

        public JobCard(JobSelectData data, int tag_, EnPlayerCareer ccc):
            base(data)
        {
            m_tag = tag_;
            m_career = ccc;
            m_jobCardBtn = new AuxDynImageDynGOImage();
        }

        public EnPlayerCareer career
        {
            get
            {
                return m_career;
            }
            set
            {
                m_career = value;
            }
        }

        public CardGroupItem cardGroupItem
        {
            get
            {
                return m_cardGroupItem;
            }
            set
            {
                m_cardGroupItem = value;
            }
        }

        public void initJobCard()
        {
            m_jobCardBtn.pntGo = UtilApi.TransFindChildByPObjAndPath(m_jobSelectData.m_form.m_GUIWin.m_uiRoot, JobSelectPath.ScrollCont);

            TableItemBase tableItem = null;
            TableJobItemBody tableJobItemBody = null;
            string jobPath = "";

            tableItem = Ctx.m_instance.m_tableSys.getItem(TableID.TABLE_JOB, (uint)m_career);
            if (tableItem != null)
            {
                tableJobItemBody = tableItem.m_itemBody as TableJobItemBody;
                jobPath = string.Format("{0}UIJobSelect/JobSelCard.prefab", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI]);
                m_jobCardBtn.setImageInfo(CVAtlasName.JobSelectDyn, tableJobItemBody.m_jobRes);
                m_jobCardBtn.prefabPath = jobPath;
                m_jobCardBtn.syncUpdateCom();

                m_jobCardBtn.selfGo.name = string.Format("JobSelCard_{0}", career);
                m_cardSetCardNumText = new AuxLabel(m_jobCardBtn.selfGo, JobSelectPath.CardSetCardNumText);
                m_cardSetCardNumText.text = string.Format("{0}/30", m_cardGroupItem.m_cardGroup.cardNum);
            }
        }

        public void add2LayoutH(AuxLayoutH layout)
        {
            layout.addElem(m_jobCardBtn.selfGo, true);
        }

        public new void findWidget()
        {
            
        }

        public new void addEventHandle()
        {
            UtilApi.addEventHandle(m_jobCardBtn.selfGo, onJobSelBtnClk);
        }

        public void onJobSelBtnClk()
        {
            if (m_jobSelectData.m_midPnl.curSelJobCard == null || !m_jobSelectData.m_midPnl.curSelJobCard.Equals(this))
            {
                m_jobSelectData.m_midPnl.curSelJobCard = this;
                m_jobSelectData.m_midPnl.toggleJob(m_tag);

                m_jobSelectData.m_rightPnl.m_jobSelProg.update();
            }
        }
    }
}