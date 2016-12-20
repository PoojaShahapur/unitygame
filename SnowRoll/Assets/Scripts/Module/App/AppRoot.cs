﻿using UnityEngine;
using SDK.Lib;

/**
 * @brief 这个模块主要是代码，启动必要的核心代码都在这里，可能某些依赖模块延迟加载
 */
public class AppRoot : MonoBehaviour 
{
    void Awake()
    {
        // Application.targetFrameRate = 24;
        // QualitySettings.vSyncCount = 2;
        // Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    // Use this for initialization
    void Start () 
    {
        init();
	}
	
	// Update is called once per frame
    void Update () 
    {
        //BugResolve();
        //try
        //{
            Ctx.mInstance.mEngineLoop.MainLoop();
        //}
        //catch(Exception err)
        //{
        //}
    }

    void OnApplicationQuit()
    {
        // 等待网络关闭
        Ctx.mInstance.mNetMgr.quipApp();
        // 卸载所有的资源
        Ctx.mInstance.unloadAll();
        // 关闭日志设备
        Ctx.mInstance.mLogSys.closeDevice();
    }

    // unity 自己产生的 bug ，DontDestroyOnLoad 的对象，加载 Level 后会再产生一个
    private void BugResolve()
    {
        GameObject[] nodestroy = GameObject.FindGameObjectsWithTag("AppGo");  //得到存在的实例列表
        if (nodestroy.Length > 1)
        {
            // 将后产生的毁掉，保留第一个
            //GameObject.Destroy(nodestroy[1]);
            UtilApi.Destroy(nodestroy[1]);
        }
    }

    //public Ctx getCtx()
    //{
    //    return Ctx.mInstance;
    //}

    public void init()
    {
        // 初始化全局数据
        Ctx.instance();
        Ctx.mInstance.init();

        // 加载模块
        if (MacroDef.PKG_RES_LOAD)
        {
            Ctx.mInstance.mModuleSys.loadModule(ModuleID.AUTOUPDATEMN);
        }
        else
        {
            Ctx.mInstance.mModuleSys.loadModule(ModuleID.LOGINMN);
            //Ctx.mInstance.mModuleSys.loadModule(ModuleID.GAMEMN);
        }
    }
}