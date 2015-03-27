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
        public UISceneMgr m_uiSceneMgr;                      // UIScene 管理器
        public ResizeMgr m_resizeMgr;              // 窗口大小修改管理器
        public IUIEvent m_cbUIEvent;                // UI 事件回调
        public CoroutineMgr m_coroutineMgr;        // 协程管理器

        public EngineLoop m_engineLoop;            // 引擎循环
        public GameAttr m_gameAttr;                 // 游戏属性
        public FObjectMgr m_fObjectMgr;            // 掉落物管理器
        public NpcMgr m_npcMgr;                    // Npc管理器
        public PlayerMgr m_playerMgr;              // Player管理器
        public MonsterMgr m_monsterMgr;            // Monster 管理器

        public ShareMgr m_shareMgr;                 // 共享数据系统
        public LayerMgr m_layerMgr;                 // 层管理器
        public ISceneEventCB m_sceneEventCB;        // 场景加载事件
        public CamSys m_camSys;

        public ISceneLogic m_sceneLogic;
        public AISystem m_aiSystem;
        public SysMsgRoute m_sysMsgRoute;           // 消息分发
        public NetDispHandle m_netHandle;           // 网络处理器
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

        public void init()
        {
            // 构造所有的数据
            constructAll();
            // 设置不释放 GameObject
            setNoDestroyObject();
            // 交叉引用的对象初始化
            PostInit();
            // 加载登陆模块
            m_moduleSys.loadModule(ModuleID.LOGINMN);

            // 运行单元测试
#if UNIT_TEST_SRC
            UnitTestMain pUnitTestMain = new UnitTestMain();
            pUnitTestMain.run();
#endif
        }

        public void constructAll()
        {
            ByteUtil.checkEndian();     // 检查系统大端小端

            m_cfg = new Config();
            m_factoryBuild = new FactoryBuild();

            m_netMgr = new NetworkMgr();
            m_log = new Logger();
            m_resLoadMgr = new ResLoadMgr();
            m_inputMgr = new InputMgr();

            m_processSys = new ProcessSys();
            m_tickMgr = new TickMgr();
            m_timerMgr = new TimerMgr();
            m_coroutineMgr = new CoroutineMgr();
            m_shareMgr = new ShareMgr();
            m_sceneSys = new SceneSys();
            m_layerMgr = new LayerMgr();

            m_uiMgr = new UIMgr();
            m_uiSceneMgr = new UISceneMgr();
            m_engineLoop = new EngineLoop();
            m_resizeMgr = new ResizeMgr();

            m_playerMgr = new PlayerMgr();
            m_monsterMgr = new MonsterMgr();
            m_fObjectMgr = new FObjectMgr();
            m_npcMgr = new NpcMgr();

            m_camSys = new CamSys();
            m_aiSystem = new AISystem();
            m_sysMsgRoute = new SysMsgRoute();
            m_moduleSys = new ModuleSys();
            m_tableSys = new TableSys();
            m_localFileSys = new LocalFileSys();
            m_langMgr = new LangMgr();
            //m_pWebSocketMgr = new WebSocketMgr();
        }

        public void PostInit()
        {
            m_resizeMgr.addResizeObject(m_uiMgr as IResizeObject);
            //m_tickMgr.AddTickObj(m_inputMgr as ITickedObject);
            m_inputMgr.postInit();
            m_tickMgr.addObject(m_playerMgr as ITickedObject);
            m_tickMgr.addObject(m_monsterMgr as ITickedObject);
            m_tickMgr.addObject(m_fObjectMgr as ITickedObject);
            m_tickMgr.addObject(m_npcMgr as ITickedObject);

            m_uiMgr.getLayerGameObject();

            //m_tableSys.loadOneTable(TableID.TABLE_SKILL);
            //m_tableSys.getItem(TableID.TABLE_SKILL, 2);
            //m_tableSys.getItem(TableID.TABLE_CARD, 11014);
            //m_xmlCfgMgr.loadMarket();
            //m_xmlCfgMgr.getXmlCfg(XmlCfgID.eXmlMarketCfg);
            m_dataPlayer.m_dataPack.postConstruct();
            m_dataPlayer.m_dataCard.registerCardAttr();     // 注册卡牌组属性

            // Test 
            ThreadWrap.getMainThreadID();
        }

        public void setNoDestroyObject()
        {
            m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Root] = UtilApi.GoFindChildByPObjAndName(NotDestroyPath.ND_CV_Root);
            UtilApi.DontDestroyOnLoad(m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Root]);

            m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_App] = UtilApi.TransFindChildByPObjAndPath(m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Root], NotDestroyPath.ND_CV_App);
            UtilApi.DontDestroyOnLoad(m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_App]);

            m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Canvas] = UtilApi.TransFindChildByPObjAndPath(m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Root], NotDestroyPath.ND_CV_Canvas);
            UtilApi.DontDestroyOnLoad(m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Canvas]);

            m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UICamera] = UtilApi.TransFindChildByPObjAndPath(m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Root], NotDestroyPath.ND_CV_UICamera);
            UtilApi.DontDestroyOnLoad(m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UICamera]);

            m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_EventSystem] = UtilApi.TransFindChildByPObjAndPath(m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Root], NotDestroyPath.ND_CV_EventSystem);
            UtilApi.DontDestroyOnLoad(m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_EventSystem]);

            // NGUI 2.7.0 之前的版本，编辑器会将 width and height 作为 transform 的 local scale ，因此需要手工重置
            m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UIBtmLayer] = UtilApi.TransFindChildByPObjAndPath(m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Root], NotDestroyPath.ND_CV_UIBtmLayer);
            UtilApi.DontDestroyOnLoad(m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UIBtmLayer]);

            m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UIFirstLayer] = UtilApi.TransFindChildByPObjAndPath(m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Root], NotDestroyPath.ND_CV_UIFirstLayer);
            UtilApi.DontDestroyOnLoad(m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UIFirstLayer]);

            m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UISecondLayer] = UtilApi.TransFindChildByPObjAndPath(m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Root], NotDestroyPath.ND_CV_UISecondLayer);
            UtilApi.DontDestroyOnLoad(m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UISecondLayer]);

            m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UIThirdLayer] = UtilApi.TransFindChildByPObjAndPath(m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Root], NotDestroyPath.ND_CV_UIThirdLayer);
            UtilApi.DontDestroyOnLoad(m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UIThirdLayer]);

            m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UIForthLayer] = UtilApi.TransFindChildByPObjAndPath(m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Root], NotDestroyPath.ND_CV_UIForthLayer);
            UtilApi.DontDestroyOnLoad(m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UIForthLayer]);

            m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UITopLayer] = UtilApi.TransFindChildByPObjAndPath(m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Root], NotDestroyPath.ND_CV_UITopLayer);
            UtilApi.DontDestroyOnLoad(m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UITopLayer]);
        }
    }
}