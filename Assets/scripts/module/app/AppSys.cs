﻿using UnityEngine;
using System.Collections;
using SDK.Common;
using SDK.Lib;

namespace Game.App
{
    /**
     * @brief 数据系统
     */
    public class AppSys : Object
    {
        public void Awake(Transform transform)
        {
            Ctx.m_instance = new Ctx();
            Ctx.m_instance.Awake();
            Ctx.m_instance.m_netMgr = new NetworkMgr();
            Ctx.m_instance.m_cfg = new Config();
            Ctx.m_instance.m_log = new Logger();
            Ctx.m_instance.m_resMgr = new ResMgr();
            Ctx.m_instance.m_inputMgr = new InputMgr();
            Ctx.m_instance.m_dataTrans = transform;

            Ctx.m_instance.m_ProcessSys = new ProcessSys();
            Ctx.m_instance.m_TickMgr = new TickMgr();
            Ctx.m_instance.m_TimerMgr = new TimerMgr();
            Ctx.m_instance.m_CoroutineMgr = new CoroutineMgr();
            Ctx.m_instance.m_shareMgr = new ShareMgr();
            Ctx.m_instance.m_sceneSys = new SceneSys();
        }

        // Use this for initialization
        public void Start()
        {
            Ctx.m_instance.Start();
        }

        // Update is called once per frame
        public void Update()
        {
            Ctx.m_instance.Update();
        }

        public void OnApplicationQuit()
        {

        }

        public void setNoDestroyObject()
        {
            GameObject nodestroy = GameObject.FindGameObjectWithTag("App");
            DontDestroyOnLoad(nodestroy);
            nodestroy = GameObject.FindGameObjectWithTag("RootLayer");
            DontDestroyOnLoad(nodestroy);
            nodestroy = GameObject.FindGameObjectWithTag("SceneLayer");
            DontDestroyOnLoad(nodestroy);

            nodestroy = GameObject.FindGameObjectWithTag("GameLayer");
            DontDestroyOnLoad(nodestroy);
            nodestroy = GameObject.FindGameObjectWithTag("UILayer");
            DontDestroyOnLoad(nodestroy);
            nodestroy = GameObject.FindGameObjectWithTag("UIRoot");
            DontDestroyOnLoad(nodestroy);

            nodestroy = GameObject.FindGameObjectWithTag("Camera");
            DontDestroyOnLoad(nodestroy);
            // NGUI 2.7.0 之前的版本，编辑器会将 width and height 作为 transform 的 local scale ，因此需要手工重置
            nodestroy = GameObject.FindGameObjectWithTag("UIFirstLayer");
            //nodestroy.transform.localScale = Vector3.one;
            DontDestroyOnLoad(nodestroy);
        }

        // 加载游戏模块
        public void loadGame()
        {
            // 初始化完成，开始加载自己的游戏场景
            LoadParam param = (Ctx.m_instance.m_resMgr as ResMgr).loadParam;
            param.m_path = Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathModule] + "Game.unity3d";
            param.m_type = ResPackType.eBundleType;
            param.m_loadedcb = onGameLoaded;
            param.m_resLoadType = Ctx.m_instance.m_cfg.m_resLoadType;
            Ctx.m_instance.m_resMgr.load(param);
        }

        public void onGameLoaded(SDK.Common.Event resEvt)
        {
            IRes res = resEvt.m_param as IRes;                         // 类型转换
            GameObject go = (res as IBundleRes).InstantiateObject("Game");
            GameObject nodestroy = GameObject.FindGameObjectWithTag("GameLayer");
            go.transform.parent = nodestroy.transform;

            // 游戏模块也不释放
            DontDestroyOnLoad(go);
            //go.SetActive(false);         // 自己会更新的，不用这里更新
        }
    }
}