using UnityEngine;

namespace SDK.Common
{
    /**
     * @brief 场景 Entity
     */
    public interface ISceneEntity
    {
        void setGameObject(GameObject go);
        GameObject getGameObject();
    }
}