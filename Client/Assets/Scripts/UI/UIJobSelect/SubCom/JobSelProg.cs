using SDK.Common;
using UnityEngine;
using UnityEngine.UI;
namespace Game.UI
{
    /**
     * @brief 进度条
     */
    public class JobSelProg : JobSelPnlBase
    {
        protected Text m_progText;
        protected Image m_maskImage;
        protected RectTransform m_trans;

        public JobSelProg(JobSelectData data):
            base(data)
        {

        }

        public new void findWidget()
        {
            m_maskImage = UtilApi.getComByP<Image>(m_jobSelectData.m_form.m_GUIWin.m_uiRoot, JobSelectPath.ProgMaskImage);
            m_trans = UtilApi.getComByP<RectTransform>(m_jobSelectData.m_form.m_GUIWin.m_uiRoot, JobSelectPath.ProgMaskImage);
            m_progText = UtilApi.getComByP<Text>(m_jobSelectData.m_form.m_GUIWin.m_uiRoot, JobSelectPath.ProgText);
        }

        public new void init()
        {
            m_progText.text = "";
        }

        public void update()
        {
            int cnt = Ctx.m_instance.m_dataPlayer.m_dataCard.m_cardListArr[(int)(m_jobSelectData.m_midPnl.curSelJobCard.career)].Count;
            m_progText.text = string.Format("{0}/30", cnt);

            int wid = (int)(((float)cnt / 30) * 260);
            //m_maskImage.rectTransform.sizeDelta = new UnityEngine.Vector2(50, 27);
            m_trans.sizeDelta = new UnityEngine.Vector2(wid, 27);
        }
    }
}