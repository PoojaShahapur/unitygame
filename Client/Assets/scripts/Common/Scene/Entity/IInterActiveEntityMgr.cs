using UnityEngine;

namespace SDK.Common
{
    public interface IInterActiveEntityMgr
    {
        void addSceneEntity(ISceneEntity entity);
        void OnMouseUp(GameObject go);
    }
}