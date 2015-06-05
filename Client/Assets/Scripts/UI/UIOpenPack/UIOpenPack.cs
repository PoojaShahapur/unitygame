using SDK.Common;
using SDK.Lib;
using UnityEngine;
using Game.Msg;
using System.Collections.Generic;

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

        protected GameObject m_openEffImg;
        protected SpriteAni m_spriteAni;
        protected AuxLabel m_textPackNum; 

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

            for (idx = 3; idx < (int)CardBtnEnum.eCardBtnTotal; ++idx)
            {
                m_cardBtnArr[idx].load();
            }
        }

        public override void onShow()
        {
            base.onShow();
            updateData();
            updatePackNum();
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

            m_cardBtnArr[(int)CardBtnEnum.ePackBtn_0].createBtn(m_GUIWin.m_uiRoot, OpenPackPath.PackBtn_0);
            m_cardBtnArr[(int)CardBtnEnum.ePackBtn_1].createBtn(m_GUIWin.m_uiRoot, OpenPackPath.PackBtn_1);
            m_cardBtnArr[(int)CardBtnEnum.ePackBtn_2].createBtn(m_GUIWin.m_uiRoot, OpenPackPath.PackBtn_2);

            m_cardBtnArr[(int)CardBtnEnum.eOpenedPackBtn_0].createBtn(m_GUIWin.m_uiRoot, OpenPackPath.OpenedPackBtn_0);
            m_cardBtnArr[(int)CardBtnEnum.eOpenedPackBtn_1].createBtn(m_GUIWin.m_uiRoot, OpenPackPath.OpenedPackBtn_1);
            m_cardBtnArr[(int)CardBtnEnum.eOpenedPackBtn_2].createBtn(m_GUIWin.m_uiRoot, OpenPackPath.OpenedPackBtn_2);
            m_cardBtnArr[(int)CardBtnEnum.eOpenedPackBtn_3].createBtn(m_GUIWin.m_uiRoot, OpenPackPath.OpenedPackBtn_3);
            m_cardBtnArr[(int)CardBtnEnum.eOpenedPackBtn_4].createBtn(m_GUIWin.m_uiRoot, OpenPackPath.OpenedPackBtn_4);

            m_openEffImg = UtilApi.TransFindChildByPObjAndPath(m_GUIWin.m_uiRoot, OpenPackPath.OpenEffImg);

            m_textPackNum = new AuxLabel(m_GUIWin.m_uiRoot, OpenPackPath.PackNum);
        }

        // 添加事件监听
        protected void addEventHandle()
        {
            m_btnArr[(int)OpenPackBtnEnum.eBtnBack].addEventHandle(onBtnClkBack);
            m_btnArr[(int)OpenPackBtnEnum.eBtnShop].addEventHandle(onBtnClkShop);

            m_cardBtnArr[(int)CardBtnEnum.ePackBtn_0].auxDynImageStaticGoButton.addEventHandle(onPackBtnClk_0);
            m_cardBtnArr[(int)CardBtnEnum.ePackBtn_1].auxDynImageStaticGoButton.addEventHandle(onPackBtnClk_1);

            m_cardBtnArr[(int)CardBtnEnum.eOpenedPackBtn_0].auxDynImageStaticGoButton.addEventHandle(onOpenedPackBtn_0);
            m_cardBtnArr[(int)CardBtnEnum.eOpenedPackBtn_1].auxDynImageStaticGoButton.addEventHandle(onOpenedPackBtn_1);
            m_cardBtnArr[(int)CardBtnEnum.eOpenedPackBtn_2].auxDynImageStaticGoButton.addEventHandle(onOpenedPackBtn_2);
            m_cardBtnArr[(int)CardBtnEnum.eOpenedPackBtn_3].auxDynImageStaticGoButton.addEventHandle(onOpenedPackBtn_3);
            m_cardBtnArr[(int)CardBtnEnum.eOpenedPackBtn_4].auxDynImageStaticGoButton.addEventHandle(onOpenedPackBtn_4);

            UtilApi.SetActive(m_goArr[(int)OpenPackGo.eOpenPackLayer], false);
            m_cardBtnArr[(int)CardBtnEnum.ePackBtn_2].auxDynImageStaticGoButton.hide();
        }

        protected void onBtnClkShop(IDispatchObject dispObj)
		{
            Ctx.m_instance.m_uiMgr.loadAndShow<UIShop>(UIFormID.eUIShop);
            UIShop shop = Ctx.m_instance.m_uiMgr.getForm<UIShop>(UIFormID.eUIShop);
            shop.m_GUIWin.m_uiRoot.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
			// 发送消息
			stReqMarketObjectInfoPropertyUserCmd cmd = new stReqMarketObjectInfoPropertyUserCmd();
			UtilMsg.sendMsg(cmd);
		}

        protected void onBtnClkBack(IDispatchObject dispObj)
        {
            if (m_spriteAni != null)
            {
                m_spriteAni.dispose();
                m_spriteAni = null;
            }
            exit();
        }

        protected void onPackBtnClk_0(IDispatchObject dispObj)
        {
            m_cardBtnArr[(int)CardBtnEnum.ePackBtn_2].objData = Ctx.m_instance.m_dataPlayer.m_dataPack.m_objList[0];
            m_cardBtnArr[(int)CardBtnEnum.ePackBtn_2].load();

            //DataItemObjectBase bojBase;

            //bojBase = Ctx.m_instance.m_dataPlayer.m_dataPack.m_objList[0];

            //// 发消息通知开
            //stUseObjectPropertyUserCmd cmd = new stUseObjectPropertyUserCmd();
            //cmd.qwThisID = bojBase.m_srvItemObject.dwThisID;
            //cmd.useType = 1;
            //UtilMsg.sendMsg(cmd);

            //UtilApi.SetActive(m_goArr[(int)OpenPackGo.eCardPackLayer], false);
            //UtilApi.SetActive(m_goArr[(int)OpenPackGo.eOpenPackLayer], true);

            m_cardBtnArr[(int)CardBtnEnum.ePackBtn_2].auxDynImageStaticGoButton.show();

            m_cardBtnArr[(int)CardBtnEnum.ePackBtn_0].auxDynImageStaticGoButton.hide();
            m_cardBtnArr[(int)CardBtnEnum.ePackBtn_1].auxDynImageStaticGoButton.hide();

            showOpenEff();
            m_openEffImg.SetActive(true);
            UtilApi.SetActive(m_textPackNum.selfGo, false);
        }

        protected void onPackBtnClk_1(IDispatchObject dispObj)
        {
            m_cardBtnArr[(int)CardBtnEnum.ePackBtn_2].objData = Ctx.m_instance.m_dataPlayer.m_dataPack.m_objList[1];
            m_cardBtnArr[(int)CardBtnEnum.ePackBtn_2].load();

            DataItemObjectBase bojBase;

            bojBase = Ctx.m_instance.m_dataPlayer.m_dataPack.m_objList[1];

            // 发消息通知开
            stUseObjectPropertyUserCmd cmd = new stUseObjectPropertyUserCmd();
            cmd.qwThisID = bojBase.m_srvItemObject.dwThisID;
            cmd.useType = 1;
            UtilMsg.sendMsg(cmd);

            UtilApi.SetActive(m_goArr[(int)OpenPackGo.eCardPackLayer], false);
            UtilApi.SetActive(m_goArr[(int)OpenPackGo.eOpenPackLayer], true);

            m_cardBtnArr[(int)CardBtnEnum.ePackBtn_2].auxDynImageStaticGoButton.show();
        }

        protected void onOpenedPackBtn_0(IDispatchObject dispObj)
        {
            //UtilApi.SetActive(m_goArr[(int)OpenPackGo.eCardPackLayer], true);
            UtilApi.SetActive(m_goArr[(int)OpenPackGo.eOpenPackLayer], false);
        }

        protected void onOpenedPackBtn_1(IDispatchObject dispObj)
        {
           // UtilApi.SetActive(m_goArr[(int)OpenPackGo.eCardPackLayer], true);
            UtilApi.SetActive(m_goArr[(int)OpenPackGo.eOpenPackLayer], false);
        }

        protected void onOpenedPackBtn_2(IDispatchObject dispObj)
        {
            //UtilApi.SetActive(m_goArr[(int)OpenPackGo.eCardPackLayer], true);
            UtilApi.SetActive(m_goArr[(int)OpenPackGo.eOpenPackLayer], false);
        }

        protected void onOpenedPackBtn_3(IDispatchObject dispObj)
        {
            //UtilApi.SetActive(m_goArr[(int)OpenPackGo.eCardPackLayer], true);
            UtilApi.SetActive(m_goArr[(int)OpenPackGo.eOpenPackLayer], false);
        }

        protected void onOpenedPackBtn_4(IDispatchObject dispObj)
        {
            //UtilApi.SetActive(m_goArr[(int)OpenPackGo.eCardPackLayer], true);
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

                m_cardBtnArr[(int)CardBtnEnum.ePackBtn_0].auxDynImageStaticGoButton.show();
                m_cardBtnArr[(int)CardBtnEnum.ePackBtn_0].objData = Ctx.m_instance.m_dataPlayer.m_dataPack.m_objList[0];
                m_cardBtnArr[(int)CardBtnEnum.ePackBtn_0].load();

                if (Ctx.m_instance.m_dataPlayer.m_dataPack.m_objList.Count > 1)
                {
                    m_cardBtnArr[(int)CardBtnEnum.ePackBtn_1].auxDynImageStaticGoButton.show();
                    m_cardBtnArr[(int)CardBtnEnum.ePackBtn_1].objData = Ctx.m_instance.m_dataPlayer.m_dataPack.m_objList[1];
                    m_cardBtnArr[(int)CardBtnEnum.ePackBtn_1].load();
                }
                else
                {
                    m_cardBtnArr[(int)CardBtnEnum.ePackBtn_1].auxDynImageStaticGoButton.hide();
                }
            }
            else
            {
                m_cardBtnArr[(int)CardBtnEnum.ePackBtn_0].auxDynImageStaticGoButton.hide();
                m_cardBtnArr[(int)CardBtnEnum.ePackBtn_1].auxDynImageStaticGoButton.hide();
            }
        }

        // 显示 5 张卡
        public void psstRetGiftBagCardsDataUserCmd(params uint[] idList)
        {
            
        }

        protected void showOpenEff()
        {
            m_spriteAni = Ctx.m_instance.m_spriteAniMgr.createAndAdd();
            m_spriteAni.selfGo = m_openEffImg;
            m_spriteAni.tableID = 6;
            m_spriteAni.bLoop = false;
            m_spriteAni.playEndEventDispatch.addEventHandle(effcPlayEnd);
            m_spriteAni.play();
        }

        public void updatePackNum()
        {
            uint packNum = 0;
  
            Dictionary<uint, DataItemObjectBase>.ValueCollection valueCol = Ctx.m_instance.m_dataPlayer.m_dataPack.m_id2ObjDic.Values;
            foreach(DataItemObjectBase value in valueCol)
            {
                packNum += value.m_srvItemObject.dwNum;
            }

            if (packNum <= 1)
                m_textPackNum.text = "";
            else
             m_textPackNum.text = string.Format("{0}", packNum);

            UtilApi.SetActive(m_textPackNum.selfGo, true);
        }

        protected void effcPlayEnd(IDispatchObject dispObj)
        {
            DataItemObjectBase bojBase;

            bojBase = Ctx.m_instance.m_dataPlayer.m_dataPack.m_objList[0];

            // 发消息通知开
            stUseObjectPropertyUserCmd cmd = new stUseObjectPropertyUserCmd();
            cmd.qwThisID = bojBase.m_srvItemObject.dwThisID;
            cmd.useType = 1;
            UtilMsg.sendMsg(cmd);

            m_spriteAni.updateImage();
            m_openEffImg.SetActive(false);
            m_cardBtnArr[(int)CardBtnEnum.ePackBtn_2].auxDynImageStaticGoButton.hide();
            UtilApi.SetActive(m_goArr[(int)OpenPackGo.eOpenPackLayer], true);
        }
    }
}