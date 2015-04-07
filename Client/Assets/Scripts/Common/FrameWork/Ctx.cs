﻿using UnityEngine;
using System;
using System.Collections.Generic;
using SDK.Lib;
using SDK.Common;

#if UNIT_TEST_SRC
using UnitTestSrc;
#endif

namespace SDK.Common
{
    /**
     * @brief 全局数据区
     */
    public class Ctx
    {
        static public Ctx m_instance;

        public NetworkMgr m_netMgr;                // 网络通信
        public Config m_cfg;                        // 整体配置文件
        public Logger m_log;                       // 日志系统
        public ResLoadMgr m_resLoadMgr;                    // 资源管理器
        public InputMgr m_inputMgr;                // 输入管理器

        public IGameSys m_gameSys;                  // 游戏系统
        public SceneSys m_sceneSys;                // 场景系统
        public TickMgr m_tickMgr;                  // 心跳管理器
        public ProcessSys m_processSys;            // 游戏处理系统

        public TimerMgr m_timerMgr;                // 定时器系统
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

        public SystemSetting m_systemSetting = new SystemSetting();
        public CoordConv m_coordConv = new CoordConv();

        public bool m_bStopNetHandle = false;       // 是否停止网络消息处理
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

        public Ctx()
        {
            m_TaskQueue.m_pTaskThreadPool = m_TaskThreadPool;
            m_TaskThreadPool.initThreadPool(1, m_TaskQueue);
        }

        public static Ctx instance()
        {
            if (m_instance == null)
            {
                m_instance = new Ctx();
            }
            return m_instance;
        }
    }
}