using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 基本的渲染器，所有与显示有关的接口都在这里
     */
    public class EntityRenderBase
    {
        virtual public GameObject gameObject()
        {
            return null;
        }

        virtual public void setGameObject(GameObject rhv)
        {

        }

        virtual public Transform transform()
        {
            return null;
        }

        virtual public void onTick(float delta)
        {
            
        }

        virtual public void dispose()
        {

        }
    }
}