using SDK.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    /**
     * @brief 场景中的提示
     */
    public class UISceneTips : SceneForm
    {
        protected GameObject m_goRoot;
        protected Text m_desc;

        public override void onReady()
        {
            base.onReady();
            m_goRoot = UtilApi.GoFindChildByPObjAndName("SceneTipsUI");
            m_desc = UtilApi.getComByP<Text>(m_goRoot, "Canvas/Text");

            m_goRoot.SetActive(false);         // 默认隐藏
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

        public void showTips(Vector3 pos, string desc)
        {
            m_desc.text = desc;
            m_goRoot.transform.localPosition = pos;
            m_goRoot.SetActive(true);
        }
    }
}