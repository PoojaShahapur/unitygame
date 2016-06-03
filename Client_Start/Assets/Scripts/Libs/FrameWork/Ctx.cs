namespace SDK.Lib
{
    /**
     * @brief 全局数据区
     */
    public class Ctx
    {
        static public Ctx m_instance;

        public NetworkMgr m_netMgr;                // 网络通信
        public Config m_cfg;                        // 整体配置文件
        public LogSys m_logSys;                       // 日志系统
        public ResLoadMgr m_resLoadMgr;                    // 资源管理器
        public InputMgr m_inputMgr;                // 输入管理器

        public IGameSys m_gameSys;                  // 游戏系统
        public SceneSys m_sceneSys;                // 场景系统
        public TickMgr m_tickMgr;                  // 心跳管理器
        public ProcessSys m_processSys;            // 游戏处理系统

        public TimerMgr m_timerMgr;                // 定时器系统
        public FrameTimerMgr m_frameTimerMgr;                // 定时器系统
        public UIMgr m_uiMgr;                      // UI 管理器
        public ResizeMgr m_resizeMgr;              // 窗口大小修改管理器
        public IUIEvent m_cbUIEvent;               // UI 事件回调
        public CoroutineMgr m_coroutineMgr;        // 协程管理器

        public EngineLoop m_engineLoop;            // 引擎循环
        public GameAttr m_gameAttr;                // 游戏属性
        public FObjectMgr m_fObjectMgr;            // 掉落物管理器
        public NpcMgr m_npcMgr;                    // Npc管理器
        public PlayerMgr m_playerMgr;              // Player管理器
        public MonsterMgr m_monsterMgr;            // Monster 管理器
        public SpriteAniMgr m_spriteAniMgr;

        public ShareData m_shareData;               // 共享数据系统
        public LayerMgr m_layerMgr;                 // 层管理器
        public ISceneEventCB m_sceneEventCB;        // 场景加载事件
        public CamSys m_camSys;

        public ISceneLogic m_sceneLogic;
        public SysMsgRoute m_sysMsgRoute;           // 消息分发
        public NetCmdNotify m_netCmdNotify;           // 网络处理器
        public MsgRouteNotify m_msgRouteNotify;           // RouteMsg 客户端自己消息流程
        public IModuleSys m_moduleSys;              // 模块
        public TableSys m_tableSys;                // 表格
        public MFileSys m_fileSys;        // 文件系统
        public FactoryBuild m_factoryBuild;        // 生成各种内容，上层只用接口

        public LangMgr m_langMgr;                  // 语言管理器
        public DataPlayer m_dataPlayer;
        public XmlCfgMgr m_xmlCfgMgr;
        public MaterialMgr m_matMgr;
        public ModelMgr m_modelMgr;
        public TextureMgr m_texMgr;
        public SkelAniMgr m_skelAniMgr;
        public SkinResMgr m_skinResMgr;
        public PrefabMgr m_prefabMgr;
        public ControllerMgr m_controllerMgr;
        public BytesResMgr m_bytesResMgr;
        public SpriteMgr mSpriteMgr;

        public SystemSetting m_systemSetting;
        public CoordConv m_coordConv;
        public FlyNumMgr m_pFlyNumMgr;              // Header Num

        public TimerMsgHandle m_pTimerMsgHandle;
        //public WebSocketMgr m_pWebSocketMgr;
        public PoolSys m_poolSys;
        public ILoginSys m_loginSys;
        public WordFilterManager m_wordFilterManager;
        public VersionSys m_versionSys;
        public AutoUpdateSys m_pAutoUpdateSys;

        public TaskQueue m_TaskQueue;
        public TaskThreadPool m_TaskThreadPool;

        public RandName m_pRandName;
        public PakSys m_pPakSys;
        public GameRunStage m_gameRunStage;
        public SoundMgr m_soundMgr;
        public MapCfg m_mapCfg;

        public IAutoUpdate m_autoUpdate;
        public AtlasMgr m_atlasMgr;
        public AuxUIHelp m_auxUIHelp;
        public WidgetStyleMgr m_widgetStyleMgr;
        public SceneEffectMgr m_sceneEffectMgr;
        public SystemFrameData m_systemFrameData;
        public SystemTimeData m_systemTimeData;
        public ScriptDynLoad m_scriptDynLoad;
        public ScenePlaceHolder m_scenePlaceHolder;

        public LuaSystem m_luaSystem;
        public MovieMgr m_movieMgr;    // 视频 Clip 播放
        public NativeInterface m_nativeInterface;   // 本地接口调用
        public GCAutoCollect m_gcAutoCollect;     // 自动垃圾回收
        public MemoryCheck m_memoryCheck;       // 内存查找
        public DepResMgr m_depResMgr;
        public MTerrainGroup m_terrainGroup;
        public TextResMgr m_textResMgr;
        public MSceneManager m_sceneManager;
        public TerrainBufferSys m_terrainBufferSys;
        public TerrainGlobalOption mTerrainGlobalOption;
        public CoroutineTaskMgr mCoroutineTaskMgr;
        public SceneNodeGraph mSceneNodeGraph;
        public TerrainEntityMgr mTerrainEntityMgr;

        public ResRedirect mResRedirect;             // 重定向

        public Ctx()
        {
            
        }

        public static Ctx instance()
        {
            if (m_instance == null)
            {
                m_instance = new Ctx();
            }
            return m_instance;
        }

        protected void preInit()
        {
            UtilByte.checkEndian();     // 检查系统大端小端
            MThread.getMainThreadID();  // 获取主线程 ID

            mTerrainGlobalOption = new TerrainGlobalOption();

            m_netCmdNotify = new NetCmdNotify();
            m_msgRouteNotify = new MsgRouteNotify();

            m_xmlCfgMgr = new XmlCfgMgr();
            m_matMgr = new MaterialMgr();
            m_modelMgr = new ModelMgr();
            m_texMgr = new TextureMgr();
            m_skelAniMgr = new SkelAniMgr();
            m_skinResMgr = new SkinResMgr();
            m_prefabMgr = new PrefabMgr();
            m_controllerMgr = new ControllerMgr();
            m_bytesResMgr = new BytesResMgr();
            mSpriteMgr = new SpriteMgr();

            m_systemSetting = new SystemSetting();
            m_coordConv = new CoordConv();
            m_pFlyNumMgr = new FlyNumMgr();              // Header Num

            m_pTimerMsgHandle = new TimerMsgHandle();
            m_poolSys = new PoolSys();
            m_wordFilterManager = new WordFilterManager();
            m_versionSys = new VersionSys();
            m_pAutoUpdateSys = new AutoUpdateSys();

            m_TaskQueue = new TaskQueue("TaskQueue");
            m_TaskThreadPool = new TaskThreadPool();

            m_pRandName = new RandName();
            m_pPakSys = new PakSys();
            m_gameRunStage = new GameRunStage();
            m_soundMgr = new SoundMgr();
            m_mapCfg = new MapCfg();

            m_atlasMgr = new AtlasMgr();
            m_auxUIHelp = new AuxUIHelp();
            m_widgetStyleMgr = new WidgetStyleMgr();
            m_systemFrameData = new SystemFrameData();
            m_systemTimeData = new SystemTimeData();
            m_scriptDynLoad = new ScriptDynLoad();
            m_scenePlaceHolder = new ScenePlaceHolder();

            m_luaSystem = new LuaSystem();
            m_movieMgr = new MovieMgr();
            m_nativeInterface = new NativeInterface();
            m_gcAutoCollect = new GCAutoCollect();
            m_memoryCheck = new MemoryCheck();
            m_depResMgr = new DepResMgr();
            m_terrainGroup = new MTerrainGroup(mTerrainGlobalOption.mTerrainSize, mTerrainGlobalOption.mTerrainWorldSize);
            m_textResMgr = new TextResMgr();
            m_terrainBufferSys = new TerrainBufferSys();
            //m_terrainGroup = new MTerrainGroup(513, 512);

            m_cfg = new Config();
            m_dataPlayer = new DataPlayer();
            m_factoryBuild = new FactoryBuild();

            m_netMgr = new NetworkMgr();
            m_resLoadMgr = new ResLoadMgr();
            m_inputMgr = new InputMgr();

            m_processSys = new ProcessSys();
            m_tickMgr = new TickMgr();
            m_timerMgr = new TimerMgr();
            m_frameTimerMgr = new FrameTimerMgr();
            m_coroutineMgr = new CoroutineMgr();
            m_shareData = new ShareData();
            m_sceneSys = new SceneSys();
            m_layerMgr = new LayerMgr();

            m_uiMgr = new UIMgr();
            m_engineLoop = new EngineLoop();
            m_resizeMgr = new ResizeMgr();

            m_playerMgr = new PlayerMgr();
            m_monsterMgr = new MonsterMgr();
            m_fObjectMgr = new FObjectMgr();
            m_npcMgr = new NpcMgr();
            m_spriteAniMgr = new SpriteAniMgr();

            m_camSys = new CamSys();
            m_sysMsgRoute = new SysMsgRoute("SysMsgRoute");
            m_moduleSys = new ModuleSys();
            m_tableSys = new TableSys();
            m_fileSys = new MFileSys();
            m_logSys = new LogSys();
            m_langMgr = new LangMgr();
            m_sceneEffectMgr = new SceneEffectMgr();

            m_sceneManager = new MOctreeSceneManager("DummyScene");
            mCoroutineTaskMgr = new CoroutineTaskMgr();
            mSceneNodeGraph = new SceneNodeGraph();
            mTerrainEntityMgr = new TerrainEntityMgr();

            mResRedirect = new ResRedirect();
        }

        protected void interInit()
        {
            m_luaSystem.init();
            m_uiMgr.init();
        }

        public void postInit()
        {
            m_resizeMgr.addResizeObject(m_uiMgr as IResizeObject);
            m_tickMgr.addTick(m_inputMgr as ITickedObject);
            m_inputMgr.postInit();
            //m_tickMgr.addTick(m_playerMgr as ITickedObject);
            //m_tickMgr.addTick(m_monsterMgr as ITickedObject);
            //m_tickMgr.addTick(m_fObjectMgr as ITickedObject);
            //m_tickMgr.addTick(m_npcMgr as ITickedObject);
            m_tickMgr.addTick(m_spriteAniMgr as ITickedObject);
            m_tickMgr.addTick(m_sceneEffectMgr as ITickedObject);
            //m_tickMgr.addTick(m_sceneCardMgr as ITickedObject);
            //m_tickMgr.addTick(m_aiSystem.aiControllerMgr as ITickedObject);

            m_uiMgr.findCanvasGO();
            m_dataPlayer.m_dataPack.postConstruct();

            // 初始化重定向
            mResRedirect.postInit();
            m_resLoadMgr.postInit();

            m_TaskQueue.m_pTaskThreadPool = m_TaskThreadPool;
            m_TaskThreadPool.initThreadPool(2, m_TaskQueue);

            m_depResMgr.init();
            mCoroutineTaskMgr.start();
            m_versionSys.loadLocalVer();
        }

        public void init()
        {
            preInit();
            interInit();

            // 设置不释放 GameObject
            setNoDestroyObject();

            postInit();
            // 交叉引用的对象初始化
            // Unity 编辑器设置的基本数据
            initBasicCfg();
        }

        public void setNoDestroyObject()
        {
            m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Root] = UtilApi.GoFindChildByName(NotDestroyPath.ND_CV_Root);
            UtilApi.DontDestroyOnLoad(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Root]);

            setNoDestroyObject_impl(NotDestroyPath.ND_CV_App, NotDestroyPath.ND_CV_Root);
            setNoDestroyObject_impl(NotDestroyPath.ND_CV_UIFirstCanvas, NotDestroyPath.ND_CV_Root);
            setNoDestroyObject_impl(NotDestroyPath.ND_CV_UISecondCanvas, NotDestroyPath.ND_CV_Root);
            setNoDestroyObject_impl(NotDestroyPath.ND_CV_UICamera, NotDestroyPath.ND_CV_Root);
            setNoDestroyObject_impl(NotDestroyPath.ND_CV_EventSystem, NotDestroyPath.ND_CV_Root);
            // NGUI 2.7.0 之前的版本，编辑器会将 width and height 作为 transform 的 local scale ，因此需要手工重置
            setNoDestroyObject_impl(NotDestroyPath.ND_CV_UIBtmLayer_FirstCanvas, NotDestroyPath.ND_CV_Root);
            setNoDestroyObject_impl(NotDestroyPath.ND_CV_UIFirstLayer_FirstCanvas, NotDestroyPath.ND_CV_Root);
            setNoDestroyObject_impl(NotDestroyPath.ND_CV_UISecondLayer_FirstCanvas, NotDestroyPath.ND_CV_Root);
            setNoDestroyObject_impl(NotDestroyPath.ND_CV_UIThirdLayer_FirstCanvas, NotDestroyPath.ND_CV_Root);
            setNoDestroyObject_impl(NotDestroyPath.ND_CV_UIForthLayer_FirstCanvas, NotDestroyPath.ND_CV_Root);
            setNoDestroyObject_impl(NotDestroyPath.ND_CV_UITopLayer_FirstCanvas, NotDestroyPath.ND_CV_Root);

            setNoDestroyObject_impl(NotDestroyPath.ND_CV_UIBtmLayer_SecondCanvas, NotDestroyPath.ND_CV_Root);
            setNoDestroyObject_impl(NotDestroyPath.ND_CV_UIFirstLayer_SecondCanvas, NotDestroyPath.ND_CV_Root);
            setNoDestroyObject_impl(NotDestroyPath.ND_CV_UISecondLayer_SecondCanvas, NotDestroyPath.ND_CV_Root);
            setNoDestroyObject_impl(NotDestroyPath.ND_CV_UIThirdLayer_SecondCanvas, NotDestroyPath.ND_CV_Root);
            setNoDestroyObject_impl(NotDestroyPath.ND_CV_UIForthLayer_SecondCanvas, NotDestroyPath.ND_CV_Root);
            setNoDestroyObject_impl(NotDestroyPath.ND_CV_UITopLayer_SecondCanvas, NotDestroyPath.ND_CV_Root);
        }

        protected void setNoDestroyObject_impl(string child, string parent)
        {
            m_layerMgr.m_path2Go[child] = UtilApi.TransFindChildByPObjAndPath(m_layerMgr.m_path2Go[parent], child);
            //UtilApi.DontDestroyOnLoad(m_layerMgr.m_path2Go[child]);
        }

        protected void initBasicCfg()
        {
            BasicConfig basicCfg = m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Root].GetComponent<BasicConfig>();
            //m_cfg.m_ip = basicCfg.getIp();
            m_cfg.m_zone = basicCfg.getPort();
        }

        // 卸载所有的资源
        public void unloadAll()
        {
            // 卸载所有的模型
            m_modelMgr.unloadAll();
            // 卸载所有的材质
            m_matMgr.unloadAll();
            // 卸载所有的纹理
            m_texMgr.unloadAll();
            // 卸载音乐
            m_soundMgr.unloadAll();
            // 场景卸载
            m_sceneSys.unloadAll();
        }
    }
}