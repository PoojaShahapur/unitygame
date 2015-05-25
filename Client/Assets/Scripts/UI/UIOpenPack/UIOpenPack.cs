using SDK.Common;
using SDK.Lib;
using UnityEngine;
using Game.Msg;

namespace Game.UI
{
    /**
     * @brief 扩展包
     */
    public class UIOpenPack : Form
    {
        protected AuxBasicButton[] m_btnArr = new AuxBasicButton[(int)OpenPackBtnEnum.eBtnTotal];
        protected GameObject[] m_goArr = new GameObject[(int)OpenPackGo.eTotal];
        protected CardCom[] m_cardBtnArr = new CardCom[(int)CardBtnEnum.eCardBtnTotal];

        public override void onReady()
        {
            base.onReady();
            int idx = 0;
            for (idx = 0; idx < (int)CardBtnEnum.eCardBtnTotal; ++idx)
            {
                m_cardBtnArr[idx] = new CardCom(idx);
            }

            findWidget();
            addEventHandle();

            for (idx = 0; idx < (int)CardBtnEnum.eCardBtnTotal; ++idx)
            {
                m_cardBtnArr[idx].load();
            }
        }

        public override void onShow()
        {
            base.onShow();
            updateData();
        }

        public override void onExit()
        {
            base.onExit();
            int idx = 0;
            for (idx = 0; idx < (int)CardBtnEnum.eCardBtnTotal; ++idx)
            {
                m_cardBtnArr[idx].dispose();
            }
        }

        // 获取控件
        protected void findWidget()
        {
            m_goArr[(int)OpenPackGo.eCardPackLayer] = UtilApi.TransFindChildByPObjAndPath(m_GUIWin.m_uiRoot, OpenPackPath.CardPackLayer);
            m_goArr[(int)OpenPackGo.eOpenPackLayer] = UtilApi.TransFindChildByPObjAndPath(m_GUIWin.m_uiRoot, OpenPackPath.OpenPackLayer);

            m_btnArr[(int)OpenPackBtnEnum.eBtnBack] = new AuxBasicButton(m_GUIWin.m_uiRoot, OpenPackPath.RetBtn);
            m_btnArr[(int)OpenPackBtnEnum.eBtnShop] = new AuxBasicButton(m_GUIWin.m_uiRoot, OpenPackPath.ShopBtn);

            m_cardBtnArr[(int)CardBtnEnum.ePackBtn_0].uiCardBtn = new AuxBasicButton(m_GUIWin.m_uiRoot, OpenPackPath.PackBtn_0);
            m_cardBtnArr[(int)CardBtnEnum.ePackBtn_1].uiCardBtn = new AuxBasicButton(m_GUIWin.m_uiRoot, OpenPackPath.PackBtn_1);
            m_cardBtnArr[(int)CardBtnEnum.ePackBtn_2].uiCardBtn = new AuxBasicButton(m_GUIWin.m_uiRoot, OpenPackPath.PackBtn_2);

            m_cardBtnArr[(int)CardBtnEnum.eOpenedPackBtn_0].uiCardBtn = new AuxBasicButton(m_GUIWin.m_uiRoot, OpenPackPath.OpenedPackBtn_0);
            m_cardBtnArr[(int)CardBtnEnum.eOpenedPackBtn_1].uiCardBtn = new AuxBasicButton(m_GUIWin.m_uiRoot, OpenPackPath.OpenedPackBtn_1);
            m_cardBtnArr[(int)CardBtnEnum.eOpenedPackBtn_2].uiCardBtn = new AuxBasicButton(m_GUIWin.m_uiRoot, OpenPackPath.OpenedPackBtn_2);
            m_cardBtnArr[(int)CardBtnEnum.eOpenedPackBtn_3].uiCardBtn = new AuxBasicButton(m_GUIWin.m_uiRoot, OpenPackPath.OpenedPackBtn_3);
            m_cardBtnArr[(int)CardBtnEnum.eOpenedPackBtn_4].uiCardBtn = new AuxBasicButton(m_GUIWin.m_uiRoot, OpenPackPath.OpenedPackBtn_4);
        }

        // 添加事件监听
        protected void addEventHandle()
        {
            m_btnArr[(int)OpenPackBtnEnum.eBtnBack].addEventHandle(onBtnClkBack);
            m_btnArr[(int)OpenPackBtnEnum.eBtnShop].addEventHandle(onBtnClkShop);

            m_cardBtnArr[(int)CardBtnEnum.ePackBtn_0].uiCardBtn.addEventHandle(onPackBtnClk_0);
            m_cardBtnArr[(int)CardBtnEnum.ePackBtn_1].uiCardBtn.addEventHandle(onPackBtnClk_1);

            m_cardBtnArr[(int)CardBtnEnum.eOpenedPackBtn_0].uiCardBtn.addEventHandle(onOpenedPackBtn_0);
            m_cardBtnArr[(int)CardBtnEnum.eOpenedPackBtn_1].uiCardBtn.addEventHandle(onOpenedPackBtn_1);
            m_cardBtnArr[(int)CardBtnEnum.eOpenedPackBtn_2].uiCardBtn.addEventHandle(onOpenedPackBtn_2);
            m_cardBtnArr[(int)CardBtnEnum.eOpenedPackBtn_3].uiCardBtn.addEventHandle(onOpenedPackBtn_3);
            m_cardBtnArr[(int)CardBtnEnum.eOpenedPackBtn_4].uiCardBtn.addEventHandle(onOpenedPackBtn_4);

            UtilApi.SetActive(m_goArr[(int)OpenPackGo.eOpenPackLayer], false);
            m_cardBtnArr[(int)CardBtnEnum.ePackBtn_2].uiCardBtn.hide();
        }

