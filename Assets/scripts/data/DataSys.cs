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

        Context.instance.Awake();
    }

	// Use this for initialization
	void Start () 
    {
        Context.instance.m_netMgr = new NetworkMgr();
        Context.instance.m_cfg = new Config();
        Context.instance.m_log = new Logger();
        Context.instance.Start();
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

        Context.instance.Update();
	}

    void OnApplicationQuit()
    {
        
    }
}
