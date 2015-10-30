using BehaviorLibrary;
using FightCore;

#if UNIT_TEST_SRC
using UnitTest;
#endif

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
        public UISceneMgr m_uiSceneMgr;            // UIScene 管理器
        public ResizeMgr m_resizeMgr;              // 窗口大小修改管理器
        public IUIEvent m_cbUIEvent;                // UI 事件回调
        public CoroutineMgr m_coroutineMgr;        // 协程管理器

        public EngineLoop m_engineLoop;            // 引擎循环
        public GameAttr m_gameAttr;                 // 游戏属性
        public FObjectMgr m_fObjectMgr;            // 掉落物管理器
        public NpcMgr m_npcMgr;                    // Npc管理器
        public PlayerMgr m_playerMgr;              // Player管理器
        public MonsterMgr m_monsterMgr;            // Monster 管理器
        public SpriteAniMgr m_spriteAniMgr;

        public ShareData m_shareData;                 // 共享数据系统
        public LayerMgr m_layerMgr;                 // 层管理器
        public ISceneEventCB m_sceneEventCB;        // 场景加载事件
        public CamSys m_camSys;

        public ISceneLogic m_sceneLogic;
        public AISystem m_aiSystem;
        public SysMsgRoute m_sysMsgRoute;           // 消息分发
        public NetDispList m_netDispList = new NetDispList();           // 网络处理器
        public MsgRouteDispList m_msgRouteList = new MsgRouteDispList();           // RouteMsg 客户端自己消息流程
        public IModuleSys m_moduleSys;              // 模块
        public TableSys m_tableSys;                // 表格
        public LocalFileSys m_localFileSys;        // 文件系统
        public FactoryBuild m_factoryBuild;        // 生成各种内容，上层只用接口

        public LangMgr m_langMgr;                  // 语言管理器
        public DataPlayer m_dataPlayer = new DataPlayer();
        public XmlCfgMgr m_xmlCfgMgr = new XmlCfgMgr();
        public MaterialMgr m_matMgr = new MaterialMgr();
        public ModelMgr m_modelMgr = new ModelMgr();
        public TextureMgr m_texMgr = new TextureMgr();
        public SkelAniMgr m_skelAniMgr = new SkelAniMgr();
        public SkinResMgr m_skinResMgr = new SkinResMgr();
        public UIPrefabMgr m_uiPrefabMgr = new UIPrefabMgr();
        public ControllerMgr m_controllerMgr = new ControllerMgr();

        public SystemSetting m_systemSetting = new SystemSetting();
        public CoordConv m_coordConv = new CoordConv();
        public FlyNumMgr m_pFlyNumMgr = new FlyNumMgr();              // Header Num

        public TimerMsgHandle m_pTimerMsgHandle = new TimerMsgHandle();
        //public WebSocketMgr m_pWebSocketMgr;
        public PoolSys m_poolSys = new PoolSys();
        public ILoginSys m_loginSys;
        public WordFilterManager m_wordFilterManager = new WordFilterManager();
        public VersionSys m_versionSys = new VersionSys();
        public AutoUpdateSys m_pAutoUpdateSys = new AutoUpdateSys();

        public TaskQueue m_TaskQueue = new TaskQueue("TaskQueue");
        public TaskThreadPool m_TaskThreadPool = new TaskThreadPool();

        public RandName m_pRandName = new RandName();
        public PakSys m_pPakSys = new PakSys();
        public GameRunStage m_gameRunStage = new GameRunStage();
        public SoundMgr m_soundMgr = new SoundMgr();
        public MapCfg m_mapCfg = new MapCfg();

        public IAutoUpdate m_autoUpdate;
        public AtlasMgr m_atlasMgr = new AtlasMgr();
        public AuxUIHelp m_auxUIHelp = new AuxUIHelp();
        public WidgetStyleMgr m_widgetStyleMgr = new WidgetStyleMgr();
        public SceneCardMgr m_sceneCardMgr;
        public SceneEffectMgr m_sceneEffectMgr;
        public SystemFrameData m_systemFrameData = new SystemFrameData();
        public SystemTimeData m_systemTimeData = new SystemTimeData();
        public ScriptDynLoad m_scriptDynLoad = new ScriptDynLoad();

        public SkillActionMgr m_skillActionMgr = new SkillActionMgr();
        public ScenePlaceHolder m_scenePlaceHolder = new ScenePlaceHolder();
        public SkillAttackFlowMgr m_skillAttackFlowMgr = new SkillAttackFlowMgr();
        public Maze m_maze = new Maze();
        public GlobalEventMgr m_globalEventMgr = new GlobalEventMgr();

        public LuaScriptMgr m_luaScriptMgr = new LuaScriptMgr();
        public MovieMgr m_movieMgr = new MovieMgr();

        public Ctx()
        {
            m_luaScriptMgr.Start();
        }

        public static Ctx instance()
        {
            if (m_instance == null)
            {
                m_instance = new Ctx();
            }
            return m_instance;
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