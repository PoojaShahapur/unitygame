using UnityEngine;
using System.Collections;
using SDK.Lib;
using SDK.Common;
using System;

#if UNIT_TEST_SRC
using UnitTestSrc;
#endif

/**
 * @brief 这个模块主要是代码，启动必要的核心代码都在这里，可能某些依赖模块延迟加载
 */
public class AppRoot : MonoBehaviour 
{
    void Awake()
    {
        //Application.targetFrameRate = 30;
    }

	// Use this for initialization
	void Start () 
    {
        Ctx.instance();
        init();
	}
	
	// Update is called once per frame
    void Update () 
    {
        //BugResolve();
        //try
        //{
            Ctx.m_instance.m_engineLoop.MainLoop();
        //}
        //catch(Exception err)
        //{
        //    Ctx.m_instance.m_logSys.log("Main Loop Error");
        //}
    }

    void OnApplicationQuit()
    {
        // 等待网络关闭
        Ctx.m_instance.m_netMgr.quipApp();
        // 卸载所有的资源
        Ctx.m_instance.unloadAll();
        // 关闭日志设备
        Ctx.m_instance.m_logSys.closeDevice();
    }

    // unity 自己产生的 bug ，DontDestroyOnLoad 的对象，加载 Level 后会再产生一个
    private void BugResolve()
    {
        GameObject[] nodestroy = GameObject.FindGameObjectsWithTag("App");  //得到存在的实例列表
        if (nodestroy.Length > 1)
        {
            // 将后产生的毁掉，保留第一个
            //GameObject.Destroy(nodestroy[1]);
            UtilApi.Destroy(nodestroy[1]);
        }
    }

    //public Ctx getCtx()
    //{
    //    return Ctx.m_instance;
    //}

    public void init()
    {
        // 构造所有的数据
        constructAll();
        // 设置不释放 GameObject
        setNoDestroyObject();
        // 交叉引用的对象初始化
        PostInit();
        // Unity 编辑器设置的基本数据
        initBasicCfg();

        // 加载模块
#if PKG_RES_LOAD
        Ctx.m_instance.m_moduleSys.loadModule(ModuleID.AUTOUPDATEMN);
#else
        Ctx.m_instance.m_moduleSys.loadModule(ModuleID.LOGINMN);
#endif

        // 运行单元测试
#if UNIT_TEST_SRC
        UnitTestMain pUnitTestMain = new UnitTestMain();
        pUnitTestMain.run();
#endif
    }

    public void constructAll()
    {
        ByteUtil.checkEndian();     // 检查系统大端小端

        Ctx.m_instance.m_cfg = new Config();
        Ctx.m_instance.m_factoryBuild = new FactoryBuild();

        Ctx.m_instance.m_netMgr = new NetworkMgr();
        Ctx.m_instance.m_logSys = new LogSys();
        Ctx.m_instance.m_resLoadMgr = new ResLoadMgr();
        Ctx.m_instance.m_inputMgr = new InputMgr();

        Ctx.m_instance.m_processSys = new ProcessSys();
        Ctx.m_instance.m_tickMgr = new TickMgr();
        Ctx.m_instance.m_timerMgr = new TimerMgr();
        Ctx.m_instance.m_coroutineMgr = new CoroutineMgr();
        Ctx.m_instance.m_shareData = new ShareData();
        Ctx.m_instance.m_sceneSys = new SceneSys();
        Ctx.m_instance.m_layerMgr = new LayerMgr();

        Ctx.m_instance.m_uiMgr = new UIMgr();
        Ctx.m_instance.m_uiSceneMgr = new UISceneMgr();
        Ctx.m_instance.m_engineLoop = new EngineLoop();
        Ctx.m_instance.m_resizeMgr = new ResizeMgr();

        Ctx.m_instance.m_playerMgr = new PlayerMgr();
        Ctx.m_instance.m_monsterMgr = new MonsterMgr();
        Ctx.m_instance.m_fObjectMgr = new FObjectMgr();
        Ctx.m_instance.m_npcMgr = new NpcMgr();

        Ctx.m_instance.m_camSys = new CamSys();
        Ctx.m_instance.m_aiSystem = new AISystem();
        Ctx.m_instance.m_sysMsgRoute = new SysMsgRoute("SysMsgRoute");
        Ctx.m_instance.m_moduleSys = new ModuleSys();
        Ctx.m_instance.m_tableSys = new TableSys();
        Ctx.m_instance.m_localFileSys = new LocalFileSys();
        Ctx.m_instance.m_langMgr = new LangMgr();
        //Ctx.m_instance.m_pWebSocketMgr = new WebSocketMgr();
    }

