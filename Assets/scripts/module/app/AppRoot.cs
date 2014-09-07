using UnityEngine;
using System.Collections;
using San.Guo;

/**
 * @brief 这个模块主要是代码，核心代码都在这里，以后代码都放在这个里面
 */
public class AppRoot : MonoBehaviour 
{
    public DataSys m_DataSys;

    void Awake()
    {
        m_DataSys = new DataSys();
        m_DataSys.Awake(transform);
    }
	// Use this for initialization
	void Start () 
    {
        DontDestroyOnLoad(transform.gameObject);    //设置该对象在加载其他level时不销毁
        m_DataSys.Start();

        // 初始化完成，开始加载自己的游戏场景
        LoadParam param = (Ctx.m_instance.m_resMgr as ResMgr).loadParam;
        param.m_path = "game.unity3d";
        param.m_type = ResType.eLevelType;
        param.m_lvlName = "game";
        Ctx.m_instance.m_resMgr.load(param);
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

        m_DataSys.Update();
	}
}