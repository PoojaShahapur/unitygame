using SDK.Common;
using UnityEngine;

namespace SDK.Lib
{
    public class ModelRes : InsResBase
    {
        public GameObject m_go;
        public GameObject m_retGO;

        public GameObject InstantiateObject(string resname)
        {
            m_retGO = null;

            if (null == m_go)
            {
                Ctx.m_instance.m_log.log("prefab 为 null");
            }
            else
            {
                m_retGO = GameObject.Instantiate(m_go) as GameObject;
                if (null == m_retGO)
                {
                    Ctx.m_instance.m_log.log("不能实例化数据");
                }
            }
            return m_retGO;
        }

        public GameObject getObject()
        {
            return m_go;
        }
    }
}