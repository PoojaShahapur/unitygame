using Game.Msg;
using SDK.Common;
using SDK.Lib;
using UnityEngine;

namespace Game.UI
{
    /**
     * @brief 登陆界面
     */
    public class UISceneMain : SceneForm
    {
        protected btn[] m_btnArr = new btn[(int)SceneMainBtnEnum.eBtnTotal];

        public override void onReady()
        {
            base.onReady();

            getWidget();
            addEventHandle();
        }

        public override void onShow()
        {
            base.onShow();
        }

        // 获取控件
        protected void getWidget()
        {
            m_btnArr[(int)SceneMainBtnEnum.eBtnShop] = new btn();
            m_btnArr[(int)SceneMainBtnEnum.eBtnShop].setGameObject(UtilApi.GoFindChildByPObjAndName("shopbtn"));

            m_btnArr[(int)SceneMainBtnEnum.eBtnExtPack] = new btn();
            m_btnArr[(int)SceneMainBtnEnum.eBtnExtPack].setGameObject(UtilApi.GoFindChildByPObjAndName("openbtn"));

            m_btnArr[(int)SceneMainBtnEnum.eBtnExtPack] = new btn();
            m_btnArr[(int)SceneMainBtnEnum.eBtnExtPack].setGameObject(UtilApi.GoFindChildByPObjAndName("box/drawer/wdscbtn"));

            m_btnArr[(int)SceneMainBtnEnum.eBtnDuiZhan] = new btn();
            m_btnArr[(int)SceneMainBtnEnum.eBtnDuiZhan].setGameObject(UtilApi.GoFindChildByPObjAndName("box/rightdoor/yuanpan/dzmoshibtn"));

            m_btnArr[(int)SceneMainBtnEnum.eBtnLianXi] = new btn();
            m_btnArr[(int)SceneMainBtnEnum.eBtnLianXi].setGameObject(UtilApi.GoFindChildByPObjAndName("box/rightdoor/yuanpan/dzmoshibtn"));

            m_btnArr[(int)SceneMainBtnEnum.eBtnJingJi] = new btn();
            m_btnArr[(int)SceneMainBtnEnum.eBtnJingJi].setGameObject(UtilApi.GoFindChildByPObjAndName("box/rightdoor/yuanpan/dzmoshibtn"));
        }

        // 添加事件监听
        protected void addEventHandle()
        {
            UtilApi.addEventHandle(UtilApi.GoFindChildByPObjAndName("shopbtnTop"), onBtnClkShop);                   // 商店
            UtilApi.addEventHandle(UtilApi.GoFindChildByPObjAndName("box/drawer/openbtn"), onBtnClkOpen);        // 打开扩展
            UtilApi.addEventHandle(UtilApi.GoFindChildByPObjAndName("box/drawer/wdscbtn"), onBtnClkWDSC);        // 我的收藏
            UtilApi.addEventHandle(UtilApi.GoFindChildByPObjAndName("box/rightdoor/yuanpan/dzmoshibtn"), onBtnClkDuiZhanMoShi); // 我的收藏

            UtilApi.addEventHandle(UtilApi.GoFindChildByPObjAndName("mission"), onBtnClkHero); // 请求 hero 数据
        }

        protected void onBtnClkShop(GameObject go)
        {
            // 发送消息
            stReqMarketObjectInfoPropertyUserCmd cmd = new stReqMarketObjectInfoPropertyUserCmd();
            UtilMsg.sendMsg(cmd);

            IUISceneShop uiShop = Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneShop) as IUISceneShop;
            if (uiShop == null)
            {
                Ctx.m_instance.m_uiSceneMgr.loadSceneForm(UISceneFormID.eUISceneShop);
            }
            uiShop = Ctx.m_instance.m_uiSceneMgr.showSceneForm(UISceneFormID.eUISceneShop) as IUISceneShop;

            // 显示内容
            //(Ctx.m_instance.m_interActiveEntityMgr.getSceneEntity("shop") as shop).show();
            uiShop.showUI();
        }

        protected void onBtnClkOpen(GameObject go)
        {
            Ctx.m_instance.m_dataPlayer.m_dataCard.reqAllCard();

            IUISceneExtPack uiPack = Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneExtPack) as IUISceneExtPack;
            if (uiPack == null)
            {
                Ctx.m_instance.m_uiSceneMgr.loadSceneForm(UISceneFormID.eUISceneExtPack);
            }
            uiPack = Ctx.m_instance.m_uiSceneMgr.showSceneForm(UISceneFormID.eUISceneExtPack) as IUISceneExtPack;

            //(Ctx.m_instance.m_interActiveEntityMgr.getSceneEntity("open") as open).show();
            uiPack.showUI();
        }

        protected void onBtnClkWDSC(GameObject go)
        {
            Ctx.m_instance.m_dataPlayer.m_dataCard.reqCardGroup();

            IUISceneWDSC uiSC = Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneWDSC) as IUISceneWDSC;
            if (uiSC == null)
            {
                Ctx.m_instance.m_uiSceneMgr.loadSceneForm(UISceneFormID.eUISceneWDSC);
            }
            uiSC = Ctx.m_instance.m_uiSceneMgr.showSceneForm(UISceneFormID.eUISceneWDSC) as IUISceneWDSC;

            //(Ctx.m_instance.m_interActiveEntityMgr.getSceneEntity("wdscjm") as wdscjm).show();
            uiSC.showUI();
        }

        protected void onBtnClkDuiZhanMoShi(GameObject go)
        {
            //(Ctx.m_instance.m_interActiveEntityMgr.getSceneEntity("moshijm") as moshijm).dzmoshi();
        }

        protected void onBtnClkHero(GameObject go)
        {
            Ctx.m_instance.m_dataPlayer.m_dataHero.reqAllHero();            //  请求 hero 数据


            UISceneHero uiSH = Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneHero) as UISceneHero;
            if (uiSH == null)
            {
                Ctx.m_instance.m_uiSceneMgr.loadSceneForm(UISceneFormID.eUISceneWDSC);
            }
            uiSH = Ctx.m_instance.m_uiSceneMgr.showSceneForm(UISceneFormID.eUISceneHero) as UISceneHero;
        }

        protected void onBtnClkLianXiMoShi(GameObject go)
        {

        }

        protected void onBtnClkJingJiMoShi(GameObject go)
        {

        }
    }
}