using System.Collections;
using UnityEngine;
using SDK.Common;

namespace SDK.Lib
{
    /**
     * @brief Coroutine 入口
     */
    public class CoroutineComponent : MonoBehaviour
    {

    }

    public class CoroutineMgr : ICoroutineMgr
    {
        protected CoroutineComponent m_CoroutineCmnt;

        public Coroutine StartCoroutine(IEnumerator routine)
        {
            if (m_CoroutineCmnt == null)
            {
                m_CoroutineCmnt = Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_App].AddComponent<CoroutineComponent>();
            }
            return m_CoroutineCmnt.StartCoroutine(routine);
        }
    }
}