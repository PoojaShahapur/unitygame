using SDK.Common;
using SDK.Lib;
using UnityEngine;
using Game.Msg;

namespace Game.UI
{
    /**
     * @brief 扩展包
     */
    public class UISceneExtPack : SceneForm
    {
        protected SceneBtnBase[] m_btnArr = new SceneBtnBase[(int)SceneExtPackBtnEnum.eBtnTotal];
        protected open m_open = new open();

        public override void onReady()
        {
            base.onReady();

            findWidget();
            addEventHandle();
        }

        public override void onShow()
        {
            base.onShow();
        }

        // 获取控件
        protected void findWidget()
        {
            m_open.setGameObject(UtilApi.GoFindChildByPObjAndName("open"));

            m_btnArr[(int)SceneShopBtnEnum.eBtnBuy] = new SceneBtnBase();
            m_btnArr[(int)SceneShopBtnEnum.eBtnBuy].setGameObject(UtilApi.GoFindChildByPObjAndName("open/3dbtn/btn"));

            m_btnArr[(int)SceneExtPackBtnEnum.eBtnShop] = new SceneBtnBase();
			m_btnArr [(int)SceneExtPackBtnEnum.eBtnShop].setGameObject(UtilApi.GoFindChildByPObjAndName("open/shopbtn"));
        }

        // 添加事件监听
        protected void addEventHandle()
        {
            UtilApi.addEventHandle(UtilApi.GoFindChildByPObjAndName("open/3dbtn/btn"), onBtnClkBack);
			UtilApi.addEventHandle(UtilApi.GoFindChildByPObjAndName("open/shopbtn"), onBtnClkShop);
        }

		protected void onBtnClkShop(GameObject go)
		{
			// 发送消息
			stReqMarketObjectInfoPropertyUserCmd cmd = new stReqMarketObjectInfoPropertyUserCmd();
			UtilMsg.sendMsg(cmd);

            UISceneShop uiShop = Ctx.m_instance.m_uiSceneMgr.getSceneUI<UISceneShop>(UISceneFormID.eUISceneShop);
			if (uiShop == null)
			{
				Ctx.m_instance.m_uiSceneMgr.loadSceneForm<UISceneShop>(UISceneFormID.eUISceneShop);
			}
			uiShop = Ctx.m_instance.m_uiSceneMgr.showSceneForm(UISceneFormID.eUISceneShop) as UISceneShop;
			
			// 显示内容
			//(Ctx.m_instance.m_interActiveEntityMgr.getSceneEntity("shop") as shop).show();
			uiShop.showUI();
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