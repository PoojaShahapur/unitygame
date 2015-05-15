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
        protected SceneBtnBase[] m_btnArr = new SceneBtnBase[(int)SceneMainBtnEnum.eBtnTotal];

        public override void onReady()
        {
            base.onReady();

            findWidget();
            addEventHandle();
        }

        public override void onShow()
        {
            base.onShow();

            // 请求所有卡牌
            Ctx.m_instance.m_dataPlayer.m_dataCard.reqAllCard();
            // 请求所有的卡牌组
            Ctx.m_instance.m_dataPlayer.m_dataCard.reqCardGroup();
        }

        // 获取控件
        protected void findWidget()
        {
            m_btnArr[(int)SceneMainBtnEnum.eBtnShop] = new SceneBtnBase();
            //m_btnArr[(int)SceneMainBtnEnum.eBtnShop].setGameObject(UtilApi.GoFindChildByPObjAndName("open/shopbtn"));

            m_btnArr[(int)SceneMainBtnEnum.eBtnExtPack] = new SceneBtnBase();
            m_btnArr[(int)SceneMainBtnEnum.eBtnExtPack].setGameObject(UtilApi.GoFindChildByPObjAndName("box/drawer/openbtn"));

            m_btnArr[(int)SceneMainBtnEnum.eBtnExtPack] = new SceneBtnBase();
            m_btnArr[(int)SceneMainBtnEnum.eBtnExtPack].setGameObject(UtilApi.GoFindChildByPObjAndName("box/drawer/wdscbtn"));

            m_btnArr[(int)SceneMainBtnEnum.eBtnDuiZhan] = new SceneBtnBase();
            m_btnArr[(int)SceneMainBtnEnum.eBtnDuiZhan].setGameObject(UtilApi.GoFindChildByPObjAndName("box/rightdoor/yuanpan/dzmoshibtn"));

            m_btnArr[(int)SceneMainBtnEnum.eBtnLianXi] = new SceneBtnBase();
            m_btnArr[(int)SceneMainBtnEnum.eBtnLianXi].setGameObject(UtilApi.GoFindChildByPObjAndName("box/rightdoor/yuanpan/dzmoshibtn"));

            m_btnArr[(int)SceneMainBtnEnum.eBtnJingJi] = new SceneBtnBase();
            m_btnArr[(int)SceneMainBtnEnum.eBtnJingJi].setGameObject(UtilApi.GoFindChildByPObjAndName("box/rightdoor/yuanpan/dzmoshibtn"));
        }

        // 添加事件监听
        protected void addEventHandle()
        {
            UtilApi.addEventHandle(UtilApi.GoFindChildByPObjAndName("shopbtnTop"), onBtnClkShop);                   // 商店
            UtilApi.addEventHandle(UtilApi.GoFindChildByPObjAndName("box/drawer/openbtn"), onBtnClkOpen);        // 打开扩展
            UtilApi.addEventHandle(UtilApi.GoFindChildByPObjAndName("box/drawer/wdscbtn"), onBtnClkWDSC);        // 我的收藏
            UtilApi.addEventHandle(UtilApi.GoFindChildByPObjAndName("box/rightdoor/yuanpan/dzmoshibtn"), onBtnClkDuiZhanMoShi); // 对战模式

            UtilApi.addEventHandle(UtilApi.GoFindChildByPObjAndName("mission"), onBtnClkHero); // 请求 hero 数据
        }

        protected void onBtnClkShop(GameObject go)
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
            //(Ctx.m_instance.m_interActiveEntityMgr.getSceneEntity("shop") as shop).show();
            uiShop.showUI();
        }

        protected void onBtnClkOpen(GameObject go)
        {
            //Ctx.m_instance.m_dataPlayer.m_dataCard.reqAllCard();

            UISceneExtPack uiPack = Ctx.m_instance.m_uiSceneMgr.getSceneUI<UISceneExtPack>(UISceneFormID.eUISceneExtPack);
            if (uiPack == null)
            {
                Ctx.m_instance.m_uiSceneMgr.loadSceneForm<UISceneExtPack>(UISceneFormID.eUISceneExtPack);
            }
            uiPack = Ctx.m_instance.m_uiSceneMgr.showSceneForm(UISceneFormID.eUISceneExtPack) as UISceneExtPack;

            //(Ctx.m_instance.m_interActiveEntityMgr.getSceneEntity("open") as open).show();
            uiPack.showUI();
        }

        protected void onBtnClkWDSC(GameObject go)
        {
            Ctx.m_instance.m_uiMgr.loadAndShow<UITuJian>(UIFormID.eUITuJian);
            UITuJian uiSC = Ctx.m_instance.m_uiMgr.getForm<UITuJian>(UIFormID.eUITuJian);
            uiSC.showUI();
        }

        protected void onBtnClkDuiZhanMoShi(GameObject go)
        {
            Ctx.m_instance.m_uiSceneMgr.loadAndShowForm<UISceneMoShi>(UISceneFormID.eUISceneMoShi);
        }

        protected void onBtnClkHero(GameObject go)
        {
            UISceneHero uiSH = Ctx.m_instance.m_uiSceneMgr.getSceneUI<UISceneHero>(UISceneFormID.eUISceneHero);
            if (uiSH == null)
            {
                Ctx.m_instance.m_uiSceneMgr.loadSceneForm<UISceneHero>(UISceneFormID.eUISceneHero);
            }
            Ctx.m_instance.m_uiSceneMgr.showSceneForm(UISceneFormID.eUISceneHero);

            Ctx.m_instance.m_dataPlayer.m_dataHero.reqAllHero();            //  请求 hero 数据
        }

        protected void onBtnClkLianXiMoShi(GameObject go)
        {

        }

        protected void onBtnClkJingJiMoShi(GameObject go)
        {

        }
    }
}