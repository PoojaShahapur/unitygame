using FightCore;
using Game.Msg;
using SDK.Lib;
using UnityEngine;

namespace Fight
{
    /**
     * @brief 场景中的提示
     */
    public class UISceneTips : SceneForm
    {
        protected SceneTipsData m_sceneTipsData;
        protected TextTips m_textTips;
        protected HistoryTips m_historyTips;

        protected TipsItemBase m_curTips;

        public UISceneTips()
        {
            m_sceneTipsData = new SceneTipsData();
        }

        public override void onReady()
        {
            base.onReady();
            m_sceneTipsData.m_goRoot = UtilApi.GoFindChildByPObjAndName("SceneTipsUI");
            m_sceneTipsData.m_goRoot.SetActive(false);         // 默认隐藏
        }

        public override void onShow()
        {
            base.onShow();

            m_sceneTipsData.m_goRoot.SetActive(true);
        }

        public override void onHide()
        {
            base.onHide();
            // 关闭界面和显示的界面
            m_sceneTipsData.m_goRoot.SetActive(false);
            m_curTips.hide();
        }

        public void showTips(Vector3 pos, string desc)
        {
            if (m_textTips == null)
            {
                m_textTips = new TextTips(m_sceneTipsData);
                m_textTips.initWidget();
            }
            m_curTips = m_textTips;
            m_textTips.showTips(pos, desc);
        }

        public void showTips(Vector3 pos, stRetBattleHistoryInfoUserCmd data)
        {
            if (m_historyTips == null)
            {
                m_historyTips = new HistoryTips(m_sceneTipsData);
                m_historyTips.initWidget();
            }
            m_curTips = m_historyTips;
            m_historyTips.showTips(pos, data);
        }
    }
}