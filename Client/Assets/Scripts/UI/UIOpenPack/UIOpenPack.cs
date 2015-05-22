using SDK.Common;
using SDK.Lib;
using UnityEngine;
using Game.Msg;
using UnityEngine.UI;

namespace Game.UI
{
    /**
     * @brief 扩展包
     */
    public class UIOpenPack : Form
    {
        protected Button[] m_btnArr = new Button[(int)OpenPackBtnEnum.eBtnTotal];
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

            m_btnArr[(int)OpenPackBtnEnum.eBtnBack] = UtilApi.getComByP<Button>(m_GUIWin.m_uiRoot, OpenPackPath.RetBtn);
            m_btnArr[(int)OpenPackBtnEnum.eBtnShop] = UtilApi.getComByP<Button>(m_GUIWin.m_uiRoot, OpenPackPath.ShopBtn);

            m_cardBtnArr[(int)CardBtnEnum.ePackBtn_0].uiCardBtn = UtilApi.getComByP<Button>(m_GUIWin.m_uiRoot, OpenPackPath.PackBtn_0);
            m_cardBtnArr[(int)CardBtnEnum.ePackBtn_1].uiCardBtn = UtilApi.getComByP<Button>(m_GUIWin.m_uiRoot, OpenPackPath.PackBtn_1);
            m_cardBtnArr[(int)CardBtnEnum.ePackBtn_2].uiCardBtn = UtilApi.getComByP<Button>(m_GUIWin.m_uiRoot, OpenPackPath.PackBtn_2);

            m_cardBtnArr[(int)CardBtnEnum.eOpenedPackBtn_0].uiCardBtn = UtilApi.getComByP<Button>(m_GUIWin.m_uiRoot, OpenPackPath.OpenedPackBtn_0);
            m_cardBtnArr[(int)CardBtnEnum.eOpenedPackBtn_1].uiCardBtn = UtilApi.getComByP<Button>(m_GUIWin.m_uiRoot, OpenPackPath.OpenedPackBtn_1);
            m_cardBtnArr[(int)CardBtnEnum.eOpenedPackBtn_2].uiCardBtn = UtilApi.getComByP<Button>(m_GUIWin.m_uiRoot, OpenPackPath.OpenedPackBtn_2);
            m_cardBtnArr[(int)CardBtnEnum.eOpenedPackBtn_3].uiCardBtn = UtilApi.getComByP<Button>(m_GUIWin.m_uiRoot, OpenPackPath.OpenedPackBtn_3);
            m_cardBtnArr[(int)CardBtnEnum.eOpenedPackBtn_4].uiCardBtn = UtilApi.getComByP<Button>(m_GUIWin.m_uiRoot, OpenPackPath.OpenedPackBtn_4);
        }

        // 添加事件监听
        protected void addEventHandle()
        {
            UtilApi.addEventHandle(m_btnArr[(int)OpenPackBtnEnum.eBtnBack], onBtnClkBack);
            UtilApi.addEventHandle(m_btnArr[(int)OpenPackBtnEnum.eBtnShop], onBtnClkShop);

            UtilApi.addEventHandle(m_cardBtnArr[(int)CardBtnEnum.ePackBtn_0].uiCardBtn, onPackBtnClk_0);
            UtilApi.addEventHandle(m_cardBtnArr[(int)CardBtnEnum.ePackBtn_1].uiCardBtn, onPackBtnClk_1);

            UtilApi.addEventHandle(m_cardBtnArr[(int)CardBtnEnum.eOpenedPackBtn_0].uiCardBtn, onOpenedPackBtn_0);
            UtilApi.addEventHandle(m_cardBtnArr[(int)CardBtnEnum.eOpenedPackBtn_1].uiCardBtn, onOpenedPackBtn_1);
            UtilApi.addEventHandle(m_cardBtnArr[(int)CardBtnEnum.eOpenedPackBtn_2].uiCardBtn, onOpenedPackBtn_2);
            UtilApi.addEventHandle(m_cardBtnArr[(int)CardBtnEnum.eOpenedPackBtn_3].uiCardBtn, onOpenedPackBtn_3);
            UtilApi.addEventHandle(m_cardBtnArr[(int)CardBtnEnum.eOpenedPackBtn_4].uiCardBtn, onOpenedPackBtn_4);

            UtilApi.SetActive(m_goArr[(int)OpenPackGo.eOpenPackLayer], false);
            UtilApi.SetActive(m_cardBtnArr[(int)CardBtnEnum.ePackBtn_2].uiCardBtn.gameObject, false);
        }

		protected void onBtnClkShop()
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

        protected void onBtnClkBack()
        {
            exit();
        }

        protected void onPackBtnClk_0()
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

            UtilApi.SetActive(m_cardBtnArr[(int)CardBtnEnum.ePackBtn_2].uiCardBtn.gameObject, true);
        }

        protected void onPackBtnClk_1()
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

            UtilApi.SetActive(m_cardBtnArr[(int)CardBtnEnum.ePackBtn_2].uiCardBtn.gameObject, true);
        }

        protected void onOpenedPackBtn_0()
        {
            UtilApi.SetActive(m_goArr[(int)OpenPackGo.eCardPackLayer], true);
            UtilApi.SetActive(m_goArr[(int)OpenPackGo.eOpenPackLayer], false);
        }

        protected void onOpenedPackBtn_1()
        {
            UtilApi.SetActive(m_goArr[(int)OpenPackGo.eCardPackLayer], true);
            UtilApi.SetActive(m_goArr[(int)OpenPackGo.eOpenPackLayer], false);
        }

        protected void onOpenedPackBtn_2()
        {
            UtilApi.SetActive(m_goArr[(int)OpenPackGo.eCardPackLayer], true);
            UtilApi.SetActive(m_goArr[(int)OpenPackGo.eOpenPackLayer], false);
        }

        protected void onOpenedPackBtn_3()
        {
            UtilApi.SetActive(m_goArr[(int)OpenPackGo.eCardPackLayer], true);
            UtilApi.SetActive(m_goArr[(int)OpenPackGo.eOpenPackLayer], false);
        }

        protected void onOpenedPackBtn_4()
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

                UtilApi.SetActive(m_cardBtnArr[(int)CardBtnEnum.ePackBtn_0].uiCardBtn.gameObject, true);

                if (Ctx.m_instance.m_dataPlayer.m_dataPack.m_objList.Count > 1)
                {
                    UtilApi.SetActive(m_cardBtnArr[(int)CardBtnEnum.ePackBtn_1].uiCardBtn.gameObject, true);
                }
                else
                {
                    UtilApi.SetActive(m_cardBtnArr[(int)CardBtnEnum.ePackBtn_1].uiCardBtn.gameObject, false);
                }
            }
            else
            {
                UtilApi.SetActive(m_cardBtnArr[(int)CardBtnEnum.ePackBtn_0].uiCardBtn.gameObject, false);
                UtilApi.SetActive(m_cardBtnArr[(int)CardBtnEnum.ePackBtn_1].uiCardBtn.gameObject, false);
            }
        }

        // 显示 5 张卡
        public void psstRetGiftBagCardsDataUserCmd(params uint[] idList)
        {
            
        }
    }
}