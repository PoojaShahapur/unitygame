using System;
using UnityEngine;
using System.Collections.Generic;
using SDK.Common;
using Game.UI;
using SDK.Lib;

namespace Game.Game
{
    public class GameSys : IGameSys
    {
        public bool m_firstLoad = true;     // 是否是第一次加载游戏场景
        public bool m_bInDZScene = false;   // 是否在对战场景

        public void Start()
        {
            initGVar();
            loadGameScene();
        }

        public void initGVar()
        {
            // 获取全局变量
            //GameObject nodestroy = GameObject.FindGameObjectWithTag("App");
            //GameObject nodestroy = UtilApi.GoFindChildByPObjAndName(NotDestroyPath.ND_CV_App);
            //AppRoot approot = nodestroy.GetComponent<AppRoot>();

            // 游戏逻辑处理
            Ctx.m_instance.m_cbUIEvent = new GameUIEventCB();
            Ctx.m_instance.m_netHandle = new GameNetHandleCB();
            Ctx.m_instance.m_bStopNetHandle = false;     // 停止网络消息处理
            Ctx.m_instance.m_sceneEventCB = new GameSceneEventCB();
            Ctx.m_instance.m_sceneLogic = new GameSceneLogic();

            //Ctx.m_instance.m_meshMgr.loadSkinInfo();
            //Ctx.m_instance.m_aiSystem.getBehaviorTreeMgr().loadBT();

            // 立即加载 UIBlurBg 界面
            Ctx.m_instance.m_uiMgr.loadForm<UIBlurBg>(UIFormID.UIBlurBg);
        }

        public void loadGameScene()
        {
            unloadDZAllUIScene();
            //Ctx.m_instance.m_sceneSys.loadScene("cave", onResLoadScene);
            Ctx.m_instance.m_sceneSys.loadScene("game", onGameResLoadScene);
            m_bInDZScene = false;
        }

        public void loadDZScene()
        {
            if (!m_bInDZScene)
            {
                m_bInDZScene = true;
                Ctx.m_instance.m_uiSceneMgr.unloadAll();
                Ctx.m_instance.m_sceneSys.loadScene("dz", onDZResLoadScene);
            }
        }

        // 这个是操作场景资源加载完成回调
        public void onGameResLoadScene(Scene scene)
        {
            if (m_firstLoad)
            {
                Ctx.m_instance.m_camSys.m_boxcam = new SDK.Lib.boxcam();

                // 卸载登陆模块，关闭登陆界面
                Ctx.m_instance.m_moduleSys.unloadModule(ModuleID.LOGINMN);
                Ctx.m_instance.m_uiMgr.exitForm(UIFormID.UILogin);
                Ctx.m_instance.m_uiMgr.exitForm(UIFormID.UIHeroSelect);
                Ctx.m_instance.m_uiMgr.exitForm(UIFormID.UIChat);      // 退出聊天

                // 请求主角基本数据
                Ctx.m_instance.m_dataPlayer.reqMainData();
            }

            Ctx.m_instance.m_log.log("场景加载成功");
            loadAllUIScene();
            Ctx.m_instance.m_camSys.m_boxcam.setGameObject(UtilApi.GoFindChildByPObjAndName("mcam"));
            Ctx.m_instance.m_sceneEventCB.onLevelLoaded();

            m_firstLoad = false;
        }

        // 这个是对战场景资源加载完成回调
        public void onDZResLoadScene(Scene scene)
        {
            Ctx.m_instance.m_dataPlayer.m_dzData.clear();
            Ctx.m_instance.m_dataPlayer.m_dzData.m_canReqDZ = true;         // 进入对战就设置这个标示位为可以继续战斗
            Ctx.m_instance.m_camSys.m_dzcam = new dzcam();

            loadAllDZUIScene();
        }

        // 加载 Main Scene UI
        protected void loadAllUIScene()
        {
            Ctx.m_instance.m_uiSceneMgr.loadSceneForm<UISceneMain>(UISceneFormID.eUISceneMain);
            Ctx.m_instance.m_uiSceneMgr.showSceneForm(UISceneFormID.eUISceneMain);

            Ctx.m_instance.m_uiSceneMgr.loadSceneForm<UISceneHero>(UISceneFormID.eUISceneHero);
            Ctx.m_instance.m_uiSceneMgr.readySceneForm(UISceneFormID.eUISceneHero);
            Ctx.m_instance.m_uiSceneMgr.loadSceneForm<UISceneBg>(UISceneFormID.eUISceneBg);
            Ctx.m_instance.m_uiSceneMgr.readySceneForm(UISceneFormID.eUISceneBg);
        }

        protected void loadAllDZUIScene()
        {
            Ctx.m_instance.m_uiMgr.loadForm<UITest>(UIFormID.UITest);
            Ctx.m_instance.m_uiMgr.loadForm<UIDZ>(UIFormID.UIDZ);      // 显示对战场景界面
            Ctx.m_instance.m_uiMgr.loadForm<UIChat>(UIFormID.UIChat);      // 显示聊天
            Ctx.m_instance.m_uiSceneMgr.loadAndShowForm<UISceneDZ>(UISceneFormID.eUISceneDZ);      // 显示对战场景界面
        }

        protected void unloadDZAllUIScene()
        {
            Ctx.m_instance.m_uiMgr.exitForm(UIFormID.UITest, true);
            Ctx.m_instance.m_uiMgr.exitForm(UIFormID.UIDZ, true);      // 显示对战场景界面
            Ctx.m_instance.m_uiMgr.exitForm(UIFormID.UIChat, true);      // 显示聊天
            Ctx.m_instance.m_uiSceneMgr.unloadAll();
        }
    }
}