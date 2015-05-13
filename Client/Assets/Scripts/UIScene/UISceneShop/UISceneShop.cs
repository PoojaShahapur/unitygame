using Game.Msg;
using SDK.Common;
using SDK.Lib;
using UnityEngine;

namespace Game.UI
{
    /**
     * @brief 扩展包
     */
    public class UISceneShop : SceneForm 
    {
        protected shop m_shop = new shop();
        protected SceneBtnBase[] m_btnArr = new SceneBtnBase[(int)SceneShopBtnEnum.eBtnTotal];
        protected shopbtn[] m_shopBtnArr = new shopbtn[(int)SceneShopShopBtnEnum.eBtnTotal];

        protected GameObject[] m_shopPack = new GameObject[4];
        protected GameObject[] m_shopPackXZ = new GameObject[4];

        protected GameObject m_shopTip;

		private ushort m_iBuyIndex;
		public ushort BuyIndex
		{
			get
			{
				return m_iBuyIndex;
			}
			set
			{
				m_iBuyIndex = value;
			}
		}

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
            m_shop.setGameObject(UtilApi.GoFindChildByPObjAndName("mcam/shop"));

            m_btnArr[(int)SceneShopBtnEnum.eBtnBuy] = new SceneBtnBase();
            m_shopTip = UtilApi.GoFindChildByPObjAndName("mcam/shop/NoGoldTip");

            m_shopPack[0] = UtilApi.GoFindChildByPObjAndName("mcam/shop/background/1pack");
            m_shopPack[1] = UtilApi.GoFindChildByPObjAndName("mcam/shop/background/5pack");
            m_shopPack[2] = UtilApi.GoFindChildByPObjAndName("mcam/shop/background/10pack");
            m_shopPack[3] = UtilApi.GoFindChildByPObjAndName("mcam/shop/background/20pack");

            m_shopPackXZ[0] = UtilApi.GoFindChildByPObjAndName("mcam/shop/background/packxz1");
            m_shopPackXZ[1] = UtilApi.GoFindChildByPObjAndName("mcam/shop/background/packxz5");
            m_shopPackXZ[2] = UtilApi.GoFindChildByPObjAndName("mcam/shop/background/packxz10");
            m_shopPackXZ[3] = UtilApi.GoFindChildByPObjAndName("mcam/shop/background/packxz20");

            /*m_btnArr[(int)SceneShopBtnEnum.eBtnBuy] = new btn();
            m_btnArr[(int)SceneShopBtnEnum.eBtnBuy].setGameObject(UtilApi.GoFindChildByPObjAndName("mcam/shop/buykuan/goldbuy"));

            m_btnArr[(int)SceneShopBtnEnum.eBtnClose] = new SceneBtnBase();
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
            m_shopBtnArr[(int)SceneShopShopBtnEnum.eBtn40f].setGameObject(UtilApi.GoFindChildByPObjAndName("mcam/shop/btn/btn40"));*/
        }

        // 添加事件监听
        protected void addEventHandle()
        {
            //UtilApi.addEventHandle(UtilApi.GoFindChildByPObjAndName("mcam/shop/close"), onBtnClkClose);
            UtilApi.addEventHandle(UtilApi.GoFindChildByPObjAndName("mcam/shop/background/backbtn"), onBtnClkClose);
            UtilApi.addEventHandle(UtilApi.GoFindChildByPObjAndName("mcam/shop/background/buybtn"), onBtnClkBuy);

            UtilApi.addEventHandle(UtilApi.GoFindChildByPObjAndName("mcam/shop/NoGoldTip/okBtn"), onOkBtnClk);
            UtilApi.addEventHandle(UtilApi.GoFindChildByPObjAndName("mcam/shop/NoGoldTip/cancelBtn"), onCancelBtnClk);

            UtilApi.addEventHandle(UtilApi.GoFindChildByPObjAndName("mcam/shop/background/1pack"), onBtnClk1f);
            UtilApi.addEventHandle(UtilApi.GoFindChildByPObjAndName("mcam/shop/background/5pack"), onBtnClk5f);
            UtilApi.addEventHandle(UtilApi.GoFindChildByPObjAndName("mcam/shop/background/10pack"), onBtnClk10f);
            UtilApi.addEventHandle(UtilApi.GoFindChildByPObjAndName("mcam/shop/background/20pack"), onBtnClk20f);
            //UtilApi.addEventHandle(UtilApi.GoFindChildByPObjAndName("mcam/shop/background/btn40"), onBtnClk40f);

            UtilApi.addEventHandle(UtilApi.GoFindChildByPObjAndName("mcam/shop/background/packxz1"), onXZBtnClk1f);
            UtilApi.addEventHandle(UtilApi.GoFindChildByPObjAndName("mcam/shop/background/packxz5"), onXZBtnClk5f);
            UtilApi.addEventHandle(UtilApi.GoFindChildByPObjAndName("mcam/shop/background/packxz10"), onXZBtnClk10f);
            UtilApi.addEventHandle(UtilApi.GoFindChildByPObjAndName("mcam/shop/background/packxz20"), onXZBtnClk20f);
        }

