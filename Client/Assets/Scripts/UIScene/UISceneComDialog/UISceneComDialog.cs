using SDK.Common;
using UnityEngine;

namespace Game.UI
{
    /**
     * @brief 主界面背景
     */
    public class UISceneComDialog : SceneForm
    {
        public yesnomsgbox m_yesnomsgbox = new yesnomsgbox();

        public override void onReady()
        {
            m_yesnomsgbox.setGameObject(UtilApi.GoFindChildByPObjAndName("mcam/yesnomsgbox"));
        }

        public override void onShow()
        {
            base.onShow();
        }


        public override void onHide()
        {
            base.onHide();
        }
    }
}