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
        public FixedTickMgr mFixedTickMgr;        // 固定间隔心跳管理器
        public LogicTickMgr mLogicTickMgr;        // 逻辑心跳管理器
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

        public ShareData mShareData;              // 共享数据系统
        public LayerMgr mLayerMgr;                // 层管理器
        public ISceneEventCB mSceneEventCB;       // 场景加载事件
        public CamSys mCamSys;

        public ISceneLogic mSceneLogic;
        public SysMsgRoute mSysMsgRoute;          // 消息分发
        public NetCmdNotify mNetCmdNotify;        // 网络处理器
        public MsgRouteNotify mMsgRouteNotify;    // RouteMsg 客户端自己消息流程
        public IModuleSys mModuleSys;             // 模块
        public TableSys mTableSys;                // 表格
        public MFileSys mFileSys;                 // 文件系统
        public FactoryBuild mFactoryBuild;        // 生成各种内容，上层只用接口

        public LangMgr mLangMgr;                  // 语言管理器
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
        public clientapp mClientApp;        // KBEngine 相关处理
        public SnowBlockMgr mSnowBlockMgr;
        public ComputerBallMgr mComputerBallMgr;
        public PlayerSnowBlockMgr mPlayerSnowBlockMgr;
        public FlyBulletMgr mFlyBulletMgr;
        public FrameCollideMgr mFrameCollideMgr;
        public SnowBallCfg mSnowBallCfg;
        public CameraPositionMgr mCameraPositonMgr;

        public HudSystem mHudSystem;
        public GlobalDelegate mGlobalDelegate;
        public CommonData mCommonData;
        public EventHandleSystem mEventHandleSystem;
        public DelayTaskMgr mDelayTaskMgr;

        public LoadProgressMgr mLoadProgressMgr;
        public SoundLoadStateCheckMgr mSoundLoadStateCheckMgr;
        public IdPoolSys mIdPoolSys;

        public UniqueStrIdGen mUniqueStrIdGen;
        public DownloadFileMgr mDownloadFileMgr;    // 文件下载模块
        public DownloadAppMgr mDownloadAppMgr;      // App下载模块
        public TDClipRect mClipRect;                // 更新裁剪矩形
        public TDTileMgr mTileMgr;                  // Tile 管理器

        public MTwoDTerrain mTwoDTerrain;           // 2D 地形

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

        protected void constructInit()
        {
            this.mUniqueStrIdGen = new UniqueStrIdGen("FindEvt", 0);

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
            this.mFixedTickMgr = new FixedTickMgr();
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
            this.mCoroutineTaskMgr = new CoroutineTaskMgr();
            this.mSceneNodeGraph = new SceneNodeGraph();
            this.mTerrainEntityMgr = new TerrainEntityMgr();

            this.mResRedirect = new ResRedirect();
            this.mDownloadMgr = new DownloadMgr();

            this.mSnowBlockMgr = new SnowBlockMgr();
            this.mComputerBallMgr = new ComputerBallMgr();
            this.mFrameCollideMgr = new FrameCollideMgr();
            this.mPlayerSnowBlockMgr = new PlayerSnowBlockMgr();
            this.mFlyBulletMgr = new FlyBulletMgr();
            this.mSnowBallCfg = new SnowBallCfg();
            this.mCameraPositonMgr = new CameraPositionMgr();

            this.mHudSystem = new HudSystem();
            this.mGlobalDelegate = new GlobalDelegate();
            this.mCommonData = new CommonData();
            this.mEventHandleSystem = new EventHandleSystem();
            this.mDelayTaskMgr = new DelayTaskMgr();

            this.mLoadProgressMgr = new LoadProgressMgr();
            this.mSoundLoadStateCheckMgr = new SoundLoadStateCheckMgr();
            this.mIdPoolSys = new IdPoolSys();

            this.mLogicTickMgr = new LogicTickMgr();
            this.mDownloadFileMgr = new DownloadFileMgr();
            this.mDownloadAppMgr = new DownloadAppMgr();
            this.mClipRect = new TDClipRect();

            this.mTileMgr = new TDTileMgr();
            this.mTwoDTerrain = new MTwoDTerrain();
        }

        public void logicInit()
        {
            this.mLogSys.init();
            this.mInputMgr.init();
            this.mDataPlayer.mDataPack.postConstruct();

            this.mTickMgr.init();
            this.mFixedTickMgr.init();

            // 初始化重定向
            this.mResRedirect.init();
            this.mResLoadMgr.init();
            this.mDownloadMgr.init();

            this.mTaskQueue.mTaskThreadPool = this.mTaskThreadPool;
            this.mTaskThreadPool.initThreadPool(2, this.mTaskQueue);

            this.mVersionSys.loadLocalVer();    // 加载版本文件
            this.mDepResMgr.init();             // 加载依赖文件
            this.mCoroutineTaskMgr.init();

            this.mLuaSystem.init();
            this.mUiMgr.init();

            this.mSnowBlockMgr.init();
            this.mComputerBallMgr.init();
            this.mFrameCollideMgr.init();
            this.mSceneSys.init();
            //this.mSnowBallCfg.init();
            this.mPlayerSnowBlockMgr.init();
            this.mFlyBulletMgr.init();
            this.mHudSystem.init();
            this.mPlayerMgr.init();
            this.mGlobalDelegate.init();
            this.mCommonData.init();
            this.mEventHandleSystem.init();
            this.mResizeMgr.init();
            this.mCameraPositonMgr.init();
            this.mDelayTaskMgr.init();
            this.mLoadProgressMgr.init();

            this.mSoundLoadStateCheckMgr.init();
            this.mIdPoolSys.init();
            this.mLogicTickMgr.init();
            this.mDownloadFileMgr.init();
            this.mDownloadAppMgr.init();

            this.mClipRect.init();
            this.mTileMgr.init();
            this.mTwoDTerrain.init();

            // 添加事件处理
            Ctx.mInstance.mCamSys.setUiCamera(Ctx.mInstance.mLayerMgr.mPath2Go[NotDestroyPath.ND_CV_App].AddComponent<UICamera>());
            Ctx.mInstance.mCamSys.setSceneCamera2UICamera();

            this.addEventHandle();
        }

        public void init()
        {
            // 构造初始化
            constructInit();
            // 设置不释放 GameObject
            setNoDestroyObject();
            // 逻辑初始化，交叉引用的对象初始化
            logicInit();
            // Unity 编辑器设置的基本数据
            initBasicCfg();
            // 添加 KBEngine组件
            this.mClientApp = this.mLayerMgr.mPath2Go[NotDestroyPath.ND_CV_Root].AddComponent<clientapp>();
        }

        public void dispose()
        {
            if (null != this.mResizeMgr)
            {
                this.mResizeMgr.dispose();
                this.mResizeMgr = null;
            }
            if (null != this.mTickMgr)
            {
                this.mTickMgr.dispose();
                this.mTickMgr = null;
            }
            if (null != this.mFixedTickMgr)
            {
                this.mFixedTickMgr.dispose();
                this.mFixedTickMgr = null;
            }
            if (null != this.mPlayerMgr)
            {
                this.mPlayerMgr.dispose();
                this.mPlayerMgr = null;
            }
            if (null != this.mSnowBlockMgr)
            {
                this.mSnowBlockMgr.dispose();
                this.mSnowBlockMgr = null;
            }
            if (null != this.mComputerBallMgr)
            {
                this.mComputerBallMgr.dispose();
                this.mComputerBallMgr = null;
            }
            if (null != this.mPlayerSnowBlockMgr)
            {
                this.mPlayerSnowBlockMgr.dispose();
                this.mPlayerSnowBlockMgr = null;
            }
            if(null != this.mFlyBulletMgr)
            {
                this.mFlyBulletMgr.dispose();
                this.mFlyBulletMgr = null;
            }
            if (null != this.mInputMgr)
            {
                this.mInputMgr.dispose();
                this.mInputMgr = null;
            }
            if (null != this.mUiMgr)
            {
                this.mUiMgr.dispose();
                this.mUiMgr = null;
            }
            if (null != this.mCameraPositonMgr)
            {
                this.mCameraPositonMgr.dispose();
                this.mCameraPositonMgr = null;
            }

            // 等待网络关闭
            if (null != this.mNetMgr)
            {
                this.mNetMgr.dispose();
                this.mNetMgr = null;
            }
            // 卸载所有的模型
            if (null != this.mModelMgr)
            {
                this.mModelMgr.dispose();
                this.mModelMgr = null;
            }
            // 卸载所有的材质
            if (null != this.mMatMgr)
            {
                this.mMatMgr.dispose();
                this.mMatMgr = null;
            }
            // 卸载所有的纹理
            if (null != this.mTexMgr)
            {
                this.mTexMgr.dispose();
                this.mTexMgr = null;
            }
            // 卸载音乐
            if (null != this.mSoundMgr)
            {
                this.mSoundMgr.dispose();
                this.mSoundMgr = null;
            }
            // 场景卸载
            if (null != this.mSceneSys)
            {
                this.mSceneSys.dispose();
                this.mSceneSys = null;
            }
            // 关闭日志设备
            if (null != this.mLogSys)
            {
                this.mLogSys.dispose();
                this.mLogSys = null;
            }
            if (null != this.mCommonData)
            {
                this.mCommonData.dispose();
                this.mCommonData = null;
            }
            if (null != this.mEventHandleSystem)
            {
                this.mEventHandleSystem.dispose();
                this.mEventHandleSystem = null;
            }
            if (null != this.mDelayTaskMgr)
            {
                this.mDelayTaskMgr.dispose();
                this.mDelayTaskMgr = null;
            }
            if(null != this.mLoadProgressMgr)
            {
                this.mLoadProgressMgr.dispose();
                this.mLoadProgressMgr = null;
            }
            if(null != this.mSoundLoadStateCheckMgr)
            {
                this.mSoundLoadStateCheckMgr.dispose();
                this.mSoundLoadStateCheckMgr = null;
            }
            if(null != this.mIdPoolSys)
            {
                this.mIdPoolSys.dispose();
                this.mIdPoolSys = null;
            }
            if(null != this.mLogicTickMgr)
            {
                this.mLogicTickMgr.dispose();
                this.mLogicTickMgr = null;
            }
            if(null != this.mDownloadFileMgr)
            {
                this.mDownloadFileMgr.dispose();
                this.mDownloadFileMgr = null;
            }
            if (null != this.mDownloadAppMgr)
            {
                this.mDownloadAppMgr.dispose();
                this.mDownloadAppMgr = null;
            }
            if(null != this.mClipRect)
            {
                this.mClipRect.dispose();
                this.mClipRect = null;
            }
            if(null != this.mTileMgr)
            {
                this.mTileMgr.dispose();
                this.mTileMgr = null;
            }
            if(null != this.mTwoDTerrain)
            {
                this.mTwoDTerrain.dispose();
                this.mTwoDTerrain = null;
            }
        }

        public void quitApp()
        {
            this.dispose();

            // 释放自己
            //mInstance = null;
        }

        // KBEngine 引擎流程退出
        public void onKBEQuit()
        {
            // 释放自己
            mInstance = null;
        }

        protected void addEventHandle()
        {
            this.mResizeMgr.addResizeObject(this.mUiMgr as IResizeObject);

            this.mTickMgr.addTick(this.mResizeMgr as ITickedObject, TickPriority.eTPResizeMgr);
            this.mTickMgr.addTick(this.mInputMgr as ITickedObject, TickPriority.eTPInputMgr);
            this.mTickMgr.addTick(this.mLoadProgressMgr as ITickedObject, TickPriority.eTPLoadProgressMgr);
            this.mTickMgr.addTick(this.mCameraPositonMgr as ITickedObject, TickPriority.eTPCameraMgr);
            this.mTickMgr.addTick(this.mDelayTaskMgr as ITickedObject, TickPriority.eTPDelayTaskMgr);
            this.mTickMgr.addTick(this.mSoundLoadStateCheckMgr as ITickedObject, TickPriority.eTPSoundLoadStateCheckMgr);

            this.mTickMgr.addTick(this.mComputerBallMgr as ITickedObject, TickPriority.eTPComputerBallMgr);
            // 静止的雪块没有必要更新
            //this.mTickMgr.addTick(this.mSnowBlockMgr as ITickedObject, TickPriority.eTPSnowBlockMgr);
            this.mTickMgr.addTick(this.mPlayerSnowBlockMgr as ITickedObject, TickPriority.eTPPlayerSnowBlockMgr);
            this.mTickMgr.addTick(this.mPlayerMgr as ITickedObject, TickPriority.eTPPlayerMgr);
            this.mTickMgr.addTick(this.mFlyBulletMgr as ITickedObject, TickPriority.eTPFlyBulletMgr);

            //if (!Ctx.mInstance.mCfg.mIsActorMoveUseFixUpdate)
            //{
            //    this.mTickMgr.addTick(this.mPlayerMgr as ITickedObject, TickPriority.eTPPlayerMgr);
            //}
            //else
            //{
            //    this.mFixedTickMgr.addTick(this.mPlayerMgr as ITickedObject, TickPriority.eTPPlayerMgr);
            //}
        }

        //public void fixUpdateActor()
        //{
        //    if (Ctx.mInstance.mCfg.mIsActorMoveUseFixUpdate)
        //    {
        //        this.mPlayerMgr.onTick(Ctx.mInstance.mSystemTimeData.getFixedTimestep());
        //        this.mSnowBlockMgr.onTick(Ctx.mInstance.mSystemTimeData.getFixedTimestep());
        //        this.mPlayerSnowBlockMgr.onTick(Ctx.mInstance.mSystemTimeData.getFixedTimestep());
        //        this.mComputerBallMgr.onTick(Ctx.mInstance.mSystemTimeData.getFixedTimestep());
        //        this.mCameraPositonMgr.onTick(Ctx.mInstance.mSystemTimeData.getFixedTimestep());
        //    }
        //}

        public void setNoDestroyObject()
        {
            this.mLayerMgr.mPath2Go[NotDestroyPath.ND_CV_Root] = UtilApi.GoFindChildByName(NotDestroyPath.ND_CV_Root);
            UtilApi.DontDestroyOnLoad(Ctx.mInstance.mLayerMgr.mPath2Go[NotDestroyPath.ND_CV_Root]);

            setNoDestroyObject_impl(NotDestroyPath.ND_CV_App, NotDestroyPath.ND_CV_Root);
            setNoDestroyObject_impl(NotDestroyPath.ND_CV_UIFirstCanvas, NotDestroyPath.ND_CV_Root);
            setNoDestroyObject_impl(NotDestroyPath.ND_CV_UISecondCanvas, NotDestroyPath.ND_CV_Root);
            setNoDestroyObject_impl(NotDestroyPath.ND_CV_HudCanvas, NotDestroyPath.ND_CV_Root);
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

            setNoDestroyObject_impl(NotDestroyPath.ND_CV_UIBtmLayer_HudCanvas, NotDestroyPath.ND_CV_Root);
            setNoDestroyObject_impl(NotDestroyPath.ND_CV_UIFirstLayer_HudCanvas, NotDestroyPath.ND_CV_Root);
            setNoDestroyObject_impl(NotDestroyPath.ND_CV_UISecondLayer_HudCanvas, NotDestroyPath.ND_CV_Root);
            setNoDestroyObject_impl(NotDestroyPath.ND_CV_UIThirdLayer_HudCanvas, NotDestroyPath.ND_CV_Root);
            setNoDestroyObject_impl(NotDestroyPath.ND_CV_UIForthLayer_HudCanvas, NotDestroyPath.ND_CV_Root);
            setNoDestroyObject_impl(NotDestroyPath.ND_CV_UITopLayer_HudCanvas, NotDestroyPath.ND_CV_Root);
        }

        protected void setNoDestroyObject_impl(string child, string parent)
        {
            this.mLayerMgr.mPath2Go[child] = UtilApi.TransFindChildByPObjAndPath(this.mLayerMgr.mPath2Go[parent], child);
            //UtilApi.DontDestroyOnLoad(mLayerMgr.mPath2Go[child]);
        }

        protected void initBasicCfg()
        {
            BasicConfig basicCfg = this.mLayerMgr.mPath2Go[NotDestroyPath.ND_CV_Root].GetComponent<BasicConfig>();
            //mCfg.mIp = basicCfg.getIp();
            this.mCfg.mZone = basicCfg.getPort();
        }

        protected System.UInt64 selAvatarDBID = 0;

        public void sendSelRole()
        {
            KBEngine.Event.fireIn("selectAvatarGame", selAvatarDBID);
        }

        public void setSelAvatarDBID(System.UInt64 id)
        {
            selAvatarDBID = id;
        }
    }
}