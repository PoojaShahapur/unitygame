using SDK.Common;
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

        public JobCard(JobSelectData data, int tag_, EnPlayerCareer ccc):
            base(data)
        {
            m_tag = tag_;
            m_career = ccc;
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

        public new void findWidget()
        {
            
        }

        public new void addEventHandle()
        {
            string objName = string.Format("{0}/JobSelCard_{1}", JobSelectPath.ScrollCont, (int)m_career);
            UtilApi.addEventHandle(m_jobSelectData.m_form.m_GUIWin.m_uiRoot, objName, onJobSelBtnClk);
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