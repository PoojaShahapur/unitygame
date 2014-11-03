using System.Collections;
using UnityEngine;
using SDK.Common;

namespace SDK.Lib
{
    /**
     * @brief Coroutine 入口
     */
    public class CoroutineBehaviour : MonoBehaviour
    {

    }

    public class CoroutineMgr : ICoroutineMgr
    {
        private static CoroutineBehaviour m_CoroutineEntry;

        public Coroutine StartCoroutine(IEnumerator routine)
        {
            if (m_CoroutineEntry == null)
            {
                //m_CoroutineEntry = new GameObject("CoroutineMgr").AddComponent<CoroutineBehaviour>();
                m_CoroutineEntry = Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_App].AddComponent<CoroutineBehaviour>();
            }
            return m_CoroutineEntry.StartCoroutine(routine);
        }
    }
}
