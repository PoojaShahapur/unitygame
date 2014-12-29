using UnityEngine;

namespace SDK.Common
{
    public interface IInterActiveEntityMgr
    {
        void addSceneEntity(GameObject go, EntityType type, EntityTag tag = 0);
        ISceneEntity getSceneEntity(string name);
        void OnMouseUp(GameObject go);
    }
}