using Game.Msg;
using SDK.Common;
using SDK.Lib;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    /**
     * @brief 登陆界面
     */
    public class UIMain : Form
    {
        protected MainData m_mainData;

        protected Button[] m_btnArr;

        public override void onReady()
        {
            m_mainData = new MainData(this);

            m_btnArr = new Button[(int)MainBtnEnum.eBtnTotal];
            findWidget();
            addEventHandle();
        }

        public override void onShow()
        {
            // 请求所有卡牌
            Ctx.m_instance.m_dataPlayer.m_dataCard.reqAllCard();
            // 请求所有的卡牌组
            Ctx.m_instance.m_dataPlayer.m_dataCard.reqCardGroup();
        }

        // 获取控件
        protected void findWidget()
        {
            m_btnArr[(int)MainBtnEnum.eBtnShop] = UtilApi.getComByP<Button>(m_mainData.m_form.m_GUIWin.m_uiRoot, MainComPath.BtnShop);
            m_btnArr[(int)MainBtnEnum.eBtnHero] = UtilApi.getComByP<Button>(m_mainData.m_form.m_GUIWin.m_uiRoot, MainComPath.BtnHero);
            m_btnArr[(int)MainBtnEnum.eBtnExtPack] = UtilApi.getComByP<Button>(m_mainData.m_form.m_GUIWin.m_uiRoot, MainComPath.BtnExtPack);
            m_btnArr[(int)MainBtnEnum.eBtnTuJian] = UtilApi.getComByP<Button>(m_mainData.m_form.m_GUIWin.m_uiRoot, MainComPath.BtnTuJian);

            m_btnArr[(int)MainBtnEnum.eBtnDuiZhan] = UtilApi.getComByP<Button>(m_mainData.m_form.m_GUIWin.m_uiRoot, MainComPath.BtnDuiZhanMode);
            m_btnArr[(int)MainBtnEnum.eBtnLianXi] = UtilApi.getComByP<Button>(m_mainData.m_form.m_GUIWin.m_uiRoot, MainComPath.BtnLianXI);
            m_btnArr[(int)MainBtnEnum.eBtnJingJi] = UtilApi.getComByP<Button>(m_mainData.m_form.m_GUIWin.m_uiRoot, MainComPath.BtnJingJi);
        }

        // 添加事件监听
        protected void addEventHandle()
        {
            UtilApi.addEventHandle(m_btnArr[(int)MainBtnEnum.eBtnShop], onBtnClkShop);                   // 商店
            UtilApi.addEventHandle(m_btnArr[(int)MainBtnEnum.eBtnHero], onBtnClkHero); // 请求 hero 数据
            UtilApi.addEventHandle(m_btnArr[(int)MainBtnEnum.eBtnExtPack], onBtnClkOpenPack);        // 打开扩展
            UtilApi.addEventHandle(m_btnArr[(int)MainBtnEnum.eBtnTuJian], onBtnClkTuJian);        // 我的收藏
            UtilApi.addEventHandle(m_btnArr[(int)MainBtnEnum.eBtnDuiZhan], onBtnClkDuiZhanMoShi); // 对战模式
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

            // 显示内容
            uiShop.showUI();
        }

        protected void onBtnClkHero()
        {
            Ctx.m_instance.m_uiMgr.loadAndShow<UIHero>(UIFormID.eUIHero);
            Ctx.m_instance.m_dataPlayer.m_dataHero.reqAllHero();            //  请求 hero 数据
        }

        protected void onBtnClkOpenPack()
        {
            Ctx.m_instance.m_uiMgr.loadAndShow<UIOpenPack>(UIFormID.eUIOpenPack);
        }

        protected void onBtnClkTuJian()
        {
            Ctx.m_instance.m_uiMgr.loadAndShow<UITuJian>(UIFormID.eUITuJian);
            UITuJian uiSC = Ctx.m_instance.m_uiMgr.getForm<UITuJian>(UIFormID.eUITuJian);
            uiSC.showUI();
        }

        protected void onBtnClkDuiZhanMoShi()
        {
            Ctx.m_instance.m_uiSceneMgr.loadAndShowForm<UISceneMoShi>(UISceneFormID.eUISceneMoShi);
        }

        protected void onBtnClkLianXiMoShi()
        {

        }

        protected void onBtnClkJingJiMoShi()
        {

        }
    }
}