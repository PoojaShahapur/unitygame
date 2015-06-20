using Game.Msg;
using SDK.Common;
using SDK.Lib;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    /**
     * @brief 登陆界面
     */
    public class UIMain : Form, IUIMain
    {
        protected MainData m_mainData;

        protected AuxBasicButton[] m_btnArr;

        public override void onReady()
        {
            base.onReady();
            m_mainData = new MainData(this);

            m_btnArr = new AuxBasicButton[(int)MainBtnEnum.eBtnTotal];
            findWidget();
            addEventHandle();
        }

        public override void onShow()
        {
            base.onShow();
            Ctx.m_instance.m_logSys.log("请求卡组数据");
            // 请求所有卡牌
            Ctx.m_instance.m_dataPlayer.m_dataCard.reqAllCard();
            // 请求所有的卡牌组
            Ctx.m_instance.m_dataPlayer.m_dataCard.reqCardGroup();
        }

        // 获取控件
        protected void findWidget()
        {
            m_btnArr[(int)MainBtnEnum.eBtnShop] = new AuxBasicButton(m_mainData.m_form.m_GUIWin.m_uiRoot, MainComPath.BtnShop);
            m_btnArr[(int)MainBtnEnum.eBtnHero] = new AuxBasicButton(m_mainData.m_form.m_GUIWin.m_uiRoot, MainComPath.BtnHero);
            m_btnArr[(int)MainBtnEnum.eBtnExtPack] = new AuxBasicButton(m_mainData.m_form.m_GUIWin.m_uiRoot, MainComPath.BtnExtPack);
            m_btnArr[(int)MainBtnEnum.eBtnTuJian] = new AuxBasicButton(m_mainData.m_form.m_GUIWin.m_uiRoot, MainComPath.BtnTuJian);

            m_btnArr[(int)MainBtnEnum.eBtnDuiZhan] = new AuxBasicButton(m_mainData.m_form.m_GUIWin.m_uiRoot, MainComPath.BtnDuiZhanMode);
            m_btnArr[(int)MainBtnEnum.eBtnLianXi] = new AuxBasicButton(m_mainData.m_form.m_GUIWin.m_uiRoot, MainComPath.BtnLianXI);
        }

        // 添加事件监听
        protected void addEventHandle()
        {
            m_btnArr[(int)MainBtnEnum.eBtnShop].addEventHandle(onBtnClkShop);              // 商店
            m_btnArr[(int)MainBtnEnum.eBtnHero].addEventHandle(onBtnClkHero);              // 请求 hero 数据
            m_btnArr[(int)MainBtnEnum.eBtnExtPack].addEventHandle(onBtnClkOpenPack);       // 打开扩展
            m_btnArr[(int)MainBtnEnum.eBtnTuJian].addEventHandle(onBtnClkTuJian);          // 我的收藏
            m_btnArr[(int)MainBtnEnum.eBtnDuiZhan].addEventHandle(onBtnClkDuiZhanMoShi);   // 对战模式
        }

        protected void onBtnClkShop(IDispatchObject dispObj)
        {
            Ctx.m_instance.m_uiMgr.loadAndShow(UIFormID.eUIShop);
            IUIShop shop = Ctx.m_instance.m_uiMgr.getForm(UIFormID.eUIShop) as IUIShop;
            UtilApi.setScale(shop.GUIWin().m_uiRoot.transform, new Vector3(1.0f, 1.0f, 1.0f));
            // 发送消息
            stReqMarketObjectInfoPropertyUserCmd cmd = new stReqMarketObjectInfoPropertyUserCmd();
            UtilMsg.sendMsg(cmd);
        }

        protected void onBtnClkHero(IDispatchObject dispObj)
        {
            Ctx.m_instance.m_uiMgr.loadAndShow(UIFormID.eUIHero);
            Ctx.m_instance.m_dataPlayer.m_dataHero.reqAllHero();            //  请求 hero 数据
        }

        protected void onBtnClkOpenPack(IDispatchObject dispObj)
        {
            Ctx.m_instance.m_uiMgr.loadAndShow(UIFormID.eUIOpenPack);
        }

        protected void onBtnClkTuJian(IDispatchObject dispObj)
        {
            Ctx.m_instance.m_uiMgr.loadAndShow(UIFormID.eUITuJian);
            IUITuJian uiSC = Ctx.m_instance.m_uiMgr.getForm(UIFormID.eUITuJian) as IUITuJian;
            uiSC.showUI();
        }

        protected void onBtnClkDuiZhanMoShi(IDispatchObject dispObj)
        {
            Ctx.m_instance.m_auxUIHelp.m_auxJobSelectData.enterDZMode();
            Ctx.m_instance.m_uiMgr.loadAndShow(UIFormID.eUIJobSelect);
        }

        protected void onBtnClkLianXiMoShi(IDispatchObject dispObj)
        {

        }

        protected void onBtnClkJingJiMoShi(IDispatchObject dispObj)
        {

        }
    }
}