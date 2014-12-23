using UnityEngine;
using System.Collections;
using SDK.Common;
using SDK.Lib;
using BehaviorLibrary;

namespace Game.App
{
    /**
     * @brief 数据系统
     */
    public class AppSys : Object
    {
        public void Awake(Transform transform)
        {
            ByteUtil.checkEndian();     // 检查系统大端小端

            Ctx.m_instance = new Ctx();
            Ctx.m_instance.Awake();
            Ctx.m_instance.m_cfg = new Config();
            Ctx.m_instance.m_netMgr = new NetworkMgr();
            Ctx.m_instance.m_log = new Logger();
            Ctx.m_instance.m_resMgr = new ResMgr();
            Ctx.m_instance.m_inputMgr = new InputMgr();
            Ctx.m_instance.m_dataTrans = transform;

            Ctx.m_instance.m_processSys = new ProcessSys();
            Ctx.m_instance.m_tickMgr = new TickMgr();
            Ctx.m_instance.m_timerMgr = new TimerMgr();
            Ctx.m_instance.m_coroutineMgr = new CoroutineMgr();
            Ctx.m_instance.m_shareMgr = new ShareMgr();
            Ctx.m_instance.m_sceneSys = new SceneSys();
            Ctx.m_instance.m_layerMgr = new LayerMgr();

            Ctx.m_instance.m_uiMgr = new UIMgr();
            Ctx.m_instance.m_engineLoop = new EngineLoop();
            Ctx.m_instance.m_resizeMgr = new ResizeMgr();

            Ctx.m_instance.m_playerMgr = new PlayerMgr();
            Ctx.m_instance.m_monsterMgr = new MonsterMgr();
            Ctx.m_instance.m_fObjectMgr = new FObjectMgr();
            Ctx.m_instance.m_npcMgr = new NpcMgr();

            Ctx.m_instance.m_camSys = new CamSys();
            Ctx.m_instance.m_meshMgr = new MeshMgr();
            Ctx.m_instance.m_aiSystem = new AISystem();
            Ctx.m_instance.m_sysMsgRoute = new SysMsgRoute();
            Ctx.m_instance.m_moduleSys = new ModuleSys();
            Ctx.m_instance.m_tableSys = new TableSys();
            Ctx.m_instance.m_localFileSys = new LocalFileSys();
            Ctx.m_instance.m_factoryBuild = new FactoryBuild();

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
            Ctx.m_instance.m_engineLoop.MainLoop();
        }

        public void OnApplicationQuit()
        {

        }

        public void PostInit()
        {
            //Ctx.m_instance.m_TickMgr.AddTickObj(Ctx.m_instance.m_resMgr as ITickedObject);
            Ctx.m_instance.m_resizeMgr.addResizeObject(Ctx.m_instance.m_uiMgr as IResizeObject);
            Ctx.m_instance.m_tickMgr.AddTickObj(Ctx.m_instance.m_inputMgr as ITickedObject);
            Ctx.m_instance.m_tickMgr.AddTickObj(Ctx.m_instance.m_playerMgr as ITickedObject);
            Ctx.m_instance.m_tickMgr.AddTickObj(Ctx.m_instance.m_monsterMgr as ITickedObject);
            Ctx.m_instance.m_tickMgr.AddTickObj(Ctx.m_instance.m_fObjectMgr as ITickedObject);
            Ctx.m_instance.m_tickMgr.AddTickObj(Ctx.m_instance.m_npcMgr as ITickedObject);

            Ctx.m_instance.m_tableSys.loadOneTable(TableID.TABLE_OBJECT);
        }

        public void setNoDestroyObject()
        {
            //GameObject nodestroy = GameObject.FindGameObjectWithTag("App");
            Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_App] = UtilApi.GoFindChildByPObjAndName(NotDestroyPath.ND_CV_App);
            //DontDestroyOnLoad(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_App]);
            UtilApi.DontDestroyOnLoad(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_App]);

            Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_RootLayer] = UtilApi.TransFindChildByPObjAndPath(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_App], NotDestroyPath.ND_CV_RootLayer);
            //DontDestroyOnLoad(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_RootLayer]);
            UtilApi.DontDestroyOnLoad(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_RootLayer]);

            Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_SceneLayer] = UtilApi.TransFindChildByPObjAndPath(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_App], NotDestroyPath.ND_CV_SceneLayer);
            //DontDestroyOnLoad(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_SceneLayer]);
            UtilApi.DontDestroyOnLoad(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_SceneLayer]);

            Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_GameLayer] = UtilApi.TransFindChildByPObjAndPath(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_App], NotDestroyPath.ND_CV_GameLayer);
            //DontDestroyOnLoad(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_GameLayer]);
            UtilApi.DontDestroyOnLoad(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_GameLayer]);

            Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UILayer] = UtilApi.TransFindChildByPObjAndPath(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_App], NotDestroyPath.ND_CV_UILayer);
            //DontDestroyOnLoad(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UILayer]);
            UtilApi.DontDestroyOnLoad(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UILayer]);

            Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UIRoot] = UtilApi.TransFindChildByPObjAndPath(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_App], NotDestroyPath.ND_CV_UIRoot);
            //DontDestroyOnLoad(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UIRoot]);
            UtilApi.DontDestroyOnLoad(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UIRoot]);

            Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Camera] = UtilApi.TransFindChildByPObjAndPath(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_App], NotDestroyPath.ND_CV_Camera);
            //DontDestroyOnLoad(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Camera]);
            UtilApi.DontDestroyOnLoad(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Camera]);

            // NGUI 2.7.0 之前的版本，编辑器会将 width and height 作为 transform 的 local scale ，因此需要手工重置
            Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UIFirstLayer] = UtilApi.TransFindChildByPObjAndPath(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_App], NotDestroyPath.ND_CV_UIFirstLayer);
            //nodestroy.transform.localScale = Vector3.one;
            //DontDestroyOnLoad(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UIFirstLayer]);
            UtilApi.DontDestroyOnLoad(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UIFirstLayer]);
        }
    }
}