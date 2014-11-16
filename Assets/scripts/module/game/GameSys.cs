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

        public void Update()
        {
            if (Input.GetKeyUp(KeyCode.M))  // 加载场景资源
            {

            }
            else if(Input.GetKeyUp(KeyCode.K))  // 加载 UI 资源
            {
                loadUI();
            }
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
        }

        public void loadUI()
        {
            //LoadParam param = (Ctx.m_instance.m_resMgr as IResMgr).getLoadParam();
            //param.m_path = Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI] + "UIScrollForm.unity3d";
            //param.m_type = ResPackType.eBundleType;
            //param.m_resLoadType = ResLoadType.eLoadDicWeb;
            //param.m_prefabName = "UIScrollForm";
            //param.m_loadedcb = onResLoad;
            //param.m_resNeedCoroutine = false;
            //param.m_loadNeedCoroutine = true;
            //Ctx.m_instance.m_resMgr.load(param);

            Ctx.m_instance.m_UIMgr.loadForm(UIFormID.UIBackPack);
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