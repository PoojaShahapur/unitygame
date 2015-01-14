using UnityEngine;
using System;
using System.Collections.Generic;
using SDK.Lib;
using SDK.Common;

namespace SDK.Common
{
    /**
     * @brief 挂载到不卸载的 GameObject，保证 Ctx 永远不被释放，否则过场景后，这里面的内容可能会被清空
     */
    public class Ctx : MonoBehaviour
    {
        static public Ctx m_instance;

        public INetworkMgr m_netMgr;                // 网络通信
        public Config m_cfg;                        // 整体配置文件
        public ILogger m_log;                       // 日志系统
        public IResLoadMgr m_resLoadMgr;                    // 资源管理器
        public IInputMgr m_inputMgr;                // 输入管理器
        public Transform m_dataTrans;               // 整个系统使用的 GameObject

        public IGameSys m_gameSys;                  // 游戏系统
        public ISceneSys m_sceneSys;                // 场景系统
        public ITickMgr m_tickMgr;                  // 心跳管理器
        public IProcessSys m_processSys;            // 游戏处理系统

        public ITimerMgr m_timerMgr;                // 定时器系统
        public IUIMgr m_uiMgr;                      // UI 管理器
        public IUISceneMgr m_uiSceneMgr;                      // UIScene 管理器
        public IResizeMgr m_resizeMgr;              // 窗口大小修改管理器
        public IUIEvent m_cbUIEvent;                // UI 事件回调
        public ICoroutineMgr m_coroutineMgr;        // 协程管理器

        public IEngineLoop m_engineLoop;            // 引擎循环
        public GameAttr m_gameAttr;                 // 游戏属性
        public IFObjectMgr m_fObjectMgr;            // 掉落物管理器
        public INpcMgr m_npcMgr;                    // Npc管理器
        public IPlayerMgr m_playerMgr;              // Player管理器
        public IMonsterMgr m_monsterMgr;            // Monster 管理器

        public ShareMgr m_shareMgr;                 // 共享数据系统
        public LayerMgr m_layerMgr;                 // 层管理器
        public ISceneEventCB m_sceneEventCB;        // 场景加载事件
        public CamSys m_camSys;

        public ISceneLogic m_sceneLogic;
        public IAISystem m_aiSystem;
        public SysMsgRoute m_sysMsgRoute;           // 消息分发
        public NetDispHandle m_netHandle;           // 网络处理器
        public IModuleSys m_moduleSys;              // 模块
        public ITableSys m_tableSys;                // 表格
        public ILocalFileSys m_localFileSys;        // 文件系统
        public IFactoryBuild m_factoryBuild;        // 生成各种内容，上层只用接口

        public ILangMgr m_langMgr;                  // 语言管理器
        //public IInterActiveEntityMgr m_interActiveEntityMgr;
        public DataPlayer m_dataPlayer = new DataPlayer();
        public XmlCfgMgr m_xmlCfgMgr = new XmlCfgMgr();
        public MaterialMgr m_matMgr = new MaterialMgr();
        public ModelMgr m_modelMgr = new ModelMgr();

        public bool m_bStopNetHandle = false;       // 是否停止网络消息处理
        public Action m_loadDZScene;

        public Ctx()
        {

        }

        public static Ctx getCtx(GameObject go)
        {
            if (m_instance == null)
            {
                m_instance = go.AddComponent<Ctx>();
            }
            return m_instance;
        }

        void Awake()
        {
            
        }

        void Start()
        {
            // 构造所有的数据
            constructAll();
            // 设置不释放 GameObject
            setNoDestroyObject();
            // 交叉引用的对象初始化
            PostInit();
            // 加载登陆模块
            m_moduleSys.loadModule(ModuleID.LOGINMN);
        }

        void Update()
        {
            m_engineLoop.MainLoop();
        }

        void OnApplicationQuit()
        {
            m_netMgr.quipApp();
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
            m_dataTrans = transform;

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
        }

        public void PostInit()
        {
            m_resizeMgr.addResizeObject(m_uiMgr as IResizeObject);
            //m_tickMgr.AddTickObj(m_inputMgr as ITickedObject);
            m_inputMgr.postInit();
            m_tickMgr.AddTickObj(m_playerMgr as ITickedObject);
            m_tickMgr.AddTickObj(m_monsterMgr as ITickedObject);
            m_tickMgr.AddTickObj(m_fObjectMgr as ITickedObject);
            m_tickMgr.AddTickObj(m_npcMgr as ITickedObject);

            m_uiMgr.getLayerGameObject();

            //m_tableSys.loadOneTable(TableID.TABLE_OBJECT);
            //m_tableSys.getItem(TableID.TABLE_OBJECT, 712);
            //m_xmlCfgMgr.loadMarket();
            //m_xmlCfgMgr.getXmlCfg(XmlCfgID.eXmlMarketCfg);
            m_dataPlayer.m_dataPack.postConstruct();
            m_dataPlayer.m_dataCard.registerCardAttr();     // 注册卡牌组属性
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