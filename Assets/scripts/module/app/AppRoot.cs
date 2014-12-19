using UnityEngine;
using System.Collections;
using SDK.Common;
using Game.App;

/**
 * @brief 这个模块主要是代码，启动必要的核心代码都在这里，可能某些依赖模块延迟加载
 */
public class AppRoot : MonoBehaviour 
{
    private AppSys m_AppSys;

    void Awake()
    {
        m_AppSys = new AppSys();
        m_AppSys.Awake(transform);
    }
	// Use this for initialization
	void Start () 
    {
        //DontDestroyOnLoad(transform.gameObject);    //设置该对象在加载其他level时不销毁
        m_AppSys.setNoDestroyObject();
        m_AppSys.Start();
        //m_AppSys.loadModule(ModuleName.GAMEMN);
        Ctx.m_instance.m_moduleSys.loadModule(ModuleName.LOGINMN);
	}
	
	// Update is called once per frame
	void Update () 
    {
        //BugResolve();
        m_AppSys.Update();
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

    public Ctx getCtx()
    {
        return Ctx.m_instance;
    }
}