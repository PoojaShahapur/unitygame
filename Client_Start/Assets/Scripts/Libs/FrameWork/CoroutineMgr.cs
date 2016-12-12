using System.Collections;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief Coroutine 入口
     */
    public class CoroutineComponent : MonoBehaviour
    {

    }

    /**
     * @brief 不支持动态停止协程，只有使用 public Coroutine StartCoroutine(string methodName);  string 作为参数的协程，并且启动的协程所在的组件就在当前协程所依赖的 MonoBehaviour 所在的 GameObject 上
     */
    public class CoroutineMgr
    {
        protected CoroutineComponent mCoroutineComponent;

        public Coroutine StartCoroutine(IEnumerator routine)
        {
            if (mCoroutineComponent == null)
            {
                mCoroutineComponent = Ctx.mInstance.mLayerMgr.mPath2Go[NotDestroyPath.ND_CV_App].AddComponent<CoroutineComponent>();
            }
            return mCoroutineComponent.StartCoroutine(routine);
        }
    }
}