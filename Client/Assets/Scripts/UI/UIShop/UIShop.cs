using Game.Msg;
using SDK.Lib;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.UI
{
    public class UIShop : Form, IUIShop
    {
        protected AuxBasicButton[] m_btnArr;
        protected ushort BuyIndex;
        protected AuxLabel m_textGoldNum;
        protected AuxLabel[] m_txtPrice;
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
            m_btnArr = new AuxBasicButton[(int)ShopBtnNum.eBtnTotal];
            m_txtPrice = new AuxLabel[(int)ShopTxtPriceNum.eTxtTotal];
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
            UpdateGoldNum(Ctx.m_instance.m_dataPlayer.m_dataMain.m_gold);
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
            m_packBtnArr[(int)PackBtnNum.eBtnPack1].createBtn(m_guiWin.m_uiRoot, ShopComPath.BtnPack1);
            m_packBtnArr[(int)PackBtnNum.eBtnPack5].createBtn(m_guiWin.m_uiRoot, ShopComPath.BtnPack5);
            m_packBtnArr[(int)PackBtnNum.eBtnPack10].createBtn(m_guiWin.m_uiRoot, ShopComPath.BtnPack10);
            m_packBtnArr[(int)PackBtnNum.eBtnPack20].createBtn(m_guiWin.m_uiRoot, ShopComPath.BtnPack20);

            m_packBtnArr[(int)PackBtnNum.eBtnPackBackImg1].createBtn(m_guiWin.m_uiRoot, ShopComPath.BtnPackBackImg1);
            m_packBtnArr[(int)PackBtnNum.eBtnPackBackImg5].createBtn(m_guiWin.m_uiRoot, ShopComPath.BtnPackBackImg5);
            m_packBtnArr[(int)PackBtnNum.eBtnPackBackImg10].createBtn(m_guiWin.m_uiRoot, ShopComPath.BtnPackBackImg10);
            m_packBtnArr[(int)PackBtnNum.eBtnPackBackImg20].createBtn(m_guiWin.m_uiRoot, ShopComPath.BtnPackBackImg20);

            m_btnArr[(int)ShopBtnNum.eBtnBack] = new AuxBasicButton(m_guiWin.m_uiRoot, ShopComPath.BtnBack);
            m_btnArr[(int)ShopBtnNum.eBtnBuy] = new AuxBasicButton(m_guiWin.m_uiRoot, ShopComPath.BtnBuy);
            m_btnArr[(int)ShopBtnNum.eBtnPack1XZ] = new AuxBasicButton(m_guiWin.m_uiRoot, ShopComPath.BtnPack1XZ);
            m_btnArr[(int)ShopBtnNum.eBtnPack5XZ] = new AuxBasicButton(m_guiWin.m_uiRoot, ShopComPath.BtnPack5XZ);
            m_btnArr[(int)ShopBtnNum.eBtnPack10XZ] = new AuxBasicButton(m_guiWin.m_uiRoot, ShopComPath.BtnPack10XZ);
            m_btnArr[(int)ShopBtnNum.eBtnPack20XZ] = new AuxBasicButton(m_guiWin.m_uiRoot, ShopComPath.BtnPack20XZ);
            m_btnArr[(int)ShopBtnNum.eBtnOk] = new AuxBasicButton(m_guiWin.m_uiRoot, ShopComPath.BtnOk);
            m_btnArr[(int)ShopBtnNum.eBtnCancel] = new AuxBasicButton(m_guiWin.m_uiRoot, ShopComPath.BtnCancel);

            m_textGoldNum = new AuxLabel(m_guiWin.m_uiRoot, ShopComPath.TextGoldNum);

            m_txtPrice[(int)ShopTxtPriceNum.eTxtPrice1] = new AuxLabel(m_guiWin.m_uiRoot, ShopComPath.TxtPrice1);
            m_txtPrice[(int)ShopTxtPriceNum.eTxtPrice5] = new AuxLabel(m_guiWin.m_uiRoot, ShopComPath.TxtPrice5);
            m_txtPrice[(int)ShopTxtPriceNum.eTxtPrice10] = new AuxLabel(m_guiWin.m_uiRoot, ShopComPath.TxtPrice10);
            m_txtPrice[(int)ShopTxtPriceNum.eTxtPrice20] = new AuxLabel(m_guiWin.m_uiRoot, ShopComPath.TxtPrice20);

            m_NoGoldTip = UtilApi.getComByP<Component>(m_guiWin.m_uiRoot, "NoGoldTip");
        }

        // 添加事件监听
        protected void addEventHandle()
        {
            m_packBtnArr[(int)PackBtnNum.eBtnPack1].auxDynImageStaticGoButton.addEventHandle(onBtnClkPack1);
            m_packBtnArr[(int)PackBtnNum.eBtnPack5].auxDynImageStaticGoButton.addEventHandle(onBtnClkPack5);
            m_packBtnArr[(int)PackBtnNum.eBtnPack10].auxDynImageStaticGoButton.addEventHandle(onBtnClkPack10);
            m_packBtnArr[(int)PackBtnNum.eBtnPack20].auxDynImageStaticGoButton.addEventHandle(onBtnClkPack20);

            m_btnArr[(int)ShopBtnNum.eBtnBack].addEventHandle(onBtnClkBack);
            m_btnArr[(int)ShopBtnNum.eBtnBuy].addEventHandle(onBtnClkBuy);
            m_btnArr[(int)ShopBtnNum.eBtnPack1XZ].addEventHandle(onBtnClkPack1XZ);
            m_btnArr[(int)ShopBtnNum.eBtnPack5XZ].addEventHandle(onBtnClkPack5XZ);
            m_btnArr[(int)ShopBtnNum.eBtnPack10XZ].addEventHandle(onBtnClkPack10XZ);
            m_btnArr[(int)ShopBtnNum.eBtnPack20XZ].addEventHandle(onBtnClkPack20XZ);
            m_btnArr[(int)ShopBtnNum.eBtnOk].addEventHandle(onBtnClkOk);
            m_btnArr[(int)ShopBtnNum.eBtnCancel].addEventHandle(onBtnCancel);
        }

        protected void onBtnClkBack(IDispatchObject dispObj)
        {
            //exit();
            Ctx.m_instance.m_uiMgr.exitForm(UIFormID.eUIShop);
            SelectPack(-1);
        }

        protected void onBtnClkPack1(IDispatchObject dispObj)
        {
            SelectPack(0);
        }

        protected void onBtnClkPack1XZ(IDispatchObject dispObj)
        {
            BuyIndex = 0;
            m_packBtnArr[(int)PackBtnNum.eBtnPack1].auxDynImageStaticGoButton.show();
            UtilApi.SetActive(m_btnArr[(int)ShopBtnNum.eBtnPack1XZ].selfGo, false);
        }

        protected void onBtnClkPack5(IDispatchObject dispObj)
        {
            SelectPack(1);
        }

        protected void onBtnClkPack5XZ(IDispatchObject dispObj)
        {
            BuyIndex = 2;
            m_packBtnArr[(int)PackBtnNum.eBtnPack5].auxDynImageStaticGoButton.show();
            m_btnArr[(int)ShopBtnNum.eBtnPack5XZ].hide();
        }

        protected void onBtnClkPack10(IDispatchObject dispObj)
        {
            SelectPack(2);
        }

        protected void onBtnClkPack10XZ(IDispatchObject dispObj)
        {
            BuyIndex = 0;
            m_packBtnArr[(int)PackBtnNum.eBtnPack10].auxDynImageStaticGoButton.show();
            m_btnArr[(int)ShopBtnNum.eBtnPack10XZ].hide();
        }

        protected void onBtnClkPack20(IDispatchObject dispObj)
        {
            SelectPack(3);
        }

        protected void onBtnClkPack20XZ(IDispatchObject dispObj)
        {
            BuyIndex = 0;
            m_packBtnArr[(int)PackBtnNum.eBtnPack20].auxDynImageStaticGoButton.show();
            m_btnArr[(int)ShopBtnNum.eBtnPack20XZ].hide();
        }

        protected void SelectPack(int index)
        {
            BuyIndex = (ushort)(index + 1);

            if(-1 == index)
            {
                for(int i=0; i<4; i++)
                {
                    m_packBtnArr[(int)PackBtnNum.eBtnPack1 + i].auxDynImageStaticGoButton.show();
                    m_btnArr[(int)ShopBtnNum.eBtnPack1XZ+i].hide();
                }
                return;
            }

            
            for (int i = 0; i < 4; i++)
            {
                if(i != index)
                {
                    m_packBtnArr[(int)PackBtnNum.eBtnPack1 + i].auxDynImageStaticGoButton.show();
                    m_btnArr[(int)ShopBtnNum.eBtnPack1XZ+i].hide();
                }
                else
                {
                    m_packBtnArr[(int)PackBtnNum.eBtnPack1 + i].auxDynImageStaticGoButton.hide();
                    m_btnArr[(int)ShopBtnNum.eBtnPack1XZ+i].show();
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

        protected void onBtnClkBuy(IDispatchObject dispObj)
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

        protected void onBtnClkOk(IDispatchObject dispObj)
        {
            UtilApi.SetActive(m_NoGoldTip.gameObject, false);
        }

        protected void onBtnCancel(IDispatchObject dispObj)
        {
            UtilApi.SetActive(m_NoGoldTip.gameObject, false);
        }
    }
}