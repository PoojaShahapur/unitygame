using Game.Msg;
using SDK.Common;
using SDK.Lib;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI
{
    public class UIShop : Form
    {
        protected Button[] m_btnArr;
        protected ushort BuyIndex;
        protected Text m_textGoldNum;
        protected Text[] m_txtPrice;
        protected Component m_NoGoldTip;
        protected CardCom[] m_packBtnArr = new CardCom[(int)PackBtnNum.ePackBtnTotal];

        public override void onInit()
        {
            exitMode = false;         // 直接隐藏
            //hideOnCreate = true;
            base.onInit();
        }

        // Use this for initialization
        public override void onReady()
        {
            base.onReady();
            int idx = 0;
            for (idx = 0; idx < (int)PackBtnNum.ePackBtnTotal; ++idx)
            {
                m_packBtnArr[idx] = new CardCom(idx);
            }
            m_btnArr = new Button[(int)ShopBtnNum.eBtnTotal];
            m_txtPrice = new Text[(int)ShopTxtPriceNum.eTxtTotal];
            findWidget();
            addEventHandle();

            for (idx = 0; idx < (int)PackBtnNum.ePackBtnTotal; ++idx)
            {
                m_packBtnArr[idx].loadShop();
            }
        }

        public override void onShow()
        {
            base.onShow();
            UpdatePackPrice();
        }

        public override void onExit()
        {
            base.onExit();
            int idx = 0;
            for (idx = 0; idx < (int)PackBtnNum.ePackBtnTotal; ++idx)
            {
                m_packBtnArr[idx].dispose();
            }
        }

        // 获取控件
        protected void findWidget()
        {
            m_packBtnArr[(int)PackBtnNum.eBtnPack1].uiCardBtn = UtilApi.getComByP<Button>(m_GUIWin.m_uiRoot, ShopComPath.BtnPack1);
            m_packBtnArr[(int)PackBtnNum.eBtnPack5].uiCardBtn = UtilApi.getComByP<Button>(m_GUIWin.m_uiRoot, ShopComPath.BtnPack5);
            m_packBtnArr[(int)PackBtnNum.eBtnPack10].uiCardBtn = UtilApi.getComByP<Button>(m_GUIWin.m_uiRoot, ShopComPath.BtnPack10);
            m_packBtnArr[(int)PackBtnNum.eBtnPack20].uiCardBtn = UtilApi.getComByP<Button>(m_GUIWin.m_uiRoot, ShopComPath.BtnPack20);

            m_btnArr[(int)ShopBtnNum.eBtnBack] = UtilApi.getComByP<Button>(m_GUIWin.m_uiRoot, ShopComPath.BtnBack);
            m_btnArr[(int)ShopBtnNum.eBtnBuy] = UtilApi.getComByP<Button>(m_GUIWin.m_uiRoot, ShopComPath.BtnBuy);
            m_btnArr[(int)ShopBtnNum.eBtnPack1XZ] = UtilApi.getComByP<Button>(m_GUIWin.m_uiRoot, ShopComPath.BtnPack1XZ);
            m_btnArr[(int)ShopBtnNum.eBtnPack5XZ] = UtilApi.getComByP<Button>(m_GUIWin.m_uiRoot, ShopComPath.BtnPack5XZ);
            m_btnArr[(int)ShopBtnNum.eBtnPack10XZ] = UtilApi.getComByP<Button>(m_GUIWin.m_uiRoot, ShopComPath.BtnPack10XZ);
            m_btnArr[(int)ShopBtnNum.eBtnPack20XZ] = UtilApi.getComByP<Button>(m_GUIWin.m_uiRoot, ShopComPath.BtnPack20XZ);
            m_btnArr[(int)ShopBtnNum.eBtnOk] = UtilApi.getComByP<Button>(m_GUIWin.m_uiRoot, ShopComPath.BtnOk);
            m_btnArr[(int)ShopBtnNum.eBtnCancel] = UtilApi.getComByP<Button>(m_GUIWin.m_uiRoot, ShopComPath.BtnCancel);

            m_textGoldNum = UtilApi.getComByP<Text>(m_GUIWin.m_uiRoot, ShopComPath.TextGoldNum);

            m_txtPrice[(int)ShopTxtPriceNum.eTxtPrice1] = UtilApi.getComByP<Text>(m_GUIWin.m_uiRoot, ShopComPath.TxtPrice1);
            m_txtPrice[(int)ShopTxtPriceNum.eTxtPrice5] = UtilApi.getComByP<Text>(m_GUIWin.m_uiRoot, ShopComPath.TxtPrice5);
            m_txtPrice[(int)ShopTxtPriceNum.eTxtPrice10] = UtilApi.getComByP<Text>(m_GUIWin.m_uiRoot, ShopComPath.TxtPrice10);
            m_txtPrice[(int)ShopTxtPriceNum.eTxtPrice20] = UtilApi.getComByP<Text>(m_GUIWin.m_uiRoot, ShopComPath.TxtPrice20);

            m_NoGoldTip = UtilApi.getComByP<Component>(m_GUIWin.m_uiRoot, "NoGoldTip");
        }

        // 添加事件监听
        protected void addEventHandle()
        {
            UtilApi.addEventHandle(m_packBtnArr[(int)PackBtnNum.eBtnPack1].uiCardBtn, onBtnClkPack1);
            UtilApi.addEventHandle(m_packBtnArr[(int)PackBtnNum.eBtnPack5].uiCardBtn, onBtnClkPack5);
            UtilApi.addEventHandle(m_packBtnArr[(int)PackBtnNum.eBtnPack10].uiCardBtn, onBtnClkPack10);
            UtilApi.addEventHandle(m_packBtnArr[(int)PackBtnNum.eBtnPack20].uiCardBtn, onBtnClkPack20);

            UtilApi.addEventHandle(m_btnArr[(int)ShopBtnNum.eBtnBack], onBtnClkBack);
            UtilApi.addEventHandle(m_btnArr[(int)ShopBtnNum.eBtnBuy], onBtnClkBuy);
            UtilApi.addEventHandle(m_btnArr[(int)ShopBtnNum.eBtnPack1XZ], onBtnClkPack1XZ);
            UtilApi.addEventHandle(m_btnArr[(int)ShopBtnNum.eBtnPack5XZ], onBtnClkPack5XZ);
            UtilApi.addEventHandle(m_btnArr[(int)ShopBtnNum.eBtnPack10XZ], onBtnClkPack10XZ);
            UtilApi.addEventHandle(m_btnArr[(int)ShopBtnNum.eBtnPack20XZ], onBtnClkPack20XZ);
            UtilApi.addEventHandle(m_btnArr[(int)ShopBtnNum.eBtnOk], onBtnClkOk);
            UtilApi.addEventHandle(m_btnArr[(int)ShopBtnNum.eBtnCancel], onBtnCancel);
        }

        protected void onBtnClkBack()
        {
            //exit();
            Ctx.m_instance.m_uiMgr.exitForm(UIFormID.eUIShop);
            SelectPack(-1);
        }

        protected void onBtnClkPack1()
        {
            SelectPack(0);
        }

        protected void onBtnClkPack1XZ()
        {
            BuyIndex = 0;
            UtilApi.SetActive(m_packBtnArr[(int)PackBtnNum.eBtnPack1].uiCardBtn.gameObject, true);
            UtilApi.SetActive(m_btnArr[(int)ShopBtnNum.eBtnPack1XZ].gameObject, false);
        }

        protected void onBtnClkPack5()
        {
            SelectPack(1);
        }

        protected void onBtnClkPack5XZ()
        {
            BuyIndex = 2;
            UtilApi.SetActive(m_packBtnArr[(int)PackBtnNum.eBtnPack5].uiCardBtn.gameObject, true);
            UtilApi.SetActive(m_btnArr[(int)ShopBtnNum.eBtnPack5XZ].gameObject, false);
        }

        protected void onBtnClkPack10()
        {
            SelectPack(2);
        }

        protected void onBtnClkPack10XZ()
        {
            BuyIndex = 0;
            UtilApi.SetActive(m_packBtnArr[(int)PackBtnNum.eBtnPack10].uiCardBtn.gameObject, true);
            UtilApi.SetActive(m_btnArr[(int)ShopBtnNum.eBtnPack10XZ].gameObject, false);
        }

        protected void onBtnClkPack20()
        {
            SelectPack(3);
        }

        protected void onBtnClkPack20XZ()
        {
            BuyIndex = 0;
            UtilApi.SetActive(m_packBtnArr[(int)PackBtnNum.eBtnPack20].uiCardBtn.gameObject, true);
            UtilApi.SetActive(m_btnArr[(int)ShopBtnNum.eBtnPack20XZ].gameObject, false);
        }

        protected void SelectPack(int index)
        {
            BuyIndex = (ushort)(index + 1);

            if(-1 == index)
            {
                for(int i=0; i<4; i++)
                {
                    UtilApi.SetActive(m_packBtnArr[(int)PackBtnNum.eBtnPack1+i].uiCardBtn.gameObject, true);
                    UtilApi.SetActive(m_btnArr[(int)ShopBtnNum.eBtnPack1XZ+i].gameObject, false);
                }
                return;
            }

            
            for (int i = 0; i < 4; i++)
            {
                if(i != index)
                {
                    UtilApi.SetActive(m_packBtnArr[(int)PackBtnNum.eBtnPack1+i].uiCardBtn.gameObject, true);
                    UtilApi.SetActive(m_btnArr[(int)ShopBtnNum.eBtnPack1XZ+i].gameObject, false);
                }
                else
                {
                    UtilApi.SetActive(m_packBtnArr[(int)PackBtnNum.eBtnPack1+i].uiCardBtn.gameObject, false);
                    UtilApi.SetActive(m_btnArr[(int)ShopBtnNum.eBtnPack1XZ+i].gameObject, true);
                }
            }
        }

        public void UpdateGoldNum(uint goldNum)
        {
            m_textGoldNum.text = string.Format("{0}", goldNum);
        }

        protected void UpdatePackPrice()
        {
            XmlMarketCfg marketCfg = Ctx.m_instance.m_xmlCfgMgr.getXmlCfg<XmlMarketCfg>(XmlCfgID.eXmlMarketCfg);
            for (int i = 0; i < (int)ShopTxtPriceNum.eTxtTotal; i++)
            {
                XmlItemMarket itemMarket = marketCfg.getXmlItem(i + 1) as XmlItemMarket;
                m_txtPrice[i].text = string.Format("{0}", itemMarket.m_price);
            }
        }

        protected void onBtnClkBuy()
        {
            GameObject _go = EventSystem.current.currentSelectedGameObject;
            if (0 == BuyIndex)
                return;

            XmlMarketCfg marketCfg = Ctx.m_instance.m_xmlCfgMgr.getXmlCfg<XmlMarketCfg>(XmlCfgID.eXmlMarketCfg);
            XmlItemMarket itemMarket = marketCfg.getXmlItem(BuyIndex) as XmlItemMarket;
            if (Ctx.m_instance.m_dataPlayer.m_dataMain.m_gold < itemMarket.m_price)
            {
                UtilApi.SetActive(m_NoGoldTip.gameObject, true);
            }
            else
            {
                stReqBuyMobileObjectPropertyUserCmd cmd = new stReqBuyMobileObjectPropertyUserCmd();
                cmd.index = BuyIndex;
                UtilMsg.sendMsg(cmd);
            }
        }

        protected void onBtnClkOk()
        {
            UtilApi.SetActive(m_NoGoldTip.gameObject, false);
        }

        protected void onBtnCancel()
        {
            UtilApi.SetActive(m_NoGoldTip.gameObject, false);
        }
    }
}