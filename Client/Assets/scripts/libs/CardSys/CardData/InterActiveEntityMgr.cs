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
        public Dictionary<string, LSBehaviour> m_name2EntityDic = new Dictionary<string, LSBehaviour>();

        public void addSceneEntity(GameObject go, EntityType type)
        {
            LSBehaviour entity = null;
            if (EntityType.eETBtn == type)
            {
                entity = new btn();
            }
            else if (EntityType.eETShop == type)
            {
                entity = new shop();
            }
            entity.setGameObject(go);

            m_name2EntityDic[go.name] = entity as LSBehaviour;
        }

        public ISceneEntity getSceneEntity(string name)
        {
            return m_name2EntityDic[name];
        }

        public void OnMouseUp(GameObject go)
        {
            if (m_name2EntityDic.ContainsKey(go.name))
            {
                m_name2EntityDic[go.name].OnMouseUp();
            }

            //foreach(GameObject key in m_go2EntityDic.Keys)
            //{
            //    if(go == key)
            //    {
            //        m_go2EntityDic[go].OnMouseUp();
            //    }
            //}
        }
    }
}