        protected void onBtnClkBuy(GameObject go)
        {
            if (0 == BuyIndex)
                return;

            XmlMarketCfg marketCfg = Ctx.m_instance.m_xmlCfgMgr.getXmlCfg<XmlMarketCfg>(XmlCfgID.eXmlMarketCfg);
            XmlItemMarket itemMarket = marketCfg.getXmlItem(BuyIndex) as XmlItemMarket;
            if(Ctx.m_instance.m_dataPlayer.m_dataMain.m_gold < itemMarket.m_price)
            {
                m_shopTip.SetActive(true);
            }
            else
            {
                stReqBuyMobileObjectPropertyUserCmd cmd = new stReqBuyMobileObjectPropertyUserCmd();
                cmd.index = BuyIndex;
                UtilMsg.sendMsg(cmd);
            }
        }

        protected void onBtnClkClose(GameObject go)
        {
            //(Ctx.m_instance.m_interActiveEntityMgr.getSceneEntity("shop") as shop).close();
            BuyIndex = 0;
            for (int i = 0; i < 4; i++)
            {
                m_shopPack[i].SetActive(true);
                m_shopPackXZ[i].SetActive(false);
            }
            m_shop.close();
        }

        protected void onBtnClk1f(GameObject go)
        {
            //m_shop.showpack(1);
            selectPack(0);
            BuyIndex = 1;
        }

        protected void onBtnClk5f(GameObject go)
        {
            //m_shop.showpack(2);
            selectPack(1);
            BuyIndex = 2;
        }

        protected void onBtnClk10f(GameObject go)
        {
            //m_shop.showpack(7);
            selectPack(2);
            BuyIndex = 3;
        }

        protected void onBtnClk20f(GameObject go)
        {
            //m_shop.showpack(15);
            selectPack(3);
            BuyIndex = 4;
        }

        protected void onBtnClk40f(GameObject go)
        {
            //m_shop.showpack(40);
			BuyIndex = 5;
        }

        protected void onXZBtnClk1f(GameObject go)
        {
            BuyIndex = 0;
            m_shopPack[0].SetActive(true);
            m_shopPackXZ[0].SetActive(false);
        }

        protected void onXZBtnClk5f(GameObject go)
        {
            BuyIndex = 0;
            m_shopPack[1].SetActive(true);
            m_shopPackXZ[1].SetActive(false);
        }
        protected void onXZBtnClk10f(GameObject go)
        {
            BuyIndex = 0;
            m_shopPack[2].SetActive(true);
            m_shopPackXZ[2].SetActive(false);
        }
        protected void onXZBtnClk20f(GameObject go)
        {
            BuyIndex = 0;
            m_shopPack[3].SetActive(true);
            m_shopPackXZ[3].SetActive(false);
        }

        protected void onOkBtnClk(GameObject go)
        {
            m_shopTip.SetActive(false);
        }

        protected void onCancelBtnClk(GameObject go)
        {
            m_shopTip.SetActive(false);
        }

        public void updateShopData()
        {
            m_shop.updateShopData();
        }

        public void showUI()
        {
            m_shop.show();
        }

        public void selectPack(int index)
        {
            if(index < 0 || index > 3)
                return;
            if (m_shopPack[index] != null)
                m_shopPack[index].SetActive(false);

            if (m_shopPackXZ[index] != null)
                m_shopPackXZ[index].SetActive(true);

            for(int i=0; i<4; i++)
            {
                if (i == index)
                    continue;

                if (m_shopPack[i] != null)
                    m_shopPack[i].SetActive(true);
                if (m_shopPackXZ[i] != null)
                    m_shopPackXZ[i].SetActive(false);
            }
        }
    }
}