using UnityEngine;

/**
 * @brief LS 模拟 MonoBehaviour 中的 GameObject
 */
public class LSBehaviour
{
    protected GameObject gameObject;           // 模拟 MonoBehaviour 中的行为
    protected Transform transform;             // 模拟 MonoBehaviour 中的行为
    protected Animation animation;

    public void setGameObject(GameObject go)
    {
        gameObject = go;
        transform = gameObject.transform;
        animation = gameObject.animation;
    }
}