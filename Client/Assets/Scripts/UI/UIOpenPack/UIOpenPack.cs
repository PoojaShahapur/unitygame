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

        public override void onReady()
        {
            findWidget();
            addEventHandle();
        }

        public override void onShow()
        {
            updateData();
        }

        // 获取控件
        protected void findWidget()
        {
            m_goArr[(int)OpenPackGo.eCardPackLayer] = UtilApi.TransFindChildByPObjAndPath(m_GUIWin.m_uiRoot, OpenPackPath.CardPackLayer);
            m_goArr[(int)OpenPackGo.eOpenPackLayer] = UtilApi.TransFindChildByPObjAndPath(m_GUIWin.m_uiRoot, OpenPackPath.OpenPackLayer);

            m_btnArr[(int)OpenPackBtnEnum.eBtnBack] = UtilApi.getComByP<Button>(m_GUIWin.m_uiRoot, OpenPackPath.RetBtn);
            m_btnArr[(int)OpenPackBtnEnum.eBtnShop] = UtilApi.getComByP<Button>(m_GUIWin.m_uiRoot, OpenPackPath.ShopBtn);

            m_btnArr[(int)OpenPackBtnEnum.ePackBtn_0] = UtilApi.getComByP<Button>(m_GUIWin.m_uiRoot, OpenPackPath.PackBtn_0);
            m_btnArr[(int)OpenPackBtnEnum.ePackBtn_1] = UtilApi.getComByP<Button>(m_GUIWin.m_uiRoot, OpenPackPath.PackBtn_1);
            m_btnArr[(int)OpenPackBtnEnum.ePackBtn_2] = UtilApi.getComByP<Button>(m_GUIWin.m_uiRoot, OpenPackPath.PackBtn_2);

            m_btnArr[(int)OpenPackBtnEnum.eOpenedPackBtn_0] = UtilApi.getComByP<Button>(m_GUIWin.m_uiRoot, OpenPackPath.OpenedPackBtn_0);
            m_btnArr[(int)OpenPackBtnEnum.eOpenedPackBtn_1] = UtilApi.getComByP<Button>(m_GUIWin.m_uiRoot, OpenPackPath.OpenedPackBtn_1);
            m_btnArr[(int)OpenPackBtnEnum.eOpenedPackBtn_2] = UtilApi.getComByP<Button>(m_GUIWin.m_uiRoot, OpenPackPath.OpenedPackBtn_2);
            m_btnArr[(int)OpenPackBtnEnum.eOpenedPackBtn_3] = UtilApi.getComByP<Button>(m_GUIWin.m_uiRoot, OpenPackPath.OpenedPackBtn_3);
            m_btnArr[(int)OpenPackBtnEnum.eOpenedPackBtn_4] = UtilApi.getComByP<Button>(m_GUIWin.m_uiRoot, OpenPackPath.OpenedPackBtn_4);
        }

        // 添加事件监听
        protected void addEventHandle()
        {
            UtilApi.addEventHandle(m_btnArr[(int)OpenPackBtnEnum.eBtnBack], onBtnClkBack);
            UtilApi.addEventHandle(m_btnArr[(int)OpenPackBtnEnum.eBtnShop], onBtnClkShop);

            UtilApi.addEventHandle(m_btnArr[(int)OpenPackBtnEnum.ePackBtn_0], onPackBtnClk_0);
            UtilApi.addEventHandle(m_btnArr[(int)OpenPackBtnEnum.ePackBtn_1], onPackBtnClk_1);

            UtilApi.addEventHandle(m_btnArr[(int)OpenPackBtnEnum.eOpenedPackBtn_0], onOpenedPackBtn_0);
            UtilApi.addEventHandle(m_btnArr[(int)OpenPackBtnEnum.eOpenedPackBtn_1], onOpenedPackBtn_1);
            UtilApi.addEventHandle(m_btnArr[(int)OpenPackBtnEnum.eOpenedPackBtn_2], onOpenedPackBtn_2);
            UtilApi.addEventHandle(m_btnArr[(int)OpenPackBtnEnum.eOpenedPackBtn_3], onOpenedPackBtn_3);
            UtilApi.addEventHandle(m_btnArr[(int)OpenPackBtnEnum.eOpenedPackBtn_4], onOpenedPackBtn_4);

            UtilApi.SetActive(m_goArr[(int)OpenPackGo.eOpenPackLayer], false);
            UtilApi.SetActive(m_btnArr[(int)OpenPackBtnEnum.ePackBtn_2].gameObject, false);
        }

		protected void onBtnClkShop()
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
			
			// 显示
			uiShop.showUI();
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

            UtilApi.SetActive(m_btnArr[(int)OpenPackBtnEnum.ePackBtn_2].gameObject, true);
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

            UtilApi.SetActive(m_btnArr[(int)OpenPackBtnEnum.ePackBtn_2].gameObject, true);
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

                UtilApi.SetActive(m_btnArr[(int)OpenPackBtnEnum.ePackBtn_0].gameObject, true);

                if (Ctx.m_instance.m_dataPlayer.m_dataPack.m_objList.Count > 1)
                {
                    UtilApi.SetActive(m_btnArr[(int)OpenPackBtnEnum.ePackBtn_1].gameObject, true);
                }
                else
                {
                    UtilApi.SetActive(m_btnArr[(int)OpenPackBtnEnum.ePackBtn_1].gameObject, false);
                }
            }
            else
            {
                UtilApi.SetActive(m_btnArr[(int)OpenPackBtnEnum.ePackBtn_0].gameObject, false);
                UtilApi.SetActive(m_btnArr[(int)OpenPackBtnEnum.ePackBtn_1].gameObject, false);
            }
        }

        // 显示 5 张卡
        public void psstRetGiftBagCardsDataUserCmd(params uint[] idList)
        {
            
        }
    }
}