    public void PostInit()
    {
        Ctx.m_instance.m_resizeMgr.addResizeObject(Ctx.m_instance.m_uiMgr as IResizeObject);
        //m_tickMgr.AddTickObj(m_inputMgr as ITickedObject);
        Ctx.m_instance.m_inputMgr.postInit();
        Ctx.m_instance.m_tickMgr.addObject(Ctx.m_instance.m_playerMgr as ITickedObject);
        Ctx.m_instance.m_tickMgr.addObject(Ctx.m_instance.m_monsterMgr as ITickedObject);
        Ctx.m_instance.m_tickMgr.addObject(Ctx.m_instance.m_fObjectMgr as ITickedObject);
        Ctx.m_instance.m_tickMgr.addObject(Ctx.m_instance.m_npcMgr as ITickedObject);

        Ctx.m_instance.m_uiMgr.getLayerGameObject();
        Ctx.m_instance.m_dataPlayer.m_dataPack.postConstruct();
        Ctx.m_instance.m_dataPlayer.m_dataCard.registerCardAttr();     // 注册卡牌组属性
        Ctx.m_instance.m_resLoadMgr.postInit();

        Ctx.m_instance.m_TaskQueue.m_pTaskThreadPool = Ctx.m_instance.m_TaskThreadPool;
        Ctx.m_instance.m_TaskThreadPool.initThreadPool(2, Ctx.m_instance.m_TaskQueue);

        // 获取主线程 ID
        MThread.getMainThreadID();
    }

    public void setNoDestroyObject()
    {
        Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Root] = UtilApi.GoFindChildByPObjAndName(NotDestroyPath.ND_CV_Root);
        //UtilApi.DontDestroyOnLoad(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Root]);

        Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_App] = UtilApi.TransFindChildByPObjAndPath(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Root], NotDestroyPath.ND_CV_App);
        UtilApi.DontDestroyOnLoad(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_App]);

        Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UICanvas] = UtilApi.TransFindChildByPObjAndPath(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Root], NotDestroyPath.ND_CV_UICanvas);
        UtilApi.DontDestroyOnLoad(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UICanvas]);

        Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UICamera] = UtilApi.TransFindChildByPObjAndPath(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Root], NotDestroyPath.ND_CV_UICamera);
        UtilApi.DontDestroyOnLoad(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UICamera]);

        Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_EventSystem] = UtilApi.TransFindChildByPObjAndPath(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Root], NotDestroyPath.ND_CV_EventSystem);
        UtilApi.DontDestroyOnLoad(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_EventSystem]);

        // NGUI 2.7.0 之前的版本，编辑器会将 width and height 作为 transform 的 local scale ，因此需要手工重置
        Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UIBtmLayer] = UtilApi.TransFindChildByPObjAndPath(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Root], NotDestroyPath.ND_CV_UIBtmLayer);
        UtilApi.DontDestroyOnLoad(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UIBtmLayer]);

        Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UIFirstLayer] = UtilApi.TransFindChildByPObjAndPath(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Root], NotDestroyPath.ND_CV_UIFirstLayer);
        UtilApi.DontDestroyOnLoad(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UIFirstLayer]);

        Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UISecondLayer] = UtilApi.TransFindChildByPObjAndPath(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Root], NotDestroyPath.ND_CV_UISecondLayer);
        UtilApi.DontDestroyOnLoad(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UISecondLayer]);

        Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UIThirdLayer] = UtilApi.TransFindChildByPObjAndPath(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Root], NotDestroyPath.ND_CV_UIThirdLayer);
        UtilApi.DontDestroyOnLoad(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UIThirdLayer]);

        Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UIForthLayer] = UtilApi.TransFindChildByPObjAndPath(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Root], NotDestroyPath.ND_CV_UIForthLayer);
        UtilApi.DontDestroyOnLoad(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UIForthLayer]);

        Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UITopLayer] = UtilApi.TransFindChildByPObjAndPath(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Root], NotDestroyPath.ND_CV_UITopLayer);
        UtilApi.DontDestroyOnLoad(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UITopLayer]);

        Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UIModel] = UtilApi.TransFindChildByPObjAndPath(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Root], NotDestroyPath.ND_CV_UIModel);
        UtilApi.DontDestroyOnLoad(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UIModel]);

        Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UILight] = UtilApi.TransFindChildByPObjAndPath(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Root], NotDestroyPath.ND_CV_UILight);
        UtilApi.DontDestroyOnLoad(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UILight]);
    }

    protected void initBasicCfg()
    {
        BasicConfig basicCfg = Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Root].GetComponent<BasicConfig>();
        //Ctx.m_instance.m_cfg.m_ip = basicCfg.getIp();
        Ctx.m_instance.m_cfg.m_zone = basicCfg.getPort();
    }
}