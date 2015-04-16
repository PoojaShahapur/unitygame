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
        public GameRouteCB m_gameRouteCB;
        public GameNetHandleCB m_gameNetHandleCB;
        protected GotoScene m_gotoScene;

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

            m_gotoScene = new GotoScene();

            // 游戏逻辑处理
            Ctx.m_instance.m_cbUIEvent = new GameUIEventCB();
            m_gameNetHandleCB = new GameNetHandleCB();
            Ctx.m_instance.m_netDispList.addOneDisp(m_gameNetHandleCB);
            m_gameRouteCB = new GameRouteCB();
            Ctx.m_instance.m_msgRouteList.addOneDisp(m_gameRouteCB);
            Ctx.m_instance.m_bStopNetHandle = false;     // 停止网络消息处理
            Ctx.m_instance.m_sceneEventCB = new GameSceneEventCB();
            Ctx.m_instance.m_sceneLogic = new GameSceneLogic();

            m_gotoScene.addSceneHandle();

            //Ctx.m_instance.m_meshMgr.loadSkinInfo();
            //Ctx.m_instance.m_aiSystem.getBehaviorTreeMgr().loadBT();

            // 立即加载 UIBlurBg 界面
            Ctx.m_instance.m_uiMgr.loadForm<UIBlurBg>(UIFormID.UIBlurBg);
        }

        public void loadGameScene()
        {
            m_gotoScene.loadGameScene();
        }

        public void loadDZScene()
        {
            m_gotoScene.loadDZScene();
        }
    }
}