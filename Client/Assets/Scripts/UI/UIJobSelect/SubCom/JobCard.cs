using SDK.Common;
using SDK.Lib;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
        protected AuxDynImageDynGO m_jobCardBtn;

        public JobCard(JobSelectData data, int tag_, EnPlayerCareer ccc):
            base(data)
        {
            m_tag = tag_;
            m_career = ccc;
            m_jobCardBtn = new AuxDynImageDynGO();
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
            TableItemBase tableItem = null;
            TableJobItemBody tableJobItemBody = null;
            string jobPath = "";

            tableItem = Ctx.m_instance.m_tableSys.getItem(TableID.TABLE_JOB, (uint)m_career);
            if (tableItem != null)
            {
                tableJobItemBody = tableItem.m_itemBody as TableJobItemBody;
                jobPath = string.Format("{0}UIJobSelect/JobSelCard.prefab", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI]);
                m_jobCardBtn.setImageInfo("Atlas/JobSelectDyn.asset", "emei_zhiyepai");
                m_jobCardBtn.prefabPath = jobPath;
                m_jobCardBtn.loadPrefab();
                m_jobCardBtn.updateImage();

                m_jobCardBtn.selfGo.name = string.Format("JobSelCard_{0}", career);
            }
        }

        public void add2LayoutH(AuxLayoutH layout)
        {
            layout.addElem(m_jobCardBtn.selfGo);
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
            int idx = 0;
            if (m_jobSelectData.m_midPnl.curSelJobCard != null)
            {
                idx = m_jobSelectData.m_midPnl.getIdxByCareerID(m_jobSelectData.m_midPnl.curSelJobCard.career);
                m_jobSelectData.m_midPnl.toggleJob(idx, false);
            }
            m_jobSelectData.m_midPnl.curSelJobCard = this;
            m_jobSelectData.m_midPnl.toggleJob(m_tag, true);

            m_jobSelectData.m_rightPnl.m_jobSelProg.update();
        }
    }
}