        protected void onBtnClkShop(IDispatchObject dispObj)
		{
            Ctx.m_instance.m_uiMgr.loadAndShow<UIShop>(UIFormID.eUIShop);
            UIShop shop = Ctx.m_instance.m_uiMgr.getForm<UIShop>(UIFormID.eUIShop);
            shop.m_GUIWin.m_uiRoot.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
			// 发送消息
			stReqMarketObjectInfoPropertyUserCmd cmd = new stReqMarketObjectInfoPropertyUserCmd();
			UtilMsg.sendMsg(cmd);

            //UISceneShop uiShop = Ctx.m_instance.m_uiSceneMgr.getSceneUI<UISceneShop>(UISceneFormID.eUISceneShop);
            //if (uiShop == null)
            //{
            //    Ctx.m_instance.m_uiSceneMgr.loadSceneForm<UISceneShop>(UISceneFormID.eUISceneShop);
            //}
            //uiShop = Ctx.m_instance.m_uiSceneMgr.showSceneForm(UISceneFormID.eUISceneShop) as UISceneShop;
			
            //// 显示
            //uiShop.showUI();
		}

        protected void onBtnClkBack(IDispatchObject dispObj)
        {
            exit();
        }

        protected void onPackBtnClk_0(IDispatchObject dispObj)
        {
            DataItemObjectBase bojBase;

            bojBase = Ctx.m_instance.m_dataPlayer.m_dataPack.m_objList[0];

            // 发消息通知开
            stUseObjectPropertyUserCmd cmd = new stUseObjectPropertyUserCmd();
            cmd.qwThisID = bojBase.m_srvItemObject.dwThisID;
            cmd.useType = 1;
            UtilMsg.sendMsg(cmd);

            UtilApi.SetActive(m_goArr[(int)OpenPackGo.eCardPackLayer], false);
            UtilApi.SetActive(m_goArr[(int)OpenPackGo.eOpenPackLayer], true);

            m_cardBtnArr[(int)CardBtnEnum.ePackBtn_2].uiCardBtn.show();
        }

        protected void onPackBtnClk_1(IDispatchObject dispObj)
        {
            DataItemObjectBase bojBase;

            bojBase = Ctx.m_instance.m_dataPlayer.m_dataPack.m_objList[1];

            // 发消息通知开
            stUseObjectPropertyUserCmd cmd = new stUseObjectPropertyUserCmd();
            cmd.qwThisID = bojBase.m_srvItemObject.dwThisID;
            cmd.useType = 1;
            UtilMsg.sendMsg(cmd);

            UtilApi.SetActive(m_goArr[(int)OpenPackGo.eCardPackLayer], false);
            UtilApi.SetActive(m_goArr[(int)OpenPackGo.eOpenPackLayer], true);

            m_cardBtnArr[(int)CardBtnEnum.ePackBtn_2].uiCardBtn.show();
        }

        protected void onOpenedPackBtn_0(IDispatchObject dispObj)
        {
            UtilApi.SetActive(m_goArr[(int)OpenPackGo.eCardPackLayer], true);
            UtilApi.SetActive(m_goArr[(int)OpenPackGo.eOpenPackLayer], false);
        }

        protected void onOpenedPackBtn_1(IDispatchObject dispObj)
        {
            UtilApi.SetActive(m_goArr[(int)OpenPackGo.eCardPackLayer], true);
            UtilApi.SetActive(m_goArr[(int)OpenPackGo.eOpenPackLayer], false);
        }

        protected void onOpenedPackBtn_2(IDispatchObject dispObj)
        {
            UtilApi.SetActive(m_goArr[(int)OpenPackGo.eCardPackLayer], true);
            UtilApi.SetActive(m_goArr[(int)OpenPackGo.eOpenPackLayer], false);
        }

        protected void onOpenedPackBtn_3(IDispatchObject dispObj)
        {
            UtilApi.SetActive(m_goArr[(int)OpenPackGo.eCardPackLayer], true);
            UtilApi.SetActive(m_goArr[(int)OpenPackGo.eOpenPackLayer], false);
        }

        protected void onOpenedPackBtn_4(IDispatchObject dispObj)
        {
            UtilApi.SetActive(m_goArr[(int)OpenPackGo.eCardPackLayer], true);
            UtilApi.SetActive(m_goArr[(int)OpenPackGo.eOpenPackLayer], false);
        }

        // 更新数据
        public void updateData()
        {
            // 每一次最多显示两个
            TableItemBase objitem;
            DataItemObjectBase bojBase;

            if (Ctx.m_instance.m_dataPlayer.m_dataPack.m_objList.Count > 0)
            {
                bojBase = Ctx.m_instance.m_dataPlayer.m_dataPack.m_objList[0];
                objitem = bojBase.m_tableItemObject;

                m_cardBtnArr[(int)CardBtnEnum.ePackBtn_0].uiCardBtn.show();

                if (Ctx.m_instance.m_dataPlayer.m_dataPack.m_objList.Count > 1)
                {
                    m_cardBtnArr[(int)CardBtnEnum.ePackBtn_1].uiCardBtn.show();
                }
                else
                {
                    m_cardBtnArr[(int)CardBtnEnum.ePackBtn_1].uiCardBtn.hide();
                }
            }
            else
            {
                m_cardBtnArr[(int)CardBtnEnum.ePackBtn_0].uiCardBtn.hide();
                m_cardBtnArr[(int)CardBtnEnum.ePackBtn_1].uiCardBtn.hide();
            }
        }

        // 显示 5 张卡
        public void psstRetGiftBagCardsDataUserCmd(params uint[] idList)
        {
            
        }
    }
}