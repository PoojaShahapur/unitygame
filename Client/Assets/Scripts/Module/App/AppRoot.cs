using UnityEngine;
using SDK.Lib;

#if UNIT_TEST
using UnitTest;
#endif

/**
 * @brief 这个模块主要是代码，启动必要的核心代码都在这里，可能某些依赖模块延迟加载
 */
public class AppRoot : MonoBehaviour 
{
    void Awake()
    {
        //Application.targetFrameRate = 24;
    }

	// Use this for initialization
	void Start () 
    {
        Ctx.instance();
        Ctx.m_instance.init();
        init();
	}
	
	// Update is called once per frame
    void Update () 
    {
        //BugResolve();
        //try
        //{
            Ctx.m_instance.m_engineLoop.MainLoop();
        //}
        //catch(Exception err)
        //{
        //    Ctx.m_instance.m_logSys.log("Main Loop Error");
        //}
    }

    void OnApplicationQuit()
    {
        // 等待网络关闭
        Ctx.m_instance.m_netMgr.quipApp();
        // 卸载所有的资源
        Ctx.m_instance.unloadAll();
        // 关闭日志设备
        Ctx.m_instance.m_logSys.closeDevice();
    }

    // unity 自己产生的 bug ，DontDestroyOnLoad 的对象，加载 Level 后会再产生一个
    private void BugResolve()
    {
        GameObject[] nodestroy = GameObject.FindGameObjectsWithTag("App");  //得到存在的实例列表
        if (nodestroy.Length > 1)
        {
            // 将后产生的毁掉，保留第一个
            //GameObject.Destroy(nodestroy[1]);
            UtilApi.Destroy(nodestroy[1]);
        }
    }

    //public Ctx getCtx()
    //{
    //    return Ctx.m_instance;
    //}

    public void init()
    {
        // 构造所有的数据
        constructAll();
        // 设置不释放 GameObject
        setNoDestroyObject();
        // 交叉引用的对象初始化
        PostInit();
        // Unity 编辑器设置的基本数据
        initBasicCfg();

        // 加载模块
        if (MacroDef.PKG_RES_LOAD)
        {
            Ctx.m_instance.m_moduleSys.loadModule(ModuleID.AUTOUPDATEMN);
        }
        else
        {
            //Ctx.m_instance.m_moduleSys.loadModule(ModuleID.LOGINMN);
            //Ctx.m_instance.m_moduleSys.loadModule(ModuleID.GAMEMN);
        }

        // 运行单元测试
#if UNIT_TEST
        UnitTestMain pUnitTestMain = new UnitTestMain();
        pUnitTestMain.run();
#endif
    }

    public void constructAll()
    {
        
    }

    public void PostInit()
    {
        Ctx.m_instance.postInit();
    }

    public void setNoDestroyObject()
    {
        Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Root] = UtilApi.GoFindChildByName(NotDestroyPath.ND_CV_Root);
        //UtilApi.DontDestroyOnLoad(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Root]);

        setNoDestroyObject_impl(NotDestroyPath.ND_CV_App, NotDestroyPath.ND_CV_Root);
        setNoDestroyObject_impl(NotDestroyPath.ND_CV_UICanvas_50, NotDestroyPath.ND_CV_Root);
        setNoDestroyObject_impl(NotDestroyPath.ND_CV_UICanvas_100, NotDestroyPath.ND_CV_Root);
        setNoDestroyObject_impl(NotDestroyPath.ND_CV_UICamera, NotDestroyPath.ND_CV_Root);
        setNoDestroyObject_impl(NotDestroyPath.ND_CV_EventSystem, NotDestroyPath.ND_CV_Root);
        // NGUI 2.7.0 之前的版本，编辑器会将 width and height 作为 transform 的 local scale ，因此需要手工重置
        setNoDestroyObject_impl(NotDestroyPath.ND_CV_UIBtmLayer_Canvas_50, NotDestroyPath.ND_CV_Root);
        setNoDestroyObject_impl(NotDestroyPath.ND_CV_UIFirstLayer_Canvas_50, NotDestroyPath.ND_CV_Root);
        setNoDestroyObject_impl(NotDestroyPath.ND_CV_UISecondLayer_Canvas_50, NotDestroyPath.ND_CV_Root);
        setNoDestroyObject_impl(NotDestroyPath.ND_CV_UIThirdLayer_Canvas_50, NotDestroyPath.ND_CV_Root);
        setNoDestroyObject_impl(NotDestroyPath.ND_CV_UIForthLayer_Canvas_50, NotDestroyPath.ND_CV_Root);
        setNoDestroyObject_impl(NotDestroyPath.ND_CV_UITopLayer_Canvas_50, NotDestroyPath.ND_CV_Root);

        setNoDestroyObject_impl(NotDestroyPath.ND_CV_UIBtmLayer_Canvas_100, NotDestroyPath.ND_CV_Root);
        setNoDestroyObject_impl(NotDestroyPath.ND_CV_UIFirstLayer_Canvas_100, NotDestroyPath.ND_CV_Root);
        setNoDestroyObject_impl(NotDestroyPath.ND_CV_UISecondLayer_Canvas_100, NotDestroyPath.ND_CV_Root);
        setNoDestroyObject_impl(NotDestroyPath.ND_CV_UIThirdLayer_Canvas_100, NotDestroyPath.ND_CV_Root);
        setNoDestroyObject_impl(NotDestroyPath.ND_CV_UIForthLayer_Canvas_100, NotDestroyPath.ND_CV_Root);
        setNoDestroyObject_impl(NotDestroyPath.ND_CV_UITopLayer_Canvas_100, NotDestroyPath.ND_CV_Root);

        setNoDestroyObject_impl(NotDestroyPath.ND_CV_UIModel, NotDestroyPath.ND_CV_Root);
        setNoDestroyObject_impl(NotDestroyPath.ND_CV_UILight, NotDestroyPath.ND_CV_Root);
    }

    protected void setNoDestroyObject_impl(string child, string parent)
    {
        Ctx.m_instance.m_layerMgr.m_path2Go[child] = UtilApi.TransFindChildByPObjAndPath(Ctx.m_instance.m_layerMgr.m_path2Go[parent], child);
        UtilApi.DontDestroyOnLoad(Ctx.m_instance.m_layerMgr.m_path2Go[child]);
    }

    protected void initBasicCfg()
    {
        BasicConfig basicCfg = Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Root].GetComponent<BasicConfig>();
        //Ctx.m_instance.m_cfg.m_ip = basicCfg.getIp();
        Ctx.m_instance.m_cfg.m_zone = basicCfg.getPort();
    }
}