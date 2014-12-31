using Game.Msg;
using SDK.Common;
using SDK.Lib;
using UnityEngine;

namespace Game.UI
{
    /**
     * @brief 扩展包
     */
    public class UISceneShop : SceneForm, IUISceneShop 
    {
        protected shop m_shop = new shop();
        protected btn[] m_btnArr = new btn[(int)SceneShopBtnEnum.eBtnTotal];
        protected shopbtn[] m_shopBtnArr = new shopbtn[(int)SceneShopShopBtnEnum.eBtnTotal];

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
            m_shop.setGameObject(UtilApi.GoFindChildByPObjAndName("mcam/shop"));

            m_btnArr[(int)SceneShopBtnEnum.eBtnBuy] = new btn();
            m_btnArr[(int)SceneShopBtnEnum.eBtnBuy].setGameObject(UtilApi.GoFindChildByPObjAndName("mcam/shop/buykuan/goldbuy"));

            m_btnArr[(int)SceneShopBtnEnum.eBtnClose] = new btn();
            m_btnArr[(int)SceneShopBtnEnum.eBtnClose].setGameObject(UtilApi.GoFindChildByPObjAndName("mcam/shop/close"));

            m_shopBtnArr[(int)SceneShopShopBtnEnum.eBtn1f] = new shopbtn();
            m_shopBtnArr[(int)SceneShopShopBtnEnum.eBtn1f].setGameObject(UtilApi.GoFindChildByPObjAndName("mcam/shop/btn/btn1"));

            m_shopBtnArr[(int)SceneShopShopBtnEnum.eBtn2f] = new shopbtn();
            m_shopBtnArr[(int)SceneShopShopBtnEnum.eBtn2f].setGameObject(UtilApi.GoFindChildByPObjAndName("mcam/shop/btn/btn2"));

            m_shopBtnArr[(int)SceneShopShopBtnEnum.eBtn7f] = new shopbtn();
            m_shopBtnArr[(int)SceneShopShopBtnEnum.eBtn7f].setGameObject(UtilApi.GoFindChildByPObjAndName("mcam/shop/btn/btn7"));

            m_shopBtnArr[(int)SceneShopShopBtnEnum.eBtn15f] = new shopbtn();
            m_shopBtnArr[(int)SceneShopShopBtnEnum.eBtn15f].setGameObject(UtilApi.GoFindChildByPObjAndName("mcam/shop/btn/btn15"));

            m_shopBtnArr[(int)SceneShopShopBtnEnum.eBtn40f] = new shopbtn();
            m_shopBtnArr[(int)SceneShopShopBtnEnum.eBtn40f].setGameObject(UtilApi.GoFindChildByPObjAndName("mcam/shop/btn/btn40"));
        }

        // 添加事件监听
        protected void addEventHandle()
        {
            UtilApi.addEventHandle(UtilApi.GoFindChildByPObjAndName("mcam/shop/close"), onBtnClkClose);
            UtilApi.addEventHandle(UtilApi.GoFindChildByPObjAndName("mcam/shop/buykuan/goldbuy"), onBtnClkBuy);

            UtilApi.addEventHandle(UtilApi.GoFindChildByPObjAndName("mcam/shop/btn/btn1"), onBtnClk1f);
            UtilApi.addEventHandle(UtilApi.GoFindChildByPObjAndName("mcam/shop/btn/btn2"), onBtnClk2f);
            UtilApi.addEventHandle(UtilApi.GoFindChildByPObjAndName("mcam/shop/btn/btn7"), onBtnClk7f);
            UtilApi.addEventHandle(UtilApi.GoFindChildByPObjAndName("mcam/shop/btn/btn15"), onBtnClk15f);
            UtilApi.addEventHandle(UtilApi.GoFindChildByPObjAndName("mcam/shop/btn/btn40"), onBtnClk40f);
        }

        protected void onBtnClkBuy(GameObject go)
        {
            stReqBuyMobileObjectPropertyUserCmd cmd = new stReqBuyMobileObjectPropertyUserCmd();
            cmd.index = 1;
            UtilMsg.sendMsg(cmd);
        }

        protected void onBtnClkClose(GameObject go)
        {
            //(Ctx.m_instance.m_interActiveEntityMgr.getSceneEntity("shop") as shop).close();
            m_shop.close();
        }

        protected void onBtnClk1f(GameObject go)
        {
            m_shop.showpack(1);
        }

        protected void onBtnClk2f(GameObject go)
        {
            m_shop.showpack(2);
        }

        protected void onBtnClk7f(GameObject go)
        {
            m_shop.showpack(7);
        }

        protected void onBtnClk15f(GameObject go)
        {
            m_shop.showpack(15);
        }

        protected void onBtnClk40f(GameObject go)
        {
            m_shop.showpack(40);
        }

        public void updateShopData()
        {
            m_shop.updateShopData();
        }

        public void showUI()
        {
            m_shop.show();
        }
    }
}