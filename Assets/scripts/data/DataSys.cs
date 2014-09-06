using UnityEngine;
using System.Collections;
using San.Guo;
/**
 * @brief 数据系统
 */
public class DataSys : MonoBehaviour 
{
    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);    //设置该对象在加载其他level时不销毁

        Ctx.m_instance = new Ctx();
        Ctx.m_instance.Awake();
        Ctx.m_instance.m_netMgr = new NetworkMgr();
        Ctx.m_instance.m_cfg = new Config();
        Ctx.m_instance.m_log = new Logger();
        Ctx.m_instance.m_resMgr = new ResMgr();
        Ctx.m_instance.m_dataTrans = transform;
    }

	// Use this for initialization
	void Start () 
    {
        Ctx.m_instance.Start();
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

        Ctx.m_instance.Update();
	}

    void OnApplicationQuit()
    {
        
    }
}
