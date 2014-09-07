using UnityEngine;
using System.Collections;
using SDK.Common;
using Game.App;

/**
 * @brief 这个模块主要是代码，核心代码都在这里，以后代码都放在这个里面
 */
public class AppRoot : MonoBehaviour 
{
    public AppSys m_AppSys;

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
        m_AppSys.loadScene();
	}
	
	// Update is called once per frame
	void Update () 
    {
        GameObject[] nodestroy = GameObject.FindGameObjectsWithTag("GoNoDestroy");  //得到存在的实例列表
        if (nodestroy.Length > 1)
        {
            // 将后产生的毁掉 保留第一个
            GameObject.Destroy(nodestroy[1]);
        }

        m_AppSys.Update();
	}

    public Ctx getCtx()
    {
        return Ctx.m_instance;
    }
}