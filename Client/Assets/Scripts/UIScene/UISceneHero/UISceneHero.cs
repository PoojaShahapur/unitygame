using Game.Msg;
using SDK.Common;
using SDK.Lib;
using UnityEngine;

namespace Game.UI
{
    /**
     * @brief hero 界面
     */
    public class UISceneHero : SceneForm
    {
        public SceneHeroData m_sceneHeroData;
        public SceneHeroCenteArear m_sceneHeroCenteArear;   // 之间区域

        public override void onReady()
        {
            base.onReady();
            m_sceneHeroData = new SceneHeroData();

            getWidget();
            m_sceneHeroCenteArear = new SceneHeroCenteArear(m_sceneHeroData);

            addEventHandle();
        }

        public override void onShow()
        {
            base.onShow();
            // 显示背景
            UISceneBg uiSB = Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneBg) as UISceneBg;
            if (uiSB == null)
            {
                Ctx.m_instance.m_uiSceneMgr.loadSceneForm<UISceneBg>(UISceneFormID.eUISceneBg);
            }
            Ctx.m_instance.m_uiSceneMgr.showSceneForm(UISceneFormID.eUISceneBg);

            m_sceneHeroData.m_goRoot.SetActive(true);
            updateAllHero();
        }

        public override void onHide()
        {
            base.onHide();

            m_sceneHeroData.m_goRoot.SetActive(false);
        }

        protected void getWidget()
        {
            m_sceneHeroData.m_goRoot = UtilApi.GoFindChildByPObjAndName("HeroSceneUI");
            m_sceneHeroData.m_goRoot.SetActive(false);         // 默认隐藏
        }

        // 添加事件监听
        protected void addEventHandle()
        {
            
        }

        // 更新所有的 hero
        public void updateAllHero()
        {
            m_sceneHeroCenteArear.updateAllHero();
        }
    }
}