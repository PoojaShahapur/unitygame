using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 基本的渲染器，所有与显示有关的接口都在这里
     */
    public class EntityRenderBase
    {
        protected SceneEntityBase m_entity;

        public EntityRenderBase(SceneEntityBase entity_)
        {
            m_entity = entity_;
        }

        virtual public void setClientDispose()
        {

        }

        virtual public bool getClientDispose()
        {
            return m_entity.getClientDispose();
        }

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

        virtual public void show()
        {

        }

        virtual public void hide()
        {

        }

        virtual public bool IsVisible()
        {
            return true;
        }

        virtual public void dispose()
        {
            onDestroy();
        }

        virtual public void onDestroy()
        {

        }

        virtual public void setPnt(GameObject pntGO_)
        {

        }

        virtual public GameObject getPnt()
        {
            return null;
        }

        virtual public bool checkRender()
        {
            return false;
        }
    }
}