using SDK.Common;
using SDK.Lib;
using UnityEngine;

namespace Game.UI
{
    /**
     * @brief 扩展包
     */
    public class UISceneExtPack : SceneForm
    {
        protected btn[] m_btnArr = new btn[(int)SceneExtPackBtnEnum.eBtnTotal];
        protected open m_open = new open();

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
            m_open.setGameObject(UtilApi.GoFindChildByPObjAndName("open"));

            m_btnArr[(int)SceneShopBtnEnum.eBtnBuy] = new btn();
            m_btnArr[(int)SceneShopBtnEnum.eBtnBuy].setGameObject(UtilApi.GoFindChildByPObjAndName("open/3dbtn/btn"));
        }

        // 添加事件监听
        protected void addEventHandle()
        {
            UtilApi.addEventHandle(UtilApi.GoFindChildByPObjAndName("open/3dbtn/btn"), onBtnClkBack);
        }

        protected void onBtnClkBack(GameObject go)
        {
            //(Ctx.m_instance.m_interActiveEntityMgr.getSceneEntity("open") as open).goback();
            m_open.goback();
        }

        public void showUI()
        {
            m_open.show();
        }

        // 显示 5 张卡
        public void psstRetGiftBagCardsDataUserCmd(params uint[] idList)
        {
            m_open.update5Card(idList);
        }
    }
}