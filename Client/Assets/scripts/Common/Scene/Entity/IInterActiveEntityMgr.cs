using UnityEngine;

namespace SDK.Common
{
    public interface IInterActiveEntityMgr
    {
        void addSceneEntity(GameObject go, EntityType type);
        ISceneEntity getSceneEntity(string name);
        void OnMouseUp(GameObject go);
    }
}