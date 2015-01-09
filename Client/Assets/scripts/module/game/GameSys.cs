using System;
using UnityEngine;
using System.Collections.Generic;
using SDK.Common;
using Game.UI;

namespace Game.Game
{
    public class GameSys
    {
        static public GameSys m_instance;
        public Ctx m_ctx;

        public void Start()
        {
            initGVar();
            loadScene();
        }

        public void initGVar()
        {
            // 获取全局变量
            //GameObject nodestroy = GameObject.FindGameObjectWithTag("App");
            GameObject nodestroy = UtilApi.GoFindChildByPObjAndName(NotDestroyPath.ND_CV_App);
            AppRoot approot = nodestroy.GetComponent<AppRoot>();
            GameSys.m_instance.m_ctx = approot.getCtx();

            // 场景逻辑处理逻辑
            GameSys.m_instance.m_ctx.m_uiMgr.SetIUIFactory(new GameUIFactory());
            GameSys.m_instance.m_ctx.m_uiSceneMgr.SetIUISceneFactory(new GameUISceneFactory());
            // 游戏逻辑处理
            Ctx.m_instance.m_cbUIEvent = new GameUIEventCB();
            Ctx.m_instance.m_netHandle = new GameNetHandleCB();
            Ctx.m_instance.m_bStopNetHandle = false;     // 停止网络消息处理
            Ctx.m_instance.m_sceneEventCB = new GameSceneEventCB();
            Ctx.m_instance.m_sceneLogic = new GameSceneLogic();

            //Ctx.m_instance.m_meshMgr.loadSkinInfo();
            //Ctx.m_instance.m_aiSystem.getBehaviorTreeMgr().loadBT();

            // 立即加载 UIBlurBg 界面
            Ctx.m_instance.m_uiMgr.loadForm(UIFormID.UIBlurBg);
        }

        public void loadScene()
        {
            //Ctx.m_instance.m_sceneSys.loadScene("cave", onResLoadScene);
            Ctx.m_instance.m_sceneSys.loadScene("game", onResLoadScene);
        }

        //public void onResLoad(EventDisp resEvt)
        //{
        //    IRes res = resEvt.m_param as IRes;                         // 类型转换
        //    GameObject go = res.InstantiateObject("UIScrollForm");
        //    GameObject nodestroy = GameObject.FindGameObjectWithTag("UIFirstLayer");
        //    go.transform.parent = nodestroy.transform;
        //}

        public void onResLoadScene(IScene scene)
        {
            //getInteractiveEntity();
            Ctx.m_instance.m_camSys.m_boxcam = new SDK.Lib.boxcam();
            Ctx.m_instance.m_camSys.m_boxcam.setGameObject(UtilApi.GoFindChildByPObjAndName("mcam"));
            loadAllUIScene();

            Ctx.m_instance.m_log.log("场景加载成功");
            Ctx.m_instance.m_sceneEventCB.onLevelLoaded();

            // 卸载登陆模块，关闭登陆界面
            Ctx.m_instance.m_moduleSys.unloadModule(ModuleID.LOGINMN);
            Ctx.m_instance.m_uiMgr.exitForm(UIFormID.UILogin);
            Ctx.m_instance.m_uiMgr.exitForm(UIFormID.UIHeroSelect);

            // 请求主角基本数据
            Ctx.m_instance.m_dataPlayer.reqMainData();
        }

        // 获取场景中可点击的对象
        //protected void getInteractiveEntity()
        //{
        //    GameObject sceneRoot = UtilApi.GoFindChildByPObjAndName("mcam");
        //    GameObject go = UtilApi.GoFindChildByPObjAndName("mcam/shop");     // 获取商店

        //    Ctx.m_instance.m_interActiveEntityMgr.addSceneEntity(go, EntityType.eETShop);

        //    go = UtilApi.GoFindChildByPObjAndName("shopbtnTop");   // 商店按钮
        //    Ctx.m_instance.m_interActiveEntityMgr.addSceneEntity(go, EntityType.eETBtn, EntityTag.eETagShop);

        //    // 商店中选择扩展包
        //    go = UtilApi.TransFindChildByPObjAndPath(sceneRoot, "shop/btn/btn1");     // 商店按钮
        //    Ctx.m_instance.m_interActiveEntityMgr.addSceneEntity(go, EntityType.eETShopSelectPack);

