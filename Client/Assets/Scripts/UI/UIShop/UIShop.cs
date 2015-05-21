using Game.Msg;
using SDK.Common;
using SDK.Lib;
using UnityEngine;
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

        public override void onInit()
        {
            exitMode = false;         // 直接隐藏
            //hideOnCreate = true;
            base.onInit();
        }

        // Use this for initialization
        public override void onReady()
        {
            m_btnArr = new Button[(int)ShopBtnNum.eBtnTotal];
            m_txtPrice = new Text[(int)ShopTxtPriceNum.eTxtTotal];
            findWidget();
            addEventHandle();
        }

        public override void onShow()
        {
            UpdatePackPrice();
        }

        // 获取控件
        protected void findWidget()
        {
            m_btnArr[(int)ShopBtnNum.eBtnBack] = UtilApi.getComByP<Button>(m_GUIWin.m_uiRoot, ShopComPath.BtnBack);
            m_btnArr[(int)ShopBtnNum.eBtnBuy] = UtilApi.getComByP<Button>(m_GUIWin.m_uiRoot, ShopComPath.BtnBuy);
            m_btnArr[(int)ShopBtnNum.eBtnPack1] = UtilApi.getComByP<Button>(m_GUIWin.m_uiRoot, ShopComPath.BtnPack1);
            m_btnArr[(int)ShopBtnNum.eBtnPack5] = UtilApi.getComByP<Button>(m_GUIWin.m_uiRoot, ShopComPath.BtnPack5);
            m_btnArr[(int)ShopBtnNum.eBtnPack10] = UtilApi.getComByP<Button>(m_GUIWin.m_uiRoot, ShopComPath.BtnPack10);
            m_btnArr[(int)ShopBtnNum.eBtnPack20] = UtilApi.getComByP<Button>(m_GUIWin.m_uiRoot, ShopComPath.BtnPack20);
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
            UtilApi.addEventHandle(m_btnArr[(int)ShopBtnNum.eBtnBack], onBtnClkBack);
            UtilApi.addEventHandle(m_btnArr[(int)ShopBtnNum.eBtnBuy], onBtnClkBuy);
            UtilApi.addEventHandle(m_btnArr[(int)ShopBtnNum.eBtnPack1], onBtnClkPack1);
            UtilApi.addEventHandle(m_btnArr[(int)ShopBtnNum.eBtnPack5], onBtnClkPack5);
            UtilApi.addEventHandle(m_btnArr[(int)ShopBtnNum.eBtnPack10], onBtnClkPack10);
            UtilApi.addEventHandle(m_btnArr[(int)ShopBtnNum.eBtnPack20], onBtnClkPack20);
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
            m_btnArr[(int)ShopBtnNum.eBtnPack1].transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            m_btnArr[(int)ShopBtnNum.eBtnPack1XZ].transform.localScale = new Vector3(0.0001f, 0.0001f, 0.0001f);
        }

        protected void onBtnClkPack5()
        {
            SelectPack(1);
        }

        protected void onBtnClkPack5XZ()
        {
            BuyIndex = 2;
            m_btnArr[(int)ShopBtnNum.eBtnPack5].transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            m_btnArr[(int)ShopBtnNum.eBtnPack5XZ].transform.localScale = new Vector3(0.0001f, 0.0001f, 0.0001f);
        }

        protected void onBtnClkPack10()
        {
            SelectPack(2);
        }

        protected void onBtnClkPack10XZ()
        {
            BuyIndex = 0;
            m_btnArr[(int)ShopBtnNum.eBtnPack10].transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            m_btnArr[(int)ShopBtnNum.eBtnPack10XZ].transform.localScale = new Vector3(0.0001f, 0.0001f, 0.0001f);
        }

        protected void onBtnClkPack20()
        {
            SelectPack(3);
        }

        protected void onBtnClkPack20XZ()
        {
            BuyIndex = 0;
            m_btnArr[(int)ShopBtnNum.eBtnPack20].transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            m_btnArr[(int)ShopBtnNum.eBtnPack20XZ].transform.localScale = new Vector3(0.0001f, 0.0001f, 0.0001f);
        }

        protected void SelectPack(int index)
        {
            BuyIndex = (ushort)(index + 1);

            if(-1 == index)
            {
                for(int i=0; i<4; i++)
                {
                    m_btnArr[(int)ShopBtnNum.eBtnPack1 + i].transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    m_btnArr[(int)ShopBtnNum.eBtnPack1XZ + i].transform.localScale = new Vector3(0.0001f, 0.0001f, 0.0001f);
                }
                return;
            }

            
            for (int i = 0; i < 4; i++)
            {
                if(i != index)
                {
                    m_btnArr[(int)ShopBtnNum.eBtnPack1 + i].transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    m_btnArr[(int)ShopBtnNum.eBtnPack1XZ + i].transform.localScale = new Vector3(0.0001f, 0.0001f, 0.0001f);
                }
                else
                {
                    m_btnArr[(int)ShopBtnNum.eBtnPack1 + i].transform.localScale = new Vector3(0.0001f, 0.0001f, 0.0001f);
                    m_btnArr[(int)ShopBtnNum.eBtnPack1XZ + i].transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
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
            if (0 == BuyIndex)
                return;

            XmlMarketCfg marketCfg = Ctx.m_instance.m_xmlCfgMgr.getXmlCfg<XmlMarketCfg>(XmlCfgID.eXmlMarketCfg);
            XmlItemMarket itemMarket = marketCfg.getXmlItem(BuyIndex) as XmlItemMarket;
            if (Ctx.m_instance.m_dataPlayer.m_dataMain.m_gold < itemMarket.m_price)
            {
                m_NoGoldTip.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
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
            m_NoGoldTip.transform.localScale = new Vector3(0.0001f, 0.0001f, 0.0001f);
        }

        protected void onBtnCancel()
        {
            m_NoGoldTip.transform.localScale = new Vector3(0.0001f, 0.0001f, 0.0001f);
        }
    }
}