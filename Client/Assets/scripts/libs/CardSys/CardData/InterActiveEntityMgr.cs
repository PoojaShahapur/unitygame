using SDK.Common;
using System.Collections.Generic;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 可交互对象管理器
     */
    public class InterActiveEntityMgr : IInterActiveEntityMgr
    {
        public Dictionary<GameObject, LSBehaviour> m_go2EntityDic = new Dictionary<GameObject, LSBehaviour>();

        public void addSceneEntity(ISceneEntity entity)
        {
            m_go2EntityDic[entity.getGameObject()] = entity as LSBehaviour;
        }

        public void OnMouseUp(GameObject go)
        {
            if(m_go2EntityDic.ContainsKey(go))
            {
                m_go2EntityDic[go].OnMouseUp();
            }
        }
    }
}