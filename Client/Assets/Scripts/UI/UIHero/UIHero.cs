using Fight;
using FightCore;
using Game.Msg;
using SDK.Common;
using SDK.Lib;
using UnityEngine;

namespace Game.UI
{
    /**
     * @brief hero 界面
     */
    public class UIHero : Form, IUIHero
    {
        public HeroData m_heroData;
        public HeroCenteArear m_heroCenteArear;   // 之间区域

        public override void onReady()
        {
            base.onReady();

            m_heroData = new HeroData();

            findWidget();
            m_heroCenteArear = new HeroCenteArear(m_heroData);

            addEventHandle();
        }

        public override void onShow()
        {
            base.onShow();
            // 显示背景
            UISceneBg uiSB = Ctx.m_instance.m_uiSceneMgr.getSceneUI<UISceneBg>(UISceneFormID.eUISceneBg);
            if (uiSB == null)
            {
                Ctx.m_instance.m_uiSceneMgr.loadSceneForm<UISceneBg>(UISceneFormID.eUISceneBg);
            }
            Ctx.m_instance.m_uiSceneMgr.showSceneForm(UISceneFormID.eUISceneBg);

            m_heroData.m_goRoot.SetActive(true);
            updateAllHero();
        }

        public override void onHide()
        {
            base.onHide();

            m_heroData.m_goRoot.SetActive(false);
        }

        protected void findWidget()
        {
            m_heroData.m_goRoot = UtilApi.TransFindChildByPObjAndPath(Ctx.m_instance.m_uiMgr.m_sceneUIRootGo, "HeroSceneUI");
            m_heroData.m_goRoot.SetActive(false);         // 默认隐藏
        }

        // 添加事件监听
        protected void addEventHandle()
        {
            
        }

        // 更新所有的 hero
        public void updateAllHero()
        {
            m_heroCenteArear.updateAllHero();
        }
    }
}