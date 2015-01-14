using Game.Msg;
using SDK.Common;
using SDK.Lib;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    /**
     * @brief 对战界面
     */
    public class UISceneDZ : SceneForm
    {
        public dzturn m_dzturn = new dzturn();          // 翻转按钮
        public hero m_self = new hero();
        public hero m_enemy = new hero();
        public luckycoin m_luckycoin = new luckycoin();

        public override void onReady()
        {
            base.onReady();
        }

        public override void onShow()
        {
            base.onShow();
        }

        // 获取控件
        protected void getWidget()
        {
            m_dzturn.setGameObject(UtilApi.GoFindChildByPObjAndName("turn"));
            m_self.setGameObject(UtilApi.GoFindChildByPObjAndName("hero"));
            m_enemy.setGameObject(UtilApi.GoFindChildByPObjAndName("enemyhero"));
            m_luckycoin.setGameObject(UtilApi.GoFindChildByPObjAndName("luckycoin"));
        }
    }
}