        //    go = UtilApi.TransFindChildByPObjAndPath(sceneRoot, "shop/btn/btn2");     // 商店按钮
        //    Ctx.m_instance.m_interActiveEntityMgr.addSceneEntity(go, EntityType.eETShopSelectPack);

        //    go = UtilApi.TransFindChildByPObjAndPath(sceneRoot, "shop/btn/btn7");     // 商店按钮
        //    Ctx.m_instance.m_interActiveEntityMgr.addSceneEntity(go, EntityType.eETShopSelectPack);

        //    go = UtilApi.TransFindChildByPObjAndPath(sceneRoot, "shop/btn/btn15");     // 商店按钮
        //    Ctx.m_instance.m_interActiveEntityMgr.addSceneEntity(go, EntityType.eETShopSelectPack);

        //    go = UtilApi.TransFindChildByPObjAndPath(sceneRoot, "shop/btn/btn40");     // 商店按钮
        //    Ctx.m_instance.m_interActiveEntityMgr.addSceneEntity(go, EntityType.eETShopSelectPack);

        //    go = UtilApi.TransFindChildByPObjAndPath(sceneRoot, "shop/close");     // 商店关闭按钮
        //    Ctx.m_instance.m_interActiveEntityMgr.addSceneEntity(go, EntityType.eETShopClose);




        //    go = UtilApi.GoFindChildByPObjAndName("openbtn");   // 打开扩展背包按钮
        //    Ctx.m_instance.m_interActiveEntityMgr.addSceneEntity(go, EntityType.eETBtn, EntityTag.eETagExtPack);

        //    go = UtilApi.GoFindChildByPObjAndName("open");
        //    Ctx.m_instance.m_interActiveEntityMgr.addSceneEntity(go, EntityType.eETOpen);
            //GameObject go;
            //go = UtilApi.GoFindChildByPObjAndName("mcam");
            //Ctx.m_instance.m_interActiveEntityMgr.addSceneEntity(go, EntityType.eETMcam);


        //    go = UtilApi.GoFindChildByPObjAndName("open/3dbtn/btn");   // 打开扩展背包按钮
        //    Ctx.m_instance.m_interActiveEntityMgr.addSceneEntity(go, EntityType.eETBtn, EntityTag.eETaggoback);

        //    go = UtilApi.GoFindChildByPObjAndName("box/drawer/wdscbtn");
        //    Ctx.m_instance.m_interActiveEntityMgr.addSceneEntity(go, EntityType.eETBtn, EntityTag.eETagwdscbtn);

        //    go = UtilApi.GoFindChildByPObjAndName("wdscjm");
        //    Ctx.m_instance.m_interActiveEntityMgr.addSceneEntity(go, EntityType.eETwdscjm);

        //    go = UtilApi.GoFindChildByPObjAndName("box/rightdoor/yuanpan/dzmoshibtn");
        //    Ctx.m_instance.m_interActiveEntityMgr.addSceneEntity(go, EntityType.eETBtn, EntityTag.eETagdzmoshibtn);

        //    go = UtilApi.GoFindChildByPObjAndName("moshijm");
        //    Ctx.m_instance.m_interActiveEntityMgr.addSceneEntity(go, EntityType.eETdzmoshibtn);

        //    go = UtilApi.GoFindChildByPObjAndName("mcam/shop/buykuan/goldbuy");
        //    Ctx.m_instance.m_interActiveEntityMgr.addSceneEntity(go, EntityType.eETBtn, EntityTag.eETaggoldbuy);
        //}

        // 加载 Main Scene UI
        protected void loadAllUIScene()
        {
            Ctx.m_instance.m_uiSceneMgr.loadSceneForm(UISceneFormID.eUISceneMain);
            Ctx.m_instance.m_uiSceneMgr.showSceneForm(UISceneFormID.eUISceneMain);

            Ctx.m_instance.m_uiSceneMgr.loadSceneForm(UISceneFormID.eUISceneHero);
            Ctx.m_instance.m_uiSceneMgr.readySceneForm(UISceneFormID.eUISceneHero);
            Ctx.m_instance.m_uiSceneMgr.loadSceneForm(UISceneFormID.eUISceneBg);
            Ctx.m_instance.m_uiSceneMgr.readySceneForm(UISceneFormID.eUISceneBg);
        }
    }
}