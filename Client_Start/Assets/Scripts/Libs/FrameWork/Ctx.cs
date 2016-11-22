namespace SDK.Lib
{
    /**
     * @brief 全局数据区
     */
    public class Ctx
    {
        static public Ctx mInstance;

        public NetworkMgr mNetMgr;                // 网络通信
        public Config mCfg;                       // 整体配置文件
        public LogSys mLogSys;                    // 日志系统
        public ResLoadMgr mResLoadMgr;            // 资源管理器
        public InputMgr mInputMgr;                // 输入管理器

        public IGameSys mGameSys;                 // 游戏系统
        public SceneSys mSceneSys;                // 场景系统
        public TickMgr mTickMgr;                  // 心跳管理器
        public ProcessSys mProcessSys;            // 游戏处理系统

        public TimerMgr mTimerMgr;                // 定时器系统
        public FrameTimerMgr mFrameTimerMgr;      // 定时器系统
        public UIMgr mUiMgr;                      // UI 管理器
        public ResizeMgr mResizeMgr;              // 窗口大小修改管理器
        public IUIEvent mCbUIEvent;               // UI 事件回调
        public CoroutineMgr mCoroutineMgr;        // 协程管理器

        public EngineLoop mEngineLoop;            // 引擎循环
        public GameAttr mGameAttr;                // 游戏属性
        public FObjectMgr mFObjectMgr;            // 掉落物管理器
        public NpcMgr mNpcMgr;                    // Npc管理器
        public PlayerMgr mPlayerMgr;              // Player管理器
        public MonsterMgr mMonsterMgr;            // Monster 管理器
        public SpriteAniMgr mSpriteAniMgr;

        public ShareData mShareData;               // 共享数据系统
        public LayerMgr mLayerMgr;                 // 层管理器
        public ISceneEventCB mSceneEventCB;        // 场景加载事件
        public CamSys mCamSys;

        public ISceneLogic mSceneLogic;
        public SysMsgRoute mSysMsgRoute;           // 消息分发
        public NetCmdNotify mNetCmdNotify;         // 网络处理器
        public MsgRouteNotify mMsgRouteNotify;     // RouteMsg 客户端自己消息流程
        public IModuleSys mModuleSys;              // 模块
        public TableSys mTableSys;                 // 表格
        public MFileSys mFileSys;                  // 文件系统
        public FactoryBuild mFactoryBuild;         // 生成各种内容，上层只用接口

        public LangMgr mLangMgr;                   // 语言管理器
        public DataPlayer mDataPlayer;
        public XmlCfgMgr mXmlCfgMgr;
        public MaterialMgr mMatMgr;
        public ModelMgr mModelMgr;
        public TextureMgr mTexMgr;
        public SkelAniMgr mSkelAniMgr;
        public SkinResMgr mSkinResMgr;
        public PrefabMgr mPrefabMgr;
        public ControllerMgr mControllerMgr;
        public BytesResMgr mBytesResMgr;
        public SpriteMgr mSpriteMgr;

        public SystemSetting mSystemSetting;
        public CoordConv mCoordConv;
        public FlyNumMgr mFlyNumMgr;              // Header Num

        public TimerMsgHandle mTimerMsgHandle;
        //public WebSocketMgr m_pWebSocketMgr;
        public PoolSys mPoolSys;
        public ILoginSys mLoginSys;
        public WordFilterManager mWordFilterManager;
        public VersionSys mVersionSys;
        public AutoUpdateSys mAutoUpdateSys;

        public TaskQueue mTaskQueue;
        public TaskThreadPool mTaskThreadPool;

        public RandName mRandName;
        public PakSys mPakSys;
        public GameRunStage mGameRunStage;
        public SoundMgr mSoundMgr;
        public MapCfg mMapCfg;

        public IAutoUpdate mAutoUpdate;
        public AtlasMgr mAtlasMgr;
        public AuxUIHelp mAuxUIHelp;
        public WidgetStyleMgr mWidgetStyleMgr;
        public SceneEffectMgr mSceneEffectMgr;
        public SystemFrameData mSystemFrameData;
        public SystemTimeData mSystemTimeData;
        public ScriptDynLoad mScriptDynLoad;
        public ScenePlaceHolder mScenePlaceHolder;

        public LuaSystem mLuaSystem;
        public MovieMgr mMovieMgr;    // 视频 Clip 播放
        public NativeInterface mNativeInterface;   // 本地接口调用
        public GCAutoCollect mGcAutoCollect;     // 自动垃圾回收
        public MemoryCheck mMemoryCheck;       // 内存查找
        public DepResMgr mDepResMgr;
        public MTerrainGroup mTerrainGroup;
        public TextResMgr mTextResMgr;
        public MSceneManager mSceneManager;
        public TerrainBufferSys mTerrainBufferSys;
        public TerrainGlobalOption mTerrainGlobalOption;
        public CoroutineTaskMgr mCoroutineTaskMgr;
        public SceneNodeGraph mSceneNodeGraph;
        public TerrainEntityMgr mTerrainEntityMgr;

        public ResRedirect mResRedirect;            // 重定向
        public DownloadMgr mDownloadMgr;            // 下载管理器
        public MKBEMainEntry mMKBEMainEntry;        // KBEngine 相关处理

        public Ctx()
        {
            
        }

        public static Ctx instance()
        {
            if (mInstance == null)
            {
                mInstance = new Ctx();
            }
            return mInstance;
        }

        public void editorToolInit()
        {
            MFileSys.init();
            this.mDataPlayer = new DataPlayer();
            this.mLogSys = new LogSys();
        }

        public void dispose()
        {
            mInstance = null;
        }

        protected void preInit()
        {
            MFileSys.init();            // 初始化本地文件系统的一些数据
            PlatformDefine.init();      // 初始化平台相关的定义
            UtilByte.checkEndian();     // 检查系统大端小端
            MThread.getMainThreadID();  // 获取主线程 ID
            ResPathResolve.initRootPath();

            mTerrainGlobalOption = new TerrainGlobalOption();

            this.mNetCmdNotify = new NetCmdNotify();
            this.mMsgRouteNotify = new MsgRouteNotify();

            this.mXmlCfgMgr = new XmlCfgMgr();
            this.mMatMgr = new MaterialMgr();
            this.mModelMgr = new ModelMgr();
            this.mTexMgr = new TextureMgr();
            this.mSkelAniMgr = new SkelAniMgr();
            this.mSkinResMgr = new SkinResMgr();
            this.mPrefabMgr = new PrefabMgr();
            this.mControllerMgr = new ControllerMgr();
            this.mBytesResMgr = new BytesResMgr();
            this.mSpriteMgr = new SpriteMgr();

            this.mSystemSetting = new SystemSetting();
            this.mCoordConv = new CoordConv();
            this.mFlyNumMgr = new FlyNumMgr();              // Header Num

            this.mTimerMsgHandle = new TimerMsgHandle();
            this.mPoolSys = new PoolSys();
            this.mWordFilterManager = new WordFilterManager();
            this.mVersionSys = new VersionSys();
            this.mAutoUpdateSys = new AutoUpdateSys();

            this.mTaskQueue = new TaskQueue("TaskQueue");
            this.mTaskThreadPool = new TaskThreadPool();

            this.mRandName = new RandName();
            this.mPakSys = new PakSys();
            this.mGameRunStage = new GameRunStage();
            this.mSoundMgr = new SoundMgr();
            this.mMapCfg = new MapCfg();

            this.mAtlasMgr = new AtlasMgr();
            this.mAuxUIHelp = new AuxUIHelp();
            this.mWidgetStyleMgr = new WidgetStyleMgr();
            this.mSystemFrameData = new SystemFrameData();
            this.mSystemTimeData = new SystemTimeData();
            this.mScriptDynLoad = new ScriptDynLoad();
            this.mScenePlaceHolder = new ScenePlaceHolder();

            this.mLuaSystem = new LuaSystem();
            this.mMovieMgr = new MovieMgr();
            this.mNativeInterface = new NativeInterface();
            this.mGcAutoCollect = new GCAutoCollect();
            this.mMemoryCheck = new MemoryCheck();
            this.mDepResMgr = new DepResMgr();
            this.mTerrainGroup = new MTerrainGroup(mTerrainGlobalOption.mTerrainSize, mTerrainGlobalOption.mTerrainWorldSize);
            this.mTextResMgr = new TextResMgr();
            this.mTerrainBufferSys = new TerrainBufferSys();
            //this.mTerrainGroup = new MTerrainGroup(513, 512);

            this.mCfg = new Config();
            this.mDataPlayer = new DataPlayer();
            this.mFactoryBuild = new FactoryBuild();

            this.mNetMgr = new NetworkMgr();
            this.mResLoadMgr = new ResLoadMgr();
            this.mInputMgr = new InputMgr();

            this.mProcessSys = new ProcessSys();
            this.mTickMgr = new TickMgr();
            this.mTimerMgr = new TimerMgr();
            this.mFrameTimerMgr = new FrameTimerMgr();
            this.mCoroutineMgr = new CoroutineMgr();
            this.mShareData = new ShareData();
            this.mSceneSys = new SceneSys();
            this.mLayerMgr = new LayerMgr();

            this.mUiMgr = new UIMgr();
            this.mEngineLoop = new EngineLoop();
            this.mResizeMgr = new ResizeMgr();

            this.mPlayerMgr = new PlayerMgr();
            this.mMonsterMgr = new MonsterMgr();
            this.mFObjectMgr = new FObjectMgr();
            this.mNpcMgr = new NpcMgr();
            this.mSpriteAniMgr = new SpriteAniMgr();

            this.mCamSys = new CamSys();
            this.mSysMsgRoute = new SysMsgRoute("SysMsgRoute");
            this.mModuleSys = new ModuleSys();
            this.mTableSys = new TableSys();
            this.mFileSys = new MFileSys();
            this.mLogSys = new LogSys();
            this.mLangMgr = new LangMgr();
            this.mSceneEffectMgr = new SceneEffectMgr();

            this.mSceneManager = new MOctreeSceneManager("DummyScene");
            mCoroutineTaskMgr = new CoroutineTaskMgr();
            mSceneNodeGraph = new SceneNodeGraph();
            mTerrainEntityMgr = new TerrainEntityMgr();

            mResRedirect = new ResRedirect();
            mDownloadMgr = new DownloadMgr();

            mMKBEMainEntry = new MKBEMainEntry();
        }

        protected void interInit()
        {

        }

        public void postInit()
        {
            this.mResizeMgr.addResizeObject(this.mUiMgr as IResizeObject);
            this.mTickMgr.addTick(this.mInputMgr as ITickedObject);
            this.mInputMgr.postInit();
            //mTickMgr.addTick(mPlayerMgr as ITickedObject);
            //mTickMgr.addTick(mMonsterMgr as ITickedObject);
            //mTickMgr.addTick(m_fObjectMgr as ITickedObject);
            //mTickMgr.addTick(mNpcMgr as ITickedObject);
            this.mTickMgr.addTick(this.mSpriteAniMgr as ITickedObject);
            this.mTickMgr.addTick(this.mSceneEffectMgr as ITickedObject);
            //mTickMgr.addTick(m_sceneCardMgr as ITickedObject);
            //mTickMgr.addTick(m_aiSystem.aiControllerMgr as ITickedObject);

            this.mUiMgr.findCanvasGO();
            this.mDataPlayer.m_dataPack.postConstruct();

            // 初始化重定向
            this.mResRedirect.postInit();
            this.mResLoadMgr.postInit();
            this.mDownloadMgr.postInit();

            this.mTaskQueue.m_pTaskThreadPool = this.mTaskThreadPool;
            this.mTaskThreadPool.initThreadPool(2, this.mTaskQueue);

            this.mVersionSys.loadLocalVer();    // 加载版本文件
            this.mDepResMgr.init();             // 加载依赖文件
            this.mCoroutineTaskMgr.start();

            this.mLuaSystem.init();
            this.mUiMgr.init();

            mMKBEMainEntry.Start();
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
            this.mLayerMgr.m_path2Go[NotDestroyPath.ND_CV_Root] = UtilApi.GoFindChildByName(NotDestroyPath.ND_CV_Root);
            UtilApi.DontDestroyOnLoad(Ctx.mInstance.mLayerMgr.m_path2Go[NotDestroyPath.ND_CV_Root]);

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
            this.mLayerMgr.m_path2Go[child] = UtilApi.TransFindChildByPObjAndPath(this.mLayerMgr.m_path2Go[parent], child);
            //UtilApi.DontDestroyOnLoad(mLayerMgr.m_path2Go[child]);
        }

        protected void initBasicCfg()
        {
            BasicConfig basicCfg = this.mLayerMgr.m_path2Go[NotDestroyPath.ND_CV_Root].GetComponent<BasicConfig>();
            //mCfg.m_ip = basicCfg.getIp();
            this.mCfg.mZone = basicCfg.getPort();
        }

        // 卸载所有的资源
        public void unloadAll()
        {
            // 卸载所有的模型
            this.mModelMgr.unloadAll();
            // 卸载所有的材质
            this.mMatMgr.unloadAll();
            // 卸载所有的纹理
            this.mTexMgr.unloadAll();
            // 卸载音乐
            this.mSoundMgr.unloadAll();
            // 场景卸载
            this.mSceneSys.unloadAll();
        }
    }
}