using UnityEngine;
using System;
using System.Collections.Generic;

namespace SDK.Common
{
    public class Ctx
    {
        static public Ctx m_instance;

        public INetworkMgr m_netMgr;                // 网络通信
        public Config m_cfg;                        // 整体配置文件
        public ILogger m_log;                       // 日志系统
        public IResMgr m_resMgr;                    // 资源管理器
        public IInputMgr m_inputMgr;                // 输入管理器
        public Transform m_dataTrans;               // 整个系统使用的 GameObject

        public IGameSys m_gameSys;                  // 游戏系统
        public ISceneSys m_sceneSys;                // 场景系统
        public ITickMgr m_tickMgr;                  // 心跳管理器
        public IProcessSys m_processSys;            // 游戏处理系统

        public ITimerMgr m_timerMgr;                // 定时器系统
        public IUIMgr m_uiMgr;                      // UI 管理器
        public IResizeMgr m_resizeMgr;              // 窗口大小修改管理器
        public IUIEvent m_cbUIEvent;                // UI 事件回调
        public ICoroutineMgr m_coroutineMgr;        // 协程管理器

        public IEngineLoop m_engineLoop;            // 引擎循环
        public GameAttr m_gameAttr;                 // 游戏属性
        public IFObjectMgr m_fObjectMgr;            // 掉落物管理器
        public INpcMgr m_npcMgr;                    // Npc管理器
        public IPlayerMgr m_playerMgr;              // Player管理器
        public IMonsterMgr m_monsterMgr;
        public ITerrainMgr m_terrainMgr;            // Terrain管理器

        public ShareMgr m_shareMgr;                 // 共享数据系统
        public LayerMgr m_layerMgr;                 // 层管理器
        public ISceneEventCB m_sceneEventCB;        // 场景加载事件
        public CamSys m_camSys;

        public ISceneLogic m_sceneLogic;
        public IMeshMgr m_meshMgr;
        public IAISystem m_aiSystem;


        public Ctx()
        {

        }

        //static public Ctx instance
        //{
        //    get
        //    {
        //        if(m_instance == null)
        //        {
        //            m_instance = new Ctx();
        //        }

        //        return m_instance;
        //    }
        //}

        public void Awake()
        {
            
        }

        // Use this for initialization
        public void Start()
        {
            //m_netMgr.openSocket(m_cfg.m_ip, m_cfg.m_port);
        }

        // Update is called once per frame
        //public void Update() 
        //{
            // 处理网络
            //ByteArray ret = m_netMgr.getMsg();

            // 处理 input
            //m_inputMgr.handleKeyBoard();
	    //}

        public void OnApplicationQuit()
        {

        }
    }
}