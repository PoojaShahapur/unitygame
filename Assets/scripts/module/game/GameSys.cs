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
            GameSys.m_instance.m_ctx.m_UIMgr.SetIUIFactory(new UIFactory());
            // 游戏逻辑处理
            Ctx.m_instance.m_cbUIEvent = new GameUIEventCB();
            Ctx.m_instance.m_sceneEventCB = new SceneEventCB();
            Ctx.m_instance.m_sceneLogic = new GameSceneLogic();

            //Ctx.m_instance.m_meshMgr.loadSkinInfo();
        }

        public void loadScene()
        {
            //Ctx.m_instance.m_sceneSys.loadScene("cave", onResLoadScene);
            Ctx.m_instance.m_sceneSys.loadScene("TestScene1f", onResLoadScene);
        }

        public void onResLoad(EventDisp resEvt)
        {
            IRes res = resEvt.m_param as IRes;                         // 类型转换
            GameObject go = res.InstantiateObject("UIScrollForm");
            GameObject nodestroy = GameObject.FindGameObjectWithTag("UIFirstLayer");
            go.transform.parent = nodestroy.transform;
        }

        public void onResLoadScene(IScene scene)
        {
            Ctx.m_instance.m_log.log("aaa");
            Ctx.m_instance.m_sceneEventCB.onLevelLoaded();
        }
    }
}