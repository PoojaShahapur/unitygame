using Game.Msg;
using SDK.Common;
using SDK.Lib;
using UnityEngine;

namespace Game.UI
{
    /**
     * @brief 扩展包
     */
    public class UISceneWDSC : SceneForm, IUISceneWDSC
    {
        protected wdscjm m_wdscjm = new wdscjm();
        protected btn[] m_btnArr = new btn[(int)SceneWDSCBtnEnum.eBtnTotal];

        public override void onReady()
        {
            base.onReady();

            getWidget();
            addEventHandle();
        }

        public override void onShow()
        {
            base.onShow();
        }

        // 获取控件
        protected void getWidget()
        {
            m_wdscjm.setGameObject(UtilApi.GoFindChildByPObjAndName("wdscjm"));

            m_btnArr[(int)SceneWDSCBtnEnum.eBtnBack] = new btn();
            m_btnArr[(int)SceneWDSCBtnEnum.eBtnBack].setGameObject(UtilApi.GoFindChildByPObjAndName("wdscjm/wdscfh/btn"));
        }

        // 添加事件监听
        protected void addEventHandle()
        {
            UtilApi.addEventHandle(UtilApi.GoFindChildByPObjAndName("wdscjm/wdscfh/btn"), onBtnClkClose);
        }

        protected void onBtnClkClose(GameObject go)
        {
            //(Ctx.m_instance.m_interActiveEntityMgr.getSceneEntity("wdscjm") as wdscjm).back();
            m_wdscjm.back();
        }

        public void showUI()
        {
            m_wdscjm.show();
        }
    }
}