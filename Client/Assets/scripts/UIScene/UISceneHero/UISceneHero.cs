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
            m_sceneHeroCenteArear = new SceneHeroCenteArear(m_sceneHeroData);

            getWidget();
            addEventHandle();
        }

        public override void onShow()
        {
            m_sceneHeroData.m_goRoot.SetActive(true);
            updateAllHero();
        }

        protected void getWidget()
        {
            m_sceneHeroData.m_goRoot = UtilApi.GoFindChildByPObjAndName("HeroSceneUI");
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