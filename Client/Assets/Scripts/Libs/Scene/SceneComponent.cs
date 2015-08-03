using SDK.Common;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 模拟 MonoBehaviour 中的 GameObject，直接放在场景中的资源的基类，就是保存一个场景中的 GameObject ，没有任何功能
     */
    public class AuxSceneComponent
    {
        protected GameObject gameObject;            // 模拟 MonoBehaviour 中的行为
        public Transform transform;              // 模拟 MonoBehaviour 中的行为
        public Animation animation;
        public string name;                      // 名字

        public virtual void setGameObject(GameObject go)
        {
            gameObject = go;
            transform = gameObject.transform;
#if UNITY_5
            animation = gameObject.GetComponent<Animation>();
#elif UNITY_4_6 || UNITY_4_5
        animation = gameObject.animation;
#endif
            name = gameObject.name;

            Awake();
            Start();
        }

        public virtual void Awake()
        {

        }

        public virtual void Start()
        {

        }

        public GameObject getGameObject()
        {
            return gameObject;
        }

        virtual public void dispose()
        {

        }
    }
}