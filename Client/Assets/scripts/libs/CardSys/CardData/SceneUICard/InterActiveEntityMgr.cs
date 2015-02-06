using Game.UI;
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

        public void addSceneEntity(GameObject go, EntityType type, EntityTag tag = 0)
        {
            InterActiveEntity entity = null;
            if (EntityType.eETBtn == type)
            {
                entity = new btn();
            }
            else if (EntityType.eETShop == type)
            {
                entity = new shop();
            }
            else if (EntityType.eETShopSelectPack == type)
            {
                entity = new shopbtn();
            }
            else if (EntityType.eETShopClose == type)
            {
                entity = new closebuy();
            }
            else if (EntityType.eETOpen == type)
            {
                entity = new open();
            }
            else if (EntityType.eETMcam == type)
            {
                entity = new boxcam();
            }
            else if (EntityType.eETwdscjm == type)
            {
                entity = new wdscjm();
            }
            else if (EntityType.eETdzmoshibtn == type)
            {
                entity = new moshijm();
            }

            //entity.m_tag = tag;

            entity.setGameObject(go);

            m_name2EntityDic[go.name] = entity as LSBehaviour;
        }

        public ISceneEntity getSceneEntity(string name)
        {
            return m_name2EntityDic[name];
        }

        public void OnMouseUp(GameObject go)
        {
            //if (m_name2EntityDic.ContainsKey(go.name))
            //{
            //    m_name2EntityDic[go.name].OnMouseUpAsButton();
            //}

            //go.SendMessage("OnClick", go);

            //foreach(GameObject key in m_go2EntityDic.Keys)
            //{
            //    if(go == key)
            //    {
            //        m_go2EntityDic[go].OnMouseUp();
            //    }
            //}
        }

        public ISceneEntity getActiveEntity(string name)
        {
            if(m_name2EntityDic.ContainsKey(name))
            {
                return m_name2EntityDic[name];
            }

            return null;
        }
    }
}