using UnityEngine;
using System.Collections;
using SDK.Common;
using SDK.Lib;

namespace Game.App
{
    /**
     * @brief 数据系统
     */
    public class AppSys : Object
    {
        public void Awake(Transform transform)
        {
            Ctx.m_instance = new Ctx();
            Ctx.m_instance.Awake();
            Ctx.m_instance.m_netMgr = new NetworkMgr();
            Ctx.m_instance.m_cfg = new Config();
            Ctx.m_instance.m_log = new Logger();
            Ctx.m_instance.m_resMgr = new ResMgr();
            Ctx.m_instance.m_inputMgr = new InputMgr();
            Ctx.m_instance.m_dataTrans = transform;

            Ctx.m_instance.m_ProcessSys = new ProcessSys();
            Ctx.m_instance.m_TickMgr = new TickMgr();
            Ctx.m_instance.m_TimerMgr = new TimerMgr();
            Ctx.m_instance.m_CoroutineMgr = new CoroutineMgr();
            Ctx.m_instance.m_shareMgr = new ShareMgr();
            Ctx.m_instance.m_sceneSys = new SceneSys();
            Ctx.m_instance.m_layerMgr = new LayerMgr();

            Ctx.m_instance.m_UIMgr = new UIMgr();
            Ctx.m_instance.m_EngineLoop = new EngineLoop();
            Ctx.m_instance.m_ResizeMgr = new ResizeMgr();

            PostInit();
        }

        // Use this for initialization
        public void Start()
        {
            Ctx.m_instance.Start();
        }

        // Update is called once per frame
        public void Update()
        {
            //Ctx.m_instance.Update();
            Ctx.m_instance.m_EngineLoop.MainLoop();
        }

        public void OnApplicationQuit()
        {

        }

        public void PostInit()
        {
            //Ctx.m_instance.m_TickMgr.AddTickObj(Ctx.m_instance.m_resMgr as ITickedObject);
            Ctx.m_instance.m_ResizeMgr.addResizeObject(Ctx.m_instance.m_UIMgr as IResizeObject);
            Ctx.m_instance.m_TickMgr.AddTickObj(Ctx.m_instance.m_inputMgr as ITickedObject);
        }

        public void setNoDestroyObject()
        {
            //GameObject nodestroy = GameObject.FindGameObjectWithTag("App");
            Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_App] = UtilApi.GoFindChildByPObjAndName(NotDestroyPath.ND_CV_App);
            DontDestroyOnLoad(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_App]);

            Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_RootLayer] = UtilApi.TransFindChildByPObjAndPath(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_App], NotDestroyPath.ND_CV_RootLayer);
            DontDestroyOnLoad(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_RootLayer]);

            Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_SceneLayer] = UtilApi.TransFindChildByPObjAndPath(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_App], NotDestroyPath.ND_CV_SceneLayer);
            DontDestroyOnLoad(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_SceneLayer]);

            Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_GameLayer] = UtilApi.TransFindChildByPObjAndPath(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_App], NotDestroyPath.ND_CV_GameLayer);
            DontDestroyOnLoad(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_GameLayer]);

            Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UILayer] = UtilApi.TransFindChildByPObjAndPath(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_App], NotDestroyPath.ND_CV_UILayer);
            DontDestroyOnLoad(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UILayer]);

            Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UIRoot] = UtilApi.TransFindChildByPObjAndPath(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_App], NotDestroyPath.ND_CV_UIRoot);
            DontDestroyOnLoad(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UIRoot]);

            Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Camera] = UtilApi.TransFindChildByPObjAndPath(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_App], NotDestroyPath.ND_CV_Camera);
            DontDestroyOnLoad(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Camera]);

            // NGUI 2.7.0 之前的版本，编辑器会将 width and height 作为 transform 的 local scale ，因此需要手工重置
            Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UIFirstLayer] = UtilApi.TransFindChildByPObjAndPath(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_App], NotDestroyPath.ND_CV_UIFirstLayer);
            //nodestroy.transform.localScale = Vector3.one;
            DontDestroyOnLoad(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UIFirstLayer]);
        }

        // 加载游戏模块
        public void loadGame()
        {
            // 初始化完成，开始加载自己的游戏场景
            LoadParam param = (Ctx.m_instance.m_resMgr as ResMgr).loadParam;
            param.m_path = Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathModule] + "Game.unity3d";
            //param.m_resPackType = ResPackType.eBundleType;
            param.m_loadedcb = onGameLoaded;
            //param.m_resLoadType = Ctx.m_instance.m_cfg.m_resLoadType;
            //Ctx.m_instance.m_resMgr.load(param);
            Ctx.m_instance.m_resMgr.loadBundle(param);
        }

        public void onGameLoaded(SDK.Common.Event resEvt)
        {
            IRes res = resEvt.m_param as IRes;                         // 类型转换
            //GameObject go = (res as IBundleRes).InstantiateObject("Game");
            //GameObject nodestroy = GameObject.FindGameObjectWithTag("GameLayer");
            Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Game] = res.InstantiateObject("Game");
            Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Game].name = NotDestroyPath.CV_GameName;
            Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Game].transform.parent = Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_GameLayer].transform;

            // 游戏模块也不释放
            DontDestroyOnLoad(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Game]);
            //go.SetActive(false);         // 自己会更新的，不用这里更新
        }
    }
}