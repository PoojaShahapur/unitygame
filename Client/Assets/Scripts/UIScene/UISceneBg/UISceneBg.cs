using SDK.Common;
using UnityEngine;

namespace Game.UI
{
    /**
     * @brief 主界面背景
     */
    public class UISceneBg : SceneForm
    {
        protected GameObject m_goRoot;

        public override void onReady()
        {
            base.onReady();
            m_goRoot = UtilApi.TransFindChildByPObjAndPath(Ctx.m_instance.m_uiMgr.m_sceneUIRootGo, "BgSceneUI");
            m_goRoot.SetActive(false);         // 默认隐藏
            UtilApi.addEventHandle(m_goRoot, onPnlClk);
        }

        public override void onShow()
        {
            base.onShow();

            m_goRoot.SetActive(true);
        }


        public override void onHide()
        {
            base.onHide();
            // 关闭界面和显示的界面
            m_goRoot.SetActive(false);
        }

        public void onPnlClk(GameObject go)
        {
            UISceneHero uiSH = Ctx.m_instance.m_uiSceneMgr.getSceneUI<UISceneHero>(UISceneFormID.eUISceneHero);
            uiSH.hide();
            hide();
        }
    }
}