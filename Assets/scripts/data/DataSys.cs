using UnityEngine;
using System.Collections;

namespace San.Guo
{
    /**
     * @brief 数据系统
     */
    public class DataSys
    {
        public void Awake(Transform transform)
        {
            Ctx.m_instance = new Ctx();
            Ctx.m_instance.Awake();
            Ctx.m_instance.m_netMgr = new NetworkMgr();
            Ctx.m_instance.m_cfg = new Config();
            Ctx.m_instance.m_log = new Logger();
            Ctx.m_instance.m_resMgr = new ResMgr();
            Ctx.m_instance.m_dataTrans = transform;
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
    }
}
