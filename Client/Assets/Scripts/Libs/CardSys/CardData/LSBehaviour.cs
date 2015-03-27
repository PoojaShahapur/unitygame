using SDK.Common;
using UnityEngine;

/**
 * @brief LS 模拟 MonoBehaviour 中的 GameObject，直接放在场景中的资源的基类
 */
public class LSBehaviour
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
#elif UNITY_4_6
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
}